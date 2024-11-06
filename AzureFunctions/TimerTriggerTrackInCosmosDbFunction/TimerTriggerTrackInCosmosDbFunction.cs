using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace TimerTriggerTrackInCosmosDbFunction
{
    public class TimerTriggerTrackInCosmosDbFunction
    {
        private const string ENDPOINT = "https://timetracktest.documents.azure.com:443/";
        private const string PRIMARY_KEY = "y00gdIP3PJr4mBVjUwLaNuJGsIvK5GaZotcTNrXktqMc9vxxBjD6I3Bn9yZE6Ys0haYntvdwC6ywACDbw52MHw==";
        private const string DATABASE_NAME = "timedb";
        private const string CONTAINER_NAME = "timedbcontainer";

        [FunctionName(nameof(TimerTriggerTrackInCosmosDbFunction))]
        public async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            var now = DateTime.Now;
            log.LogInformation($"C# Timer trigger function executed at: {now}");

            try
            {
                var client = new CosmosClient(ENDPOINT, PRIMARY_KEY).GetContainer(DATABASE_NAME, CONTAINER_NAME);

                var newItem = new Item
                {
                    id = Guid.NewGuid().ToString(),
                    time = DateTime.Now
                };

                await client.CreateItemAsync(newItem);

                log.LogInformation($"C# Timer trigger function inserted record with id {newItem.id} at: {now}");
            }
            catch (Exception ex)
            {
                log.LogError($"C# Timer trigger function exception at: {now} - {ex.Message}");
            }
        }
    }
}
