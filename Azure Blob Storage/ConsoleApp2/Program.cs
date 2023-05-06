using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BlobServiceClient blobServiceClient = new("DefaultEndpointsProtocol=https;AccountName=formaciondemobcr;AccountKey=xQg/3uyE5MX5uEoHqQgTZ7DjG1g+X2k1KK8HtqFb/4fU6JgySzg8D4Sw1VYQbf81Xd7PU/Gt/4m8+ASthJOO3Q==;EndpointSuffix=core.windows.net");

            var containers = blobServiceClient.GetBlobContainers();
            foreach (var item in containers)
            {
                Console.WriteLine($"Nombre del contenedor = {item.Name}");

                // Listado de Blobs
                var containerClient = blobServiceClient.GetBlobContainerClient(item.Name);
                var blobs = containerClient.GetBlobs();

                foreach (var blob in blobs) 
                
                {
                    Console.WriteLine($"  > {blob.Name}");
                }
            }

            // Download de un Blob logos/logo-eoi.png
            BlobServiceClient blobServiceClient2 = new("DefaultEndpointsProtocol=https;AccountName=formaciondemobcr;AccountKey=xQg/3uyE5MX5uEoHqQgTZ7DjG1g+X2k1KK8HtqFb/4fU6JgySzg8D4Sw1VYQbf81Xd7PU/Gt/4m8+ASthJOO3Q==;EndpointSuffix=core.windows.net");
            var containerClient2 = blobServiceClient2.GetBlobContainerClient("ficheros");
            BlobClient blobClient2 = containerClient2.GetBlobClient("logos/logo-eoi.png");
            blobClient2.DownloadTo("file.png");
        }
    }
}