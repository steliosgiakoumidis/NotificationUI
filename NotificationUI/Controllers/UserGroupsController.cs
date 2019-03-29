using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotificationUI.Models;
using NotificationUI.Utilities;
using Serilog;

namespace NotificationUI.Controllers
{
    public class UserGroupsController : Controller
    {
        private IHttpClientFactory _clientFactory;
        private static string _userGroupsUri = "http://localhost:5005/api/userGroups";
        public UserGroupsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> GetAllGroups()
        {
            try
            {
                IEnumerable<UserGroups> viewResult = null; 
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync(_userGroupsUri);
                viewResult = await ParseUtilities.ParseCollectionsResponse<UserGroups>(response);
            }catch(Exception ex)
            {
                Log.Error("Get user groups failed with error: " + ex);
            }
            return View();//The correct view
        }




    }
}