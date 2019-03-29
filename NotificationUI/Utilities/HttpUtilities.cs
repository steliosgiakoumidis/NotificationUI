using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NotificationUI.Utilities
{
    public class HttpUtilities
    {
        public static async Task<IEnumerable<T>> GetAllItems<T>(IHttpClientFactory clientFactory,
            string url)
        {
            var client = clientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5005/api/users");
            return await ParseUtilities.ParseCollectionsResponse<T>(response);  
        }
    }
}