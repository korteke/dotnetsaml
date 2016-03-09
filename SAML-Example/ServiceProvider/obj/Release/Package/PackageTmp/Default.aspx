<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SamlShibboleth.ServiceProvider.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SP Demo</title>
    <link rel="Stylesheet" href="Css/Styles.css" />
</head>
<body>
    <form id="myform" runat="server">
        <div class="templatecontent">
            <div class="header">
                <a href="https://kvak.net">
                    <img src="Css/kvak3.png" /></a>
            </div>
            <h4>
                <b>ServiceProvider</b></h4>
            <br />
            <br />
            <div class="content">
                <table border="0" cellspacing="0" cellpadding="3">
                    <tr>
                        <td style="white-space: nowrap">
                            <b>Logged in as:</b></td>
                        <td width="100%">
                            <%=Context.User.Identity.Name%><br />
                        </td>
                        </tr>
                        <tr>
                        <b>Attributes:</b><br />

                        <asp:BulletedList id="blAttrs" 
                          BulletStyle="Disc"
                          DisplayMode="Text" 
                          runat="server">
                        </asp:BulletedList>

                        <b>Roles:</b><br />

                        <asp:BulletedList id="blRoles" 
                          BulletStyle="Disc"
                          DisplayMode="Text" 
                          runat="server">
                        </asp:BulletedList>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button runat="server" CssClass="button" ID="btnLogout" Text="Logout" OnClick="btnLogout_Click" /></td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
