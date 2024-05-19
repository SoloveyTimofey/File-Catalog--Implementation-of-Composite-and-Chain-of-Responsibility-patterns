using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using DirectoryCatalog.Models;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using DirectoryCatalog.Composite;
using DirectoryCatalog.Repositories;

namespace DirectoryCatalog
{
    /// <summary>
    /// Логика взаимодействия для CreateCatalogItemWindow.xaml
    /// </summary>
    public partial class CreateCatalogItemWindow : Window
    {
        private List<string> Types = new List<string>
        {
            "Директорія",
            "Системне",
            "Офісне",
            "Ігрове",
            "Мультимедія"
        };
        private CatalogItemCreated catalogItemCreated;
        private DirectoryItem owner;
        public CreateCatalogItemWindow(CatalogItemCreated catalogItemCreated, DirectoryItem owner)
        {
            InitializeComponent();

            this.catalogItemCreated += catalogItemCreated;

            this.owner = owner;
            TypeComboBox.ItemsSource = Types;
        }

        private void CreateCatalogItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(ItemNameTextBox.Text))
            {
                MessageBox.Show("Оберіть ім'я файлу");
                return;
            }

            string itemName = ItemNameTextBox.Text;
            CatalogItem catalogItem;
            switch (TypeComboBox.SelectedItem)
            {
                case "Директорія":
                    catalogItem = new DirectoryItem(itemName, owner);
                    break;
                case "Системне":
                    catalogItem = new FileItem(itemName, owner, ItemType.System);
                    break;
                case "Офісне":
                    catalogItem = new FileItem(itemName, owner, ItemType.Office);
                    break;
                case "Ігрове":
                    catalogItem = new FileItem(itemName, owner, ItemType.Gaming);
                    break;
                case "Мультимедія":
                    catalogItem = new FileItem(itemName, owner, ItemType.Multimedia);
                    break;
                default:
                    MessageBox.Show("Оберіть тип файлу.");
                    return;
            }

            //owner.Add(catalogItem);

            catalogItemCreated();

            this.Close();
        }
    }
}
