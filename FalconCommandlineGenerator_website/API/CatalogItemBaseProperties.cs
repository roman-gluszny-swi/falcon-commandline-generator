namespace FalconCommandlineGenerator_website.API
{
    public class CatalogItemBaseProperties
    {
        public CatalogItemType ItemType;
        public string Name;
        public CatalogStage CatalogStage;
        public ProductStage ProductStage;
    }

    public enum CatalogItemType
    {
        Product=1,
        Component
    }
}
