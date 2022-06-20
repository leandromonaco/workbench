using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace ServiceName.Core.Common.Security
{
    public static class SecurityHelper
    {
        public static RsaSecurityKey GetRsaSecurityKey(string publicKey)
        {
            var publicKeyBase64 = Convert.FromBase64String(publicKey);
            var asymmetricKeyParameter = PublicKeyFactory.CreateKey(publicKeyBase64);
            var rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
            var rsaParameters = new RSAParameters
            {
                Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned(),
                Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned()
            };
            var rsaSecurityKey = new RsaSecurityKey(rsaParameters);
            return rsaSecurityKey;
        }        
    }
}
