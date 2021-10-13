using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace FalconCommandlineGenerator_website.API
{
    public class CommandlineGenerator : ICommandLineGenerator
    {
        public CommandlineGeneratorModel CommandlineGeneratorModel { get; set; }

        public CommandlineGenerator()
        {
            CommandlineGeneratorModel = new CommandlineGeneratorModel();
        }
        public string GenerateCommandline()
        {
            string commandline = $"/catalogstage={CommandlineGeneratorModel.CatalogStage} ";
            commandline += IterateCatalogItemsVersions("products", CommandlineGeneratorModel.CatalogItems["products"]);
            commandline += IterateCatalogItemsVersions("components", CommandlineGeneratorModel.CatalogItems["components"]);
            commandline += $"/autoupdate={CommandlineGeneratorModel.Autoupdate} ";

            if (CommandlineGeneratorModel.NoTests)
            {
                commandline += "/noTests ";
            }

            if (CommandlineGeneratorModel.Silent)
            {
                commandline += $@"/s /ConfigFile=\config.xml ";
            }

            var productsToInstall = CommandlineGeneratorModel.CatalogItems["products"].Where(x => x.UpdateOnly == false).ToList();
            commandline += IterateCatalogItemsNames("productsToInstall", productsToInstall);
            CommandlineGeneratorModel.CommandLine = commandline;
            return commandline;
        }

        private string IterateCatalogItemsNames(string itemType, List<CatalogItem> catalogItems)
        {
            string commandline = "";
            if (catalogItems.Count >= 1)
            {
                commandline += $"/{itemType}={GetItems(catalogItems)} ";
            }
            return commandline;
        }

        private string GetItems(List<CatalogItem> catalogItems)
        {
            string items = "";
            foreach (var item in catalogItems)
            {
                items += $"{item.Name}";
                if (item != catalogItems.Last())
                {
                    items += ",";
                }
            }

            return items;
        }

        private string IterateCatalogItemsVersions(string itemType, List<CatalogItem> catalogItems)
        {
            string commandline = "";
            if (catalogItems.Count >= 1)
            {
                commandline += $"/{itemType}={GetItemsVersions(catalogItems)} ";

            }
            return commandline;
        }

        private string GetItemsVersions(List<CatalogItem> catalogItems)
        {
            string items = "";
            foreach (var item in catalogItems)
            {
                items += $"{item}";
                if (item != catalogItems.Last())
                {
                    items += ",";
                }
            }
            return items;
        }

        public void AddCatalogItem(CatalogItem product, string itemType)
        {
            if (product == null || (itemType!="products" && itemType!="components"))
                throw new ArgumentNullException();
            CommandlineGeneratorModel.CatalogItems[itemType].Add(product);
        }

        public void CopyToClipboard()
        {
            Thread thread = new Thread(() => Clipboard.SetText(CommandlineGeneratorModel.CommandLine));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        public void GenerateConfig()
        {
            string xml = $@"
<SilentConfig>
    <InstallerConfiguration>
        <CatalogStage>{CommandlineGeneratorModel.CatalogStage}</CatalogStage>
        <ProductStage></ProductStage>
        <ProductsToInstall>{GetItems(CommandlineGeneratorModel.CatalogItems["products"])}</ProductsToInstall>
        <Products>{GetItemsVersions(CommandlineGeneratorModel.CatalogItems["products"])}</Products>
        <Components>{GetItemsVersions(CommandlineGeneratorModel.CatalogItems["components"])}</Components>
        <AdditionalFeatures></AdditionalFeatures>
        <InstallPath></InstallPath>
        <WebConsoleUserName>Admin</WebConsoleUserName>
        <WebConsolePassword></WebConsolePassword>
        <AdvancedInstallMode>False</AdvancedInstallMode>
        <SkipConfigurationWizardRun>False</SkipConfigurationWizardRun>
        <VerifyErrorStrings>finished due to error;Login failed for user;ERROR ConfigurationProgressScene</VerifyErrorStrings>
        <!-- (fill next three properties only when installing on Scalability Engine -->
        
        <IsStandby>false</IsStandby>
    </InstallerConfiguration>
</SilentConfig>
                 ";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            doc.PreserveWhitespace = true;
            doc.Save("config.xml");

        }
    }
}
