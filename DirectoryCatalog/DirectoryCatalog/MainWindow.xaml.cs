using DirectoryCatalog.ChainOfResponsibility;
using DirectoryCatalog.Composite;
using DirectoryCatalog.Repositories;
using DirectoryCatalog.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DirectoryCatalog
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CatalogRepository catalogRepository { get; set; }
        private DirectoryItem rootDirectory { get; set; }
        private List<string> Types = new List<string>
        {
            "Усі",
            "Директорія",
            "Системне",
            "Офісне",
            "Ігрове",
            "Мультимедія"
        };
        public MainWindow()
        {
            catalogRepository = CatalogRepository.GetInstance();

            InitializeComponent();

            rootDirectory = catalogRepository.GetCatalogItems();

            TreeViewItem rootTreeViewItem = new TreeViewItem ();

            PopulateTreeView(rootTreeViewItem);

            TypeComboBox.ItemsSource = Types;
        }

        private void PopulateTreeView(TreeViewItem rootTreeViewItem)
        {
            #region AddingControls
            StackPanel stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(new Label { Content = rootDirectory.Name, Foreground = new SolidColorBrush(GetForegroundColor(rootDirectory)) });
            Button addButton = new Button { Content = "Додати", Width = 65, Height = 20, FontSize = 10, Background = new SolidColorBrush(Colors.Transparent) };
            addButton.Click += (sender, e) => OpenCreateCatalogWindow(sender, e, rootDirectory);
            stackPanel.Children.Add(addButton);

            rootTreeViewItem.Header = stackPanel;
            #endregion

            BuildTreeView(rootDirectory, rootTreeViewItem);

            CatalogTreeView.Items.Clear();
            CatalogTreeView.Items.Add(rootTreeViewItem);
            rootTreeViewItem.IsExpanded = true;
        }

        private void BuildTreeView(DirectoryItem directoryItem, ItemsControl parentTreeViewItem)
        {
            
            foreach (var item in directoryItem.Children)
            {
                TreeViewItem treeViewItem = new TreeViewItem();

                parentTreeViewItem.Items.Add(treeViewItem);

                StackPanel stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
                if (item is DirectoryItem directory)
                {
                    #region AddingControls
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri(Paths.FolderImagePath, UriKind.Relative);
                    bi.EndInit();
                    stackPanel.Children.Add(new Image { Source = bi, Width = 20, Height = 18 });
                    stackPanel.Children.Add(new Label { Content = item.Name, Foreground = new SolidColorBrush(GetForegroundColor(item)) });
                    Button addButton = new Button { Content = "Додати", Width = 65, Height = 20, FontSize = 10, Background = new SolidColorBrush(Colors.Transparent) };
                    addButton.Click += (sender, e) => OpenCreateCatalogWindow(sender, e, directory);
                    stackPanel.Children.Add(addButton);
                    Button deleteButton = new Button { Content = "Видалити", Width = 48, Height = 20, FontSize = 10, Background = new SolidColorBrush { Color = Colors.Yellow, Opacity = 0.5 } };
                    deleteButton.Click += (sender, e) => DeleteItemFromDirectory(sender, e, item, item.Owner);
                    stackPanel.Children.Add(deleteButton);

                    treeViewItem.Header = stackPanel;
                    #endregion

                    BuildTreeView(directory, treeViewItem);
                }
                else
                {
                    #region AddingConrols
                    stackPanel.Children.Add(new Label { Content = item.Name, Foreground = new SolidColorBrush(GetForegroundColor(item)), FontWeight = FontWeights.SemiBold });
                    Button deleteButton = new Button { Content = "Видалити", Width = 48, Height = 20, FontSize = 10, Background = new SolidColorBrush { Color = Colors.Yellow, Opacity = 0.5 } };
                    deleteButton.Click += (sender, e) => DeleteItemFromDirectory(sender, e, item, item.Owner);
                    stackPanel.Children.Add(deleteButton);

                    treeViewItem.Header = stackPanel;
                    #endregion
                }
            }
        }

        private void DeleteItemFromDirectory(object sender, RoutedEventArgs e,CatalogItem targetItem, DirectoryItem owner)
        {
            owner.Remove(targetItem);

            CatalogUpdatedHandler();
        }

        private void OpenCreateCatalogWindow(object sender, RoutedEventArgs e, DirectoryItem owner)
        {
            CreateCatalogItemWindow createCatalogItemWindow = new CreateCatalogItemWindow(CatalogUpdatedHandler,owner);
            createCatalogItemWindow.Show();
        }

        private void CatalogUpdatedHandler()
        {
            catalogRepository.SaveCatalog(rootDirectory);
            DirectoryItem refreshedRootDir = catalogRepository.GetCatalogItems();

            TreeViewItem rootTreeViewItem = new TreeViewItem { Header = refreshedRootDir.Name };

            PopulateTreeView(rootTreeViewItem);
        }

        private Color GetForegroundColor(CatalogItem catalogItem)
        {
            switch (catalogItem.Type)
            {
                case ItemType.Directory:
                    return Colors.Black;
                case ItemType.System:
                    return Colors.Blue;
                case ItemType.Office:
                    return Colors.CadetBlue;
                case ItemType.Gaming:
                    return Colors.Red;
                case ItemType.Multimedia:
                    return Colors.Purple;
                default:
                    throw new ArgumentException(nameof(catalogItem));
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(ItemNameTextBox.Text))
            {
                MessageBox.Show("Оберіть ім'я.");
                return;
            }

            ItemType? itemType;
            switch (TypeComboBox.SelectedItem)
            {
                case "Усі":
                    itemType = null;
                    break;
                case "Директорія":
                    itemType = ItemType.Directory;
                    break;
                case "Системне":
                    itemType = ItemType.System;
                    break;
                case "Офісне":
                    itemType = ItemType.Office;
                    break;
                case "Ігрове":
                    itemType = ItemType.Gaming;
                    break;
                case "Мультимедія":
                    itemType = ItemType.Multimedia;
                    break;
                default:
                    MessageBox.Show("Оберіть тип файлу.");
                    return;
            }

            SearchRequest searchRequest = new SearchRequest(ItemNameTextBox.Text, itemType);

            List<(string, CatalogItem)> results = new List<(string, CatalogItem)>();
            SearchingHandler searchingHandler = new SearchingHandler(rootDirectory);

            searchingHandler.Handle(searchRequest, results);

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var result in results)
            {
                stringBuilder.AppendLine(result.Item1.ToString());
            }

            ResultsLabel.Visibility = Visibility.Visible;
            SearchingResultTextBlock.Text = stringBuilder.ToString();
        }
    }
}
