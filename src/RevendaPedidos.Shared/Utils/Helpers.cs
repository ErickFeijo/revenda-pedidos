using System.Text.RegularExpressions;

namespace RevendaPedidos.Shared.Utils
{
    public static class Helpers
    {
        public static bool ValidarCnpj(string cnpj)
        {
            var regex = new Regex(@"^\d{14}$");
            return regex.IsMatch(cnpj?.Replace(".", "").Replace("/", "").Replace("-", "") ?? "");
        }

        public static bool ValidarEmail(string email)
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }
    }
}
