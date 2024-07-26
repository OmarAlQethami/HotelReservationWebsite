<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="showRoles.aspx.cs" 
    Inherits="HotelWebsiteProject.admin.showRoles" EnableEventValidation="false" ValidateRequest="false"
%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
    <h3> User Role Management Console</h3>
    <div>
        <table border="1">
            <tr>
                <th>
                    Roles 
                </th>
                <th>
                    Users
                </th>
                <th>
                    <asp:Button ID="btnUserRoleAssign" runat="server" 
                    Text="Link User Role" OnClick="btnUserRoleAssign_Click" Width="120px" Visible="true" />
                </th>
            </tr>
            <tr>
                <td valign="top">
                    <asp:CheckBoxList ID="cBLRoles" runat="server"></asp:CheckBoxList>
                </td>
                 <td valign="top">
                    <asp:CheckBoxList ID="cBLUsers" runat="server">
                    </asp:CheckBoxList>
                </td>
                 <td valign="top">
                    <asp:Button ID="btnUnlinkUserRoles" runat="server" OnClick="btnUnlinkUserRoles_Click" Text="Unlink User Role" Width="120px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;</td>
                <td>
                     <asp:Button ID="btnDeleteRoles" runat="server" OnClick="btnDeleteRoles_Click" Text="Delete Roles" Width="120px" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnDeleteUsers" runat="server" OnClick="btnDeleteUsers_Click" Text="Delete Users" Width="120px" />
                </td>
            </tr>
        </table>
    </div>

       <table  border="2">
           <tr>
               <td colspan ="5" align="center" >
                   <asp:Label ID="lblMsg" runat="server"  Text=""></asp:Label>
               </td>
           </tr>
           <tr>
               <td>
                   User
               </td>
               <td align="center" class="style4">
                   <asp:TextBox ID="txtUser" runat="server" autocomplete="off"></asp:TextBox>
               </td>
               <td align="center" class="style4" style="width: 42px">
                   Role
               </td>
               <td align="center" class="style4" style="width: 61px">
                   <asp:TextBox ID="txtRole" runat="server" autocomplete="off"></asp:TextBox>
               </td>
               <td style="width: 177px">
                   <asp:Button ID="btnLinkUserRole" runat="server" OnClick="btnLinkUserRole_Click" Text="Link User Role"
                       Width="124px" style="margin-left: 0px" TabIndex="6" />
               </td>
               </tr>
               <tr>
                   <td>
                       Password
                   </td>
                   <td>
                       <asp:TextBox ID="txtPassword" runat="server" TabIndex="1" autocomplete="off"></asp:TextBox>
                   </td>
                   <td>
                   </td>
                   <td>
                   </td>
                   <td style="width: 177px">
                       <asp:Button ID="btnUnLinkUserToRole" runat="server" OnClick="btnUnLinkUserToRole_Click"
                           Text="UnlinkUsertoRole" Width="124px" />
                   </td>
               </tr>
               <tr>
                   <td class="style3" style="width: 68px">
                       Email</td>
                   <td align="center" class="style4">
                       <asp:TextBox ID="txtEmail" runat="server" TabIndex="2" autocomplete="off"></asp:TextBox>
                   </td>
                   <td align="center" class="style4" style="width: 42px">
                       &nbsp;</td>
                   <td align="center" >
                       &nbsp;</td>
                   <td >
                       <asp:Button ID="btnShowAllUser" runat="server" OnClick="btnShowAllUser_Click" Text="Show All Users"
                           Width="124px" />
                   </td>
               </tr>
               <tr>
                   <td class="style3" style="width: 68px">
                       &nbsp;
                   </td>
                   <td align="center" class="style4">
                       <asp:Button ID="btnCreateUser0" runat="server" OnClick="btnCreateUser_Click" Style="margin-left: 0px"
                           Text="Create User" TabIndex="3" />
                   </td>
                   <td align="center" class="style4" style="width: 42px">
                       &nbsp;</td>
                   <td align="center" >
                       <asp:Button ID="btnCreateRole0" runat="server" OnClick="btnCreateRole_Click" 
                           Text="Create Role" Width="98px" TabIndex="5" />
                       </td>
                   <td >
                       <asp:Button ID="btnShowAllRoles" runat="server" OnClick="btnShowAllRoles_Click" 
                           Text="Show All Roles" Width="124px"
                           />
                   </td>
               </tr>
               <tr>
                   <td class="style3">
                       &nbsp;
                   </td>
                   <td align="center" class="style4">
                       <asp:Button ID="btnDeleteUser0" runat="server" OnClick="btnDeleteUser_Click" Text="Delete User" Width="105px" />
                   </td>
                   <td align="center" class="style4" style="width: 42px">
                       </td>
                   <td align="center">
                       
                       <asp:Button ID="btnDeleteRole1" runat="server" OnClick="btnDeleteRole_Click" Text="Delete Role" />
                   </td>
                   <td style="width: 177px">
                       <asp:Button ID="btnUpdateUser" runat="server" OnClick="btnUpdateUser_Click" Text="Update User" Width="125px" />
                   </td>
           </tr>
       </table>
        <h3>Your Roles</h3>
        <fieldset >
        <legend>Roles 1</legend>
            <asp:GridView ID="grdRoles" DataSourceID="srcRoles" EmptyDataText="You are not a member of any roles"
                GridLines="none" runat="server" />
            <asp:ObjectDataSource ID="srcRoles" 
                TypeName="System.Web.Security.Roles" 
                SelectMethod="GetRolesForUser"
                runat="server" />
        </fieldset>
    
        <br />

<%--        <fieldset>
        <h3>All Users</h3>
            <legend>Users</legend>
            <asp:GridView ID="gvUsers" runat="server" DataSourceID="objAllUsers">
            </asp:GridView>
            <asp:ObjectDataSource ID="objAllUsers" runat="server" SelectMethod="GetAllUsers"
                TypeName="System.Web.Security.Membership"></asp:ObjectDataSource>
        </fieldset>
 --%>
    </div>
    <fieldset   style="width: auto; ">
        <legend>Users & Roles</legend>
        <table cellpadding="10" style= "">
            <tr>
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td valign="top">
                 DB Users
                    <asp:GridView ID="gvUsers" runat="server">
                    </asp:GridView>
                </td>            
                <td valign="top">
                 DB Roles
                    <asp:GridView ID="gvRoles" runat="server">
                    </asp:GridView>
                    <asp:ObjectDataSource ID="ObjGetAllRoles" runat="server" SelectMethod="GetAllRoles"
                        TypeName="System.Web.Security.Roles"></asp:ObjectDataSource>
                </td>
                <td>
                    &nbsp;
                </td>
                <td valign="top">
                    None Ansi InnerJoin
                    <asp:GridView ID="gvNonAnsiInnerJoin" runat="server">
                    </asp:GridView>
                </td>
                <td valign="top">
                    Inner Join
                    <asp:GridView ID="gvInnerJoin" runat="server">
                    </asp:GridView>
                </td>
                <td valign="top">
                    LeftOuter Join Role
                    <asp:GridView ID="gvLeftOuterJoin" runat="server">
                    </asp:GridView>
                </td>
                <td valign="top">
                    RightOuter Join Role
                    <asp:GridView ID="gvRightOuterJoin" runat="server">
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </fieldset>
    <br /><br /><br /><br />
    <p>All Reservations:</p>
    <asp:Button ID="btnExcel" runat="server" Text="Export to Excel" OnClick="btnExcel_Click" style="font-size: medium" />
    <asp:Button ID="btnDeleteAll" runat="server" Text="Delete All Reservations" style="font-size: medium"
        OnClientClick="return confirm('Are you sure you want to delete every reservation?')" OnClick="btnDeleteAll_Click"/>
    <br /><br />
    <div class="text-sm-center">
    <asp:GridView ID="gvReservations" runat="server" style="font-size: medium" CssClass="table-bordered"></asp:GridView>
    <br /><br />
    </div>
</asp:Content>
