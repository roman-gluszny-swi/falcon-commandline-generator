using System;
using System.Collections.Generic;
using FalconCommandlineGenerator_website.API;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FalconCommandlineGenerator_website.Controllers
{
    [Route("api/[controller]")]
    public class StageController : Controller
    {
        [HttpGet("[action]")]
        public IEnumerable<string> CatalogStages()
        {
            return Enum.GetNames(typeof(CatalogStage));
        }

        [HttpGet("[action]")]
        public IEnumerable<string> ProductStages()
        {
            return Enum.GetNames(typeof(ProductStage));
        }
    }
}
