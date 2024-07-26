<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="HotelWebsiteProject.pages.dashboard"  EnableEventValidation="false" ValidateRequest="false"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h4>
        <p><asp:Label ID="lblConfirmation" runat="server" Text="Your reservation is successful!"></asp:Label></p>
        <p>Down below you'll find your reservation details...</p>

        <br /><br />

        <h6 class="text-sm-center"><asp:GridView ID="gvReservation" runat="server" Height="130px" GridLines="None" CssClass="table-bordered" style="font-size: small"></asp:GridView></h6>
        <br /><br /><br />

        <table class="nav-justified">
            <tr>
                <td class="text-right" style="width: 140px; font-size: medium; height: 28px;">Reservation Id:</td>
                <td style="width: 300px; height: 28px;">
                    <asp:TextBox ID="txtReservationId" runat="server" Width="200px" ReadOnly="True" style="font-size: medium" autocomplete="off"></asp:TextBox>
                </td>
                <td style="height: 28px">

        <asp:Button ID="btnSelect" runat="server" Text="Select" OnClick="btnSelect_Click" style="font-size: medium" />
                </td>
            </tr>
            <tr>
                <td class="text-right" style="width: 140px; font-size: medium;">First Name:</td>
                <td style="width: 300px">
                    <asp:TextBox ID="txtFName" runat="server" Width="200px" style="font-size: medium" autocomplete="off"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="text-right" style="height: 20px; width: 140px; font-size: medium;">Last Name:</td>
                <td style="height: 20px; width: 300px">
                    <asp:TextBox ID="txtLName" runat="server" Width="200px" style="font-size: medium" autocomplete="off"></asp:TextBox>
                </td>
                <td style="height: 20px">
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" style="font-size: medium" />
                </td>
            </tr>
            <tr>
                <td class="text-right" style="width: 140px; font-size: medium;">Phone Number:</td>
                <td style="width: 300px">
                    <asp:TextBox ID="txtPhoneNumber" runat="server" Width="200px" style="font-size: medium" autocomplete="off"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="text-right" style="height: 26px; width: 140px; font-size: medium;">Email:</td>
                <td style="height: 26px; width: 300px">
                    <asp:TextBox ID="txtEmail" runat="server" Width="200px" style="font-size: medium" autocomplete="off"></asp:TextBox>
                </td>
                <td style="height: 26px">
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" style="font-size: medium"
                    onClientClick ="return confirm('Do you want to delete your reservation?')"/>
                </td>
            </tr>
            <tr>
                <td class="text-right" style="width: 140px; height: 22px; font-size: medium;">Room Number:</td>
                <td style="width: 300px; height: 22px;">
                    <asp:TextBox ID="txtRoomNumber" runat="server" Width="200px" style="font-size: medium" ReadOnly="True" autocomplete="off"></asp:TextBox>
                </td>
                <td style="height: 22px"></td>
            </tr>
            <tr>
                <td class="text-right" style="width: 140px; font-size: medium;">Check-in Date:</td>
                <td style="width: 300px">
                    <asp:TextBox ID="txtCheckInDate" runat="server" Width="200px" style="font-size: medium" ReadOnly="True" autocomplete="off"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnIssue" runat="server" OnClick="btnIssue_Click" style="font-size: medium" Text="Issue Receipt" />
                    <asp:Button ID="btnEmail" runat="server" Text="Send receipt to Email" OnClick="btnEmail_Click" style="font-size: medium" />
                    
                </td>
            </tr>
            <tr>
                <td class="text-right" style="width: 140px; font-size: medium;">Check-out Date:</td>
                <td style="width: 300px">
                    <asp:TextBox ID="txtCheckOutDate" runat="server" Width="200px" style="font-size: medium" ReadOnly="True" autocomplete="off"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="text-right" style="width: 140px; font-size: medium;">Services:</td>
                <td style="width: 300px">
                    <asp:TextBox ID="txtServices" runat="server" Width="200px" style="font-size: medium" ReadOnly="True" autocomplete="off"></asp:TextBox>
                <td>
                    <asp:Button ID="btnExcel" runat="server" Text="Export to Excel" OnClick="btnExcel_Click" style="font-size: medium" />
                </td>
            </tr>
            <tr>
                <td class="text-right" style="width: 140px; font-size: medium;">Total Price:</td>
                <td style="width: 300px">
                    <asp:TextBox ID="txtTotalPrice" runat="server" Width="200px" style="font-size: medium" ReadOnly="True" autocomplete="off"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="text-right" style="width: 140px; font-size: medium;">Reservation Time:</td>
                <td style="width: 300px">
                    <asp:TextBox ID="txtReservationTime" runat="server" Width="200px" style="font-size: medium" ReadOnly="True" autocomplete="off"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="text-sm-center" style="width: 140px">&nbsp;</td>
                <td style="width: 300px">&nbsp;</td>
                <td>
                    <asp:Label ID="lblOutput" runat="server" style="font-size: medium"></asp:Label>
                </td>
            </tr>
    </table>
    </h4>
</asp:Content>
