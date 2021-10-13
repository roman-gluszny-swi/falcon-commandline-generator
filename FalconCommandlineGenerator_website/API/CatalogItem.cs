using System.Collections.Generic;

namespace FalconCommandlineGenerator_website.API
{
    public class CatalogItem
    {
        public string Name { get; set; }
        public CatalogStage CatalogStage { get; set; }
        public ProductStage ProductStage { get; set; }
        public string VersionNumber { get; set; }
        public string BuildNumber { get; set; }
        public List<string> VersionNumbers { get; set; }
        public List<string> BuildNumbers { get; set; }
        public string ProductStageNote { get; set; }
        public bool UpdateOnly { get; set; }
        public CatalogItemType ItemType { get; set; }

        public override string ToString()
        {
            if (BuildNumber != "*")
            {
                return $"{Name}:{BuildNumber}";
            }
            return $"{Name}:{VersionNumber}.*";
        }
    }
}
