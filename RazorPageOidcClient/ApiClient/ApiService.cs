﻿using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RazorPageOidcClient
{
    public class ApiService
    {
        private readonly IOptions<AuthConfigurations> _authConfigurations;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApiTokenCacheClient _apiTokenClient;

        public ApiService(
            IOptions<AuthConfigurations> authConfigurations,
            IHttpClientFactory clientFactory,
            ApiTokenCacheClient apiTokenClient)
        {
            _authConfigurations = authConfigurations;
            _clientFactory = clientFactory;
            _apiTokenClient = apiTokenClient;
        }

        public async Task<JArray> GetUnsecureApiDataAsync()
        {
            try
            {
                var client = _clientFactory.CreateClient();

                client.BaseAddress = new Uri(_authConfigurations.Value.ProtectedApiUrl);
                var response = await client.GetAsync("api/values");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var data = JArray.Parse(responseContent);

                    return data;
                }

                throw new ApplicationException($"Status code: {response.StatusCode}, Error: {response.ReasonPhrase}");
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Exception {e}");
            }

        }
        public async Task<JArray> GetApiDataAsync()
        {
            try
            {
                var client = _clientFactory.CreateClient();

                client.BaseAddress = new Uri(_authConfigurations.Value.ProtectedApiUrl);

                var access_token = await _apiTokenClient.GetApiToken(
                    "CC_STS_A",
                    "scope_a",
                    "cc_secret"
                );

                client.SetBearerToken(access_token);

                var response = await client.GetAsync("api/values");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var data = JArray.Parse(responseContent);

                    return data;
                }

                throw new ApplicationException($"Status code: {response.StatusCode}, Error: {response.ReasonPhrase}");
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Exception {e}");
            }
        }
    }
}
