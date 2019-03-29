using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NotificationUI.Models;
using NotificationUI.Utilities;
using Serilog;

namespace NotificationUI.Controllers
{
    public class UsersController : Controller
    {
        private IHttpClientFactory _clientFactory;
        private string _usersUri = "http://localhost:5005/api/users";

        public UsersController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

         public async Task<IActionResult> Users()
        {
            IEnumerable<User> viewResult = null;
            try
            {
                // var client = _clientFactory.CreateClient();
                // var response = await client.GetAsync("http://localhost:5005/api/users");
                viewResult = await HttpUtilities.GetAllItems<User>(_clientFactory, _usersUri);//await ParseUtilities.ParseCollectionsResponse<User>(response);                
            }
            catch (Exception ex)
            {                   
                Log.Error("Get users request failed with error: " + ex);
            }                        
            return View("~/Views/Home/Users/Users.cshtml", viewResult);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue || id == null)
            {
                return NotFound();
            }
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync(
                    "http://localhost:5005/api/users/" + id.Value);
                var viewResult = ParseUtilities.ParseResponse<User>(response);
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
            if(!id.HasValue || id != user.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var httpContent = ParseUtilities.PrepareHttpContent(user);
                    var client = _clientFactory.CreateClient();             
                    var response = await client.PutAsync(
                        "http://localhost:5005/api/users", httpContent);
                    if (!response.IsSuccessStatusCode)
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

        public async Task<IActionResult> AddUser()
        {
            return View("~/Views/Home/Users/AddUser.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Add([Bind("Username, Email, Facebook")] User user)
        {
            if (ModelState.IsValid)
            {
                var httpContent = ParseUtilities.PrepareHttpContent(user);
                var client = _clientFactory.CreateClient();
                var response = await client.PostAsync("http://localhost:5005/api/users", 
                    httpContent);
                if (!response.IsSuccessStatusCode)
                {
                    Log.Error("Add request failed with unsuccessfull status code");
                    return BadRequest();
                }
                return RedirectToAction("Users");
            }
            return BadRequest("Something was wrong in your details, please retry");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue || id == null)
            {
                return NotFound();
            }
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync(
                    "http://localhost:5005/api/users/" + id.Value);
                var viewResult = ParseUtilities.ParseResponse<User>(response);
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
        public async Task<IActionResult> DeleteConfirmed(int? id, 
            [Bind("Id, Username, Email, Facebook")] User user)
        {
            if (!id.HasValue || id == null)
            {
                return NotFound();
            }
            try
            {
                var httpContent = ParseUtilities.PrepareHttpContent(user);
                var client = _clientFactory.CreateClient();             
                var response = await client.PostAsync(
                    "http://localhost:5005/api/users/" + id, httpContent);       
                if (response.IsSuccessStatusCode == false)
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