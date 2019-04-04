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
    public class RegularSendoutsController : Controller
    {
        private IHttpClientFactory _clientFactory;
        private static string _regularSendoutUri = "http://localhost:5005/api/regularsendout/";
        public RegularSendoutsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> RegularSendouts()
        {
            IEnumerable<RegularSendout> viewResult = null;
            try
            {
                viewResult = await HttpUtilities.GetAllEntries<RegularSendout>(_clientFactory,
                        _regularSendoutUri);              
            }
            catch (Exception ex)
            {
                Log.Error("Get templates failerd with error: " + ex);
            }
            return View("~/Views/Home/RegularSendouts/RegularSendouts.cshtml", viewResult);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue || id == null) return NotFound();
            try
            {
                var viewResult = await HttpUtilities.GetSpecificEntry<RegularSendout>(_clientFactory, 
                    _regularSendoutUri, id);
                return View("~/Views/Home/RegularSendouts/EditSendout.cshtml", viewResult);
            }
            catch (Exception ex)
            {
                Log.Error("Edit user request failed with error: "+ ex);
                return NotFound();
            }
        } 
        
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Edit(
            [Bind("Id, ReminderName, StartDate, RepetitionFrequency, ExecutionTime, DayOfTheWeek, LastRunAt, Parameters, Username, UserGroup, Priority")] 
            RegularSendout sendout)
        {
            if (ModelState.IsValid)
            {
                try
                {        
                    var isUpdatedSuccessfully = await HttpUtilities.EditEntry(
                            _clientFactory, _regularSendoutUri, sendout);
                    if (!isUpdatedSuccessfully)
                    {
                        Log.Error("Edit template request returned a non successfull status code");
                        return StatusCode(500);
                    }
                    return RedirectToAction("RegularSendouts");
                }
                catch(Exception ex)
                {
                    Log.Error("Edit template request failed with error: "+ ex);
                    return StatusCode(500);
                }
            }
            return BadRequest("Something was wrong with your details, please retry");
        }

        public async Task<IActionResult> Add()
        {
            return View("~/Views/Home/RegularSendouts/AddSendout.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Add(
            [Bind("ReminderName, StartDate, RepetitionFrequency, ExecutionTime, DayOfTheWeek, LastRunAt, Parameters, Username, UserGroup, Priority")]
            RegularSendout sendout)
        {
            if(!ModelState.IsValid) return StatusCode(500, sendout);

            var isAddedSuccessfully = await HttpUtilities.AddEntry(_clientFactory,
                 _regularSendoutUri, sendout);
            if(!isAddedSuccessfully)
            {
                Log.Error("Add request failed with unsuccessfull status code");
                return BadRequest("Add request failed with unsuccessfull status code, please check your inputs");
            }
            return RedirectToAction("RegularSendouts");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue || id == null) return NotFound();
            try
            {
                var viewResult =  await HttpUtilities.GetSpecificEntry<RegularSendout>(_clientFactory,
                    _regularSendoutUri, id);
                return View("~/Views/Home/RegularSendouts/DeleteSendout.cshtml", viewResult);
            }
            catch(Exception ex)
            {
                Log.Error("Delete user group request failed with error: "+ ex);
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Delete(RegularSendout sendout)
        {
            if(!ModelState.IsValid) return StatusCode(500, sendout);
            if (sendout.Id == 0) return NotFound();
            try
            {           
                var isDeletedSuccessfully = await HttpUtilities.DeleteEntry(_clientFactory, 
                    _regularSendoutUri, sendout);   
                if (!isDeletedSuccessfully)
                {
                    return NotFound();
                }
                return RedirectToAction("RegularSendouts");
            }
            catch (Exception ex)
            {
                Log.Error("Delete user request failed with error: "+ ex);
                return StatusCode(500);
            }
        }
        
    }
}