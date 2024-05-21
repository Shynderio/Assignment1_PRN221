using Estore.Models;
using Estore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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

namespace Estore.Views.Admin
{
    /// <summary>
    /// Interaction logic for AddStaffView.xaml
    /// </summary>
    public partial class AddStaffView : Page
    {

        private IStaffRepository staffRepository;
        private Estore.Models.Staff _staff;
        private readonly Frame _contentFrame;

        public AddStaffView(Frame content)
        {
            _contentFrame = content;
      
            InitializeComponent();
            MyStoreContext dbContext = new MyStoreContext();
            staffRepository = new StaffRepository(dbContext);
            
        }


        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            var password = txtNewPass.Text;
            var password2 = txtNewPass2.Text;
            var name = txtCarName.Text;
            var role = cbxRole.SelectedIndex;

            if (!password.Equals(password2))
            {
                MessageBox.Show("The password is not match, Please again!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                Estore.Models.Staff staff = new Estore.Models.Staff();
                staff.Name = name;
                staff.Role = role;
                staff.Password = password;

                staffRepository.AddNewStaff(staff);
                var mess = MessageBox.Show("Staff was created successully", "Success",MessageBoxButton.OK, MessageBoxImage.Information);
                if (mess == MessageBoxResult.OK)
                {
                    StaffsManageView staffs = new StaffsManageView(_contentFrame);
                    _contentFrame.Content = staffs;
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            StaffsManageView staffView = new StaffsManageView(_contentFrame);
            _contentFrame.Content = staffView;
        }
    }

    
}
