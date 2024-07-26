<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="reservation.aspx.cs" Inherits="HotelWebsiteProject.pages.reservation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <h3>
        <asp:Label ID="lblGreeting" runat="server" Text="Hello"></asp:Label>
    </h3>
    <br /><br /><br />
    <h4>
        <p>What type of room do you want?</p>
        <asp:RadioButtonList ID="rblRoomType" runat="server" DataSourceID="SqlDataSource1" DataTextField="roomTypeName" DataValueField="roomTypeName" AutoPostBack="True" OnSelectedIndexChanged="rblRoomType_SelectedIndexChanged"></asp:RadioButtonList>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HotelWebsiteProjectConStr %>" SelectCommand="SELECT [roomTypeName] FROM [roomType]"></asp:SqlDataSource>
        <br /><br />

        <p>Choose your room:</p>
        <asp:DropDownList ID="ddlRooms" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRooms_SelectedIndexChanged"></asp:DropDownList>
        <br /><br />

        <p>Choose Check-in Date:</p>
        <asp:TextBox ID="txtCheckIn" runat="server" TextMode="Date" AutoPostBack="True" OnTextChanged="DateChanged"></asp:TextBox>
        <br /><br />

        <p>Choose Check-out Date:</p>
        <asp:TextBox ID="txtCheckOut" runat="server" TextMode="Date" AutoPostBack="True" OnTextChanged="DateChanged"></asp:TextBox>
        <br /><br />

        <p>Choose your services:</p>
        <asp:Repeater ID="rptServices" runat="server" OnItemDataBound="rptServices_ItemDataBound">
            <ItemTemplate>
                <asp:CheckBoxList ID="cblServices" runat="server"></asp:CheckBoxList>
            </ItemTemplate>
        </asp:Repeater>
        <br /><br />

        <asp:Label ID="lblPrice" runat="server" Text="" Visible="false"></asp:Label>
        <br /><br />

        <asp:Label ID="lblTotalPrice" runat="server" Text="" Visible="false"></asp:Label>
        <br /><br />

        <asp:Label ID="lblOutput" runat="server" Text=""></asp:Label>
        <br />

        <asp:Button ID="btnConfirm" runat="server" Text="Confirm Reservation" OnClick="btnConfirm_Click" />
    </h4>
</asp:Content>
