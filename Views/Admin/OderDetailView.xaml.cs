﻿using Estore.Models;
using Estore.Repositories;
using System.Windows;

namespace Estore.Views.Admin
{
    public partial class OrderDetailView : Window
    {
        private readonly int _orderId;
        private readonly OrderRepository _orderRepository;

        public OrderDetailView(int orderId)
        {
            InitializeComponent();
            _orderId = orderId;
            _orderRepository = new OrderRepository(new MyStoreContext()); 

            LoadOrderDetails();
        }

        private async void LoadOrderDetails()
        {
            try
            {
                var orderDetails = await _orderRepository.GetOrderDetailsByOrderId(_orderId);
                var order = await _orderRepository.GetOrderByOrderId(_orderId);

                ProductDataGrid.ItemsSource = orderDetails;

                OrderIdTextBox.Text = order.OrderId.ToString();
                OrderDateTextBox.Text = order.OrderDate.ToString();
                StaffNameTextBox.Text   = order.Staff.Name.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}");
            }
        }
    }
}
