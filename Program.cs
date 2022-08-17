using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TesteConsumoDeAPI
{
    class Program
    {
        static void Main(string[] args)
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
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://viacep.com.br/ws/" + cepString + "/json/");
                request.AllowAutoRedirect = false;
                HttpWebResponse ChecaServidor = (HttpWebResponse)request.GetResponse();

                if (ChecaServidor.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine("Servidor indisponível");
                    return; // Sai da rotina
                }

                using (Stream webStream = ChecaServidor.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            string response = responseReader.ReadToEnd();
                            response = Regex.Replace(response, "[{},]", string.Empty);
                            response = response.Replace("\"", "");

                            String[] substrings = response.Split('\n');

                            int cont = 0;
                            foreach (var substring in substrings)
                            {
                                if (cont == 1)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    if (valor[0] == "  erro")
                                    {
                                        Console.WriteLine("CEP não encontrado");
                                        return;
                                    }
                                }

                                //Logradouro
                                if (cont == 2)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    Console.WriteLine($"\nLogradouro:{valor[1].ToString()}");
                                }

                                //Complemento
                                if (cont == 3)
                                {
                                    string[] valor = substring.Split(":".ToCharArray()); 
                                    Console.WriteLine($"Complemento:{valor[1].ToString()}");
                                }

                                //Bairro
                                if (cont == 4)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    Console.WriteLine($"Bairro:{valor[1].ToString()}");
                                }

                                //Localidade (Cidade)
                                if (cont == 5)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    Console.WriteLine($"Cidade:{valor[1].ToString()}");
                                }

                                //Estado (UF)
                                if (cont == 6)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    Console.WriteLine($"Estado:{valor[1].ToString()}");
                                }
                                cont++;

                                //Créditos: https://www.blogson.com.br/busca-automatica-de-cep-em-c-windows-forms/
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Finalizando Programa");
            }            
        }
    }
}
    
