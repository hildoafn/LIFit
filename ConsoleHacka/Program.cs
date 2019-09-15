using System;
using RestSharp;

namespace ConsoleHacka
{
	class Program
	{
		const string URL_BigBoost = "https://gateway.gr1d.io/sandbox/bigdata/bigboost/v1/peoplev2";
		const string URL_MOngeral = "https://gateway.gr1d.io/sandbox/mongeral/v1/simulacao";
		const string URL_Strava = "https://www.strava.com/api/v3/athlete";
		const string sTokenStrava = "2ec670bd9f5dcbc46842c0fd3347d0c76274ab6f";

		static void Main(string[] args)
		{
			Console.WriteLine("Informe seu CPF!");
			string sCPF = Console.ReadLine();
			Console.WriteLine("Seguem os dados obtidos");
			string sRetornoGetDadosCliente = string.Empty;
			string sRetornoGetSimulacaoMongeral = string.Empty;
			string sRetornoStrava = string.Empty;
			try
			{
				sRetornoGetDadosCliente = GetDadosCliente(sCPF, URL_BigBoost);
				sRetornoGetSimulacaoMongeral = GetSimulacaoMongeral(URL_MOngeral, "Hildo", "34783940827", "1988-07-04", "", 0, 0, "RJ");
				sRetornoStrava = GetStravaAthleteActivity(URL_Strava, sTokenStrava);

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
				throw;
			}
			Console.WriteLine(sRetornoGetDadosCliente);

			Console.WriteLine("Seguem dados de simulação da Mongeral");
			Console.WriteLine(sRetornoGetSimulacaoMongeral);

			Console.WriteLine("Seguem dados do atleta");
			Console.WriteLine(sRetornoStrava);

			Console.Read();
		}

		public static string GetDadosCliente(string sCPF, string sURL)
		{
			var client = new RestClient(sURL);

			var request = new RestRequest(string.Empty, Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("X-Api-Key", "52d2e55d-0561-4178-b812-079491fa1769");
			request.AddJsonBody(new
			{
				Datasets = "basic_data,occupation_data,addresses",
				q = string.Concat("doc{", sCPF, "}"),
				AccessToken = ""
			}
			);

			var response = client.Execute(request);
			return response.Content;
		}

		public static string GetSimulacaoMongeral(string sURL, string sNome, string sCPF, string sDataNascimento, string sProfissao, decimal dRenda, int nSexo, string sUF)
		{
			var client = new RestClient(sURL);

			var request = new RestRequest(string.Empty, Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("x-api-key", "3cd346aa-a061-4242-b249-08985f4ce862");
			request.AddJsonBody(new
			{
				simulacoes = new
				{
					proponente = new
					{
						tipoRelacaoSeguradoId = 0,
						nome = sNome,
						cpf = sCPF,
						dataNascimento = sDataNascimento,
						profissaoCbo = sProfissao,
						renda = dRenda,
						sexoId = nSexo,
						uf = sUF,
						declaracaoIRId = 0
					}
				},
				periodicidadeCobrancaId = 0,
				prazoCerto = 0
			}
			);

			var response = client.Execute(request);
			return response.Content;
		}

		public static string GetStravaAthleteActivity(string sURL, string sToken)
		{
			var client = new RestClient(sURL);
			var request = new RestRequest(string.Empty, Method.GET);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Authorization", "Bearer " + sToken);

			var response = client.Execute(request);
			return response.Content;
		}
	}
}