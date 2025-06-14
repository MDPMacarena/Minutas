using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MinutasManage.Services
{
    public class Part
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class Content
    {
        [JsonPropertyName("parts")]
        public List<Part> Parts { get; set; }
    }

    public class GeminiRequest
    {
        [JsonPropertyName("contents")]
        public List<Content> Contents { get; set; }
    }

    public class Candidate
    {
        [JsonPropertyName("content")]
        public Content Content { get; set; }
    }

    public class GeminiResponse
    {
        [JsonPropertyName("candidates")]
        public List<Candidate> Candidates { get; set; }
    }

    public class IaService
    {
        private readonly HttpClient _httpClient;
      

        public IaService()
        {
            _httpClient = new();
            
        }

        public async Task<string?> GenerarRespuestaAsync(string prompt)
        {
            var requestBody = new GeminiRequest
            {
                Contents = new List<Content>
                {
                    new Content
                    {
                        Parts = new List<Part>
                        {
                            new Part { Text = prompt }
                        }
                    }
                }
            };

            string apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-pro-latest:generateContent?key=AIzaSyB7mw28mvZpmIA673FOy3kxYh9kv_aonf0";

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, requestBody, options);

                if (response.IsSuccessStatusCode)
                {
                    var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
                    return geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al llamar a Gemini API: {response.StatusCode}\n{error}");
                }
            }
            catch (Exception ex)
            {
                // Aquí puedes loguear el error si usas ILogger
                throw new Exception("Error en el servicio IA", ex);
            }
        }
    }
}
