using Estore.Models;
using Estore.Repositories;
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
        private string _keyword = string.Empty;
        private int _totalProducts;
        private int _totalPages;
        private int _currentPage = 1;
        private IEnumerable<Product> _allProducts;
        private IEnumerable<Product> _currentProducts;
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
            listCategories.ItemsSource = await _productRepository.GetCategories();
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

        private async void tbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _keyword = tbxSearch.Text;
            _skip = 0;
            _currentProducts = _allProducts.Where(o => o.ProductName.Contains(_keyword));
            _totalProducts = _currentProducts.Count();
            _currentPage = _totalProducts == 0 ? 0 : 1;
            _totalPages = _totalProducts / _take;
            txtCurrent.Text = _currentPage.ToString();
            txtTotal.Text = (_totalProducts % _take != 0) ? (++_totalPages).ToString() : _totalPages.ToString();
            listProducts.ItemsSource = _currentProducts.Skip(_skip).Take(_take);
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
                            _skip = 0;
                            _currentProducts = _allProducts.Where(o => o.ProductName.Contains(_keyword));
                            _totalProducts = _currentProducts.Count();
                            _currentPage = _totalProducts == 0 ? 0 : 1;
                            _totalPages = _totalProducts / _take;
                            txtCurrent.Text = _currentPage.ToString();
                            txtTotal.Text = (_totalProducts % _take != 0) ? (++_totalPages).ToString() : _totalPages.ToString();
                            listProducts.ItemsSource = _currentProducts.Skip(_skip).Take(_take);
                        }
                    }
                }
            }
        }

        private void listCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _keyword = tbxSearch.Text;
            _skip = 0;
            _currentProducts = _currentProducts.Where(o => o.CategoryId == Convert.ToInt32(listCategories.SelectedValue.ToString()));
            _totalProducts = _currentProducts.Count();
            _currentPage = _totalProducts == 0 ? 0 : 1;
            _totalPages = _totalProducts / _take;
            txtCurrent.Text = _currentPage.ToString();
            txtTotal.Text = (_totalProducts % _take != 0) ? (++_totalPages).ToString() : _totalPages.ToString();
            listProducts.ItemsSource = _currentProducts.Skip(_skip).Take(_take);
        }

        private void tbxMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            int min = int.MinValue;
            var isInt = Int32.TryParse(string.IsNullOrEmpty(tbxMin.Text) ? min.ToString() : tbxMin.Text, out min);
            if (isInt)
            {
                _keyword = tbxSearch.Text;
                _skip = 0;
                _currentProducts = _currentProducts.Where(o => o.UnitPrice >= min);
                _totalProducts = _currentProducts.Count();
                _currentPage = _totalProducts == 0 ? 0 : 1;
                _totalPages = _totalProducts / _take;
                txtCurrent.Text = _currentPage.ToString();
                txtTotal.Text = (_totalProducts % _take != 0) ? (++_totalPages).ToString() : _totalPages.ToString();
                listProducts.ItemsSource = _currentProducts.Skip(_skip).Take(_take);
            }

        }

        private void tbxMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            int max = int.MaxValue;
            var isInt = Int32.TryParse(string.IsNullOrEmpty(tbxMax.Text) ? max.ToString() : tbxMin.Text, out max);
            if (isInt)
            {
                _keyword = tbxSearch.Text;
                _skip = 0;
                _currentProducts = _currentProducts.Where(o => o.UnitPrice <= max);
                _totalProducts = _currentProducts.Count();
                _currentPage = _totalProducts == 0 ? 0 : 1;
                _totalPages = _totalProducts / _take;
                txtCurrent.Text = _currentPage.ToString();
                txtTotal.Text = (_totalProducts % _take != 0) ? (++_totalPages).ToString() : _totalPages.ToString();
                listProducts.ItemsSource = _currentProducts.Skip(_skip).Take(_take);
            }
        }
    }
}
