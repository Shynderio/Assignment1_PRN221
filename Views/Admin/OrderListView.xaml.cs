using Estore.Models;
using Estore.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        public void FilterOrdersById(int orderId)
        {
            _filteredOrders = _allOrders.Where(o => o.OrderId == orderId).ToList();
            CurrentPage = 1;
            UpdatePagedView();
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




        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                // If search text is empty, reset to display all orders
                _filteredOrders = _allOrders;
            }
            else
            {
                // Perform the search by order ID or staff name
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

            // Reset current page to 1 after search
            CurrentPage = 1;
            UpdatePagedView();
        }
    }


    public class OrderDetail
    {
        // Assuming OrderDetail properties, adjust as necessary
    }
}
