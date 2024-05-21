using Estore.Models;
using Estore.Session_Login;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Estore.Views
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : Page
    {
        private readonly MyStoreContext context;

        public ProfileView()
        {
            SessionManage sessionManage = SessionManage.Instance;
            string username = (string)sessionManage.GetSession("Username");

            InitializeComponent();
            context = new MyStoreContext();
            var staff = context.Staffs.FirstOrDefault(o =>  o.Name == username);

            this.DataContext = staff;
        }

        private void btnEditName_Click(object sender, RoutedEventArgs e)
        {
            if (txtStaffName != null)
            {
                txtStaffName.IsReadOnly = false;
                stpButtonChangeName.Visibility = Visibility.Visible;
                stpButtonHandle.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Không tìm thấy ID!");
            }
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (txtStaffName != null)
            {
                txtStaffName.IsReadOnly = false;
                stpButtonPassWord.Visibility = Visibility.Visible;
                stpPasswordTextBox.Visibility = Visibility.Visible;
                stpButtonHandle.Visibility = Visibility.Hidden;
                stpStaffInfo.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Không tìm thấy ID!");
            }
        }

        private void btnUpdateName_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtStaffName.Text))
            {
                MessageBox.Show("Tên không hợp lệ");
                return;
            }
            try
            {
                var staff = (Models.Staff)DataContext; // Lay staff hien tai tu DataContext
                if (staff != null)
                {
                    staff.Name = txtStaffName.Text;
                    context.Staffs.Update(staff);
                    if (context.SaveChanges() > 0)
                    {
                        MessageBox.Show("Cập nhật thành công!");
                        txtStaffName.IsReadOnly = true;
                        stpButtonChangeName.Visibility = Visibility.Hidden;
                        stpButtonHandle.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (System.Exception)
            {
                MessageBox.Show("Tên không hợp lệ");
            }
        }

        private void btnCancelName_Click(object sender, RoutedEventArgs e)
        {
            var staff = (Models.Staff)DataContext; // ep kieu cho Datacontxt
            if (staff != null)
            {
                txtStaffName.Text = staff.Name;
                txtStaffName.IsReadOnly = true;
                stpButtonChangeName.Visibility = Visibility.Hidden;
                stpButtonHandle.Visibility = Visibility.Visible;
            }
        }

        private void btnUpdatePassWord_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOldPassword.Password) ||
                string.IsNullOrWhiteSpace(txtPassword.Password) ||
                string.IsNullOrWhiteSpace(txtPassword_2.Password))
            {
                MessageBox.Show("Mật khẩu không được để trống");
                return;
            }

            var staff = (Models.Staff)DataContext;
            if (staff != null)
            {
                if (txtOldPassword.Password == staff.Password)
                {
                    if (txtPassword.Password == txtPassword_2.Password)
                    {
                        staff.Password = txtPassword.Password;
                        context.Staffs.Update(staff);
                        if (context.SaveChanges() > 0)
                        {
                            MessageBox.Show("Cập nhật mật khẩu thành công!");
                            txtStaffName.IsReadOnly = true;
                            stpButtonPassWord.Visibility = Visibility.Hidden;
                            stpPasswordTextBox.Visibility = Visibility.Hidden;
                            stpButtonHandle.Visibility = Visibility.Visible;
                            stpStaffInfo.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mật khẩu mới và mật khẩu xác nhận không khớp nhau");
                    }
                }
                else
                {
                    MessageBox.Show("Mật khẩu cũ không đúng");
                }
            }
        }


        private void btnCancelPassWord_Click(object sender, RoutedEventArgs e)
        {
            var staff = (Models.Staff)DataContext;
            if (staff != null)
            {
                txtStaffName.Text = staff.Name;
                txtStaffName.IsReadOnly = true;
                stpButtonPassWord.Visibility = Visibility.Hidden;
                stpPasswordTextBox.Visibility = Visibility.Hidden;
                stpButtonHandle.Visibility = Visibility.Visible;
                stpStaffInfo.Visibility = Visibility.Visible;
            }
        }
    }
}
