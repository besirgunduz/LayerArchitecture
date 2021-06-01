using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class TokenOptions
    {
        public string Audience { get; set; } //kullanıcı kitlesi
        public string Issuer { get; set; } //imzalayan
        public int AccessTokenExpiration { get; set; } //dakika cinsinden
        public string SecurityKey { get; set; } //güvenlik anahtarı
    }
}
