using System.Security.Cryptography;
using System.Text;

namespace ShootingClub.Application.Services.Cryptography
{
    public class PasswordEncripter
    {
        private readonly string _additionalkey;
        public PasswordEncripter(string additionalkey) => _additionalkey = additionalkey;

        public string Encrypt(string password)
        {
            var newPassword = $"{password}{_additionalkey}";
            var bytes = Encoding.UTF8.GetBytes(newPassword);
            var hashBytes = SHA512.HashData(bytes);

            return StringBytes(hashBytes);
        }

        private static string StringBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach(byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
}
