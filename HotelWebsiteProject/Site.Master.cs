using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelWebsiteProject
{
    public partial class SiteMaster : MasterPage
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{

        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            // this code hide admin menu from all other users
            if (!Roles.IsUserInRole("admin"))
            {
                MenuItemCollection menuItems = NavigationMenu.Items;
                MenuItem adminItem = new MenuItem();
                foreach (MenuItem menuItem in menuItems)
                {
                    if (menuItem.Text == "Admin")// to hide specific menu put  the name here ... Warning, case sensitive 
                        adminItem = menuItem;
                }
                menuItems.Remove(adminItem);
            }
            if (!Roles.IsUserInRole("finance"))
            {
                MenuItemCollection menuItems = NavigationMenu.Items;
                MenuItem adminItem = new MenuItem();
                foreach (MenuItem menuItem in menuItems)
                {
                    if (menuItem.Text == "Finance")// to hide specific menu put  the name here ... Warning, case sensitive 
                        adminItem = menuItem;
                }
                menuItems.Remove(adminItem);
            }
            if (!Roles.IsUserInRole("marketing"))
            {
                MenuItemCollection menuItems = NavigationMenu.Items;
                MenuItem adminItem = new MenuItem();
                foreach (MenuItem menuItem in menuItems)
                {
                    if (menuItem.Text == "Marketing")// to hide specific menu put  the name here ... Warning, case sensitive 
                        adminItem = menuItem;
                }
                menuItems.Remove(adminItem);
            }
        }

    }
}