using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FalconCommandlineGenerator_website.API;

namespace FalconCommandlineGenerator_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public ObservableCollection<CatalogItem> ProductItems { get; set; }

        public ProductCatalogHelper pch { get; set; }

        public CommandlineGenerator CommandlineGenerator { get; set; }
        public CatalogItemBaseProperties ProductBaseProperties { get; set; }
        public CatalogItemBaseProperties ComponentBaseProperties { get; set; }

        public List<string> ProductNames { get; set; }
        public List<string> ComponentNames { get; set; }
        public ObservableCollection<CatalogStage> CatalogStages { get; set; }
        public ObservableCollection<ProductStage> ProductStages { get; set; }
        public ObservableCollection<FalconCatalogStage> FalconCatalogStages { get; set; }

        public MainWindow()
        {
            CommandlineGenerator= new CommandlineGenerator();
            ComponentBaseProperties = new CatalogItemBaseProperties();
            ProductBaseProperties = new CatalogItemBaseProperties();
            InitializeComponent();
            DataContext = this;

            pch = new ProductCatalogHelper();
            ProductNames=new List<string>(pch.GetAllProductNames());
            ComponentNames=new List<string>(pch.GetAllComponentNames());


            CatalogStages = new ObservableCollection<CatalogStage>(Enum.GetValues(typeof(CatalogStage)).Cast<CatalogStage>());
            ProductStages = new ObservableCollection<ProductStage>(Enum.GetValues(typeof(ProductStage)).Cast<ProductStage>());
            FalconCatalogStages = new ObservableCollection<FalconCatalogStage>(Enum.GetValues(typeof(FalconCatalogStage)).Cast<FalconCatalogStage>());

            ProductsListView.ItemsSource = CommandlineGenerator.CommandlineGeneratorModel.CatalogItems["products"];
            ComponentsListView.ItemsSource = CommandlineGenerator.CommandlineGeneratorModel.CatalogItems["components"];
            CommandLineText.Text = CommandlineGenerator.CommandlineGeneratorModel.CommandLine;
            //NoTestsCheckbox.item
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            CatalogItem newItem = new CatalogItem();
            newItem.Name=pch.GetAllProductNames()[0];
            newItem.CatalogStage = CatalogStage.Ci;
            newItem.ProductStage = ProductStage.Build;
            newItem.ItemType = CatalogItemType.Product;
            CommandlineGenerator.AddCatalogItem(newItem, "products");
            ProductsListView.Items.Refresh();
            RefreshCmd(sender, e);
        }

        private void AddComponentButton_Click(object sender, RoutedEventArgs e)
        {
            CatalogItem newItem = new CatalogItem();
            newItem.Name = pch.GetAllComponentNames()[0];
            newItem.CatalogStage = CatalogStage.Ci;
            newItem.ProductStage = ProductStage.Build;
            newItem.ItemType = CatalogItemType.Component;
            CommandlineGenerator.AddCatalogItem(newItem, "components");
            ComponentsListView.Items.Refresh();
            RefreshCmd(sender, e);
        }

        public static T FindAncestorOrSelf<T>(DependencyObject obj)
            where T : DependencyObject
        {
            while (obj != null)
            {
                T objTest = obj as T;

                if (objTest != null)
                    return objTest;

                obj = VisualTreeHelper.GetParent(obj);
            }
            return null;
        }

        private void UpdateComponentBaseProperties(int index)
        {
            var  item = CommandlineGenerator.CommandlineGeneratorModel.CatalogItems["components"][index];

            if (!string.IsNullOrEmpty(item.Name))
            {
                ComponentBaseProperties.Name = item.Name;
                ComponentBaseProperties.CatalogStage = item.CatalogStage;
                ComponentBaseProperties.ProductStage = item.ProductStage;
                ComponentBaseProperties.ItemType = item.ItemType;
            }
        }

        private void UpdateProductBaseProperties(int index)
        {
            var item = CommandlineGenerator.CommandlineGeneratorModel.CatalogItems["products"][index];

            if (!string.IsNullOrEmpty(item.Name))
            {
                ProductBaseProperties.Name = item.Name;
                ProductBaseProperties.CatalogStage = item.CatalogStage;
                ProductBaseProperties.ProductStage = item.ProductStage;
                ProductBaseProperties.ItemType = item.ItemType;
            }
        }

        private void RefreshVersionNumbers(int index, CatalogItemType type)
        {
            if (type == CatalogItemType.Product)
            {
                CommandlineGenerator.CommandlineGeneratorModel.CatalogItems["products"][index].VersionNumbers = new List<string>(pch.GetAllVersionNumbers(ProductBaseProperties));
            }
            else
            {
                CommandlineGenerator.CommandlineGeneratorModel.CatalogItems["components"][index].VersionNumbers = new List<string>(pch.GetAllVersionNumbers(ComponentBaseProperties));
            }
        }

        private void RefreshBuildNumbers(int index, CatalogItemType type)
        {
            if (type == CatalogItemType.Product)
            {
                var versionNumber = CommandlineGenerator.CommandlineGeneratorModel.CatalogItems["products"][index].VersionNumber;
                CommandlineGenerator.CommandlineGeneratorModel.CatalogItems["products"][index].BuildNumbers = new List<string>(pch.GetAllBuildNumbers(ProductBaseProperties, versionNumber));
                CommandlineGenerator.CommandlineGeneratorModel.CatalogItems["products"][index].ProductStageNote = GetProductStageNote(ProductBaseProperties, CommandlineGenerator.CommandlineGeneratorModel.CatalogItems["products"][index].BuildNumber);
            }
            else
            {
                var versionNumber = CommandlineGenerator.CommandlineGeneratorModel.CatalogItems["components"][index].VersionNumber;
                CommandlineGenerator.CommandlineGeneratorModel.CatalogItems["components"][index].BuildNumbers = new List<string>(pch.GetAllBuildNumbers(ComponentBaseProperties, versionNumber));
                CommandlineGenerator.CommandlineGeneratorModel.CatalogItems["components"][index].ProductStageNote = GetProductStageNote(ComponentBaseProperties, CommandlineGenerator.CommandlineGeneratorModel.CatalogItems["components"][index].BuildNumber);
            }
        }

        private string GetProductStageNote(CatalogItemBaseProperties baseProperties, string buildNumber)
        {
            return pch.GetProductstageNote(baseProperties, buildNumber);
        }

        private int GetDeletedIndex(object sender, EventArgs eventArgs)
        {
            ListViewItem lvItem = FindAncestorOrSelf<ListViewItem>(sender as Button);
            ListView listView = ItemsControl.ItemsControlFromItemContainer(lvItem) as ListView;
            return listView.ItemContainerGenerator.IndexFromContainer(lvItem);
        }

       
        private void Refresh(object sender, EventArgs eventArgs)
        {
            ListViewItem lvItem = FindAncestorOrSelf<ListViewItem>(sender as ComboBox);
            ListView listView = ItemsControl.ItemsControlFromItemContainer(lvItem) as ListView;
            var index =  listView.ItemContainerGenerator.IndexFromContainer(lvItem);
            CatalogItemType type;
            type = listView.Name == "ProductsListView" ? CatalogItemType.Product : CatalogItemType.Component;
            if (listView.Name == "ProductsListView")
            {
                UpdateProductBaseProperties(index);
            }
            else
            {
                UpdateComponentBaseProperties(index);
            }
            RefreshVersionNumbers(index, type);
            RefreshBuildNumbers(index, type);
            CommandLineText.Text=CommandlineGenerator.GenerateCommandline();
            ProductsListView.Items.Refresh();
            ComponentsListView.Items.Refresh();
        }

        private void RefreshCmd(object sender, EventArgs eventArgs)
        {
            CommandLineText.Text = CommandlineGenerator.GenerateCommandline();
        }

        private void DeleteProduct(object sender, EventArgs eventArgs)
        {
            var index = GetDeletedIndex(sender, eventArgs);
            CommandlineGenerator.CommandlineGeneratorModel.CatalogItems["products"].RemoveAt(index);
            CommandLineText.Text = CommandlineGenerator.GenerateCommandline();
            ProductsListView.Items.Refresh();
        }

        private void DeleteComponent(object sender, EventArgs eventArgs)
        {
            var index = GetDeletedIndex(sender, eventArgs);
            CommandlineGenerator.CommandlineGeneratorModel.CatalogItems["components"].RemoveAt(index);
            CommandLineText.Text = CommandlineGenerator.GenerateCommandline();
            ComponentsListView.Items.Refresh();
        }

        private void RunInstaller(object sender, RoutedEventArgs e)
        {
            if (CommandlineGenerator.CommandlineGeneratorModel.Silent)
            {
                CommandlineGenerator.GenerateConfig();
                pch.RunSilentInstaller(CommandlineGenerator.CommandlineGeneratorModel.InstallerCatalogStage.ToString());
            }
            else
            {
                pch.RunInstaller(CommandlineGenerator.CommandlineGeneratorModel.InstallerCatalogStage.ToString(), CommandlineGenerator.CommandlineGeneratorModel.CommandLine);
            }
        }

        private void CopyToClipboard(object sender, RoutedEventArgs e)
        {
            CommandlineGenerator.CopyToClipboard();
        }
    }

}