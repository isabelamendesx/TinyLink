using System.Text;

namespace GeradorSenha
{
    internal class Program
    {
        private const string LetrasMaiusculas = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LetrasMinusculas = "abcdefghijklmnopqrstuvwxyz";
        private const string Numeros = "1234567890";
        private const string CaracteresEspeciais = "@#$%^&*()-_+=!";
        static void Main(string[] args)
        {
        }

        static string GerarSenha(RequisicaoSenha requisicaoSenha)
        {
            var senha = new StringBuilder();
            var random = new Random();

            var chars = 

            var randomStr = new string(Enumerable.Repeat(chars, 4)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }

        public static Dictionary<string, bool> ObterPropriedades(RequisicaoSenha requisicao)
        {
            return typeof(RequisicaoSenha)
                .GetProperties()
                .Where(prop => prop.PropertyType == typeof(bool))
                .ToDictionary(prop => prop.Name, prop => (bool)prop.GetValue(requisicao));
        }
    }
}
