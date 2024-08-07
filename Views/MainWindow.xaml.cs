﻿using Estore.Models;
using Estore.Repositories;
using Estore.Session_Login;
using Estore.Views;
using Estore.Views.Admin;
using Estore.Views.Staff;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.IO;
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
        private readonly MyStoreContext _context;
        private readonly string _defaultEmail;
        private readonly string _defaultPassword;
        public MainWindow(IProductRepository productRepository, MyStoreContext context, IOrderRepository orderRepository)
        {
            InitializeComponent();
            _productRepository = productRepository;
            _orderRepository = orderRepository; 
            _context = context;
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _defaultEmail = configuration["EmailSetting:email"];
            _defaultPassword = configuration["EmailSetting:password"];
            //_context = new MyStoreContext();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            SessionManage sessionManage = SessionManage.Instance;
            string username = tbUsername.Text;
            string password = pbPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username và Password không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (username == _defaultEmail && password == _defaultPassword)
            {
                // Login as default admin
                sessionManage.SetSession("Username", "admin");
                sessionManage.SetSession("Role", 1);

                AdminView adminView = new AdminView(_productRepository, _orderRepository);
                adminView.Show();
                this.Close();
                return;
            }

            var staff = _context.Staffs.FirstOrDefault(s => s.Name == username && s.Password == password);

            if (staff != null)
            {
                // Lưu thông tin đăng nhập vào session
                sessionManage.SetSession("Username", staff.Name);
                sessionManage.SetSession("Role", staff.Role);

                if (staff.Role == 1)
                {
                  
                    //profileWindowView profileWindowView = new profileWindowView(staff);
                    //profileWindowView.Show();
                    AdminView adminView = new AdminView(_productRepository, _orderRepository);
                    adminView.Show();
                    
                }
                else if (staff.Role == 2)
                {
                    // Chuyển hướng đến StaffView
                    StaffView staffView = new StaffView(_productRepository, _orderRepository);
                    staffView.Show();
                }

                this.Close(); // Đóng cửa sổ đăng nhập
            }
            else
            {
                MessageBox.Show("Sai Username hoặc Password.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}