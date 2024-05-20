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
        private readonly int skip = 0;
        private readonly int take = 10;
        private IEnumerable<Product> _allProducts;
        public ProductsManageView(IProductRepository productRepository)
        {
            InitializeComponent();
            DataContext = this;
            productRepository = _productRepository;
            InitializeProductAsync();
        }
        private async Task InitializeProductAsync()
        {
            var allProducts = await _productRepository.GetProducts(string.Empty, new List<int>());
            _allProducts = allProducts.ToList();
        }

        private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var addProductView = new ProductDetailView();
            Content = addProductView;
        }

        private void btnPrev_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
