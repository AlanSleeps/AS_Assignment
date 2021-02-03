<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="AS_Assignment.Register" ValidateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="margin: 0 auto; padding: 20px; background-color: #f2f2f2;">
        <h1 style="text-align: center;">Registration</h1>
        <div class="row" style="padding-top: 20px; padding-bottom: 20px;">
            <div class="col-md-6">
                <asp:Label ID="Label1" AssociatedControlID="tb_fName" runat="server" Text="Label">First Name:</asp:Label>
                <asp:TextBox ID="tb_fName" class="form-control" runat="server" Required="true"></asp:TextBox>
            </div>
            <div class="col-md-6">
                <asp:Label ID="Label2" AssociatedControlID="tb_lName" runat="server" Text="Label">Last Name:</asp:Label>
                <asp:TextBox ID="tb_lName" class="form-control" runat="server" Required="true"></asp:TextBox>
            </div>
        </div>
        <div class="row" style="padding-top: 20px; padding-bottom: 20px;">
            <div class="col-md-12">
                <asp:Label ID="Label4" AssociatedControlID="tb_dob" runat="server" Text="Label">Date of Birth</asp:Label>
                <asp:TextBox ID="tb_dob" class="form-control" runat="server" TextMode="Date" Required="true"></asp:TextBox>
                <asp:Label ID="lbl_dobCheck" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div class="row" style="padding-top: 20px; padding-bottom: 20px;">
            <div class="col-md-12">
                <asp:Label ID="Label3" AssociatedControlID="tb_cCardNum" runat="server" Text="Label">Credit Card Number:</asp:Label>
                <asp:TextBox ID="tb_cCardNum" class="form-control" runat="server" TextMode="Number" Required="true"></asp:TextBox>
                <asp:Label ID="lbl_cCardNumCheck" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div class="row" style="padding-top: 20px; padding-bottom: 20px;">
            <div class="col-md-12">
                <asp:Label ID="Label5" AssociatedControlID="tb_email" runat="server" Text="Label">Email:</asp:Label>
                <asp:TextBox ID="tb_email" class="form-control" runat="server" TextMode="Email" Required="true"></asp:TextBox>
                <asp:Label ID="lbl_forEmailCheck" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div class="row" style="padding-top: 20px; padding-bottom: 20px;">
            <div class="col-md-12">
                <asp:Label ID="Label6" AssociatedControlID="tb_password" runat="server" Text="Label">Password:</asp:Label>
                <asp:TextBox ID="tb_password" class="form-control" runat="server" onkeyup="javascript:checkStrength()" TextMode="Password" Required="true"></asp:TextBox>
                <asp:Table runat="server" style="margin-top: 5px; width: 45%; text-align: center; color:white;">
                    <asp:TableRow>
                        <asp:TableCell id="weakPwd" style="width: 15%; border: 2px solid white;"></asp:TableCell>
                        <asp:TableCell id="mediumPwd" style="width: 15%; border: 2px solid white;"></asp:TableCell>
                        <asp:TableCell id="strongPwd" style="width: 15%; border: 2px solid white;"></asp:TableCell>
                    </asp:TableRow>
                 </asp:Table>
                <asp:Label ID="lbl_forPwdStrength" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div class="row" style="padding-top: 20px; padding-bottom: 20px;">
            <div class="col-md-12">
                <asp:Label ID="Label7" AssociatedControlID="tb_cfmPwd" runat="server" Text="Label">Confirm Password:</asp:Label>
                <asp:TextBox ID="tb_cfmPwd" class="form-control" runat="server" onkeyup="javascript:checkPwd()" TextMode="Password"></asp:TextBox>
                <asp:Label ID="lbl_cfmPwd" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <asp:Button ID="btn_submit" class="btn btn-primary btn-block" runat="server" Text="Submit" OnClick="btn_submit_Click" />
        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
    </div>
    <script type="text/javascript">
        function checkStrength() {
            let str = document.getElementById("<%=tb_password.ClientID%>").value;

            function weak() {
                document.getElementById("<%=weakPwd.ClientID%>").innerHTML = "Weak";
                document.getElementById("<%=weakPwd.ClientID%>").style.backgroundColor = "red";
                document.getElementById("<%=mediumPwd.ClientID%>").innerHTML = "";
                document.getElementById("<%=mediumPwd.ClientID%>").style.backgroundColor = "";
                document.getElementById("<%=strongPwd.ClientID%>").innerHTML = "";
                document.getElementById("<%=strongPwd.ClientID%>").style.backgroundColor = "";
            }

            if (str.length < 8) {
                document.getElementById("<%=lbl_forPwdStrength.ClientID%>").innerHTML = "Password length MUST be at least 8 Characters";
                weak();
                return;
            } else if (str.search(/[0-9]/) == -1) {
                document.getElementById("<%=lbl_forPwdStrength.ClientID%>").innerHTML = "Password MUST have at least 1 number";
                weak();
                return;
            } else if (str.search(/[a-z]/) == -1) {
                document.getElementById("<%=lbl_forPwdStrength.ClientID%>").innerHTML = "Password MUST have at least 1 character";
                weak();
                return;
            } else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("<%=lbl_forPwdStrength.ClientID%>").innerHTML = "Password MUST have at least 1 uppercase character";
                weak();
                return;
            } else if (str.search(/[^a-zA-Z0-9]/) == -1) {
                document.getElementById("<%=lbl_forPwdStrength.ClientID%>").innerHTML = "Password SHOULD contain at least 1 special character";
                document.getElementById("<%=weakPwd.ClientID%>").innerHTML = "";
                document.getElementById("<%=weakPwd.ClientID%>").style.backgroundColor = "blue";
                document.getElementById("<%=mediumPwd.ClientID%>").innerHTML = "Medium";
                document.getElementById("<%=mediumPwd.ClientID%>").style.backgroundColor = "blue";
                document.getElementById("<%=strongPwd.ClientID%>").innerHTML = "";
                document.getElementById("<%=strongPwd.ClientID%>").style.backgroundColor = "";
                return;
            }
            document.getElementById("<%=lbl_forPwdStrength.ClientID%>").innerHTML = "";
            document.getElementById("<%=weakPwd.ClientID%>").innerHTML = "";
            document.getElementById("<%=weakPwd.ClientID%>").style.backgroundColor = "green";
            document.getElementById("<%=mediumPwd.ClientID%>").innerHTML = "";
            document.getElementById("<%=mediumPwd.ClientID%>").style.backgroundColor = "green";
            document.getElementById("<%=strongPwd.ClientID%>").innerHTML = "Strong";
            document.getElementById("<%=strongPwd.ClientID%>").style.backgroundColor = "green";
        }

        function checkPwd() {
            let pwd = document.getElementById("<%=tb_password.ClientID%>").value;
            let retypePwd = document.getElementById("<%=tb_cfmPwd.ClientID%>").value

            if (pwd != retypePwd) {
                document.getElementById("<%=lbl_cfmPwd.ClientID%>").innerHTML = "Password is not the same!";
                return;
            }
            document.getElementById("<%=lbl_cfmPwd.ClientID%>").innerHTML = "";
        }
    </script>
</asp:Content>
