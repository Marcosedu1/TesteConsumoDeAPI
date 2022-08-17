using System.Text.Json;
using System.Net.Http.Json;

namespace TesteConsumoDeAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Digite um CEP Válido");
            string cepString = Console.ReadLine();
            cepString = cepString.Replace(".", "").Replace("-", "").Trim();

            while (cepString.Length != 8)
            {
                Console.WriteLine("Digite um CEP Válido");
                cepString = Console.ReadLine();
                if (cepString == "")
                {
                    break;
                }
            }
            if (cepString.Length == 8)
            {
                using HttpClient client = new()
                {
                    BaseAddress = new Uri("https://viacep.com.br/ws/")
                };

                ViaCep? cep = await client.GetFromJsonAsync<ViaCep>(cepString + "/json/");

                HttpResponseMessage response = await client.PostAsJsonAsync("CEP", cep);

                Console.WriteLine($"CEP: {cep?.cep}");
                Console.WriteLine($"Logradouro: {cep?.logradouro}");
                Console.WriteLine($"Complemento: {cep?.complemento}");
                Console.WriteLine($"Localidade: {cep?.localidade}");
                Console.WriteLine($"UF: {cep?.uf}");
                Console.WriteLine($"IBGE: {cep?.ibge}");
                Console.WriteLine($"GIA: {cep?.gia}");
                Console.WriteLine($"DDD: {cep?.ddd}");
                Console.WriteLine($"SIAFI: {cep?.siafi}");

                //https://docs.microsoft.com/pt-br/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
            }
            else
            {
                Console.WriteLine("Finalizando Programa");
            }
        }
    }
}
    
