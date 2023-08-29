using System.Text.RegularExpressions;

namespace Estacionamento.Exceptions
{
    public class ExceptionsPlaca : Exception
    {
        private const string PlacaInvalida = "Placa inválida";
        private const string PlacaNaoEncontrada = "Placa não encontrada ou digitada incorretamente";

        public ExceptionsPlaca(string message = PlacaInvalida) : base(message)
        {
        }
        public static bool PlacaValida(string placa)
        {
            bool placaValida = Regex.IsMatch(placa, @"^[A-Z]{3}\d{4}$") || Regex.IsMatch(placa, @"^[A-Z]{3}\d[A-Z]\d{2}$");
            bool comprimentoValido = placa.Length == 7;
            return placaValida;
        }


        public static void ThrowIfInvalidPlaca(string placa, string message = PlacaNaoEncontrada)
        {
            if (!PlacaValida(placa))
            {
                if (placa.Length != 7)
                {
                    throw new ExceptionsPlaca("Placa deve ter 7 caracteres");
                }
                else
                {
                    throw new ExceptionsPlaca("Formato de placa inválido");
                }
            }

        }
    }
}
