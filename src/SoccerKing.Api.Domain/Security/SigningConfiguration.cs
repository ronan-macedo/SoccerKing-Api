using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SoccerKing.Api.Domain.Security
{
    public class SigningConfiguration
    {
        public SecurityKey Key { get; set; }
        public SigningCredentials SigningCredentials { get; set; }

        public SigningConfiguration()
        {
            using (RSACryptoServiceProvider provider = new(2048))
                Key = new RsaSecurityKey(provider.ExportParameters(true));

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}
