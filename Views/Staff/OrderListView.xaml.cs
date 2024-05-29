using Estore.Repositories;
using Estore.Session_Login;
using Estore.Views.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Estore.Repositories.OrderRepository;

namespace Estore.Views.Staff
{
    /// <summary>
    /// Interaction logic for OrderListView.xaml
    /// </summary>
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
        string _staffName = string.Empty;

        public OrderListView(IOrderRepository orderRepository)
        {
            InitializeComponent();
            DataContext = this;
            _orderRepository = orderRepository;
            string role = (string)Application.Current.Properties["role"]!;
            _startDate = DateTime.Now;

            SessionManage sessionManage = SessionManage.Instance;
            _staffName = (string)sessionManage.GetSession("Username");

            StartDate.Text = _startDate.ToString("dd/MM/yyyy");
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

            _allOrders = await _orderRepository.GetOrdersByOrderDate(_startDate, _staffName);
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
            _filteredOrders = _allOrders;
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

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrderView addOrderView = new AddOrderView(_orderRepository);
            addOrderView.ShowDialog();
            
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is OrderDto selectedOrder)
            {
                OrderDetailView orderDetailView = new OrderDetailView(selectedOrder.OrderId, _orderRepository);
                orderDetailView.Show();
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
            LoadOrders();
        }

        private async Task LoadOrders()
        {
            _allOrders = await _orderRepository.GetOrdersByOrderDate(_startDate, _staffName);
           
            UpdatePagedView();
        }

        private void StartDatePicker_Click(object sender, RoutedEventArgs e)

        {

            DatePicker1.IsDropDownOpen = true;

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

                if (_isStaff)
                {
                    _allOrders = await _orderRepository.GetOrdersByOrderDate(_startDate, _staffName);

                }
                else
                {
                    _allOrders = await _orderRepository.GetOrdersByPeriod(_startDate, _startDate);
                }

                _filteredOrders = _allOrders;

                CurrentPage = 1;

                UpdatePagedView();

            }

        }



    
    }

}
