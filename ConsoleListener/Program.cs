// This example requires the System and System.Net namespaces.
using Newtonsoft.Json;
using System.Net;

namespace ConsoleListener
{
    internal class Program
    {
        public static void Main()
        {
            for (int i = 0; i < 1000; i++)
            {
                SimpleListenerExample("http://192.168.0.10:4080/");
            }
        }

        public static async void SimpleListenerExample(string uri)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8080/index/".
            if (uri == null || uri.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.

            listener.Prefixes.Add(uri);

            listener.Start();
            Console.WriteLine("Listening...");
            // Note: The GetContext method blocks while waiting for a request.
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;

            ShowRequestData(request);

            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            // Construct a response.

            string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();
            listener.Stop();
            SimpleListenerExample(uri);
        }

        public static void ShowRequestData(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                Console.WriteLine("No client data was sent with the request.");
                return;
            }
            System.IO.Stream body = request.InputStream;
            System.Text.Encoding encoding = request.ContentEncoding;
            System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
            if (request.ContentType != null)
            {
                Console.WriteLine("Client data content type {0}", request.ContentType);
            }
            Console.WriteLine("Client data content length {0}", request.ContentLength64);

            Console.WriteLine("Start of client data:");
            // Convert the data to a string and display it on the console.
            string jsonString = reader.ReadToEnd();

            PostData json1 = JsonConvert.DeserializeObject<PostData>(jsonString);
            Console.WriteLine(json1.test);

            Console.WriteLine("End of client data:");
            body.Close();
            reader.Close();
            // If you are finished with the request, it should be closed also.
        }

        internal class SomeSubData
        {
            public string line1 { get; set; }
            public string line2 { get; set; }
        }

        internal class PostData
        {
            public string test { get; set; }
            public SomeSubData lines { get; set; }
        }
    }
}