using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Toci.Haia.ChatGPT
{
    public interface IChatGptService
    {
        Task<string> GenerateJokeAsync(string commentText);
    }

    public class ChatGptService : IChatGptService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "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"; // Wstaw swój klucz API OpenAI

        public ChatGptService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GenerateJokeAsync(string commentText)
        { //text-davinci-003
            //gpt-4
            //gpt-3.5-turbo
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                new { role = "system", content = "You are a funny assistant." },
                new { role = "user", content = $"Powiedz żart o: {commentText}" }
            },
                temperature = 0.7
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                return jsonResponse?.choices[0]?.message?.content.ToString();
            }

            return "Could not generate a joke at the moment.";
        }
    }

}
