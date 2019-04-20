using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NotificationCommon.Models;
using NotificationUI.Utilities;
using Serilog;

namespace NotificationUI.Controllers
{
    public class UserGroupsController : Controller
    {
        private IHttpClientFactory _clientFactory;
        private static string _userGroupsUri;
        public UserGroupsController(IHttpClientFactory clientFactory, IOptions<Config> config)
        {
            _clientFactory = clientFactory;
            _userGroupsUri = config.Value.UserGroupsUri;
        }

        public async Task<IActionResult> UserGroups()
        {
            IEnumerable<UserGroup> viewResult = null; 
            try
            {
                viewResult = await HttpUtilities.GetAllEntries<UserGroup>(_clientFactory,
                     _userGroupsUri);
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
                var viewResult = await HttpUtilities.GetSpecificEntry<UserGroup>(_clientFactory, 
                    _userGroupsUri, id);
                return View("~/Views/Home/UserGroups/EditUserGroup.cshtml", viewResult);
            }
            catch (Exception ex)
            {
                Log.Error("Edit user request failed with error: "+ ex);
                return NotFound();
            }
        } 
        
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Edit([Bind("Id, GroupName, UserIds")] UserGroup userGroups)
        {
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

        public IActionResult AddUserGroup()
        {
            return View("~/Views/Home/UserGroups/AddUserGroup.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Add([Bind("GroupName, UserIds")] UserGroup userGroup)
        {
            if(!ModelState.IsValid) return StatusCode(500, userGroup);
            var idAddedSuccessfully = await HttpUtilities.AddEntry(_clientFactory,
                 _userGroupsUri, userGroup);
            if(!idAddedSuccessfully)
            {
                Log.Error("Add request failed with unsuccessfull status code");
                return BadRequest();
            }
            return RedirectToAction("UserGroups");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue || id == null) return NotFound();
            try
            {
                var viewResult =  await HttpUtilities.GetSpecificEntry<UserGroup>(_clientFactory, 
                    _userGroupsUri, id);
                return View("~/Views/Home/UserGroups/DeleteUserGroup.cshtml", viewResult);
            }
            catch(Exception ex)
            {
                Log.Error("Delete user group request failed with error: "+ ex);
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Delete(UserGroup userGroup)
        {
            if(!ModelState.IsValid) return StatusCode(500, userGroup);
            if (userGroup.Id == 0) return NotFound();
            try
            {           
                var isDeletedSuccessfully = await HttpUtilities.DeleteEntry(_clientFactory, 
                    _userGroupsUri, userGroup);   
                if (!isDeletedSuccessfully)
                {
                    return NotFound();
                }
                return RedirectToAction("UserGroups");
            }
            catch (Exception ex)
            {
                Log.Error("Delete user request failed with error: "+ ex);
                return StatusCode(500);
            }
        }
    }
}