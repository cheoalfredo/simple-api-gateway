using K8SCore.Domain.Ports;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Readers;
using NSwag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace K8SCore.Infrastructure.Adapters
{
    public class SwaggerManager : ISwaggerManager
    {

        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, string> swaggList = new Dictionary<string, string> {
            {"petstore", "https://petstore.swagger.io/v2/swagger.json"},
            {"httpbin", "https://httpbin.org/spec.json" },
            {"bancolombia QR", "https://developer.bancolombia.com/es/ibm_apim/swaggerjson/UVItTWFuYWdlbWVudDoxLjAuMA%2C%2C" }
        };


        public SwaggerManager(IHttpClientFactory upstream)
        {
            _httpClient = upstream.CreateClient();
        }


        public async Task<List<KeyValuePair<string, string>>> GetResourceEndpoints(string apiName, string resource)
        {

            try
            {
                var endpoints = new List<KeyValuePair<string, string>>();

                /* var response = await _httpClient.GetAsync(swaggList[apiName]);
                 var content = await response.Content.ReadAsStringAsync();
                 var document = await OpenApiDocument.FromJsonAsync(content);

                 var lookFor = resource.Replace("--", "/");

                 var baseUrl = document.BaseUrl;

                 foreach (var endpoint in document.Paths.Where(x => x.Key.Equals(lookFor)))
                 {
                     var calls = endpoint.Value.Keys;
                     var scripplet = string.Empty;
                     foreach (var opKey in calls)
                     {
                         var apiOp = endpoint.Value[opKey];
                         var paramsString = ParseParameters(apiOp.Parameters);
                         var template = System.IO.File.ReadAllText($"Templates/{opKey.ToLower()}Template.js");
                         scripplet = template.Replace("_url", baseUrl + endpoint.Key).Replace("_data", paramsString);
                         endpoints.Add(new KeyValuePair<string, string>($"{endpoint.Key}[{opKey}]" + endpoint.Key, scripplet));
                     }
                 }*/

                var stream = await _httpClient.GetStreamAsync(swaggList[apiName]);
                
                var openApiDocument = new OpenApiStreamReader().Read(stream, out var diagnostic);

                var swaggJson = openApiDocument.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Json);
                var document = await OpenApiDocument.FromJsonAsync(swaggJson);

                var lookFor = resource.Replace("--", "/");
                //openApiDocument.Components.Examples.Values

                var baseUrl = openApiDocument.Servers.Single(x => x.Url.Contains("https")).Url;

                foreach (var endpoint in document.Paths.Where(x => x.Key.Equals(lookFor)))
                {
                    var calls = endpoint.Value.Keys;
                    var scripplet = string.Empty;
                    foreach (var opKey in calls)
                    {
                        var apiOp = endpoint.Value[opKey];
                        var paramsString = ParseParameters(apiOp.Parameters);
                        var template = System.IO.File.ReadAllText($"Templates/{opKey.ToLower()}Template.js");
                        scripplet = template.Replace("_url", baseUrl + endpoint.Key).Replace("_data", paramsString);
                        endpoints.Add(new KeyValuePair<string, string>($"{endpoint.Key}({opKey.ToUpper()})", scripplet));
                    }
                }

                return endpoints;
            } catch (Exception ex)
            {
                Console.Write(ex.Message);
                return new List<KeyValuePair<string, string>>();
            }
        }

        public async Task<List<KeyValuePair<string, string>>> GetResources(string apiName)
        {
            var resources = new List<KeyValuePair<string, string>>();

            var stream = await _httpClient.GetStreamAsync(swaggList[apiName]);
            // Read V3 as YAML
            var openApiDocument = new OpenApiStreamReader().Read(stream, out var diagnostic);
            var swaggJson = openApiDocument.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Json);
            var document = await OpenApiDocument.FromJsonAsync(swaggJson);

            foreach (var endpoint in document.Paths)
            {
                resources.Add(new KeyValuePair<string, string>(endpoint.Key, apiName));
            }
            
            return resources;
        }

        string ParseParameters(IEnumerable<OpenApiParameter> parameters)
        {
            var paramString = string.Empty;
            foreach (var p in parameters)
            {
                var sample = p.ToSampleJson();
                paramString += paramString.Equals(string.Empty) ? string.Empty : ",";
                paramString += $"{p.Name}: {GetParamType(p)}";

            }
            return paramString;
        }

        public async Task<List<KeyValuePair<string, string>>> RetrieveApis()
        {
            return await Task.FromResult(swaggList.ToList());
        }

        string GetParamType(OpenApiParameter param)
        {

            var sample = param.ToSampleJson();
            if (sample == null)
                return param.Type.ToString();
            return (string)(sample.ToString().Equals("") ? param.Type.ToString() : sample.ToString());


        }
    }
}
