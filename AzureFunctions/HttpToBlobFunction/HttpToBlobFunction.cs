using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using System.Text;

namespace HttpToBlobFunction
{
    public static class HttpToBlobFunction
    {
        private const string SA_CONNECTION_STRING = "";
        private const string CONTAINER_NAME = "blobContainer";

        [FunctionName(nameof(HttpToBlobFunction))]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var fileName = req.Query["fileName"];
            var fileContent = await new StreamReader(req.Body).ReadToEndAsync() ?? string.Empty;

            if (string.IsNullOrEmpty(fileName))
            {
                return new OkObjectResult("Please provide fileName as query parameter.");
            }

            using MemoryStream stream = new(Encoding.UTF8.GetBytes(fileContent));

            string responseMessage;

            var blobServiceClient = new BlobServiceClient(SA_CONNECTION_STRING);
            var containerClient = blobServiceClient.GetBlobContainerClient(CONTAINER_NAME);
            var blobClient = containerClient.GetBlobClient(fileName);
            var fileExists = await blobClient.ExistsAsync();
            switch (req.Method)
            {
                case "GET":
                    
                    if (fileExists)
                    {
                        var downloadResponse = await blobClient.DownloadAsync();
                        var reader = new StreamReader(downloadResponse.Value.Content);
                        var content = await reader.ReadToEndAsync();
                        responseMessage = $"File name: {fileName}, content: {content}";
                    }
                    else
                    {
                        responseMessage = $"File with name {fileName} is not found";
                    }
                    break;
                case "POST":
                    if (!fileExists)
                    {
                        await containerClient.UploadBlobAsync(fileName, stream);
                        responseMessage = $"File with name {fileName} inserted.";
                    }
                    else
                    {
                        responseMessage = $"File with name {fileName} already exists";
                    }
                    break;
                case "PUT":
                    if (fileExists)
                    {
                        await containerClient.DeleteBlobIfExistsAsync(fileName);
                        await containerClient.UploadBlobAsync(fileName, stream);
                        responseMessage = $"File with name {fileName} updated.";
                    }
                    else
                    {
                        responseMessage = $"File with name {fileName} is not found";
                    }
                    break;
                case "DELETE":
                    if (fileExists)
                    {
                        await containerClient.DeleteBlobIfExistsAsync(fileName);
                        responseMessage = $"File with name {fileName} deleted.";
                    }
                    else
                    {
                        responseMessage = $"File with name {fileName} is not found";
                    }
                    break;
                default:
                    responseMessage = "Undefined HTTP Method.";
                    break;
            }


            return new OkObjectResult(responseMessage);
        }
    }
}
