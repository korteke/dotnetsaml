<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserLogin.aspx.cs" Inherits="SamlShibboleth.ServiceProvider.UserLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>Service Provider</title>
    <link rel="Stylesheet" href="Css/Styles.css" />
</head>
<body>
    <form id="myform" runat="server">
        <div class="templatecontent">
            <div class="header">
                <a href="https://kvak.net">
                    <img src="Css/kvak3.png" /></a>
            </div>
                <b>Login at the Identity Provider</b>
            <asp:Button runat="server" CssClass="okbutton" ID="btnIdPLogin" Text="Login" OnClick="btnIdPLogin_Click" />
            
        </div>
    </form>
</body>
</html>
