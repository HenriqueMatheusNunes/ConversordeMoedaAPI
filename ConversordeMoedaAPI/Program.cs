using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConversordeMoedaAPI
{
    internal class Program
    {
        enum Opcao
        {
            ConverterMoeda = 1,
            Sair = 2
        }

        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            bool rodarMenu = true;

            while (rodarMenu)
            {
                Console.WriteLine("\nSelecione uma das opções abaixo:");
                Console.WriteLine("1 - Converter Moeda\n2 - Sair");

                if (int.TryParse(Console.ReadLine(), out int index))
                {
                    Opcao opcaoSelecionada = (Opcao)index;

                    switch (opcaoSelecionada)
                    {
                        case Opcao.ConverterMoeda:

                            // 1. Leitura e conversão segura do valor numérico
                            Console.WriteLine("\nDigite um valor de entrada:");
                            string entradaValor = Console.ReadLine();

                            if (!decimal.TryParse(entradaValor, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal valorOrigem))
                            {
                                Console.WriteLine("Valor numérico inválido!");
                                break;
                            }

                            // 2. Validação da moeda de origem
                            string moedaOrigem = "";
                            bool moedaOrigemValida = false;

                            while (!moedaOrigemValida)
                            {
                                Console.WriteLine("Informe a moeda de origem (USD, BRL, EUR):");
                                moedaOrigem = Console.ReadLine().ToUpper().Trim();

                                if (moedaOrigem == "USD" || moedaOrigem == "BRL" || moedaOrigem == "EUR")
                                {
                                    moedaOrigemValida = true;
                                }
                                else
                                {
                                    Console.WriteLine("Moeda inválida! Tente novamente.");
                                }
                            }

                            // 3. Validação da moeda de destino
                            string moedaDestino = "";
                            bool moedaDestinoValida = false;

                            while (!moedaDestinoValida)
                            {
                                Console.WriteLine("Esse dinheiro vai se transformar em qual moeda? (USD, BRL, EUR):");
                                moedaDestino = Console.ReadLine().ToUpper().Trim();

                                if (moedaDestino == "USD" || moedaDestino == "BRL" || moedaDestino == "EUR")
                                {
                                    if (moedaDestino == moedaOrigem)
                                    {
                                        Console.WriteLine("A moeda de destino deve ser diferente da moeda de origem!");
                                    }
                                    else
                                    {
                                        moedaDestinoValida = true;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Moeda inválida! Tente novamente.");
                                }
                            }

                            // 4. Regra de Negócio: Tratamento da limitação da AwesomeAPI para BRL na origem
                            string rotaApi;
                            string parMoedas;
                            bool eOperacaoInversa = (moedaOrigem == "BRL");

                            if (eOperacaoInversa)
                            {
                                // A API não aceita BRL-USD ou BRL-EUR. Invertemos a busca para a rota que existe.
                                rotaApi = $"{moedaDestino}-{moedaOrigem}";
                                parMoedas = moedaDestino + moedaOrigem;
                            }
                            else
                            {
                                rotaApi = $"{moedaOrigem}-{moedaDestino}";
                                parMoedas = moedaOrigem + moedaDestino;
                            }

                            // 5. Requisição HTTP
                            string endpointUrl = $"https://economia.awesomeapi.com.br/json/last/{rotaApi}";
                            var respostaHttp = await httpClient.GetAsync(endpointUrl);

                            if (respostaHttp.IsSuccessStatusCode)
                            {
                                var json = await respostaHttp.Content.ReadAsStringAsync();
                                var cotacoesPorPar = JsonSerializer.Deserialize<Dictionary<string, Informacoes>>(json);

                                if (cotacoesPorPar != null && cotacoesPorPar.TryGetValue(parMoedas, out var dadosCotacao))
                                {
                                    Console.WriteLine("\n--- Dados da Cotação ---");
                                    Console.WriteLine($"Cotação (Bid): {dadosCotacao.Bid}");
                                    Console.WriteLine($"Máxima (High): {dadosCotacao.High}");
                                    Console.WriteLine($"Mínima (Low):  {dadosCotacao.Low}");
                                    Console.WriteLine($"Moedas:        {dadosCotacao.Code} -> {dadosCotacao.CodeIn}");

                                    decimal valorBid = decimal.Parse(dadosCotacao.Bid, CultureInfo.InvariantCulture);
                                    decimal valorConvertido;

                                    // Se a consulta foi invertida, dividimos; caso contrário, multiplicamos.
                                    if (eOperacaoInversa)
                                    {
                                        valorConvertido = valorOrigem / valorBid;
                                       
                                    }
                                    else
                                    {
                                        valorConvertido = valorOrigem * valorBid;
                                    }

                                    Console.WriteLine($"\nResultado Final: {valorOrigem:F2} {moedaOrigem} = {valorConvertido:F2} {moedaDestino}");
                                }
                                else
                                {
                                    Console.WriteLine("Erro ao extrair a chave da resposta JSON.");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Não foi encontrado. Status: {respostaHttp.StatusCode}");
                            }

                            break;

                        case Opcao.Sair:
                            rodarMenu = false;
                            Console.WriteLine("Encerrando o programa...");
                            break;

                        default:
                            Console.WriteLine("Opção inválida! Selecione 1 ou 2.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Entrada inválida! Digite apenas o número da opção.");
                }
            }
        }
    }
}