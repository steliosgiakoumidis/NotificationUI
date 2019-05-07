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
    public class SendoutsController : Controller
    {
        private IHttpClientFactory _clientFactory;
        private static string _sendoutUri;
        private string _notificationServiceUri;
        public SendoutsController(IHttpClientFactory clientFactory, IOptions<Config> config)
        {
            _clientFactory = clientFactory;
            _sendoutUri = config.Value.SendoutUri;
            _notificationServiceUri = config.Value.NotificationServiceUri;
        }

        public async Task<IActionResult> Sendouts()
        {
            IEnumerable<Sendout> viewResult = null;
            try
            {
                viewResult = await HttpUtilities.GetAllEntries<Sendout>(_clientFactory,
                        _sendoutUri);              
            }
            catch (Exception ex)
            {
                Log.Error("Get templates failerd with error: " + ex);
            }
            return View("~/Views/Home/Sendouts/Sendouts.cshtml", viewResult);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue || id == null) return NotFound();
            try
            {
                var viewResult = await HttpUtilities.GetSpecificEntry<Sendout>(_clientFactory, 
                    _sendoutUri, id);
                return View("~/Views/Home/Sendouts/EditSendout.cshtml", viewResult);
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
            Sendout sendout)
        {
            if (ModelState.IsValid)
            {
                try
                {        
                    var isUpdatedSuccessfully = await HttpUtilities.EditEntry(
                            _clientFactory, _sendoutUri, sendout);
                    if (!isUpdatedSuccessfully)
                    {
                        Log.Error("Edit template request returned a non successfull status code");
                        return StatusCode(500);
                    }
                    return RedirectToAction("Sendouts");
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
            return View("~/Views/Home/Sendouts/AddSendout.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Add(
            [Bind("ReminderName, StartDate, RepetitionFrequency, ExecutionTime, DayOfTheWeek, LastRunAt, Parameters, Username, UserGroup, Priority")]
            Sendout sendout)
        {
            if(!ModelState.IsValid) return StatusCode(500, sendout);

            var isAddedSuccessfully = await HttpUtilities.AddEntry(_clientFactory,
                 _sendoutUri, sendout);
            if(!isAddedSuccessfully)
            {
                Log.Error("Add request failed with unsuccessfull status code");
                return BadRequest("Add request failed with unsuccessfull status code, please check your inputs");
            }
            if (sendout.RepetitionFrequency == Enums.RepetitionFrequency.Now) await HttpUtilities.AddEntry(_clientFactory, _notificationServiceUri, sendout);
            return RedirectToAction("Sendouts");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue || id == null) return NotFound();
            try
            {
                var viewResult =  await HttpUtilities.GetSpecificEntry<Sendout>(_clientFactory,
                    _sendoutUri, id);
                return View("~/Views/Home/Sendouts/DeleteSendout.cshtml", viewResult);
            }
            catch(Exception ex)
            {
                Log.Error("Delete user group request failed with error: "+ ex);
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Delete(Sendout sendout)
        {
            if(!ModelState.IsValid) return StatusCode(500, sendout);
            if (sendout.Id == 0) return NotFound();
            try
            {           
                var isDeletedSuccessfully = await HttpUtilities.DeleteEntry(_clientFactory, 
                    _sendoutUri, sendout);   
                if (!isDeletedSuccessfully)
                {
                    return NotFound();
                }
                return RedirectToAction("Sendouts");
            }
            catch (Exception ex)
            {
                Log.Error("Delete user request failed with error: "+ ex);
                return StatusCode(500);
            }
        }
        
    }
}