using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STG.Component
{
    public class AuthUserJWTOptions
    {
        public const string ISSUER = "STGONLINE"; // издатель токена
        public const string AUDIENCE = "User"; // потребитель токена
        const string KEY = "fJHs92FGdop43y7nbedske*(js3GFl3mfvj4ly9";   // ключ для шифрации
        //public const int LIFETIME = 1; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
