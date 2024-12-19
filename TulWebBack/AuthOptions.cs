using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TulWebBack
{
    public class AuthOptions
    {
        public const string ISSUER = "VanyaInc"; // издатель токена
        public const string AUDIENCE = "Customers"; // потребитель токена
        const string KEY = "fgfhasfjdsafkhgdskjgfdshgfjdkshghjkdsfghdfskjhgjhkd!123";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
