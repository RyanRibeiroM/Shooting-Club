using System.Text.RegularExpressions;

namespace ShootingClub.Application.Utils
{
    public static class CpfFormatter
    {
        public static string Format(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return cpf;

            if (Regex.IsMatch(cpf, @"^\d{3}\.\d{3}\.\d{3}-\d{2}$"))
                return cpf;

            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            if (cpf.Length != 11)
                return cpf;

            return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
        }
    }
}
