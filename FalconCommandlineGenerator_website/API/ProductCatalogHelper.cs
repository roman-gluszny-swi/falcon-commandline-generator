using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace FalconCommandlineGenerator_website.API
{
    public class ProductCatalogHelper : IProductCatalogHelper
    {
        private readonly PowerShell ps;

        public ProductCatalogHelper()
        {
            ps = PowerShell.Create();
        }

        public List<string> GetAllProductNames()
        {
            string script =
                @"$response = Invoke-WebRequest -Uri ""https://product-catalog.solarwinds.com/api/web/packages/"" -Method ""POST"" -ContentType ""application/json;charset=UTF-8"" -Body ""{`""PackageType`"":[`""Product`""],`""Advanced`"":[]}""
                    $items= $response.Content | ConvertFrom-Json

                    $names = $items | Select-Object  -ExpandProperty Name
                    $names
                    ";
            return ps.AddScript(script).Invoke().Select(x => x.ToString()).ToList();
        }

        public List<string> GetAllComponentNames()
        {
            string script =
                @"$response = Invoke-WebRequest -Uri ""https://product-catalog.solarwinds.com/api/web/packages/"" -Method ""POST"" -ContentType ""application/json;charset=UTF-8"" -Body ""{`""PackageType`"":[`""Component`""],`""Advanced`"":[]}""
                $items= $response.Content | ConvertFrom-Json
                
                $names = $items | Select-Object  -ExpandProperty Name
                $names
                ";
            return ps.AddScript(script).Invoke().Select(x => x.ToString()).ToList();
        }

        public List<string> GetAllVersionNumbers(CatalogItemBaseProperties baseProperties)
        {
            var productstage = "";
            if (baseProperties.ProductStage != ProductStage.Build)
            {
                productstage = baseProperties.ProductStage.ToString();
            }
            string script =
                $@"$response = Invoke-WebRequest -Uri ""https://product-catalog.solarwinds.com/api/web/versions/[@packageType]"" -Method ""POST"" -ContentType ""application/json;charset=UTF-8"" -Body ""{{`""PackageType`"":[`""{baseProperties.ItemType}`"",`""Update`""],`""CatalogStage`"":`""{baseProperties.CatalogStage}`"",`""Advanced`"":[]}}"" 
                $items= $response.Content | ConvertFrom-Json

                $productStageItems =  $items | Where-Object -FilterScript {{ $_.ProductStage -like ""*{productstage}*""}} 
                $productStageItemsVersions = $productStageItems | Select-Object  -ExpandProperty DisplayVersion
                $productStageItemsVersions
                ";
            script = script.Replace("[@packageType]", baseProperties.Name);

            PowerShell ps = PowerShell.Create();
            var results = ps.AddScript(script).Invoke();
            List<string> strings = results.Distinct().Select(x => x.ToString()).ToList();
            return strings;
        }

        public List<string> GetAllBuildNumbers(CatalogItemBaseProperties baseProperties, string versionNumber)
        {
            var productstage = "";
            if (baseProperties.ProductStage != ProductStage.Build)
            {
                productstage = baseProperties.ProductStage.ToString();
            }
            string script =
                $@"$response = Invoke-WebRequest -Uri ""https://product-catalog.solarwinds.com/api/web/versions/[@packageType]"" -Method ""POST"" -ContentType ""application/json;charset=UTF-8"" -Body ""{{`""PackageType`"":[`""{baseProperties.ItemType}`"",`""Update`""],`""CatalogStage`"":`""{baseProperties.CatalogStage}`"",`""Advanced`"":[]}}"" 
                $items= $response.Content | ConvertFrom-Json

                $productStageItems =  $items | Where-Object -FilterScript {{ $_.ProductStage -like ""*{productstage}*"" -and $_.DisplayVersion -like ""*{versionNumber}*""}} 
                $productStageItemsVersions = $productStageItems | Select-Object  -ExpandProperty Version
                $productStageItemsVersions
                ";
            script = script.Replace("[@packageType]", baseProperties.Name);

            PowerShell ps = PowerShell.Create();
            var results = ps.AddScript(script).Invoke();
            List<string> strings = results.Distinct().Select(x => x.ToString()).ToList();
            return strings;
        }
        public string GetProductstageNote(CatalogItemBaseProperties baseProperties, string buildNumber)
        {
            string script =
                $@"$response = Invoke-WebRequest -Uri ""https://product-catalog.solarwinds.com/api/web/versions/[@packageType]"" -Method ""POST"" -ContentType ""application/json;charset=UTF-8"" -Body ""{{`""PackageType`"":[`""{baseProperties.ItemType}`"",`""Update`""],`""CatalogStage`"":`""{baseProperties.CatalogStage}`"",`""Advanced`"":[]}}"" 
                $items= $response.Content | ConvertFrom-Json

                $productStageItems =  $items | Where-Object -FilterScript {{ $_.Version -eq ""{buildNumber}""}} 
                $productStageNote = $productStageItems | Select-Object  -ExpandProperty ProductStage
                $productStageNote
                ";
            script = script.Replace("[@packageType]", baseProperties.Name);

            PowerShell ps = PowerShell.Create();
            var results = ps.AddScript(script).Invoke();
            var note = results.Distinct().Select(x => x.ToString()).FirstOrDefault();
            return note;
        }


        public CatalogItem GetCatalogItem(CatalogItemBaseProperties baseProperties, string versionNumber, string buildNumber, bool updateOnly)
        {
            var item = new CatalogItem()
            {
                Name = baseProperties.Name,
                BuildNumber = buildNumber,
                VersionNumber = versionNumber,
                CatalogStage = baseProperties.CatalogStage,
                ProductStage = baseProperties.ProductStage,
                UpdateOnly = updateOnly
            };
            return item;
        }

        public void RunInstaller(string stage, string commandLineText)
        {
            string script =
                $@"$response = Invoke-WebRequest -Uri ""https://product-catalog.solarwinds.com/api/installer/[@stage]"" -Method ""GET"" -OutFile installer.exe
                ";
            script = script.Replace("[@stage]", stage);


            PowerShell ps = PowerShell.Create();
            ps.AddScript(script).Invoke();
            Process.Start("CMD.exe", $"/c start installer.exe {commandLineText}");
        }

        public void RunSilentInstaller(string stage)
        {
            string script =
                $@"Invoke-WebRequest -Uri ""https://product-catalog.solarwinds.com/api/installer/[@stage]"" -Method ""GET"" -OutFile installer.exe
                ";
            script = script.Replace("[@stage]", stage);

            PowerShell ps = PowerShell.Create();
            ps.AddScript(script).Invoke();
            var args = $@"/c start /wait installer.exe /s /ConfigFile=""{Directory.GetCurrentDirectory()}\config.xml""";
            System.Diagnostics.Process.Start("CMD.exe", args);
        }
    }
}
