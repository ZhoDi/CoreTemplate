using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.Domain.APIModel.User
{
    public class TokenAuthConfiguration
    {
        public SymmetricSecurityKey SecurityKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public SigningCredentials SigningCredentials { get; set; }

        public TimeSpan Expiration { get; set; }
    }
}
