using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NotificationUI.Models;
using NotificationUI.Utilities;
using Serilog;

namespace NotificationUI.Controllers
{
    public class TemplatesController : Controller
    {
        public IHttpClientFactory _clientFactory;
        private static string _templatesUri;

        public TemplatesController(IHttpClientFactory clientFactory, IOptions<Config> config)
        {
            _clientFactory = clientFactory;
            _templatesUri = config.Value.TemplatesUri;
        }

        public async Task<IActionResult> Templates()
        {
            IEnumerable<Template> viewResult = null;
            try
            {
                viewResult = await HttpUtilities.GetAllEntries<Template>(_clientFactory,
                        _templatesUri);
               
            }
            catch (Exception ex)
            {
                Log.Error("Get templates failerd with error: " + ex);
            }
            return View("~/Views/Home/Templates/Templates.cshtml", viewResult);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue || id == null) return NotFound();
            try
            {
                var viewResult = await HttpUtilities.GetSpecificEntry<Template>(_clientFactory, 
                    _templatesUri, id);
                return View("~/Views/Home/Templates/EditTemplate.cshtml", viewResult);
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
            [Bind("Id, NotificationText, NotificationName, NotificationPriority")] 
            Template template)
        {
            if (ModelState.IsValid)
            {
                try
                {        
                    var isUpdatedSuccessfully = await HttpUtilities.EditEntry(
                            _clientFactory, _templatesUri, template);
                    if (!isUpdatedSuccessfully)
                    {
                        Log.Error("Edit template request returned a non successfull status code");
                        return StatusCode(500);
                    }
                    return RedirectToAction("Templates");
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
            return View("~/Views/Home/Templates/AddTemplate.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Add([Bind("NotificationText, NotificationName, NotificationPriority")]
            Template template)
        {
            if(!ModelState.IsValid) return StatusCode(500, template);
            var idAddedSuccessfully = await HttpUtilities.AddEntry(_clientFactory,
                 _templatesUri, template);
            if(!idAddedSuccessfully)
            {
                Log.Error("Add request failed with unsuccessfull status code");
                return BadRequest();
            }
            return RedirectToAction("Templates");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue || id == null) return NotFound();
            try
            {
                var viewResult =  await HttpUtilities.GetSpecificEntry<Template>(_clientFactory, 
                    _templatesUri, id);
                return View("~/Views/Home/Templates/DeleteTemplate.cshtml", viewResult);
            }
            catch(Exception ex)
            {
                Log.Error("Delete user group request failed with error: "+ ex);
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Delete(Template template)
        {
            if(!ModelState.IsValid) return StatusCode(500, template);
            if (template.Id == 0) return NotFound();
            try
            {           
                var isDeletedSuccessfully = await HttpUtilities.DeleteEntry(_clientFactory, 
                    _templatesUri, template);   
                if (!isDeletedSuccessfully)
                {
                    return NotFound();
                }
                return RedirectToAction("Templates");
            }
            catch (Exception ex)
            {
                Log.Error("Delete user request failed with error: "+ ex);
                return StatusCode(500);
            }
        }
    }
}