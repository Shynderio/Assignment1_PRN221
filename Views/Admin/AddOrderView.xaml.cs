using Estore.Models;
using Estore.Repositories;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Estore.Views.Admin
{
    public partial class AddOrderView : Window
    {
        private readonly IOrderRepository _orderRepository;

        public AddOrderView(IOrderRepository orderRepository)
        {
            InitializeComponent();
            _orderRepository = orderRepository;

            OderDateTextBox.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(StaffIdTextBox.Text, out int staffId) || StaffIdTextBox.Text == "Enter staff ID")
                {
                    MessageBox.Show("Please enter a valid staff ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!decimal.TryParse(TotalPriceTextBox.Text, out decimal totalPrice) || TotalPriceTextBox.Text == "Enter total price")
                {
                    MessageBox.Show("Please enter a valid total price.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Order newOrder = new Order
                {
                    OrderDate = DateTime.Parse(OderDateTextBox.Text),
                    StaffId = staffId,
                };

                await _orderRepository.AddOrderAsync(newOrder);

                MessageBox.Show("Order added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.Text = string.Empty;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Name switch
                {
                    "StaffIdTextBox" => "Enter staff ID",
                    "TotalPriceTextBox" => "Enter total price",
                    _ => string.Empty,
                };
            }
        }
    }
}
