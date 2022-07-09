using Newtonsoft.Json;

namespace ConsoleHttp
{
    internal class Program
    {
        private static async Task Main()
        {
            using (var client = new HttpClient())
            {
                await PostContent(client);
            }
        }

        private static async Task GetContent(HttpClient client)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                //запрос Get на получения ответа от конкретного ресурса
                HttpResponseMessage response = await client.GetAsync("http://www.contoso.com/");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        private static async Task PostContent(HttpClient client)
        {
            PostData data = new PostData
            {
                test = "Кирилл",
                lines = new SomeSubData
                {
                    line1 = "a line",
                    line2 = "a second line"
                }
            };

            string json = JsonConvert.SerializeObject(data);

            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            //StringContent httpContent1 = new StringContent("asdasd");
            // var response = await client.PostAsync("http://192.168.0.10:4080/", httpContent1);
            var response = await client.PostAsync("http://192.168.0.10:4080/", httpContent);

            string str = "" + response.Content + " : " + response.StatusCode;
        }
    }
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

// HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.