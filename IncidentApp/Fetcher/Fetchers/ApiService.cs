using IncidentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace IncidentApp.Fetcher.Fetchers
{
    public static class ApiService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        static ApiService()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:7015");
        }

        public static async Task<List<IncidentDataModel>> GetIncidentAsync()
        {
            var response = await _httpClient.GetAsync("api/Incident");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<IncidentDataModel>>();

            return result;
        }

        public static async Task AddIncidentAsync(IncidentDataModel incident)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Incident", incident);
            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode} - {errorContent}");
            }
        }

        public static async Task RemoveIncidentAsync(IncidentDataModel incident)
        {
            var response = await _httpClient.DeleteAsync($"/api/Incident/{incident.Id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
