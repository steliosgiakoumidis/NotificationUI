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
        private static string _userGroupsUri = "http://localhost:5005/api/userGroups/";
        public UserGroupsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> UserGroups()
        {
            IEnumerable<UserGroups> viewResult = null; 
            try
            {
                viewResult = await HttpUtilities.GetAllEntries<UserGroups>(_clientFactory, _userGroupsUri);
            }
            catch(Exception ex)
            {
                Log.Error("Get user groups failed with error: " + ex);
            }
            return View("~/Views/Home/UserGroups/UserGroups.cshtml", viewResult);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue || id == null) return NotFound();
            try
            {
                var viewResult = await HttpUtilities.GetSpecificEntry<UserGroups>(_clientFactory, 
                    _userGroupsUri, id);
                return View("~/Views/Home/UserGroups/EditUserGroups.cshtml", viewResult);
            }
            catch (Exception ex)
            {
                Log.Error("Edit user request failed with error: "+ ex);
                return NotFound();
            }
        } 

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Edit(int? id,
             [Bind("Id, GroupName, USerIds")] UserGroups userGroups)
        {
            if(!id.HasValue || id != userGroups.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {        
                    var isUpdatedSuccessfully = await HttpUtilities.EditEntry(
                            _clientFactory, _userGroupsUri,  userGroups);
                    if (!isUpdatedSuccessfully)
                    {
                        Log.Error("Edit user request returned a non successfull status code");
                        return StatusCode(500);
                    }
                    return RedirectToAction("UserGroups");
                }
                catch(Exception ex)
                {
                    Log.Error("Edit user groups request failed with error: "+ ex);
                    return StatusCode(500);
                }
            }
            return BadRequest("Something was wrong in your details, please retry");
        }




    }
}