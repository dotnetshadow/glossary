using System.Net;
using System.Text.Json.Nodes;
using Glossary.Web.Models;

namespace Glossary.Web.Services
{
    public class GlossaryService
    {
        private readonly HttpClient _httpClient;

        public GlossaryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // set in appsettings.json                
            //_httpClient.BaseAddress = new Uri("https://localhost:7252/");
        }

        public async Task<Result> CreateGlossaryItem(GlossaryItem glossaryItem)
        {
            try
            {
                var content = JsonContent.Create(glossaryItem);
                var res = await _httpClient.PostAsync("api/glossary/", content);
                var json = await res.Content.ReadAsStringAsync();
                var message = GetMessage(json);

                if (res.IsSuccessStatusCode)
                {
                    return Result.Ok();
                }
                else if (res.StatusCode == HttpStatusCode.Conflict)
                {
                    return Result.Fail<GlossaryItem>(message);
                }

                return Result.Fail<GlossaryItem>(message);
            }
            catch (Exception e)
            {
                // Log error
                Console.WriteLine(e);
                return Result.Fail<GlossaryItem>("Glossary service is currently unavailable");
                //throw;
            }
            
        }

        public async Task<Result> DeleteGlossaryItemById(int id)
        {
            try
            {
                await _httpClient.DeleteAsync($"api/glossary/{id}");
                return Result.Ok();
            }
            catch (Exception e)
            {
                // Log error
                Console.WriteLine(e);
                //throw;
                return Result.Fail<GlossaryItem>("Glossary service is currently unavailable");
            }
        }

        public async Task<IEnumerable<GlossaryItem>> GetGlossaryListAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<GlossaryItem>>("api/glossary");
            return result ?? new List<GlossaryItem>();
        }
        
        public async Task<GlossaryItem?> GetGlossaryItemById(int id)
        {
            var res = await _httpClient.GetAsync($"api/glossary/{id}");
            
            if (res.IsSuccessStatusCode)
            {
                return await res.Content.ReadFromJsonAsync<GlossaryItem?>();
            }

            return null;
        }

        public async Task<Result> EditGlossaryItem(GlossaryItem glossaryItem)
        {
            try
            {
                var content = JsonContent.Create(glossaryItem);
                var res = await _httpClient.PutAsync("api/glossary/", content);
                var json = await res.Content.ReadAsStringAsync();
                var message = GetMessage(json);

                if (res.IsSuccessStatusCode)
                {
                    return Result.Ok();
                }
                else if (res.StatusCode == HttpStatusCode.Conflict)
                {
                    return Result.Fail<GlossaryItem>(message);
                }

                return Result.Fail<GlossaryItem>(message);
            }
            catch (Exception e)
            {
                // Log error
                Console.WriteLine(e);
                return Result.Fail<GlossaryItem>("Glossary service is currently unavailable");
            }
            
        }

        private string GetMessage(string json)
        {
            var message = string.Empty;

            if (string.IsNullOrEmpty(json)) return message;

            var test = JsonNode.Parse(json);
            message = (string)test?["message"]!;

            return message;
        }
    }
}
