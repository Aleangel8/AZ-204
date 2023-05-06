using StorageRestApiAuth;
using System.Globalization;
using StorageRestApiAuth;
using System.Xml.Linq;

namespace ConsoleAppStorageAccount
{
    internal class Program
    {
        static string storageName = "formaciondemobcr";
        static string storageKey = "xQg/3uyE5MX5uEoHqQgTZ7DjG1g+X2k1KK8HtqFb/4fU6JgySzg8D4Sw1VYQbf81Xd7PU/Gt/4m8+ASthJOO3Q==";


        static void Main(string[] args)
        {
            ListContainers();
            GetBlob("ficheros", "logos/logo-eoi.png");
        }

        static void ListContainers() 
        {
            HttpClient client = new HttpClient();
            string uri = $"https://{storageName}.blob.core.windows.net/?comp=list&include=system,deleted";

            //Mensaje de petición
            HttpRequestMessage request = new(HttpMethod.Get, uri);//Lo mismo q new HttpRequestMessage(HttpMethod.Get, uri) ;
            
            // Body
            request.Content = null;//Sobra

            // Headers
            DateTime fecha = DateTime.UtcNow;
            request.Headers.Add("x-ms-date", fecha.ToString("R",CultureInfo.InvariantCulture));
            request.Headers.Add("x-ms-version","2021-12-02");
            request.Headers.Authorization = AzureStorageAuthenticationHelper.GetAuthorizationHeader(
                storageName, storageKey,fecha, request);
        
            // Mensaje de respuesta
            HttpResponseMessage response = client.Send(request);

            if(response.StatusCode == System.Net.HttpStatusCode.OK) 
            {
                string xmlString = response.Content.ReadAsStringAsync().Result;                
                XElement xml = XElement.Parse(xmlString);
                foreach(var item in xml.Element("Containers").Elements("Container")) 
                {
                    Console.WriteLine($"Nombre del contenedor: { item.Element("Name").Value}");
                    ListBlobs(item.Element("Name").Value);
                }
            }
            else { Console.WriteLine($"Error: {response.StatusCode}({response.ReasonPhrase})"); }
    
        }

        static void ListBlobs(string nameContainer)
        {
            HttpClient client = new HttpClient();
            string uri = $"https://{storageName}.blob.core.windows.net/{nameContainer}?restype=container&comp=list";

            //Mensaje de petición
            HttpRequestMessage request = new(HttpMethod.Get, uri);//Lo mismo q new HttpRequestMessage(HttpMethod.Get, uri) ;

            // Body
            request.Content = null;//Sobra

            // Headers
            DateTime fecha = DateTime.UtcNow;
            request.Headers.Add("x-ms-date", fecha.ToString("R", CultureInfo.InvariantCulture));
            request.Headers.Add("x-ms-version", "2021-12-02");
            request.Headers.Authorization = AzureStorageAuthenticationHelper.GetAuthorizationHeader(
                storageName, storageKey, fecha, request);

            // Mensaje de respuesta
            HttpResponseMessage response = client.Send(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string xmlString = response.Content.ReadAsStringAsync().Result;
                XElement xml = XElement.Parse(xmlString);
                foreach (var item in xml.Element("Blobs").Elements("Blob"))
                {
                    Console.WriteLine($"  > {item.Element("Name").Value}");

                }
            }
            else { Console.WriteLine($"Error: {response.StatusCode}({response.ReasonPhrase})"); }

        }

        static void GetBlob(string nameContainer, string nameBlob)
        {
            HttpClient client = new HttpClient();
            string uri = $"https://{storageName}.blob.core.windows.net/{nameContainer}/{nameBlob}";

            //Mensaje de petición
            HttpRequestMessage request = new(HttpMethod.Get, uri);//Lo mismo q new HttpRequestMessage(HttpMethod.Get, uri) ;

            // Body
            request.Content = null;//Sobra

            // Headers
            DateTime fecha = DateTime.UtcNow;
            request.Headers.Add("x-ms-date", fecha.ToString("R", CultureInfo.InvariantCulture));
            request.Headers.Add("x-ms-version", "2021-12-02");
            request.Headers.Authorization = AzureStorageAuthenticationHelper.GetAuthorizationHeader(
                storageName, storageKey, fecha, request);

            // Mensaje de respuesta
            HttpResponseMessage response = client.Send(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //string contenido = response.Content.ReadAsStringAsync().Result;
                //Console.WriteLine(contenido);

                Console.WriteLine($"Content-Type: {response.Content.Headers.ContentType.MediaType}");

                var file = new FileStream("file.png", FileMode.Create, FileAccess.Write);
                response.Content.ReadAsStream().CopyTo(file);
                file.Close();
                file.Dispose();
            }
            else { Console.WriteLine($"Error: {response.StatusCode}({response.ReasonPhrase})"); }

        }
    }
}