using System.Net;
#if !NET20
using System.Net.Http;
#endif
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Redmine.Api
{
    #if NETFULL
    /// <summary>
    /// 
    /// </summary>
    public sealed class TrustAllCertificatePolicy : ICertificatePolicy
    {
        /// <summary>
        /// 
        /// </summary>
        public TrustAllCertificatePolicy() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="cert"></param>
        /// <param name="req"></param>
        /// <param name="problem"></param>
        /// <returns></returns>
        public bool CheckValidationResult(ServicePoint sp, X509Certificate cert,WebRequest req, int problem)
        {
            return true;
        }
    }
    #elif NETSTANDARD
    /// <summary>
    /// 
    /// </summary>
    public sealed class TrustAllCertificatePolicy 
    {
        /// <summary>
        /// 
        /// </summary>
        public TrustAllCertificatePolicy() {}

   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <param name="x509Certificate2"></param>
        /// <param name="x509Chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        public bool CheckValidationResult(HttpRequestMessage requestMessage, X509Certificate2 x509Certificate2, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
    #endif
}