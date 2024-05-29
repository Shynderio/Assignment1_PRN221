using Estore.Models;
using Estore.Repositories;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Estore.Views.Admin
{
    /// <summary>
    /// Interaction logic for ProductsManageView.xaml
    /// </summary>
    public partial class ProductsManageView : Page
    {
        private readonly IProductRepository _productRepository;
        private int _skip = 0;
        private int _take = 5;
        private int _totalProducts;
        private int _totalPages;
        private int _currentPage = 1;
        private IEnumerable<Product> _allProducts;
        private IEnumerable<Product> _currentProducts;
        private IEnumerable<Category> _allCategories;
        private string _keyword = string.Empty;
        private int _categoryId = 0;
        private int _minPrice = int.MinValue;
        private int _maxPrice = int.MaxValue;
        public ProductsManageView(IProductRepository productRepository)
        {
            InitializeComponent();
            _productRepository = productRepository;
            InitializeProductAsync();
        }
        private async Task InitializeProductAsync()
        {
            _allProducts = await _productRepository.GetProducts();
            _currentProducts = _allProducts;
            _totalProducts = _currentProducts.Count();
            _totalPages = _totalProducts / _take;
            txtCurrent.Text = _currentPage.ToString();
            txtTotal.Text = (_totalProducts % _take != 0) ? (++_totalPages).ToString() : _totalPages.ToString();
            listProducts.ItemsSource = _currentProducts.Skip(_skip).Take(_take);
            var categories = (await _productRepository.GetCategories()).ToList();
            _allCategories = categories;
            categories.Add(new Category()
            {
                CategoryId = 0,
                CategoryName = "All",
            });
            listCategories.ItemsSource = categories.OrderBy(o => o.CategoryId);
            listCategories.SelectedValue = categories.FirstOrDefault(o => o.CategoryId == _categoryId)?.CategoryId;
        }

        private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var addProductView = new ProductDetailView(_productRepository);
            this.NavigationService.Navigate(addProductView);
        }

        private void btnPrev_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_skip - _take >= 0)
            {
                txtCurrent.Text = (--_currentPage).ToString();
                _skip -= _take;
                listProducts.ItemsSource = _currentProducts.Skip(_skip).Take(_take);
            }
        }

        private void btnNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_skip + _take < _totalProducts)
            {
                txtCurrent.Text = (++_currentPage).ToString();
                _skip += _take;
                if (_skip >= _totalPages * _take)
                {
                    listProducts.ItemsSource = _currentProducts.Skip(_skip).Take(_totalProducts - _skip);
                }
                else
                {
                    listProducts.ItemsSource = _currentProducts.Skip(_skip).Take(_take);
                }
            }
        }

        private void tbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _keyword = tbxSearch.Text;
            FilterProducts();
        }

        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                // Find the DataGridRow that contains the button
                DataGridRow row = FindParent<DataGridRow>(button);
                if (row != null)
                {
                    // Get the item bound to the DataGridRow
                    Product item = row.Item as Product;
                    if (item != null)
                    {
                        var addProductView = new ProductDetailView(_productRepository, item.ProductId);
                        this.NavigationService.Navigate(addProductView);
                    }
                }
            }
        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            if (parentObject is T parent) return parent;
            return FindParent<T>(parentObject);
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                // Find the DataGridRow that contains the button
                DataGridRow row = FindParent<DataGridRow>(button);
                if (row != null)
                {
                    // Get the item bound to the DataGridRow
                    Product item = row.Item as Product;
                    if (item != null)
                    {
                        // Show confirmation message box
                        MessageBoxResult result = MessageBox.Show(
                            $"Are you sure you want to delete {item.ProductName}?",
                            "Confirmation",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning
                        );

                        if (result == MessageBoxResult.Yes)
                        {
                            _allProducts =  await _productRepository.DeleteProduct(item.ProductId);
                            FilterProducts();
                        }
                    }
                }
            }
        }

        private void listCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _categoryId = Convert.ToInt32(listCategories.SelectedValue.ToString());
            FilterProducts();
        }

        private void tbxMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            var isInt = Int32.TryParse(string.IsNullOrEmpty(tbxMin.Text) ? int.MinValue.ToString() : tbxMin.Text, out _minPrice);
            FilterProducts();
        }

        private void tbxMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            var isInt = Int32.TryParse(string.IsNullOrEmpty(tbxMax.Text) ? int.MaxValue.ToString() : tbxMax.Text, out _maxPrice);
            FilterProducts();
        }

        private void FilterProducts()
        {
            _skip = 0;
            _currentProducts = _allProducts
                .Where(o => o.ProductName.Contains(_keyword)
                && o.UnitPrice >= _minPrice
                && o.UnitPrice <= _maxPrice);
            if (_categoryId != 0)
                _currentProducts = _currentProducts.Where(o => o.CategoryId == _categoryId);
            _totalProducts = _currentProducts.Count();
            _currentPage = _totalProducts == 0 ? 0 : 1;
            _totalPages = _totalProducts / _take;
            txtCurrent.Text = _currentPage.ToString();
            txtTotal.Text = (_totalProducts % _take != 0) ? (++_totalPages).ToString() : _totalPages.ToString();
            listProducts.ItemsSource = _currentProducts.Skip(_skip).Take(_take);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ExportToJson(_allProducts.ToArray(), @"D:\products.json");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = @"D:\products.json";
                IEnumerable<Product> productsInFile = ReadFromJson<Product>(filePath);
                foreach (Product product in productsInFile)
                {
                    //int currentId;
                    //var isDigit = Int32.TryParse(product.UnitPrice.ToString(), out currentId);
                    //if (!isDigit)
                    //    continue;
                    //if (currentId > 0)
                    //    continue;
                    if (string.IsNullOrEmpty(product.CategoryId.ToString()))
                        continue;
                    int currentCategoryId;
                    var isDigit = Int32.TryParse(product.CategoryId.ToString(), out currentCategoryId);
                    if (!isDigit)
                        continue;
                    if (!_allCategories.Select(o => o.CategoryId).ToList().Contains(currentCategoryId))
                        continue;
                    int currentPrice;
                    isDigit = Int32.TryParse(product.UnitPrice.ToString(), out currentPrice);
                    if (!isDigit)
                        continue;
                    var existProduct = _allProducts
                        .Any(o => o.ProductName.ToLower() == product.ProductName.Trim().ToLower());
                    if (!existProduct)
                        //await _productRepository.UpsertProduct(product);
                        continue;
                }
                MessageBox.Show("Import products successfully!");
                _allProducts = productsInFile;
                FilterProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void ExportToJson<T>(T[] array, string filePath)
        {
            // Serialize the array to JSON format
            string jsonString = JsonSerializer.Serialize(array, new JsonSerializerOptions { WriteIndented = true });

            // Write the JSON string to a file
            File.WriteAllText(filePath, jsonString);
            MessageBox.Show("Export products successfully!");
        }

        public static T[] ReadFromJson<T>(string filePath)
        {
            // Read the JSON string from the file
            string jsonString = File.ReadAllText(filePath);

            // Deserialize the JSON string into an array of objects
            T[] array = JsonSerializer.Deserialize<T[]>(jsonString);

            return array;
        }
    }
}
