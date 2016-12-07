using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace tagur.Helpers
{
    public static class StreamHelper
    {
        public async static void SendStreamToAnalyticsAsync(double latitude, double longitude, int count)
        {
            HttpClient client = new HttpClient();

            string analyticsUrl = "[POWER BI APP STREAMING URL CREATED AFTER YOU CREATE AN API]";

            string postData = String.Format("[{{ \"latitude\": {0}, \"longitude\": {1}, \"count\": {2}, \"ts\": \"{3}\" }}]", latitude, longitude, count++, DateTime.Now);
            var response = await client.PostAsync(analyticsUrl, new StringContent(postData));

        }
    }
}

 