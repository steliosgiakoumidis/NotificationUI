using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IdentityModel;
using NotificationUI.Models;
using System;

namespace NotificationUI.Utilities
{
    public class ParseUtilities
    {
        public async static Task<IEnumerable<T>> ParseCollectionsResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode == false)
            {
                return new List<T>();
            }
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<T>>(result);      
        }

        public async static Task<T> ParseResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode == false)
            {
                throw new HttpRequestException("Request status is unsuccessfull");
            }
            var result = await response.Content.ReadAsStringAsync();
            var viewResult = JsonConvert.DeserializeObject<T>(result);
            if (viewResult == null)
            {
                throw new Exception("Database response is empty");
            }      
            return viewResult;
        }

        public static StringContent PrepareHttpContent<T>(T item)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(item).ToString());
            httpContent.Headers.ContentType = 
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return httpContent;

        }
    }
}