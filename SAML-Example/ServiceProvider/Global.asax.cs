using System;
using System.IO;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.Web.Configuration;
using ComponentPro.Saml2;
using System.Security.Cryptography;
using System.Deployment.Internal.CodeSigning;
using System.Collections.Generic;
using ComponentPro.Saml.Diagnostics;

namespace SamlShibboleth.ServiceProvider
{
    public class Global : HttpApplication
    {
        //private const string SpCertFile = "SPKey.pfx";
        private const string SpCertFile = "SPCert256.pfx";
        private const string SpCertPass = "password";
        public const string SpCertKey = "spX509Certificate";

        private const string IdPCertFile = "IdpCertificate.cer";
        public const string IdPCertKey = "idpX509Certificate";

        //Azure
        //public const string entityId = "http://saml-sp2.azurewebsites.net";
        //Local Test
        public const string entityId = "http://saml-sp2-local.azurewebsites.net";

        public const string AttributeQueryUrl = "https://kvak.net/attribute-query/";

        public Dictionary<string, string> samlAttributes;

        /// <summary>
        /// Verifies the remote Secure Sockets Layer (SSL) certificate used for authentication.
        /// </summary>
        /// <param name="sender">An object that contains state information for this validation.</param>
        /// <param name="certificate">The certificate used to authenticate the remote party.</param>
        /// <param name="chain">The chain of certificate authorities associated with the remote certificate.</param>
        /// <param name="sslPolicyErrors">One or more errors associated with the remote certificate.</param>
        /// <returns>A System.Boolean value that determines whether the specified certificate is accepted for authentication.</returns>
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Loads the certificate file.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="fileName">The certificate file name.</param>
        /// <param name="password">The password for this certificate file.</param>
        private static X509Certificate2 LoadCertificate(string fileName, string password)
        {
            if (!File.Exists(fileName))
            {
                throw new ArgumentException("The certificate file " + fileName + " doesn't exist.");
            }

            try
            {
                return new X509Certificate2(fileName, password, X509KeyStorageFlags.MachineKeySet);
            }

            catch (Exception exception)
            {
                throw new ArgumentException("The certificate file " + fileName + " couldn't be loaded - " + exception.Message);
            }
        }

        void Application_Start(object sender, EventArgs e)
        {
            CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription),"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");
            ComponentPro.Saml.SamlSettings.LogWriter = 
                new ComponentPro.Saml.Diagnostics.FileLogWriter(AppDomain.CurrentDomain.BaseDirectory + "saml.log",
            ComponentPro.Saml.Diagnostics.LogLevel.Verbose, false);

            // In a test environment, trust all certificates.
            ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;

            // Load the SP certificate.
            string fileName = Path.Combine(HttpRuntime.AppDomainAppPath, SpCertFile);
            Application[SpCertKey] = LoadCertificate(fileName, SpCertPass);

            // Load the IdP certificate.
            fileName = Path.Combine(HttpRuntime.AppDomainAppPath, IdPCertFile);
            Application[IdPCertKey] = LoadCertificate(fileName, null);
        }

        void Session_Start(object sender, EventArgs e)
        {
            
        }


        #region Config

        public static SamlBinding SingleSignOnServiceBinding
        {
            get
            {
                return SamlBindingUri.UriToBinding(WebConfigurationManager.AppSettings["SingleSignOnServiceBinding"]);
            }
        }

        public static string SingleSignOnServiceURL
        {
            get
            {
                switch (SingleSignOnServiceBinding)
                {
                    case SamlBinding.HttpPost:
                        return SingleSignOnServiceUrlHttpPost;

                    case SamlBinding.HttpRedirect:
                        return SingleSignOnServiceUrlHttpRedirect;

                    case SamlBinding.HttpArtifact:
                        return SingleSignOnServiceUrlHttpArtifact;

                    default:
                        throw new ArgumentException("Unknown SSO Service Binding");
                }
            }
        }

        public static string SingleSignOnServiceUrlHttpPost
        {
            get
            {
                return WebConfigurationManager.AppSettings["SingleSignOnServiceUrlHttpPost"];
            }
        }

        public static string SingleSignOnServiceUrlHttpRedirect
        {
            get
            {
                return WebConfigurationManager.AppSettings["SingleSignOnServiceUrlHttpRedirect"];
            }
        }

        public static string SingleSignOnServiceUrlHttpArtifact
        {
            get
            {
                return WebConfigurationManager.AppSettings["SingleSignOnServiceUrlHttpArtifact"];
            }
        }

        public static string ArtifactServiceUrl
        {
            get
            {
                return WebConfigurationManager.AppSettings["ArtifactServiceUrl"];
            }
        }

        #endregion
    }
}