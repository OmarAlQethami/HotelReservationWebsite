using HotelWebsiteProject.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelWebsiteProject.admin
{
    public partial class showRoles : System.Web.UI.Page
    {
        CRUD myKfmcCrud = new CRUD();

        string appName = "";
        string appNameWithProvider = "";
        /// <summary>
        /// check if user exists in the Db
        /// </summary>
        private bool CheckUserExistance(string userName)
        {
            if (Membership.GetUser(userName) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected void postMsg(string msg)
        {
            lblMsg.Text = msg.ToString();
        }
        protected void postMsgClient(string msg)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert(msg)", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // get application Name into the query 
            appName = Membership.ApplicationName;
            // post application name
            appNameWithProvider = Membership.ApplicationName + "<BR>";
            appNameWithProvider += Membership.Provider + "<BR>";
            postMsg(appNameWithProvider);

            populateGvAllUsers();

            // If user is not authenticated, redirect to Login page
            if (!Request.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
                Response.End();
            }

            // show membership users

            if (!Page.IsPostBack)
            {
                populateCheckBoxListRolesUsers();
            }
            populateGvReservation();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        protected void populateCheckBoxListRolesUsers()
        {
            cBLUsers.DataSource = System.Web.Security.Membership.GetAllUsers();
            cBLUsers.DataBind();

            cBLRoles.DataSource = System.Web.Security.Roles.GetAllRoles();
            cBLRoles.DataBind();
        }
        void AssignDefaultRoleToUser(string userName, string NewRole)
        { 
            if (User.Identity.Name == userName)
            {
                if (!Roles.IsUserInRole(NewRole))
                {
                    Roles.AddUserToRole(User.Identity.Name, NewRole);
                    postMsg("Success");
                }
                else
                {
                    postMsg("User already in the role");
                }
            }
        }
        void addUserToRole(string user, string role)
        {
            if (!Roles.IsUserInRole(role))
                Roles.AddUserToRole(user, role);
            else
                ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('User already in this role')", true);
            return;
        }
        protected void btnCreateRole_Click(object sender, EventArgs e)
        {
            if (!Roles.RoleExists(txtRole.Text))
            {
                createRole(txtRole.Text);
                populateGvAllUsers();

            }
            else
            {
                postMsg("Role already exists!!");
            }
            populateCheckBoxListRolesUsers();
        }
        void createRole(string myRole)
        {
            if (!Roles.RoleExists(myRole))
            {
                Roles.CreateRole(myRole);
                postMsg("New Role Created");
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('Role already Exists')", true);
                postMsg("Role already exists");
                return;
            }
        }
        protected void btnDeleteRole_Click(object sender, EventArgs e)
        {
            if (!Roles.RoleExists(txtRole.Text))
            {
                lblMsg.Text = "Role Does not Exist";
                return;
            }
            // check if any roles has users
            if (Roles.RoleExists(txtRole.Text) && Roles.GetUsersInRole(txtRole.Text).Length == 0)
            {
                Roles.DeleteRole(txtRole.Text);
                lblMsg.Text = "Role Deleted";
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('Role is in use by other users')", true);
                postMsg("Role is in use by other users");
                //return;
            }
            populateCheckBoxListRolesUsers();
            populateGvAllUsers();
        }
        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            // if user does not exist, create a new user
            //if (Membership.GetUser(txtUser.Text) != null)
            //{
            try
            {
                string vUser = txtUser.Text.ToString();
                string vPassword = txtPassword.Text.ToString();
                string vEmail = txtEmail.Text.ToString();
                if (!Membership.ValidateUser(vUser, vPassword)) // if user not valid, then create it
                {
                    Membership.CreateUser(vUser, vPassword, vEmail);
                    postMsg("User Created Successfuly");
                    populateGvAllUsers();
                }

                else
                {
                    postMsg("User already exists!!");
                    return;
                }
                populateCheckBoxListRolesUsers();
            }

            catch (Exception ex)
            {
                postMsg(ex.Message.ToString());  // to do , log the errors  rtn
                                                 // postMsgClient(ex.Message);
            }
        }
        protected void btnLinkUserRole_Click(object sender, EventArgs e)
        {// todo... enhance this code WITH SWITCH 
            if (string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtRole.Text))
            {
                lblMsg.Text = "Fill both User and Role fields";
                return;
            }
            // 
            if (Membership.FindUsersByName(txtUser.Text).Count == 0)
            {
                lblMsg.Text = "User Does not Exists";
                return;
            }
            if (!Roles.RoleExists(txtRole.Text))
            {
                lblMsg.Text = "Role Does not Exists";
                return;
            }
            //  if (Roles.RoleExists(txtRole.Text) && Membership.FindUsersByName(txtUser.Text).Count >= 1) // OLD 
            if (Roles.RoleExists(txtRole.Text) && !Roles.IsUserInRole(txtUser.Text, txtRole.Text)) // BETTER
            {
                Roles.AddUserToRole(txtUser.Text, txtRole.Text);
                lblMsg.Text = "Linked Successful";
                populateGvAllUsers(); // repopulate gv
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('User already assigned a role')", true);
                postMsg("User already assigned a role");
                return;
            }
        }
        protected void btnShowAllUser_Click(object sender, EventArgs e)
        {
            //MembershipUserCollection users = Membership.GetAllUsers();
            //gvUsers.DataSource = users;
            //gvUsers.DataBind();
        }
        protected void btnShowAllRoles_Click(object sender, EventArgs e)
        {
            Page_Load(this, null);
        }
        protected void btnDeleteUser_Click(object sender, EventArgs e)
        { // check if user exists, there is two ways 
          //  if (Membership.FindUsersByName(txtUser.Text).Count != 0)  // one way

            // is to create an instance of MembershipUser and check to fill it from db, if not null means user exists, 
            MembershipUser existingUser = null;  // second way 
            existingUser = Membership.GetUser(txtUser.Text);
            if (existingUser != null)  // if user exists, then delete him
            {
                Guid userId = (Guid)Membership.GetUser(txtUser.Text).ProviderUserKey;

                CRUD myCrud = new CRUD();
                string mySql = @"DELETE FROM Customers WHERE UserId = @UserId";

                Dictionary<string, object> myPara = new Dictionary<string, object>();
                myPara.Add("@UserId", userId);
                int rtn = myCrud.InsertUpdateDelete(mySql, myPara);
                if (rtn >= 1)
                {
                    if (Membership.DeleteUser(txtUser.Text))
                    {
                        postMsg("Was Deleted");
                        populateGvAllUsers();
                    }
                    else
                    {
                        postMsg("Was Not Deleted");
                    }
                }
                else
                {
                    postMsg("Was Not Deleted from both tables");
                }
            }
            else
            {
                postMsg("User does not exists!!");
            }
            populateCheckBoxListRolesUsers();
        }
        protected void btnUnLinkUserToRole_Click(object sender, EventArgs e)
        {  // to unlink a role, the user should be in same role to unlink it
           // todo: how to secify  a user 
           // if (Roles.RoleExists(txtRole.Text) && User.IsInRole(txtRole.Text))
            if (Roles.IsUserInRole(txtUser.Text, txtRole.Text))
            {
                Roles.RemoveUserFromRole(txtUser.Text, txtRole.Text);
                lblMsg.Text = "Unlinked Successfully";

                // repopulate gv
                populateGvAllUsers();
            }
            else
            {
                lblMsg.Text = "Unlinked Failed: User is not in the specified role";
                ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('Unlinked Failed: User is not in the specified role')", true);
                return;
            }
        }
        /// <summary>
        ///     non-ANSI outer join operators ("*=" or "=*").
        ///     ANSI outer join operators (LEFT OUTER JOIN, RIGHT OUTER JOIN).
        ///     
        ///     This is an important method that shows how to use left outer join and right outer join
        /// </summary>
        protected void populateGvAllUsers()
        {
            ////get a collection of all users
            //MembershipUserCollection users = Membership.GetAllUsers();
            ////  string email = users["some_username"].Email;
            //gvAllUsers.DataSource = users;
            //gvAllUsers.DataBind();

            string userCmd = "select username from dbo.aspnet_Users order by username";
            populateUserRole(userCmd, gvUsers);

            string roleCmd = "select rolename  from dbo.aspnet_Roles order by rolename";
            populateUserRole(roleCmd, gvRoles);

            string selectCommandNonANSI = @"select u.username ,r.rolename " +
                                "from dbo.aspnet_Users u, dbo.aspnet_Roles r  ,dbo.aspnet_UsersInRoles ur,dbo.aspnet_Applications  ap "
                              + "where u.UserId= ur.UserId  "
                              + "and ur.RoleId= r.RoleId "
                             + " and   applicationname ='" + appName + "'"
                                + "  order by username";
            populateUserRole(selectCommandNonANSI, gvNonAnsiInnerJoin);

            string innerJoin = @"select u.username ,r.rolename  "
                               + " from dbo.aspnet_Users u "
                               + " inner join dbo.aspnet_UsersInRoles ur on  u.UserId= ur.UserId   "
                               + " inner join   dbo.aspnet_Roles r   on ur.RoleId= r.RoleId  "
                               + " inner join dbo.aspnet_Applications  ap on u.applicationId= ap.applicationId"
                               + " where applicationname ='" + appName + "'"
                               + "  order by username";
            populateUserRole(innerJoin, gvInnerJoin);

            string leftOuterJoin = "select u.username ,r.rolename  "
                               + " from dbo.aspnet_Users u"
                               + " left outer join dbo.aspnet_UsersInRoles ur on  u.UserId= ur.UserId  "
                               + " left outer join   dbo.aspnet_Roles r   on ur.RoleId= r.RoleId  "
                               + " inner join dbo.aspnet_Applications  ap on u.applicationId= ap.applicationId"
                               + " where applicationname ='" + appName + "'"
                               + " order by username";
            populateUserRole(leftOuterJoin, gvLeftOuterJoin);

            string rightOuterJoin = "select distinct  u.username, r.rolename "
                               + " from dbo.aspnet_Applications ap inner join dbo.aspnet_Membership m on ap.applicationid=m.applicationid"
                               + " inner join dbo.aspnet_Users u on m.applicationid = u.applicationid "
                               + " right outer join dbo.aspnet_UsersInRoles ur on  u.UserId= ur.UserId   "
                               + " right outer join   dbo.aspnet_Roles r   on ur.RoleId= r.RoleId  "
                               //     + " where applicationname ='" + appName + "'" 
                               + "  order by username";
            populateUserRole(rightOuterJoin, gvRightOuterJoin);


            //string conString = //WebConfigurationManager.ConnectionStrings["FtreeConStrlocal"].ConnectionString;
            //SqlDataAdapter dad = new SqlDataAdapter(selectCommandNonANSI, conString);
            //DataTable dtUserRoles = new DataTable();
            //dad.Fill(dtUserRoles);
            //gvAllUsersRoles.DataSource = dtUserRoles;
            //gvAllUsersRoles.DataBind();
        }
        private string getApplicationName()
        {
            // post application name
            string info = Membership.ApplicationName + "<BR>";
            info += Membership.Provider + "<BR>";
            return info;

        }

        private void populateUserRole(string sqlCmd, GridView gvName)
        {
            string conString = CRUD.conStr; //WebConfigurationManager.ConnectionStrings["conStrInternship"].ConnectionString;//WebConfigurationManager.ConnectionStrings["FtreeConStrlocal"].ConnectionString;
            SqlDataAdapter dad = new SqlDataAdapter(sqlCmd, conString);
            DataTable dtUserRoles = new DataTable();
            dad.Fill(dtUserRoles);
            gvName.DataSource = dtUserRoles;
            gvName.DataBind();
        }
        protected void btnUserRoleAssign_Click(object sender, EventArgs e)
        {
            string strX = " ";
            // iterate the CheckboxList and update user role by making selection
            foreach (ListItem itemRole in cBLRoles.Items)
            {
                if (itemRole.Selected)
                {
                    foreach (ListItem itemUser in cBLUsers.Items)
                    {
                        if (itemUser.Selected && !Roles.IsUserInRole(itemUser.Text, itemRole.Text))
                            Roles.AddUserToRole(itemUser.Text, itemRole.Text);
                    }
                    strX += itemRole.Text;
                }

                else
                {
                    foreach (ListItem itemUser2 in cBLUsers.Items)
                    {
                        if (itemUser2.Selected && Roles.IsUserInRole(itemUser2.Text, itemRole.Text))//(User.IsInRole(itemRole.Text))
                            Roles.RemoveUserFromRole(itemUser2.Text, itemRole.Text);
                    }
                }
            }
            Response.Redirect(Request.Path);
            populateGvAllUsers();
        }
        /// <summary>
        /// https://stackoverflow.com/questions/8986294/how-can-i-change-or-update-password-in-asp-net-membership-via-sql-server
        /// </summary>
        protected void btnUpdateUser_Click(object sender, EventArgs e)
        {

        }

        protected void btnDeleteRoles_Click(object sender, EventArgs e)
        {
            // iterate the CheckboxList and capture user role by making selection
            foreach (ListItem itemRole in cBLRoles.Items)
            {
                // check if role has users
                if (itemRole.Selected)
                {
                    if (Roles.GetUsersInRole(itemRole.Text).Length == 0)
                        Roles.DeleteRole(itemRole.Text);
                    else
                        lblMsg.Text = "Cannot Delete Role when user is assigned it";
                }
            }
            populateGvAllUsers();
            populateCheckBoxListRolesUsers();
        }

        protected void btnDeleteUsers_Click(object sender, EventArgs e)
        {
            // iterate the CheckboxList and capture user role by making selection
            foreach (ListItem itemUser in cBLUsers.Items)
            {
                if (itemUser.Selected)
                {
                    Membership.DeleteUser(itemUser.Text);
                }
            }
            populateGvAllUsers();
            populateCheckBoxListRolesUsers();
        }
        protected void btnUnlinkUserRoles_Click(object sender, EventArgs e)
        {
            
            string strX = " ";
            
            foreach (ListItem itemRole in cBLRoles.Items)
            {
                if (itemRole.Selected)
                {
                    foreach (ListItem itemUser in cBLUsers.Items)
                    {
                        if (itemUser.Selected)
                        {
                            if (Roles.IsUserInRole(itemUser.Text, itemRole.Text))
                            {
                                Roles.RemoveUserFromRole(itemUser.Text, itemRole.Text);
                                lblMsg.Text = "Unlinked Successfully";
                            }
                        }
                    }
                }
            }
            populateCheckBoxListRolesUsers();
            populateGvAllUsers();
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
                            INNER JOIN reservations ON customers.customerId = reservations.customerId";
            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@UserId", userId);
            SqlDataReader dr = myCrud.getDrPassSql(mySql, myPara);
            gvReservations.DataSource = dr;
            gvReservations.DataBind();
        }

        private Guid GetUserId()
        {
            if (Membership.GetUser() is MembershipUser user)
            {
                return (Guid)user.ProviderUserKey;
            }
            return Guid.Empty;
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            ExportGridToExcel(gvReservations);
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

        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            CRUD myCrud = new CRUD();
            string mySql = @"DELETE FROM reservations";

            try
            {
                int rtn = myCrud.InsertUpdateDelete(mySql);
                if (rtn >= 1)
                {
                    bool roomUpdateSuccess = UpdateRoomInfo();
                    if (roomUpdateSuccess)
                    {
                        Response.Redirect("~/admin/showRoles.aspx");
                    }
                    else
                    {
                        lblMsg.Text = "Reservations deleted, but updating room information failed. Please try again.";
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    lblMsg.Text = "Deleting reservations has failed. Please try again.";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "An error occurred: " + ex.Message;
            }
        }

        private bool UpdateRoomInfo()
        {
            CRUD myCrud = new CRUD();
            string mySql = @"UPDATE rooms 
                            SET status = 0, ownedBy = NULL";
            
            try
            {
                int rtn = myCrud.InsertUpdateDelete(mySql);
                return rtn > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error updating room info: " + ex.Message);
                return false;
            }
        }
    }
}