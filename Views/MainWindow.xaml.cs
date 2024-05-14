using Estore.Models;
using Estore.Repositories;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Estore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IProductRepository _productRepository;
        public MainWindow(IProductRepository productRepository)
        {
            InitializeComponent();
            _productRepository = productRepository;
        }
 

        private void RefreshProductList()
        {
            products.ItemsSource = _productRepository.GetProducts();
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            // Here, you would open a dialog or a new window to gather product information
            // and then add the product to the repository
            // For demonstration purposes, let's assume there's a method to add a product
            Product newProduct = new Product
            {
                ProductName = "New Product",
                CategoryId = 1, // Assuming a default category ID
                UnitPrice = 0 // Assuming a default unit price
            };
            _productRepository.InsertProduct(newProduct);
            RefreshProductList();
        }

        private void UpdateProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (products.SelectedItem != null)
            {
                // Here, you would open a dialog or a new window to edit product information
                // and then update the product in the repository
                // For demonstration purposes, let's assume there's a method to update a product
                Product selectedProduct = products.SelectedItem as Product;
                // Update selectedProduct properties with new values
                _productRepository.UpdateProduct(selectedProduct);
                RefreshProductList();
            }
            else
            {
                MessageBox.Show("Please select a product to update.");
            }
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (products.SelectedItem != null)
            {
                // Confirm deletion
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Deletion", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Delete the selected product from the repository
                    Product selectedProduct = products.SelectedItem as Product;
                    _productRepository.DeleteProduct(selectedProduct);
                    RefreshProductList();
                }
            }
            else
            {
                MessageBox.Show("Please select a product to delete.");
            }
        }
    }
}