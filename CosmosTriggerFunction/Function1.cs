using System.Collections.Generic;
using CosmosTriggerFunction.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CosmosTriggerFunction
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([CosmosDBTrigger(
            databaseName: "orders",
            containerName: "bookshop_db",
            Connection = "ConnectionString",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)]IReadOnlyList<Order> input,
            ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                foreach (var inputItem in input) 
                {
                    log.LogInformation(inputItem.ToString());
                }
            }
        }
    }
}
