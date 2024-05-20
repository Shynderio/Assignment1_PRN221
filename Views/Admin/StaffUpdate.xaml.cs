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
    public partial class StaffUpdate : Window
    {
       private IStaffRepository staffRepository;
       private Estore.Models.Staff _staff;

        int staffId = 0;
        public StaffUpdate( int id)
        {
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

                txtManufacturer.Text = ConvertRole(_staff.Role);
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
    }
}
