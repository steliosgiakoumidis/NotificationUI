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
            var response = await client.GetAsync(url);
            return await ParseUtilities.ParseCollectionsResponse<T>(response);  
        }

        public static async Task<T> GetSpecificEntry<T>(IHttpClientFactory clientFactory,
            string url, int? id) 
        {
            var client = clientFactory.CreateClient();
            var response = await client.GetAsync(
                url + id.Value);
            return await ParseUtilities.ParseResponse<T>(response);
        }

        public static async Task<bool> EditEntry<T>(IHttpClientFactory clientFactory,
             string url, T updatedEntry)
        {
        var httpContent = ParseUtilities.PrepareHttpContent(updatedEntry);
        var client = clientFactory.CreateClient();             
        var response = await client.PutAsync(
            url, httpContent);
        return response.IsSuccessStatusCode;
        }

        public static async Task<bool> AddEntry<T>(IHttpClientFactory clientFactory,
            string url, T entryToBeAdded)
        {
            var httpContent = ParseUtilities.PrepareHttpContent(entryToBeAdded);
            var client = clientFactory.CreateClient();             
            var response = await client.PostAsync(url, 
                httpContent);
            return response.IsSuccessStatusCode;
        }
        
        public static async Task<bool> DeleteEntry<T>(IHttpClientFactory clientFactory,
            string url, T entryToBeDeleted)
            {
                url = url + "delete";
                var httpContent = ParseUtilities.PrepareHttpContent(entryToBeDeleted);
                var client = clientFactory.CreateClient();             
                var response = await client.PostAsync(
                    url, httpContent); 
                return response.IsSuccessStatusCode;
            }
    }

}