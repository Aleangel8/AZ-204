using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
//using System.ComponentModel.DataAnnotations.Schema;



namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]

        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            [Table("operaciones", Connection = "AzureWebJobsStorage")] ICollector<DataInfo> outputTable
            )
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            var dataInfo = new DataInfo()
            {
                PartitionKey = "Function1",
                RowKey = Guid.NewGuid().ToString(),
                Name = name,
                Message = responseMessage,
            };

            outputTable.Add(dataInfo);  

            return new OkObjectResult(responseMessage);
        }
    }

    public class DataInfo
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}
