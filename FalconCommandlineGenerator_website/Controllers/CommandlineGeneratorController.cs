using System.Collections.Generic;
using FalconCommandlineGenerator_website.API;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FalconCommandlineGenerator_website.Controllers
{
    [Route("api/[controller]")]
    public class CommandlineGeneratorController : Controller
    {
        private readonly CommandlineGenerator _commandlineGenerator;
        private readonly ProductCatalogHelper _productCatalogHelper;

        public CommandlineGeneratorController()
        {
            _commandlineGenerator = new CommandlineGenerator();
            _productCatalogHelper = new ProductCatalogHelper();
        }

        [HttpPost("[action]")]
        public List<string> Generate([FromBody] CommandlineGeneratorModel commandlineGeneratorModel)
        {
            _commandlineGenerator.CommandlineGeneratorModel = commandlineGeneratorModel;
            var commandline = _commandlineGenerator.GenerateCommandline();
            var result = new List<string>();
            result.Add(commandline);
            return result;
        }

        [HttpPost("[action]")]
        public IActionResult Install([FromBody] CommandlineGeneratorModel commandlineGeneratorModel)
        {
            if (commandlineGeneratorModel.Silent)
            {
                _commandlineGenerator.CommandlineGeneratorModel = commandlineGeneratorModel;
                _commandlineGenerator.GenerateConfig();
                _productCatalogHelper.RunSilentInstaller(commandlineGeneratorModel.InstallerCatalogStage.ToString());
            }
            else
            {
                _productCatalogHelper.RunInstaller(commandlineGeneratorModel.InstallerCatalogStage.ToString(), commandlineGeneratorModel.CommandLine);
            }
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Copy([FromBody] CommandlineGeneratorModel commandlineGeneratorModel)
        {
            _commandlineGenerator.CommandlineGeneratorModel = commandlineGeneratorModel;
            _commandlineGenerator.CopyToClipboard();
            return Ok();
        }
    }
}
