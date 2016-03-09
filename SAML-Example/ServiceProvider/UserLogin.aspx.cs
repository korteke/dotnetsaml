using System;
using System.Security.Cryptography.X509Certificates;
using System.Web.Security;
using System.Xml;
using ComponentPro.Saml;
using ComponentPro.Saml.Binding;
using ComponentPro.Saml2;
using ComponentPro.Saml2.Binding;
using System.Security.Cryptography;
using System.Deployment.Internal.CodeSigning;

namespace SamlShibboleth.ServiceProvider
{
    public partial class UserLogin : System.Web.UI.Page
    {
        protected string NavigateUrl;

        private const string errorQueryParameter = "error";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnIdPLogin_Click(object sender, EventArgs e)
        {
            
            // Get the authentication request.
            Issuer issuer = new Issuer(Global.entityId);
            AuthnRequest authnRequest = Util.GetAuthnRequest(this);
            authnRequest.Issuer.NameIdentifier = Global.entityId;


            // Get SP Resource URL.
            string spResourceUrl = Util.GetAbsoluteUrl(this, FormsAuthentication.GetRedirectUrl("", false));
            // Create relay state.
            string relayState = Guid.NewGuid().ToString();
            // Save the SP Resource URL to the cache.
            SamlSettings.CacheProvider.Insert(relayState, spResourceUrl, new TimeSpan(1, 0, 0));

            switch (Global.SingleSignOnServiceBinding)
            {
                case SamlBinding.HttpRedirect:
                    X509Certificate2 x509Certificate = (X509Certificate2)Application[Global.SpCertKey];

                    // Send authentication request using HTTP Redirect.
                    System.Diagnostics.Debug.WriteLine("Sending redirect request to " + Global.SingleSignOnServiceURL);
                    authnRequest.Redirect(Response, Global.SingleSignOnServiceURL, relayState, x509Certificate.PrivateKey, SignatureAlgorithms.RsaSha256);
                    break;

                case SamlBinding.HttpPost:
                    // Send authentication request using HTTP POST form.
                    System.Diagnostics.Debug.WriteLine("Sending POST request to " + Global.SingleSignOnServiceURL);
                    authnRequest.SendHttpPost(Response, Global.SingleSignOnServiceURL, relayState);

                    // End the response.
                    Response.End();
                    break;

                case SamlBinding.HttpArtifact:
                    // Create a new http artifact.
                    string identificationUrl = Util.GetAbsoluteUrl(this, "~/");
                    Saml2ArtifactType0004 httpArtifact = new Saml2ArtifactType0004(SamlArtifact.GetSourceId(identificationUrl), SamlArtifact.GetHandle());

                    // Save the authentication request for subsequent sending using the artifact resolution protocol.
                    SamlSettings.CacheProvider.Insert(httpArtifact.ToString(), authnRequest.GetXml(), new TimeSpan(1, 0, 0));

                    // Send the artifact using HTTP POST form.
                    httpArtifact.SendHttpPost(Response.OutputStream, Global.SingleSignOnServiceURL, relayState);

                    // End the response.
                    Response.End();
                    break;

                default:
                    throw new ApplicationException("Invalid binding type");
            }
        }

    }
}