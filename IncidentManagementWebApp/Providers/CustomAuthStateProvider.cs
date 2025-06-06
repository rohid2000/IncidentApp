namespace IncidentManagementWebApp.Providers
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Components.Authorization;
    using Blazored.LocalStorage; // Required for token storage
    using System.Net.Http.Headers;
    using IncidentManagementWebApp.Helpers;

    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;

        public CustomAuthStateProvider(
            ILocalStorageService localStorage,
            HttpClient http)
        {
            _localStorage = localStorage;
            _http = http;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Get the JWT token from local storage
            var token = await _localStorage.GetItemAsync<string>("authToken");

            // If no token, return empty auth state (user is not logged in)
            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // Set the token in the HTTP client for API requests
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            // Parse claims from the JWT token
            var claims = JwtParserHelper.ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwtAuthType");
            var user = new ClaimsPrincipal(identity);

            // Return authenticated user
            return new AuthenticationState(user);
        }

        // Notify the app when the user logs in
        public void NotifyUserAuthentication(string token)
        {
            var claims = JwtParserHelper.ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwtAuthType");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(user)));
        }

        // Notify the app when the user logs out
        public void NotifyUserLogout()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(anonymousUser)));
        }
    }
}
