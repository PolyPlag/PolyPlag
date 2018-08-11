using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocAPI.Models.TextAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Test")]
    public class TestController : Controller
    {
        // GET api/docs 
        //temporary has to be removed later, only debug
        [HttpGet]
        public IActionResult Get()
        {
            AzureTextAnalytics azureTextAnalytics = new AzureTextAnalytics();
            List<string> origin = new List<string>();
            origin.Add("Test");

            return new ObjectResult(azureTextAnalytics.Check(origin, new List<Uri>(), new List<string>()));
        }
    }
}