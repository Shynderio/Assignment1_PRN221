using Estore.Models;
using Estore.Repositories;
using Estore.Views.Admin;
using Estore.Views.Staff;
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
        IOrderRepository _orderRepository;
        public MainWindow(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            InitializeComponent();
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        private void NavigateToAdminView(object sender, RoutedEventArgs e)
        {
            var adminView = new AdminView(_productRepository, _orderRepository);
            adminView.Show();
            Close();
        }

        private void NavigateToStaffView(object sender, RoutedEventArgs e)
        {
            var staffView = new StaffView();
            staffView.Show();
            Close();
        }
    }
}