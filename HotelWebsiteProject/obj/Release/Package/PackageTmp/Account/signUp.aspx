<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="signUp.aspx.cs" Inherits="HotelWebsiteProject.Account.signUp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>
    <h4>
        <table class="style1">
            <tr><td colspan="2"></td></tr>
            <tr>
                <td class="style2">
                    <strong>Sign Up</strong></td>
                <td>
                    Welcome</td>
            </tr>
            <tr>
                <td class="style2">
                    First Name:</td>
                <td>
                    <asp:TextBox ID="txtFName" runat="server" autocomplete="off"></asp:TextBox>
  
                </td>
            </tr>
            <tr>
                <td class="style2">
                    Last Name:</td>
                <td>
                    <asp:TextBox ID="txtLName" runat="server" autocomplete="off"></asp:TextBox>
  
                </td>
            </tr>
            <tr>
                <td class="style2">
                    Phone Number: </td>
                <td>
                    <asp:TextBox ID="txtPhoneNumber" runat="server" autocomplete="off"></asp:TextBox>
  
                </td>
            </tr>
            <tr>
                <td class="style2">
                    Email:</td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" autocomplete="off"></asp:TextBox>
                    
  
                </td>
            </tr>
            <tr>
                <td class="style2">
                    Username:</td>
                <td>
                    <asp:TextBox ID="txtUsername" runat="server" autocomplete="off"></asp:TextBox>
  
                </td>
            </tr>
            <tr>
                <td class="style2">
                    Password:</td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox> 
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
                    <asp:Button ID="btnSignUp" runat="server" ForeColor="#0000FF" style="font-weight: bold" onclick="btnSignUp_Click" 
                        Text="Sign Up" OnClientClick="return confirm('Are all your information correct?')"/>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnBack" runat="server" Text="Back" PostBackUrl="~/default.aspx" ForeColor="#000000" style="font-weight: bold"/>
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
        <br />
        
    
    </div>

</asp:Content>
