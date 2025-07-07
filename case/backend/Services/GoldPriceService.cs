
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace backend.Services
{


    public class MetalsObject
    {
        [JsonPropertyName("gold")]
        public double Gold { get; set; }
    }

    public class RealMetalsDevApiResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("metals")]
        public MetalsObject Metals { get; set; }
    }


    public class GoldPriceService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GoldPriceService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        }

        public async Task<double> GetGoldPriceAsync()
        {
            var apiKey = "J98YDRPNUXIH8CTJYWR6331TJYWR6";
            var requestUrl = $"https://metals.dev/api/latest?api_key={apiKey}&base=USD";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode(); 

                var jsonResponse = await response.Content.ReadAsStringAsync();
                
                var apiData = JsonSerializer.Deserialize<RealMetalsDevApiResponse>(jsonResponse);

                if (apiData == null || apiData.Status != "success")
                {
                    throw new Exception("API isteği başarısız oldu veya yanıt 'success' durumunda değil.");
                }

                if (apiData.Metals == null)
                {
                    throw new Exception("API yanıtı 'metals' verisini içermiyor.");
                }

                double pricePerOunce = apiData.Metals.Gold;
                
                return Math.Round(pricePerOunce, 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API işlenirken hata oluştu: {ex.Message}");
                return 3300.00; // api ya istek atarken hata alırsak diye sabit bir fiyat
            }
        }
    }
}