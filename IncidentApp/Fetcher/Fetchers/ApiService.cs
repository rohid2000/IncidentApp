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

        public async Task<List<DataModel>> GetDataAsync()
        {
            var response = await _httpClient.GetAsync(" api/data");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<DataModel>>();
        }

        public async Task PostDataAsync(DataModel data)
        {
            var response = await _httpClient.PostAsync("/api/data", data);

            response.EnsureSuccessStatusCode();
        }
    }
}
