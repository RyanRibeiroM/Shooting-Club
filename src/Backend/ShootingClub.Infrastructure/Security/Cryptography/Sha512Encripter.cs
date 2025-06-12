using ShootingClub.Domain.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace ShootingClub.Infrastructure.Security.Cryptography
{
    internal class Sha512Encripter : ISenhaEncripter
    {
        private readonly string _additionalkey;
        public Sha512Encripter(string additionalkey) => _additionalkey = additionalkey;

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
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
}
