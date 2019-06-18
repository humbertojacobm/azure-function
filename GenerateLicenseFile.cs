using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace pluralsightfuncs
{
    public static class GenerateLicenseFile
    {
        [FunctionName("GenerateLicenseFile")]
        public static void Run(
            [QueueTrigger("orders", Connection = "AzureWebJobsStorage")]Order order, 
            [Blob("licenses/{rand-guid}.lic")] TextWriter ouputBlob,
            ILogger log)
        {
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
