using ComponentPro.Saml2;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace SamlShibboleth.ServiceProvider
{
    public partial class Default : System.Web.UI.Page
    {
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            /*
            LogoutRequest logoutRequest = new LogoutRequest();
            logoutRequest.Issuer = new Issuer((Global.entityId));
            logoutRequest.NameId = new NameId(Context.User.Identity.Name);

            // Send the logout request to the IdP over HTTP redirect.
            string logoutUrl = WebConfigurationManager.AppSettings["SingleSignOnServiceUrlHttpRedirect"];
            X509Certificate2 x509Certificate = (X509Certificate2)Application[Global.SpCertKey];

            // Logout locally.
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Abandon();

            logoutRequest.Redirect(Response, logoutUrl, null, x509Certificate.PrivateKey);
            */
            FormsAuthentication.SignOut();
            
            Response.Redirect("UserLogin.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Count > 0 && HttpContext.Current.Session["samlAttributes"] != null)
            {
                
                Dictionary<string, string> dict = (Dictionary<string, string>)HttpContext.Current.Session["samlAttributes"];
                foreach (KeyValuePair<string, string> entry in dict)
                {
                    // do something with entry.Value or entry.Key
                    ListItem li = new ListItem();
                    li.Value = "";
                    li.Text = entry.Key + " - " + entry.Value;
                    blAttrs.Items.Add(li);
                }

                if (HttpContext.Current.Session["samlroles"] != null)
                {
                    Dictionary<string, string> roles = (Dictionary<string, string>)HttpContext.Current.Session["samlRoles"];
                    foreach (KeyValuePair<string, string> entry in dict)
                    {
                        // do something with entry.Value or entry.Key
                        ListItem li = new ListItem();
                        li.Value = "";
                        li.Text = entry.Key + " - " + entry.Value;
                        blRoles.Items.Add(li);
                    }
                }
            }
        }
    }
}