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
        private int skip = 0;
        private int take = 5;
        private string keyword = string.Empty;
        private int _totalProducts;
        private int _totalPages;
        private int _currentPage = 1;
        private IEnumerable<Product> _currentProducts;
        public ProductsManageView(IProductRepository productRepository)
        {
            InitializeComponent();
            _productRepository = productRepository;
            InitializeProductAsync();
        }
        private async Task InitializeProductAsync()
        {
            _currentProducts = await _productRepository.GetProducts();
            _totalProducts = _currentProducts.Count();
            _totalPages = _totalProducts / take;
            txtCurrent.Text = _currentPage.ToString();
            txtTotal.Text = (_totalProducts % take != 0) ? (++_totalPages).ToString() : _totalPages.ToString();
            listProducts.ItemsSource = _currentProducts.Skip(skip).Take(take);
        }

        private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var addProductView = new ProductDetailView(_productRepository);
            this.NavigationService.Navigate(addProductView);
        }

        private void btnPrev_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (skip - take >= 0)
            {
                txtCurrent.Text = (--_currentPage).ToString();
                skip -= take;
                listProducts.ItemsSource = _currentProducts.Skip(skip).Take(take);
            }
        }

        private void btnNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (skip + take < _totalProducts)
            {
                txtCurrent.Text = (++_currentPage).ToString();
                skip += take;
                if (skip >= _totalPages * take)
                {
                    listProducts.ItemsSource = _currentProducts.Skip(skip).Take(_totalProducts - skip);
                }
                else
                {
                    listProducts.ItemsSource = _currentProducts.Skip(skip).Take(take);
                }
            }
        }

        private void tbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            keyword = tbxSearch.Text;
            skip = 0;
            _currentProducts = _currentProducts.Where(o => o.ProductName.Contains(keyword));
            _totalProducts = _currentProducts.Count();
            _currentPage = _totalProducts == 0 ? 0 : 1;
            _totalPages = _totalProducts / take;
            txtCurrent.Text = _currentPage.ToString();
            txtTotal.Text = (_totalProducts % take != 0) ? (++_totalPages).ToString() : _totalPages.ToString();
            listProducts.ItemsSource = _currentProducts.Skip(skip).Take(take);
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
                            var products = await _productRepository.DeleteProduct(item.ProductId);
                            skip = 0;
                            _currentProducts = products.Where(o => o.ProductName.Contains(keyword));
                            _totalProducts = _currentProducts.Count();
                            _currentPage = _totalProducts == 0 ? 0 : 1;
                            _totalPages = _totalProducts / take;
                            txtCurrent.Text = _currentPage.ToString();
                            txtTotal.Text = (_totalProducts % take != 0) ? (++_totalPages).ToString() : _totalPages.ToString();
                            listProducts.ItemsSource = _currentProducts.Skip(skip).Take(take);
                        }
                    }
                }
            }
        }
    }
}
