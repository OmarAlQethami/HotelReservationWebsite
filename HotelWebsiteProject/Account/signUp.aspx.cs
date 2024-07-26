using HotelWebsiteProject.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelWebsiteProject.Account
{
    public partial class signUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            string newUser = txtUsername.Text.ToString();
            string newPassword = txtPassword.Text.ToString();
            string newEmail = txtEmail.Text.ToString();
            if (!Membership.ValidateUser(newUser, newPassword)) // if user not valid, then create it
            {
                MembershipUser newUserObj = Membership.CreateUser(newUser, newPassword, newEmail);
                Guid userId = (Guid)newUserObj.ProviderUserKey;

                string strFName = txtFName.Text;
                string strLName = txtLName.Text;
                string strPhoneNumber = txtPhoneNumber.Text;
                string strEmail = txtEmail.Text;

                CRUD myCrud = new CRUD();
                string mySql = @"INSERT INTO customers (fName, lName, phoneNumber, email, UserId)
                           VALUES (@fName, @lName, @phoneNumber, @email, @UserId)";

                Dictionary<string, object> myPara = new Dictionary<string, object>();
                myPara.Add("@fName", strFName);
                myPara.Add("@lName", strLName);
                myPara.Add("@phoneNumber", strPhoneNumber);
                myPara.Add("@Email", strEmail);
                myPara.Add("@UserId", userId);
                int rtn = myCrud.InsertUpdateDelete(mySql, myPara);
                if (rtn >= 1)
                {
                    Roles.AddUserToRole(newUser, "customer");
                    lblOutput.Text = "User Created Successfully. Please return to login page.";
                }
                else
                {
                    lblOutput.Text = "Signing up has failed. Please try again.";
                }
            }
            else
            {
                lblOutput.Text = "The Username you chose is unavailable. Please try with a different username.";
                return;
            }
        }
    }
}