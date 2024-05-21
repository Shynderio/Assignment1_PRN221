using Estore.Models;
using Estore.Repositories;
using System.Windows;
using System.Windows.Controls;

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
            if (skip + take <= _totalProducts)
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
            _currentProducts = _currentProducts.Where(o => o.ProductName.Contains(keyword)).Skip(skip).Take(take);
            listProducts.ItemsSource = _currentProducts;
        }
    }
}
