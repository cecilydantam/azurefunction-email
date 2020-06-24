using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace strategycorps_func_email
{
    public static class FireEmail
    {
        [FunctionName("FireEmail")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Queue("emails", Connection = "AzureWebJobsStorage")] IAsyncCollector<Email> emailQueue,
            ILogger log)
        {
            log.LogInformation("Send Email processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var email = JsonConvert.DeserializeObject<Email>(requestBody);
            email.PartitionKey = "basic";
            email.RowKey = Guid.NewGuid().ToString();

            await emailQueue.AddAsync(email);

            return new OkObjectResult("Email queued.");
        }
    }

    public class Email
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
