using Estore.Models;
using Estore.Repositories;
using System;
using System.Windows;

namespace Estore.Views.Admin
{
    public partial class OrderDetailView : Window
    {
        private readonly IOrderRepository _orderRepository;
        private readonly int _orderId;

        public OrderDetailView(int orderId, IOrderRepository orderRepository)
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
                var order = await _orderRepository.GetOrderByOrderId(_orderId);
                if (order != null)
                {
                    OrderIdTextBox.Text = order.OrderId.ToString();
                    OrderDateTextBox.Text = order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss");
                    StaffNameTextBox.Text = order.Staff.Name;

                    var orderDetails = await _orderRepository.GetOrderDetailsByOrderId(_orderId);
                    ProductDataGrid.ItemsSource = orderDetails;
                }
                else
                {
                    MessageBox.Show("Order not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddProductToOrderView(_orderId, _orderRepository);
            addProductWindow.ProductAdded += AddProductWindow_ProductAdded;
            addProductWindow.Show();
            this.Close();
        }

        private async void AddProductWindow_ProductAdded(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
