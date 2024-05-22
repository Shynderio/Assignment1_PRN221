using Estore.Models;
using Estore.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private DateTime _startDate;
        private DateTime _endDate;
        public OrderListView(IOrderRepository orderRepository)
        {
            InitializeComponent();
            DataContext = this;
            _orderRepository = orderRepository;
            string role = (string)Application.Current.Properties["role"]!;
            _startDate = DateTime.Now.AddMonths(-1);
            _endDate = DateTime.Now;
            EndDate.Text = _endDate.ToString("dd/MM/yyyy");
            StartDate.Text = _startDate.ToString("dd/MM/yyyy");
            _isStaff = role.Equals("staff");
            InitializeOrdersAsync();
            
        }


        private async Task InitializeOrdersAsync()
        {
            _allOrders = await _orderRepository.GetOrdersByPeriod(_startDate, _endDate);
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




        //private void StartDate_Changed(object sender, TextChangedEventArgs e)
        //{
        //    string searchText = StartDate.Text.Trim();

        //    if (string.IsNullOrEmpty(searchText) || DateTime.Parse())
        //    {
        //        // If search text is empty, reset to display all orders
        //        return;
        //    }
        //    else
        //    {
        //        // Perform the search by order ID or staff name
        //        int orderId;
        //        bool isNumeric = int.TryParse(searchText, out orderId);

        //        if (isNumeric)
        //        {
        //            _filteredOrders = _allOrders.Where(o => o.OrderId == orderId).ToList();
        //        }
        //        else
        //        {
        //            _filteredOrders = _allOrders.Where(o => o.StaffName.ToLower().Contains(searchText.ToLower())).ToList();
        //        }
        //    }

        //    // Reset current page to 1 after search
        //    CurrentPage = 1;
        //    UpdatePagedView();
        //}



        private void StartDatePicker_Click(object sender, RoutedEventArgs e)
        {
            DatePicker1.IsDropDownOpen = true;
        }
        private void EndDatePicker_Click(object sender, RoutedEventArgs e)
        {
            DatePicker2.IsDropDownOpen = true;
        }

        private async void Selected_StartDate(object sender, RoutedEventArgs e)
        {
            if (DatePicker1.SelectedDate.HasValue)
            {
                // Update the TextBox with the selected date
                StartDate.Text = DatePicker1.SelectedDate.Value.ToString("dd/MM/yyyy");
                // Hide the DatePicker after a date is selected
                DatePicker1.Visibility = Visibility.Collapsed;
                _startDate = DatePicker1.SelectedDate.Value;
                _allOrders = await _orderRepository.GetOrdersByPeriod(_startDate, _endDate);
                _filteredOrders = _allOrders;
                CurrentPage = 1;
                UpdatePagedView();
            }
        }

        private async void Selected_EndDate(object sender, RoutedEventArgs e)
        {
            if (DatePicker2.SelectedDate.HasValue)
            {
                // Update the TextBox with the selected date
                EndDate.Text = DatePicker2.SelectedDate.Value.ToString("dd/MM/yyyy");
                // Hide the DatePicker after a date is selected
                DatePicker2.Visibility = Visibility.Collapsed;
                _endDate = DatePicker2.SelectedDate.Value;

                //if (_startDate)
                //{
                    // Fetch orders only if both dates are set
                _allOrders = await _orderRepository.GetOrdersByPeriod(_startDate, _endDate);
                _filteredOrders = _allOrders;
                CurrentPage = 1;
                UpdatePagedView();

            }
        }


        //private void Button_Calendar_Click(object sender, RoutedEventArgs e)
        //{
        //    DatePicker.IsDropDownOpen = true;
        //}


        public class OrderDetail
        {
            // Assuming OrderDetail properties, adjust as necessary
        }
    }

}