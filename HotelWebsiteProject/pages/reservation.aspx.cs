using HotelWebsiteProject.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelWebsiteProject.pages
{
    public partial class reservation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/Default.aspx");
            }
            else if (!IsPostBack)
            {
                lblGreeting.Text = "Hello " + User.Identity.Name + "!";

                txtCheckIn.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
                txtCheckOut.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");

                BindServices();
            }
            Guid userId = GetUserId();
            if (userId == Guid.Empty)
            {
                lblOutput.Text = "User ID not found.";
                lblOutput.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (UserHasReservation(userId))
            {
                Response.Redirect("~/pages/dashboard.aspx");
                return;
            }
        }
        protected void rblRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedRoomType = rblRoomType.SelectedValue;
            PopulateAvailableRooms(selectedRoomType);
            CalculateOneNightPrice();
            CalculateTotalPrice();
        }
        protected void ddlRooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedRoom = ddlRooms.SelectedValue;
            CalculateTotalPrice();
        }

        private void PopulateAvailableRooms(string roomType)
        {
            CRUD myCrud = new CRUD();
            string mySql = "SELECT roomNumber FROM v_rooms WHERE roomTypeName = @RoomType AND status = 0";

            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@roomType", roomType);

            using (SqlDataReader dr = myCrud.getDrPassSql(mySql, myPara))
            {
                ddlRooms.DataSource = dr;
                ddlRooms.DataTextField = "roomNumber";
                ddlRooms.DataValueField = "roomNumber";
                ddlRooms.DataBind();
            }

            ddlRooms.Items.Insert(0, new ListItem("Select a room", "0"));
        }
        protected void DateChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice();
        }
        private void CalculateTotalPrice()
        {
            DateTime checkInDate;
            DateTime checkOutDate;

            if (DateTime.TryParse(txtCheckIn.Text, out checkInDate) && DateTime.TryParse(txtCheckOut.Text, out checkOutDate))
            {
                if (checkOutDate >= checkInDate)
                {
                    int days = (checkOutDate - checkInDate).Days + 1;
                    string selectedRoomType = rblRoomType.SelectedValue;

                    decimal roomPrice = GetRoomPrice(selectedRoomType);

                    if (ddlRooms.SelectedValue != "0")
                    {
                        decimal totalPrice = days * roomPrice;
                        lblTotalPrice.Text = $"Total Price = {totalPrice:N2}";
                        lblTotalPrice.Visible = true;
                    }
                    else
                    {
                        lblTotalPrice.Text = "Please select a room.";
                        lblTotalPrice.Visible = true;
                    }
                }
                else
                {
                    lblTotalPrice.Text = "Check-out date cannot be before check-in date.";
                    lblTotalPrice.Visible = true;
                }
            }
            else
            {
                lblTotalPrice.Text = "Please enter valid dates.";
                lblTotalPrice.Visible = true;
            }
        }

        private void CalculateOneNightPrice()
        {
            string selectedRoomType = rblRoomType.SelectedValue;

            decimal roomPrice = GetRoomPrice(selectedRoomType);

            lblPrice.Text = $"Price per night = {roomPrice:N2}";
            lblPrice.Visible = true;
        }
        private decimal GetRoomPrice(string roomType)
        {
            decimal price = 0;

            CRUD myCrud = new CRUD();
            string mySql = @"SELECT price FROM roomType WHERE roomTypeName = @roomTypeName";
            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@roomTypeName", roomType);
            SqlDataReader dr = myCrud.getDrPassSql(mySql, myPara);
            if (dr.Read())
            {
                price = dr.GetDecimal(0);
            }
            dr.Close();
            return price;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                lblOutput.Text = "Please log in to make a reservation.";
                lblOutput.ForeColor = System.Drawing.Color.Red;
                return;
            }

            Guid userId = GetUserId();
            if (userId == Guid.Empty)
            {
                lblOutput.Text = "User ID not found.";
                lblOutput.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (string.IsNullOrEmpty(rblRoomType.SelectedValue) || ddlRooms.SelectedValue == "0" || string.IsNullOrEmpty(txtCheckIn.Text) || string.IsNullOrEmpty(txtCheckOut.Text))
            {
                lblOutput.Text = "Please complete all fields.";
                lblOutput.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (!DateTime.TryParse(txtCheckIn.Text, out DateTime checkInDate) || !DateTime.TryParse(txtCheckOut.Text, out DateTime checkOutDate))
            {
                lblOutput.Text = "Please enter valid dates.";
                lblOutput.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (checkInDate < DateTime.Today)
            {
                lblOutput.Text = "Check-in date cannot be in the past.";
                lblOutput.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (checkOutDate < checkInDate)
            {
                lblOutput.Text = "Check-out date cannot be before check-in date.";
                lblOutput.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (UserHasReservation(userId))
            {
                Response.Redirect("~/pages/dashboard.aspx");
                return;
            }

            int days = (checkOutDate - checkInDate).Days + 1;
            decimal roomPrice = GetRoomPrice(rblRoomType.SelectedValue);
            decimal totalPrice = days * roomPrice;

            string selectedServices = GetSelectedServices();

            if (InsertReservation(userId, int.Parse(ddlRooms.SelectedValue), checkInDate, checkOutDate, totalPrice, selectedServices))
            {
                Response.Redirect("~/pages/dashboard.aspx");
            }
            else
            {
                lblOutput.Text = "An error occurred while making the reservation. Please try again.";
                lblOutput.ForeColor = System.Drawing.Color.Red;
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


        private bool UserHasReservation(Guid userId)
        {
            CRUD myCrud = new CRUD();
            string mySql = "SELECT COUNT(*) FROM reservations WHERE customerId = (SELECT customerId FROM customers WHERE UserId = @UserId)";

            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@UserId", userId);

            int count = 0;
            using (SqlDataReader dr = myCrud.getDrPassSql(mySql, myPara))
            {
                if (dr.Read())
                {
                    count = dr.GetInt32(0);
                }
            }
            return count > 0;
        }

        private bool InsertReservation(Guid userId, int roomNumber, DateTime checkInDate, DateTime checkOutDate, decimal totalPrice, string selectedServices)
        {
            try
            {
                if (InsertReservationToDb(userId, roomNumber, checkInDate, checkOutDate, totalPrice, selectedServices))
                {
                    string customerFullName = GetCustomerFullName(userId);

                    return UpdateRoomStatus(roomNumber, customerFullName);
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool InsertReservationToDb(Guid userId, int roomNumber, DateTime checkInDate, DateTime checkOutDate, decimal totalPrice, string selectedServices)
        {
            CRUD myCrud = new CRUD();
            string mySql = @"INSERT INTO reservations (customerId, roomNumber, checkInDate, checkOutDate, totalPrice, services, reservationTime) 
                     VALUES ((SELECT customerId FROM customers WHERE UserId = @UserId), @roomNumber, @checkInDate, @checkOutDate, @totalPrice, @services, @reservationTime)";

            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@UserId", userId);
            myPara.Add("@roomNumber", roomNumber);
            myPara.Add("@checkInDate", checkInDate);
            myPara.Add("@checkOutDate", checkOutDate);
            myPara.Add("@totalPrice", totalPrice);
            myPara.Add("@services", selectedServices);
            myPara.Add("@reservationTime", DateTime.Now);

            int rtn = myCrud.InsertUpdateDelete(mySql, myPara);
            return rtn > 0;
        }

        private bool UpdateRoomStatus(int roomNumber, string customerFullName)
        {
            CRUD myCrud = new CRUD();
            string mySql = @"UPDATE rooms SET status = 1, ownedBy = @ownedBy WHERE roomNumber = @roomNumber";

            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@ownedBy", customerFullName);
            myPara.Add("@roomNumber", roomNumber);

            int rtn = myCrud.InsertUpdateDelete(mySql, myPara);
            return rtn > 0;
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
        private void BindServices()
        {
            CRUD myCrud = new CRUD();
            string mySql = "SELECT serviceID, serviceName FROM services";
            Dictionary<string, object> myPara = new Dictionary<string, object>();

            SqlDataReader dr = myCrud.getDrPassSql(mySql, myPara);

            rptServices.DataSource = dr;
            rptServices.DataBind();

            dr.Close();
        }

        protected void rptServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var dataItem = (System.Data.Common.DbDataRecord)e.Item.DataItem;
                CheckBoxList cblServices = (CheckBoxList)e.Item.FindControl("cblServices");

                ListItem listItem = new ListItem
                {
                    Text = dataItem["serviceName"].ToString(),
                    Value = dataItem["serviceID"].ToString()
                };

                cblServices.Items.Add(listItem);

            }
        }
        private string GetSelectedServices()
        {
            List<string> selectedServices = new List<string>();
            foreach (RepeaterItem item in rptServices.Items)
            {
                CheckBoxList cblServices = item.FindControl("cblServices") as CheckBoxList;
                if (cblServices != null)
                {
                    foreach (ListItem service in cblServices.Items)
                    {
                        if (service.Selected)
                        {
                            selectedServices.Add(service.Text);
                        }
                    }
                }
            }
            return string.Join(", ", selectedServices);
        }
    }
}

