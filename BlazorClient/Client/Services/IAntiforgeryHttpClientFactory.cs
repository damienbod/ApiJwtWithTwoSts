﻿namespace BlazorClient.Client.Services
{
    public interface IAntiforgeryHttpClientFactory
    {
        Task<HttpClient> CreateClientAsync(string clientName = AuthDefaults.AuthorizedClientName);
    }
}