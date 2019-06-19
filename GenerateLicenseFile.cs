using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace pluralsightfuncs
{
    public static class GenerateLicenseFile
    {
        [FunctionName("GenerateLicenseFile")]
        public static async Task Run(
            [QueueTrigger("orders", Connection = "AzureWebJobsStorage")]Order order, 
            IBinder binder,
            ILogger log)
        {

            var ouputBlob = await binder.BindAsync<TextWriter>(
                new BlobAttribute(blobPath: $"licenses/{order.OrderId}.lic")
                {
                    Connection = "AzureWebJobsStorage"
                });

            log.LogInformation($"C# Queue trigger function processed: {order}");
            ouputBlob.WriteLine($"orderId: {order.OrderId}");
            ouputBlob.WriteLine($"Email: {order.Email}");
            ouputBlob.WriteLine($"ProductId: {order.ProductId}");
            ouputBlob.WriteLine($"PurchaseDate: {DateTime.UtcNow}");
            var md5 = System.Security.Cryptography.MD5.Create();
            var hash = md5.ComputeHash(
                  System.Text.Encoding.UTF8.GetBytes(order.Email + "secret"));
            ouputBlob.WriteLine($"Secreate code : {BitConverter.ToString(hash).Replace("-", "")}");
        }
    }
}
