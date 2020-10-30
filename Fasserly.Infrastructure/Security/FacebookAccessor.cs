using Fasserly.Infrastructure.Interface;
using Fasserly.Infrastructure.Mediator.UserMediator;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Security
{
    public class FacebookAccessor : IFacebookAccessor
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<FacebookAppSettings> _options;

        public FacebookAccessor(IOptions<FacebookAppSettings> options)
        {
            _options = options;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com/")
            };
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<FacebookUserInfo> FacebookLogin(string accessToken)
        {
            //verify if token is valid
            var verifyToken = await _httpClient.GetAsync($"debug_token?input_token={accessToken}&access_token={_options.Value.AppId}|{_options.Value.AppSecret}");

            if (!verifyToken.IsSuccessStatusCode)
                return null;
            var result = await GetAsync<FacebookUserInfo>(accessToken, "me", "fields=name,email,picture.width(100).height(100)");
            return result;
        }

        private async Task<T> GetAsync<T>(string accessToken, string endpoint, string args)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");

            if (!response.IsSuccessStatusCode)
                return default(T);
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
