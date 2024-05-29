using System;
using System.Linq;
using System.Windows;
using Estore.Session_Login;
using Estore.Models;
using Estore.Repositories;
using Estore.Views.Staff;

namespace Estore
{
    public partial class AddOrderView : Window
    {
        private readonly IOrderRepository _orderRepository;

        public AddOrderView(IOrderRepository orderRepository)
        {
            InitializeComponent();
            _orderRepository = orderRepository;
            LoadData();
        }

        private void LoadData()
        {
            // Set the current date
            OrderDateTextBox.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Get the staff name from session
            string staffName = SessionManage.Instance.GetSession("Username") as string;
            StaffNameTextBox.Text = staffName ?? "Unknown"; // Set to "Unknown" if staffName is null
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MyStoreContext())
            {
                // Get staff name from TextBox
                string staffName = StaffNameTextBox.Text;

                // Get the StaffId from StaffName
                var staff = context.Staffs.SingleOrDefault(s => s.Name == staffName);
                if (staff == null)
                {
                    MessageBox.Show("Staff not found.");
                    return;
                }

                // Create a new Order
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    StaffId = staff.StaffId
                };

                // Add order to the context
                context.Orders.Add(order);
                context.SaveChanges();

                MessageBox.Show("Order added successfully!");

                // Navigate to OrderDetailView
                var orderDetailView = new Estore.Views.Admin.OrderDetailView(order.OrderId, _orderRepository);
                orderDetailView.Show();
                this.Close();
            }
        }
    }
}
