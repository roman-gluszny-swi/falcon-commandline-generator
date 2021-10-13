using System.Collections.Generic;

namespace FalconCommandlineGenerator_website.API
{
    interface IProductCatalogHelper
    {
        /// <summary>
        /// Returns list of all products in catalog
        /// </summary>
        /// <returns>Products</returns>
        List<string> GetAllProductNames();

        /// <summary>
        /// Returns list of all components in catalog
        /// </summary>
        /// <returns>Components</returns>
        List<string> GetAllComponentNames();

        /// <summary>
        /// Return all major versions for item with properties
        /// </summary>
        /// <param name="baseProperties"></param>
        /// <returns>Major versions</returns>
        List<string> GetAllVersionNumbers(CatalogItemBaseProperties baseProperties);

        /// <summary>
        /// Return all minor versions for item with properties
        /// </summary>
        /// <param name="baseProperties"></param>
        /// <param name="versionNumber"></param>
        /// <returns>Minor versions</returns>
        List<string> GetAllBuildNumbers(CatalogItemBaseProperties baseProperties, string versionNumber);

        /// <summary>
        /// Returns Catalog item with properties
        /// </summary>
        /// <param name="baseProperties"></param>
        /// <param name="versionNumber"></param>
        /// <param name="buildNumber"></param>
        /// <returns>Catalog Item</returns>
        CatalogItem GetCatalogItem(CatalogItemBaseProperties baseProperties, string versionNumber, string buildNumber, bool updateOnly);
    }
}
