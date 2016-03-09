using System;

namespace SamlShibboleth.ServiceProvider
{
    public partial class AssertionService : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ComponentPro.Saml2.Response samlResponse = null;
                string relayState = null;

                // Get and process the SAML response.
                System.Diagnostics.Debug.WriteLine("GOT SAML response");
                Util.ProcessResponse(this, out samlResponse, out relayState);

                // If the SAML response indicates success.
                if (samlResponse.IsSuccess())
                {
                    System.Diagnostics.Debug.WriteLine("IsSuccess OK");
                    Util.SamlSuccessRedirect(this, samlResponse, relayState);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("IsSuccess NOK");
                    Util.SamlErrorRedirect(this, samlResponse);
                }
            }

            catch (Exception exception)
            {
                Trace.Write("ServiceProvider", "An Error occurred", exception);
            }
        }
    }
}