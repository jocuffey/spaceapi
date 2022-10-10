using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http;

namespace GetTokenFunction
{
    public static class GetNotionContent
    {
        [FunctionName("GetNotionContent")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string? apiUrl = Environment.GetEnvironmentVariable("NotionApiUrl", EnvironmentVariableTarget.Process);
            string getUsersUrl = apiUrl + "users";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                string accessKey = Environment.GetEnvironmentVariable("NotionApiKey");
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessKey);

                response = await client.GetAsync(getUsersUrl).ConfigureAwait(false);

            }

            catch (Exception ex)
            {
                log.LogError("Error communicating with API", ex.Message);
            }

            log.LogInformation("C# HTTP trigger function processed a request.");
            log.LogInformation(response.Content.ToString());

            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                return new OkObjectResult(resp);
            }

            else
            {
                return new BadRequestObjectResult(response);
            }

        }
    }
}
