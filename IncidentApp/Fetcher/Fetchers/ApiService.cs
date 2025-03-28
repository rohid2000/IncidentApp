using IncidentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace IncidentApp.Fetcher.Fetchers
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7015");
        }

        public async Task<List<IncidentDataModel>> GetIncidentAsync()
        {
            var response = await _httpClient.GetAsync("api/Incident");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<IncidentDataModel>>();
        }

        public async Task AddIncidentAsync(IncidentDataModel incident)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Incident", incident);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveIncidentAsync(IncidentDataModel incident)
        {
            var response = await _httpClient.DeleteAsync("/api/Incident/{id}");  
            response.EnsureSuccessStatusCode();
        }
    }
}
