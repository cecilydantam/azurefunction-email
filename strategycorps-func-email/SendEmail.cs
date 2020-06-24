using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace strategycorps_func_email
{
    public static class SendEmail
    {
        [FunctionName("SendEmail")]
        public static void Run([QueueTrigger("emails", Connection = "AzureWebJobsStorage")]Email email, 
                                [SendGrid(ApiKey = "SendGridApiKey")] out SendGridMessage message,
                                ILogger log)
        {
            message = new SendGridMessage {From = new EmailAddress(email.Sender), Subject = email.Subject, PlainTextContent = email.Body};
            message.AddTo(new EmailAddress(email.Recipient));
        }
    }
}
