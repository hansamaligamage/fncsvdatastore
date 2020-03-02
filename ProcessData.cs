using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;

namespace processcsvdata
{
    public static class ProcessData
    {
        [FunctionName("ProcessData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log, ExecutionContext executionContext)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var lines = File.ReadLines(Directory.GetParent(executionContext.FunctionDirectory).FullName+"\\courses.csv");

            Console.WriteLine("Azure Cosmos DB Table - .NET Core example");
            Console.WriteLine();

            string tableName = "Courses";

            CloudTable table = await DBConnection.CreateTableAsync(tableName);
            foreach (var row in lines)
            {
                var columns = row.Split(',');
                Session session = new Session(columns[0], columns[4])
                {
                    Lectures = columns[1],
                    Labs = columns[2],
                    Points = Convert.ToDouble(columns[3]),
                    IsWeekend = Convert.ToBoolean(columns[5])
                };
               await EntityManager.InsertOrUpdateEntityAsync(table, session);
            }

            Console.WriteLine("Reading a created Entity.");
            Session retrievedSession = await EntityManager.RetrieveEntityAsync(table, "Computer System Architecture", "Greg Graffin");
            Console.WriteLine();

            return new OkObjectResult(retrievedSession);
        }
    }
}
