using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GetTokenFunction
{
    public static class GetNotionContent
    {
        [FunctionName("GetNotionContent")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string apiUrl = Environment.GetEnvironmentVariable("NotionApiUrl", EnvironmentVariableTarget.Process);
            string authKey = Environment.GetEnvironmentVariable("NotionApiKey", EnvironmentVariableTarget.Process);

            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{apiUrl}v1/users"),
                Headers =
                {
                    { "Notion-Version", "2022-02-22" },
                    { "Authorization", $"Bearer {authKey}" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return new OkObjectResult(body);
            }
        }
    }
}
