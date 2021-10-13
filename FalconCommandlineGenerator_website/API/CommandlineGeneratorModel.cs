using System.Collections.Generic;

namespace FalconCommandlineGenerator_website.API
{
    public class CommandlineGeneratorModel
    {
        public CommandlineGeneratorModel()
        {
            CatalogItems = new Dictionary<string, List<CatalogItem>>()
            {
                { "products",new List<CatalogItem>()},
                { "components",new List<CatalogItem>()}
            };
            CatalogStage = FalconCatalogStage.CI;
            InstallerCatalogStage = FalconCatalogStage.CI;
            Autoupdate = true;
            NoTests = false;
            Silent = false;
        }

        public Dictionary<string, List<CatalogItem>> CatalogItems { get; set; }
        public FalconCatalogStage CatalogStage { get; set; }
        public FalconCatalogStage InstallerCatalogStage { get; set; }
        public bool Autoupdate { get; set; }
        public bool NoTests { get; set; }
        public bool Silent { get; set; }
        public string CommandLine { get; set; }
    }
}