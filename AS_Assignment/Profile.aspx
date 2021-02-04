<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="AS_Assignment.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lbl_profileName" runat="server" Text="" Font-Size="X-Large"></asp:Label>
    <hr />
    <div>
        <h3>Change Password</h3>
        <div class="row" style="padding-top: 20px; padding-bottom: 20px;">
            <div class="col">
                <asp:Label ID="Label1" AssociatedControlID="tb_oldPwd" runat="server" Text="Label">Old password:</asp:Label>
                <asp:TextBox ID="tb_oldPwd" class="form-control" runat="server" TextMode="Password"></asp:TextBox>
            </div>
        </div>
        <div class="row" style="padding-top: 20px; padding-bottom: 20px;">
            <div class="col">
                <asp:Label ID="Label2" AssociatedControlID="tb_newPwd" runat="server" Text="Label">New password:</asp:Label>
                <asp:TextBox ID="tb_newPwd" class="form-control" runat="server" TextMode="Password"></asp:TextBox>
            </div>
        </div>
        <div class="row" style="padding-top: 20px; padding-bottom: 20px;">
            <div class="col">
                <asp:Label ID="Label3" AssociatedControlID="tb_cfmNewPwd" runat="server" Text="Label">Confirm New password:</asp:Label>
                <asp:TextBox ID="tb_cfmNewPwd" class="form-control" runat="server" TextMode="Password"></asp:TextBox>
            </div>
        </div>
        <asp:Label ID="lbl_statusInfo" runat="server" Text="" Font-Size="X-Large"></asp:Label>
        <asp:Button ID="btn_changePwd" class="btn btn-primary btn-block" runat="server" Text="Change Password" OnClick="btn_changePwd_Click" />
    </div>

</asp:Content>
