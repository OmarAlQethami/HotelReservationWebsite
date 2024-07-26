using HotelWebsiteProject.App_Code;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocToPDFConverter;
using Syncfusion.Pdf;
using System.Data;

namespace HotelWebsiteProject.pages
{
    public partial class dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/Default.aspx");
            }
            else if (!IsPostBack)
            {
                lblConfirmation.Text = "Your reservation is successful, " + User.Identity.Name + "!";
                populateTxtReservation();
            }
            
            populateGvReservation();
            

        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            populateTxtReservation();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Guid userId = GetUserId();
            if (userId == Guid.Empty)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            string fName = txtFName.Text;
            string lName = txtLName.Text;
            string phoneNumber = txtPhoneNumber.Text;
            string email = txtEmail.Text;

            CRUD myCrud = new CRUD();
            string mySql = @"UPDATE customers
                            SET fName = @fName, lName = @lName, phoneNumber = @phoneNumber, email = @email
                            WHERE customerId = (SELECT customerId FROM customers WHERE UserId = @UserId)";

            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@UserId", userId);
            myPara.Add("@fName", fName);
            myPara.Add("@lName", lName);
            myPara.Add("@phoneNumber", phoneNumber);
            myPara.Add("@email", email);
            try
            {
                int rtn = myCrud.InsertUpdateDelete(mySql, myPara);
                if (rtn >= 1)
                {
                    bool roomUpdateSuccess = UpdateRoomInfo(userId, false);
                    if (roomUpdateSuccess)
                    {
                        lblOutput.Text = "Your information have been updated successfully.";
                        lblOutput.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblOutput.Text = "Information updated, but updating room information failed. Please try again.";
                        lblOutput.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    lblOutput.Text = "Updating your information has failed. Please try again.";
                    lblOutput.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblOutput.Text = "An error occurred: " + ex.Message;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Guid userId = GetUserId();
            if (userId == Guid.Empty)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            CRUD myCrud = new CRUD();
            string mySql = @"DELETE FROM reservations WHERE customerId
                        = (SELECT customerId FROM customers WHERE UserId = @UserId)";

            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@UserId", userId);
            try
            {
                int rtn = myCrud.InsertUpdateDelete(mySql, myPara);
                if (rtn >= 1)
                {
                    bool roomUpdateSuccess = UpdateRoomInfo(userId, true);
                    if (roomUpdateSuccess)
                    {
                        Response.Redirect("~/Default.aspx");
                    }
                    else
                    {
                        lblOutput.Text = "Reservation deleted, but updating room information failed. Please try again.";
                        lblOutput.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    lblOutput.Text = "Deleting your reservation has failed. Please try again.";
                    lblOutput.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblOutput.Text = "An error occurred: " + ex.Message;
            }
        }

        protected void btnEmail_Click(object sender, EventArgs e)
        {
            Guid userId = GetUserId();
            if (userId == Guid.Empty)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            int reservationId = GetReservationId(userId);
            if (reservationId == 0)
            {
                lblOutput.Text = "Reservation not found.";
                return;
            }

            string filePath = Server.MapPath($"~/Result/{reservationId}_receipt.pdf");

            if (!File.Exists(filePath))
            {
                lblOutput.Text = "Receipt is not found for your reservation. First, Please click Issue Receipt button.";
                lblOutput.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string rtn = "";
            using (mailMgr myMailMgr = new mailMgr())
            {
                myMailMgr.myTo = GetCustomerEmail(userId);
                myMailMgr.mySubject = "Your Recent Stay - Reservation Receipt";
                myMailMgr.myBody = @"
Dear Valued Guest,

Thank you for choosing our hotel for your recent stay. Attached, you will find your reservation receipt.

We hope you enjoy your stay with us and look forward to welcoming you back soon.

If you have any questions or require further assistance, please do not hesitate to contact us.

Warm regards,
";

                rtn = myMailMgr.sendEmailWithAttachment(filePath);
                lblOutput.Text = rtn;
            }
        }


        protected string GetCustomerEmail(Guid userId)
        {
            CRUD myCrud = new CRUD();
            string mySql = "SELECT email FROM customers WHERE UserId = @UserId";
            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@UserId", userId);

            string email = string.Empty;
            using (SqlDataReader dr = myCrud.getDrPassSql(mySql, myPara))
            {
                if (dr.Read())
                {
                    email = dr["email"].ToString();
                }
            }
            return email;
        }



        protected void btnExcel_Click(object sender, EventArgs e)
        {
            ExportGridToExcel(gvReservation);
        }

        protected void populateGvReservation()
        {
            Guid userId = GetUserId();
            if (userId == Guid.Empty)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            CRUD myCrud = new CRUD();
            string mySql = @"SELECT reservationId, fName, lName, phoneNumber, email, roomNumber,
                            checkInDate, checkOutDate, totalPrice, services, reservationTime FROM customers
                            INNER JOIN reservations ON customers.customerId = reservations.customerId
                            WHERE customers.UserId = @UserId";
            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@UserId", userId);
            SqlDataReader dr = myCrud.getDrPassSql(mySql, myPara);
            gvReservation.DataSource = dr;
            gvReservation.DataBind();
        }
        protected void populateTxtReservation()
        {
            Guid userId = GetUserId();
            if (userId == Guid.Empty)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            CRUD myCrud = new CRUD();
            string mySql = @"SELECT reservationId, fName, lName, phoneNumber, email, roomNumber,
                            checkInDate, checkOutDate, services, totalPrice, reservationTime 
                            FROM customers
                            INNER JOIN reservations ON customers.customerId = reservations.customerId
                            WHERE customers.UserId = @UserId";
            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@UserId", userId);

            using (SqlDataReader dr = myCrud.getDrPassSql(mySql, myPara))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        txtReservationId.Text = dr["reservationId"].ToString();
                        txtFName.Text = dr["fName"].ToString();
                        txtLName.Text = dr["lName"].ToString();
                        txtPhoneNumber.Text = dr["phoneNumber"].ToString();
                        txtEmail.Text = dr["email"].ToString();
                        txtRoomNumber.Text = dr["roomNumber"].ToString();
                        txtCheckInDate.Text = dr["checkInDate"].ToString();
                        txtCheckOutDate.Text = dr["checkOutDate"].ToString();
                        txtServices.Text = dr["services"].ToString();
                        txtTotalPrice.Text = dr["totalPrice"].ToString();
                        txtReservationTime.Text = dr["reservationTime"].ToString();
                    }
                }
            }
        }

        private Guid GetUserId()
        {
            if (Membership.GetUser() is MembershipUser user)
            {
                return (Guid)user.ProviderUserKey;
            }
            return Guid.Empty;
        }
        private string GetCustomerFullName(Guid userId)
        {
            CRUD myCrud = new CRUD();
            string mySql = "SELECT fName, lName FROM customers WHERE UserId = @UserId";
            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@UserId", userId);

            string fullName = "";
            using (SqlDataReader dr = myCrud.getDrPassSql(mySql, myPara))
            {
                if (dr.Read())
                {
                    fullName = $"{dr["fName"]} {dr["lName"]}";
                }
            }
            return fullName;
        }

        private bool UpdateRoomInfo(Guid userId, bool isDeleted)
        {
            int roomNumber = GetRoomNumberByUserId(userId);
            if (roomNumber == 0)
            {
                return false;
            }

            CRUD myCrud = new CRUD();
            string mySql;
            Dictionary<string, object> myPara = new Dictionary<string, object>();

            if (isDeleted)
            {
                mySql = @"UPDATE rooms 
                  SET status = 0, ownedBy = NULL 
                  WHERE roomNumber = @roomNumber";
                myPara.Add("@roomNumber", roomNumber);
            }
            else
            {
                string customerFullName = GetCustomerFullName(userId);
                mySql = @"UPDATE rooms 
                  SET status = 1, ownedBy = @ownedBy 
                  WHERE roomNumber = @roomNumber";
                myPara.Add("@ownedBy", customerFullName);
                myPara.Add("@roomNumber", roomNumber);
            }

            try
            {
                int rtn = myCrud.InsertUpdateDelete(mySql, myPara);
                return rtn > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error updating room info: " + ex.Message);
                return false;
            }
        }


        private int GetRoomNumberByUserId(Guid userId)
        {
            string customerFullName = GetCustomerFullName(userId);

            if (string.IsNullOrEmpty(customerFullName))
            {
                throw new Exception("Customer full name not found.");
            }

            CRUD myCrud = new CRUD();
            string getRoomNumberSql = "SELECT roomNumber FROM rooms WHERE ownedBy = @OwnedBy";
            Dictionary<string, object> roomPara = new Dictionary<string, object>();
            roomPara.Add("@OwnedBy", customerFullName);

            int roomNumber = 0;
            using (SqlDataReader dr = myCrud.getDrPassSql(getRoomNumberSql, roomPara))
            {
                if (dr.Read())
                {
                    roomNumber = Convert.ToInt32(dr["roomNumber"]);
                }
            }

            return roomNumber;
        }


        public static void ExportGridToExcel(GridView myGv)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Charset = "";
            string FileName = "ExportedReport_" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            myGv.GridLines = GridLines.Both;
            myGv.HeaderStyle.Font.Bold = true;
            myGv.RenderControl(htmltextwrtter);
            HttpContext.Current.Response.Write(strwritter.ToString());
            HttpContext.Current.Response.End();
        }

        protected void btnIssue_Click(object sender, EventArgs e)
        {
            Guid userId = GetUserId();
            if (userId == Guid.Empty)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            int reservationId = GetReservationId(userId);
            if (reservationId == 0)
            {
                lblOutput.Text = "Reservation not found.";
                return;
            }

            using (WordDocument template = new WordDocument())
            {
                template.Open(Server.MapPath("~/myTemplates/reservationReceipt.dotx"));
                
                DataTable customer = GetCustomer(userId);
                
                if (!Directory.Exists(Path.GetFullPath(@"../../Result/")))
                    Directory.CreateDirectory(Path.GetFullPath(@"../../Result/"));
                foreach (DataRow dataRow in customer.Rows)
                {
                    WordDocument document = template.Clone();
                   
                    document.MailMerge.Execute(dataRow);

                    DocToPDFConverter converter = new DocToPDFConverter();
                    
                    PdfDocument pdfDocument = converter.ConvertToPDF(document);
                    
                    document.Close();
                    
                    converter.Dispose();
                    pdfDocument.Save(Server.MapPath($"~/Result/{reservationId}_receipt.pdf"));
                    pdfDocument.Close(true);
                    
                }
            }
            lblOutput.Text = "Receipt is issued successfully!";
            lblOutput.ForeColor = System.Drawing.Color.Green;
        }
        protected DataTable GetCustomer(Guid userId)
        {
            CRUD myCrud = new CRUD();
            string mySql = @"SELECT reservationId, fName, lName, phoneNumber, email, roomNumber,
                    checkInDate, checkOutDate, totalPrice, reservationTime 
                    FROM customers
                    INNER JOIN reservations ON customers.customerId = reservations.customerId
                    WHERE customers.UserId = @UserId";

            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@UserId", userId);

            DataTable dt = myCrud.getDTPassSqlDic(mySql, myPara);
            return dt;
        }

        protected int GetReservationId(Guid userId)
        {
            CRUD myCrud = new CRUD();
            string mySql = "SELECT reservationId FROM reservations WHERE customerId = (SELECT customerId FROM customers WHERE UserId = @UserId)";
            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@UserId", userId);

            int reservationId = 0;
            using (SqlDataReader dr = myCrud.getDrPassSql(mySql, myPara))
            {
                if (dr.Read())
                {
                    reservationId = Convert.ToInt32(dr["reservationId"]);
                }
            }
            return reservationId;
        }

    }
}