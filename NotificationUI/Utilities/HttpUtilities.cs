using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NotificationUI.Utilities
{
    public class HttpUtilities
    {
        public static async Task<IEnumerable<T>> GetAllEntries<T>(IHttpClientFactory clientFactory,
            string url)
        {
            var client = clientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5005/api/users");
            return await ParseUtilities.ParseCollectionsResponse<T>(response);  
        }

        public static async Task<T> GetSpecificEntry<T>(IHttpClientFactory clientFactory,
            string url, int? id) 
        {
            var client = clientFactory.CreateClient();
            var response = await client.GetAsync(
                "http://localhost:5005/api/users/" + id.Value);
            return await ParseUtilities.ParseResponse<T>(response);
        }

        public static async Task<bool> EditEntry<T>(IHttpClientFactory clientFactory,
             string url, T updatedEntry)
        {
        var httpContent = ParseUtilities.PrepareHttpContent(updatedEntry);
        var client = clientFactory.CreateClient();             
        var response = await client.PutAsync(
            "http://localhost:5005/api/users", httpContent);
        return response.IsSuccessStatusCode;
        }

        public static async Task<bool> AddEntry<T>(IHttpClientFactory clientFactory,
            string url, T entryToBeAdded)
        {
            var httpContent = ParseUtilities.PrepareHttpContent(entryToBeAdded);
            var client = clientFactory.CreateClient();             
            var response = await client.PostAsync("http://localhost:5005/api/users", 
                httpContent);
            return response.IsSuccessStatusCode;
        }
        
        public static async Task<bool> DeleteEntry(IHttpClientFactory clientFactory,
            string url, T entryToBeDeleted, int? id)
            {
                var httpContent = ParseUtilities.PrepareHttpContent(entryToBeDeleted);
                var client = clientFactory.CreateClient();             
                var response = await client.PostAsync(
                    "http://localhost:5005/api/users/" + id, httpContent); 
                return response.IsSuccessStatusCode;
            }
    }

}