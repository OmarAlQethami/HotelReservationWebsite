<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" 
    Inherits="HotelWebsiteProject.Account.login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <div>
    
          <h4>
        <table class="style1">
            <tr><td colspan="2"></td></tr>
            <tr>
                <td class="style2">
                    <strong>Login</strong></td>
                <td>
                    Welcome</td>
            </tr>
            <tr>
                <td class="style2">
                    User Name</td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" autocomplete="off" style="font-size: medium"></asp:TextBox>
  
                </td>
            </tr>
            <tr>
                <td class="style2">
                    Password</td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" style="font-size: medium"></asp:TextBox> 
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnLogin" runat="server" ForeColor="#0000FF" style="font-weight: bold; font-size: medium;" onclick="btnLogin_Click" 
                        Text="Login" />
<%--                    <asp:Button ID="btnCreateAdmin" runat="server" OnClick="btnCreateAdmin_Click" Text="Admin" Visible="False" />--%>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" PostBackUrl="~/default.aspx" ForeColor="#FF3300" style="font-weight: bold; font-size: medium;"/>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblOutput" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
              </h4>
    
    </div>
</asp:Content>
