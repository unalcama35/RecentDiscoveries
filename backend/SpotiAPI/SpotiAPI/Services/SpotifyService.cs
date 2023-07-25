namespace SpotiAPI.Services
{

    using SpotifyAPI.Web;
    using SpotifyAPI.Web.Auth;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Newtonsoft.Json.Linq;
    using Microsoft.Extensions.Caching.Memory;

    public class SpotifyService
    {

        private readonly DataContext context;
        private readonly IMemoryCache memoryCache;

        private readonly string clientID = "AddClientID";
        private readonly string clientSecret = "AddClientSecret";
        private readonly Uri redirectUri = new Uri("https://localhost:7214/api/song/callback");

        public SpotifyService(DataContext context, IMemoryCache memoryCache)
        {
            this.context = context;
            this.memoryCache = memoryCache;
        }


        public async void LoginAsync()
        {
            var scopes = new List<string> { Scopes.UserTopRead, Scopes.UserLibraryRead, Scopes.UserFollowRead }; // Modify the scopes as per your application's requirements
            var server = new EmbedIOAuthServer(redirectUri, 5543);
            await server.Start();
            var loginRequest = new LoginRequest(redirectUri, clientID, LoginRequest.ResponseType.Code)
            {
                Scope = scopes
            };
            BrowserUtil.Open(loginRequest.ToUri());
        }

        public async Task<string> GenerateToken(string authorizationCode)
        {
            var tokenRequest = new AuthorizationCodeTokenRequest(clientID, clientSecret, authorizationCode, redirectUri);
            var res = await new OAuthClient().RequestToken(tokenRequest);
            var accessToken = res.AccessToken;
            memoryCache.Set("AccessToken", accessToken);
            return accessToken;
        }

        public async Task<JToken> GetRecentsAsync()
        {
            var response = await GetResponse($"https://api.spotify.com/v1/me/tracks");
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseBody);
                var items = json["items"];
                return items;

            }
            else
            {
                throw new Exception($"Failed to retrieve saved tracks. StatusCode: {response.StatusCode}");
            }
        }

        public async Task<JToken> GetLongTermSongs()
        {
            var response = await GetResponse($"https://api.spotify.com/v1/me/top/tracks");
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseBody);
                var items = json["items"];
                return items;
            }
            else
            {
                throw new Exception($"Failed to retrieve top tracks. StatusCode: {response.StatusCode}");
            }
        }

        private async Task<HttpResponseMessage> GetResponse(String url)
        {
            if (!memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                throw new Exception("Access token not found.");
            }
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await httpClient.GetAsync(url);
            return response;

        }





    }
}
