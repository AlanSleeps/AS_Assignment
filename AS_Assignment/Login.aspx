<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AS_Assignment.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="margin: 0 auto; padding: 20px; background-color: #f2f2f2;">
        <h1 style="text-align: center;">Login</h1>
        <div class="row" style="padding-top: 20px; padding-bottom: 20px;">
            <div class="col">
                <asp:Label ID="Label1" AssociatedControlID="tb_email" runat="server" Text="Label">Email:</asp:Label>
                <asp:TextBox ID="tb_email" class="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row" style="padding-top: 20px; padding-bottom: 20px;">
            <div class="col">
                <asp:Label ID="Label2" AssociatedControlID="tb_password" runat="server" Text="Label">Password:</asp:Label>
                <asp:TextBox ID="tb_password" class="form-control" runat="server" TextMode="Password"></asp:TextBox>
            </div>
        </div>
        <asp:Label ID="lbl_debug" runat="server" Text=""></asp:Label>
        <asp:Label ID="lbl_cCardNo" runat="server" Text=""></asp:Label>
        <asp:Button ID="btn_login" class="btn btn-primary btn-block" runat="server" Text="Login" OnClick="btn_login_Click" />
        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
    </div>
</asp:Content>
