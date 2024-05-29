using Estore.Models;
using Estore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Estore.Views.Admin
{
    public partial class AddProductToOrderView : Window
    {
        private readonly IOrderRepository _orderRepository;
        private readonly int _orderId;

        public AddProductToOrderView(int orderId, IOrderRepository orderRepository)
        {
            InitializeComponent();
            _orderId = orderId;
            _orderRepository = orderRepository;
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                var products = await _orderRepository.GetAllProductsAsync();
                ProductNameComboBox.ItemsSource = products.Select(p => p.ProductName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ProductNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Get selected product name
                var selectedProductName = ProductNameComboBox.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedProductName))
                {
                    return;
                }

                // Get selected product
                var selectedProduct = await _orderRepository.GetProductByNameAsync(selectedProductName);
                if (selectedProduct == null)
                {
                    return;
                }

                // Display unit price
                UnitPriceTextBox.Text = selectedProduct.UnitPrice.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public event EventHandler ProductAdded;

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lấy tên sản phẩm đã chọn từ ComboBox
                var selectedProductName = ProductNameComboBox.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedProductName))
                {
                    MessageBox.Show("Please select a product.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Lấy thông tin sản phẩm từ tên sản phẩm đã chọn
                var selectedProduct = await _orderRepository.GetProductByNameAsync(selectedProductName);
                if (selectedProduct == null)
                {
                    MessageBox.Show("Selected product not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Validate số lượng
                if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Invalid quantity.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Tạo một đối tượng OrderDetail mới
                var orderDetail = new OrderDetail
                {
                    OrderId = _orderId,
                    ProductId = selectedProduct.ProductId,
                    Quantity = quantity,
                    UnitPrice = selectedProduct.UnitPrice,
                };

                // Thực hiện thêm OrderDetail vào database
                using (var context = new MyStoreContext())
                {
                    context.OrderDetails.Add(orderDetail);
                    await context.SaveChangesAsync();
                }

                MessageBox.Show("Product added to order successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                OrderDetailView orderDetailView = new OrderDetailView(_orderId, _orderRepository);
                orderDetailView.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
