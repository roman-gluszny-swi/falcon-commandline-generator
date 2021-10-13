namespace FalconCommandlineGenerator_website.API
{
    public interface ICommandLineGenerator
    {
        /// <summary>
        /// Generates commandline for installer
        /// </summary>
        /// <returns>Commandline</returns>
        string GenerateCommandline();

        /// <summary>
        /// Add CatalogItem of type
        /// </summary>
        /// <param name="item">item</param>
        /// <param name="itemType">itemType</param>
        void AddCatalogItem(CatalogItem item, string itemType);

        /// <summary>
        /// Copies generated commandline to clipboard
        /// </summary>
        void CopyToClipboard();
    }
}
