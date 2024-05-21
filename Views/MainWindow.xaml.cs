using Estore.Models;
using Estore.Repositories;
using Estore.Session_Login;
using Estore.Views;
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
        private readonly IProductRepository _productRepository;
        private readonly MyStoreContext _context;

        public MainWindow(IProductRepository productRepository, MyStoreContext context)
        {
            InitializeComponent();
            _productRepository = productRepository;
            _context = context;
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

            var staff = _context.Staffs.FirstOrDefault(s => s.Name == username && s.Password == password);

            if (staff != null)
            {
                // Lưu thông tin đăng nhập vào session
                sessionManage.SetSession("Username", staff.Name);
                sessionManage.SetSession("Role", staff.Role);

                if (staff.Role == 1)
                {
                  
                    profileWindowView profileWindowView = new profileWindowView(staff);
                    profileWindowView.Show();
                    
                }
                else if (staff.Role == 2)
                {
                    // Chuyển hướng đến StaffView
                    StaffView staffView = new StaffView();
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