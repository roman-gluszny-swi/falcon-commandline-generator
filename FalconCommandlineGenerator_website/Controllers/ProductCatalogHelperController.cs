using System;
using System.Collections.Generic;
using FalconCommandlineGenerator_website.API;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FalconCommandlineGenerator_website.Controllers
{
    [Route("api/[controller]")]
    public class ProductCatalogHelperController : Controller
    {
        private readonly ProductCatalogHelper _productCatalogHelper;

        public ProductCatalogHelperController()
        {
            _productCatalogHelper = new ProductCatalogHelper();
        }

        [HttpGet("[action]")]
        public IEnumerable<string> AllProductNames()
        {
            return _productCatalogHelper.GetAllProductNames();
        }

        [HttpGet("[action]")]
        public IEnumerable<string> AllComponentNames()
        {
            return _productCatalogHelper.GetAllComponentNames();
        }

        [HttpGet("[action]/{name}/{catalogStage}/{productStage}/{itemType}")]
        public IEnumerable<string> VersionNumbers(string name, string catalogStage, string productStage, string itemType)
        {
            itemType = itemType.Remove(itemType.Length - 1);
            var baseProps = new CatalogItemBaseProperties();
            baseProps.Name = name;
            baseProps.ItemType = (CatalogItemType)Enum.Parse(typeof(CatalogItemType), itemType, true);
            baseProps.CatalogStage = (CatalogStage)Enum.Parse(typeof(CatalogStage), catalogStage, true);
            baseProps.ProductStage = (ProductStage)Enum.Parse(typeof(ProductStage), productStage, true);
            return _productCatalogHelper.GetAllVersionNumbers(baseProps);
        }

        [HttpGet("[action]/{name}/{catalogStage}/{productStage}/{versionNumber}/{itemType}")]
        public IEnumerable<string> BuildNumbers(string name, string catalogStage, string productStage, string versionNumber, string itemType)
        {
            itemType = itemType.Remove(itemType.Length - 1);
            var baseProps = new CatalogItemBaseProperties();
            baseProps.Name = name;
            baseProps.ItemType = (CatalogItemType)Enum.Parse(typeof(CatalogItemType), itemType, true);
            baseProps.CatalogStage = (CatalogStage)Enum.Parse(typeof(CatalogStage), catalogStage, true);
            baseProps.ProductStage = (ProductStage)Enum.Parse(typeof(ProductStage), productStage, true);
            return _productCatalogHelper.GetAllBuildNumbers(baseProps, versionNumber);
        }

        [HttpGet("[action]/{name}/{catalogStage}/{productStage}/{buildNumber}/{itemType}")]
        public IEnumerable<string> Note(string name, string catalogStage, string productStage, string buildNumber, string itemType)
        {
            itemType = itemType.Remove(itemType.Length - 1);
            var baseProps = new CatalogItemBaseProperties();
            baseProps.Name = name;
            baseProps.ItemType = (CatalogItemType)Enum.Parse(typeof(CatalogItemType), itemType, true);
            baseProps.CatalogStage = (CatalogStage)Enum.Parse(typeof(CatalogStage), catalogStage, true);
            baseProps.ProductStage = (ProductStage)Enum.Parse(typeof(ProductStage), productStage, true);
            var note = _productCatalogHelper.GetProductstageNote(baseProps, buildNumber);
            var res = new List<string>
            {
                note
            };
            return res;
        }
    }
}
