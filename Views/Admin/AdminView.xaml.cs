using Estore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Estore.Views.Admin
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : Window
    {
        IProductRepository _productRepository;
        IOrderRepository _orderRepository;
        public AdminView(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            InitializeComponent();
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        private void NavigateToProducts(object sender, RoutedEventArgs e)
        {
            var productManager = new ProductsManageView();
            Content = productManager;
        }

        private void NavigateToStaffs(object sender, RoutedEventArgs e)
        {
            var staffManager = new StaffsManageView();
            Content= staffManager;
        }

        private void NavigateToOrders(object sender, RoutedEventArgs e)
        {
            var orderListView = new OrderListView(_orderRepository);
            Content = orderListView;
        }

    }
}
