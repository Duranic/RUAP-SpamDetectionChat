using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Spam_detection_chat
{
    public class StringTable
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }


    public class SpamChecker
    {

        private async Task<bool> InvokeRequestResponseService(string message)
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {

                    Inputs = new Dictionary<string, StringTable>() {
                        {
                            "input1",
                            new StringTable()
                            {
                                ColumnNames = new string[] {"label", "message", "Column 2", "Column 3", "Column 4"},
                                Values = new string[,] {  { "value", message, "value", "value", "value" }  }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                const string apiKey = "m9OYFjxx3T4ZPQaPtBsffaUYJ97arGN59normGfdexVtyY0jQ1BgvQssnlfFjg3cIE8zXiTeVbDibjmSHcgNbA=="; //ML studio api key
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/a86822d3c9fd49cd9af6fbadf0957e5b/services/8779b6b923404215900145f154d73edd/execute?api-version=2.0&details=true");

                // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)


                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    string[] results;

                    // split the resulting string by commas to get individual values, the value we need is second to last
                    results = result.Split(',');
                    result = results[results.Length-2];
                    // split the value by double quotes to get only the number
                    results = result.Split('"');
                    result = results[1];
                    return result!="0";
                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                    System.Diagnostics.Debug.WriteLine(response.Headers.ToString());


                    string responseContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(responseContent);
                    return false;
                }
            }
        }

        public async Task<bool> checkSpam(String message) {
            
            System.Diagnostics.Debug.WriteLine(message);
            try
            {
                bool isSpam = await InvokeRequestResponseService(message);
                System.Diagnostics.Debug.WriteLine(message);
                return isSpam;
            }
            catch(Exception e) {
                System.Diagnostics.Debug.WriteLine(e);
                return false;
            }
            
        }
    }
}