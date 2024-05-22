using Estore.Models;
using Estore.Repositories;
using System.Windows;
using System.Windows.Controls;

namespace Estore.Views.Admin
{
    /// <summary>
    /// Interaction logic for ProductDetailView.xaml
    /// </summary>
    public partial class ProductDetailView : Page
    {
        private readonly IProductRepository _productRepository;
        public ProductDetailView(IProductRepository productRepository, int productId = 0)
        {
            _productRepository = productRepository;
            InitializeComponent();
            InitializeDetailAsync(productId);
        }
        private async Task InitializeDetailAsync(int id = 0)
        {
            listCategories.ItemsSource = await _productRepository.GetCategories();
            if (id != 0)
            {
                var product = await _productRepository.GetProductByID(id);
                productId.Text = product.ProductId.ToString();
                listCategories.SelectedValue = product.CategoryId;
                productName.Text = product.ProductName;
                unitPrice.Text = product.UnitPrice.ToString();
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int price;
                var isInt = Int32.TryParse(unitPrice.Text, out price);
                if (!isInt)
                    throw new Exception("Unit price must be a positive integer!");
                Product product = new Product()
                {
                    ProductId = string.IsNullOrEmpty(productId.Text) ? 0 : Convert.ToInt32(productId.Text),
                    CategoryId = string.IsNullOrEmpty(listCategories.SelectedValue?.ToString()) ? 
                        0 : Convert.ToInt32(listCategories.SelectedValue.ToString()),
                    ProductName = productName.Text ?? string.Empty,
                    UnitPrice = price,
                };
                var result = await _productRepository.UpsertProduct(product);
                if (result == null)
                    throw new Exception("Error while saving changes!");
                productId.Text = result.ProductId.ToString();
                listCategories.SelectedValue = product.CategoryId;
                productName.Text = result.ProductName;
                unitPrice.Text = result.UnitPrice.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var listProductView = new ProductsManageView(_productRepository);
            this.NavigationService.Navigate(listProductView);
        }
    }
}
