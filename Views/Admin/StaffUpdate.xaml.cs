using Castle.Core.Internal;
using Estore.Models;
using Estore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Estore.Views.Admin
{
    /// <summary>
    /// Interaction logic for StaffUpdate.xaml
    /// </summary>
    /// 

    public partial class StaffUpdate : Page
    {

       private IStaffRepository staffRepository;
       private Estore.Models.Staff _staff;
        private readonly Frame _contentFrame;
        int staffId = 0;
        public StaffUpdate( int id, Frame content)
        {
            _contentFrame = content;
            this.staffId = id;
            InitializeComponent();
            MyStoreContext dbContext = new MyStoreContext();
            staffRepository = new StaffRepository(dbContext);
            loadData();
        }

        private void loadData()
        {
             _staff = staffRepository.GetStaffById(staffId);
            if (_staff != null) { DataContext = _staff;

                //txtManufacturer.Text = ConvertRole(_staff.Role);
            }
        }

        private String ConvertRole(int role)
        {
            if (role == 1)
            {
                return "Admin";
            } else if (role == 2)
            {
                return "Staff";
            } else
            {
                return "Unknow";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            staffRepository.DeleteStaff(_staff);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StaffsManageView staffView = new StaffsManageView(_contentFrame);
            _contentFrame.Content = staffView;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // ham update
            var name = txtCarName.Text;
            var role = cbxRole.SelectedIndex;
            var password = txtNewPass.Text;
            var isUpdated = false;

            if (!name.Equals(_staff.Name))
            {
                _staff.Name = name;
                isUpdated = true;
            }

            if (!role.Equals(_staff.Role))
            {
                _staff.Role = role;
                isUpdated = true;
            }

            if (!password.Equals(_staff.Password) && !password.IsNullOrEmpty()) 
            {
                _staff.Password = password;
                isUpdated = true;
            }

            if (isUpdated)
            {
                staffRepository.UpdateStaff(_staff);
               
                var mess = MessageBox.Show("Updated successfully!", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                if (mess == MessageBoxResult.OK)
                {
                    StaffsManageView staffView = new StaffsManageView(_contentFrame);
                    _contentFrame.Content = staffView;
                }
            } else
            {
                MessageBox.Show("Please change information before update !", "Error Update",MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
