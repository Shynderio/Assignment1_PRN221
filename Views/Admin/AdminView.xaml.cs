﻿using Estore.Models;
using Estore.Repositories;
using Estore.Session_Login;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
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
    /// 
   
    public partial class AdminView : Window
    {
        private MyStoreContext _context;
        IProductRepository _productRepository;
        IOrderRepository _orderRepository;
        public AdminView(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            InitializeComponent();
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            Application.Current.Properties["role"] = "admin";
        }

        private void NavigateToProducts(object sender, RoutedEventArgs e)
        {
            var productManager = new ProductsManageView(_productRepository);
            ContentFrame.Navigate(productManager);
        }

        private void NavigateToStaffs(object sender, RoutedEventArgs e)
        {
            var staffManager = new StaffsManageView(this.ContentFrame);
            ContentFrame.Navigate(staffManager);
        } 
        private void NavigateToProfile(object sender, RoutedEventArgs e)
        {

            var profileView = new ProfileView();
            ContentFrame.Navigate(profileView);
        }

        private void NavigateToReports(object sender, RoutedEventArgs e)
        {
            var reportView = new ReportView(_orderRepository);
            ContentFrame.Navigate(reportView);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private bool IsMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    WindowState = WindowState.Normal;
                    Width = 1080;
                    Height = 720;

                    IsMaximized = false;
                }
                else
                {
                    WindowState = WindowState.Maximized;
                    IsMaximized= true;
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(_productRepository, _context, _orderRepository);

            // Show the MainWindow
            mainWindow.Show();

            // Close the current AdminView
            this.Close();
            SessionManage sessionManage = SessionManage.Instance;
            sessionManage.RemoveSession(Uid);
        }
    }
}
