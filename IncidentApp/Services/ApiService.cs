using IncidentApp.Models;
using System.Net.Http.Json;

namespace IncidentApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public ApiService()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:7015");
        }

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7015");
        }

        public virtual async Task<List<IncidentDataModel>> GetIncidentAsync()
        {
            var response = await _httpClient.GetAsync("api/Incident");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode} - {errorContent}");
            }

            var result = await response.Content.ReadFromJsonAsync<List<IncidentDataModel>>();

            return result;
        }

        public virtual async Task<List<IncidentDataModel>> GetIncidentsByUserId(int userId)
        {
            var response = await _httpClient.GetAsync("api/Incident/" + userId);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode} - {errorContent}");
            }

            var result = await response.Content.ReadFromJsonAsync<List<IncidentDataModel>>();

            return result;
        }

        public virtual async Task AddIncidentAsync(IncidentDataModel incident)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Incident", incident);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode} - {errorContent}");
            }
        }

        public virtual async Task UpdateIncidentProperties(IncidentDataModel incident)
        {
            var response = await _httpClient.PatchAsJsonAsync("/api/Incident", incident);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode} - {errorContent}");
            }
        }

        public virtual async Task RemoveIncidentAsync(IncidentDataModel incident)
        {
            var response = await _httpClient.DeleteAsync($"/api/Incident/{incident.Id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode} - {errorContent}");
            }
        }

        public virtual async Task AddUserAsync(UserAdminDataModel user)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/user", user);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode} - {errorContent}");
            }
        }

        public virtual async Task<UserDataModel> TryAuthenticate(UserAdminDataModel user)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/login", user);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode} - {errorContent}");
            }

            var result = await response.Content.ReadFromJsonAsync<UserDataModel>();

            return result;
        }
    }
}
