using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NotificationUI.Models;
using NotificationUI.Utilities;
using Serilog;

namespace NotificationUI.Controllers
{
    public class UsersController : Controller
    {
        private IHttpClientFactory _clientFactory;
        private string _usersUri;

        public UsersController(IHttpClientFactory clientFactory, IOptions<Config> config)
        {
            _clientFactory = clientFactory;
            _usersUri = config.Value.UsersUri;
        }

        public async Task<IActionResult> Users()
        {
            IEnumerable<User> viewResult = null;
            try
            {
                viewResult = await HttpUtilities.GetAllEntries<User>(_clientFactory, _usersUri);                
            }
            catch (Exception ex)
            {                   
                Log.Error("Get users request failed with error: " + ex);
            }                        
            return View("~/Views/Home/Users/Users.cshtml", viewResult);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue || id == null) return NotFound();
            try
            {
                var viewResult = await HttpUtilities.GetSpecificEntry<User>(_clientFactory, 
                    _usersUri, id);
                return View("~/Views/Home/Users/EditUser.cshtml", viewResult);
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
            [Bind("Id, Username, Email, Facebook")] User user)
        {
            if(!id.HasValue || id != user.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {        
                    var isUpdatedSuccessfully = await HttpUtilities.EditEntry(
                            _clientFactory, _usersUri,  user);
                    if (!isUpdatedSuccessfully)
                    {
                        Log.Error("Edit user request returned a non successfull status code");
                        return StatusCode(500);
                    }
                    return RedirectToAction("Users");
                }
                catch(Exception ex)
                {
                    Log.Error("Edit user request failed with error: "+ ex);
                    return StatusCode(500);
                }
            }
            return BadRequest("Something was wrong in your details, please retry");
        }

        public IActionResult Add()
        {
            return View("~/Views/Home/Users/AddUser.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Add([Bind("Username, Email, Facebook")] User user)
        {
            if (!ModelState.IsValid) return StatusCode(500, user);   
            try
            {
                var idAddedSuccessfully = await HttpUtilities.AddEntry(
                    _clientFactory, _usersUri, user);
                if (!idAddedSuccessfully)
                {
                    Log.Error("Add request failed with unsuccessfull status code");
                    return BadRequest();
                }
                return RedirectToAction("Users"); 
            }
            catch(Exception ex)
            {
                Log.Error("Add user request failed with error: "+ ex);
                return StatusCode(500);
            }                     
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue || id == null) return NotFound();
            try
            {
                var viewResult =  await HttpUtilities.GetSpecificEntry<User>(_clientFactory, 
                    _usersUri, id);
                return View("~/Views/Home/Users/Deleteuser.cshtml", viewResult);
            }
            catch(Exception ex)
            {
                Log.Error("Delete user request failed with error: "+ ex);
                return NotFound();
            }     
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(User user)
        {
            if(!ModelState.IsValid) return StatusCode(500, user);
            if (user.Id == 0) return NotFound();
            try
            {           
                var isDeletedSuccessfully = await HttpUtilities.DeleteEntry(_clientFactory, 
                    _usersUri, user);   
                if (!isDeletedSuccessfully)
                {
                    return NotFound();
                }
                return RedirectToAction("Users");
            }
            catch (Exception ex)
            {
                Log.Error("Delete user request failed with error: "+ ex);
                return StatusCode(500);
            }           
        }
    }
}