using Estore.Models;
using Estore.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Estore.Repositories.OrderRepository;

namespace Estore.Views.Admin
{
    public partial class OrderListView : Page, INotifyPropertyChanged
    {
        private const int ItemsPerPage = 8;
        private int _currentPage = 1;
        private readonly IOrderRepository _orderRepository;
        private List<OrderDto> _allOrders;
        private List<OrderDto> _pagedOrders;
        private List<OrderDto> _filteredOrders;
        private string _searchTerm;
        private bool _isStaff;
        public OrderListView(IOrderRepository orderRepository)
        {
            InitializeComponent();
            DataContext = this;
            _orderRepository = orderRepository;
            string role = (string)Application.Current.Properties["role"]!;
            _isStaff = role.Equals("staff");
            InitializeOrdersAsync();
        }

        private async Task InitializeOrdersAsync()
        {
            _allOrders = await _orderRepository.GetAllOrdersAsync();
            _filteredOrders = _allOrders;
            UpdatePagedView();
        }

        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));
                    UpdatePagedView();
                }
            }
        }

        public bool IsStaff
        {
            get => _isStaff;
            set
            {
                _isStaff = value;
                OnPropertyChanged(nameof(IsStaff));
            }
        }

        private void UpdatePagedView()
        {
            _pagedOrders = _filteredOrders.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();
            dataGrid.ItemsSource = _pagedOrders;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < (_allOrders.Count + ItemsPerPage - 1) / ItemsPerPage)
            {
                CurrentPage++;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                _filteredOrders = _allOrders;
            }
            else
            {
                int orderId;
                bool isNumeric = int.TryParse(searchText, out orderId);

                if (isNumeric)
                {
                    _filteredOrders = _allOrders.Where(o => o.OrderId == orderId).ToList();
                }
                else
                {
                    _filteredOrders = _allOrders.Where(o => o.StaffName.ToLower().Contains(searchText.ToLower())).ToList();
                }
            }

            CurrentPage = 1;
            UpdatePagedView();
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrderView addOrderView = new AddOrderView(_orderRepository);
            addOrderView.ShowDialog();

            LoadOrders();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is OrderDto selectedOrder)
            {
                OrderDetailView orderDetailView = new OrderDetailView(selectedOrder.OrderId);
                orderDetailView.ShowDialog();
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is OrderDto selectedOrder)
            {
                try
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this order?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        await _orderRepository.DeleteOrderAsync(selectedOrder.OrderId);

                        await LoadOrders();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task LoadOrders()
        {
            _allOrders = await _orderRepository.GetAllOrdersAsync();
            UpdatePagedView();
        }
    }
}
