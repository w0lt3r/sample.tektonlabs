using Microsoft.Extensions.Configuration;
using sample.tektonlabs.core.Interfaces;
using sample.tektonlabs.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace sample.tektonlabs.infrastructure.Implementations;

public class ExternalPriceProvider : IExternalPriceProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    public ExternalPriceProvider(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }
    public async Task<double> GetPrice(string key)
    {
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(_configuration["ExternalPriceProvider"]);
        var responseMessage = await client.GetAsync(string.Empty);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        using var contentStream =
             await responseMessage.Content.ReadAsStreamAsync();
        var content = await JsonSerializer.DeserializeAsync<ExternalPriceProviderResponse>(contentStream, options);
        return content.Price;
    }
}
