using System;
using System.Collections.Generic;
using System.Globalization;
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
using Estore.Models;
using Microsoft.EntityFrameworkCore;
using Estore.Repositories;

namespace Estore.Views.Admin
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    /// 
    public partial class StaffsManageView : Window
    {
        IStaffRepository staffRepository;

        List<Tuple<Estore.Models.Staff, int>> lstStaffs;
        public StaffsManageView(IStaffRepository staff)
        {   
            
            
            InitializeComponent();
            staffRepository = staff;
            loadData();
        }

        //Action to tap

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SearchStaff();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Function to write

        private int getTotalOrderByID(int idStaff)
        {
            var total = 0;
            using (var dbContext = new MyStoreContext())
            {
                total = dbContext.Orders.Count(o => o.StaffId == idStaff);
            }
            return total;
        }


        private void SearchStaff()
        {
            var searchName = txtName.Text;
            var searchRole = cbxRole.SelectedIndex;
            var searchOrders = cbxOrders.SelectedIndex;

            List<int> countArr = new List<int>();
            //List<Tuple<Estore.Models.Staff, int>> combineList = new List<Tuple<Estore.Models.Staff, int>>();
            var lstStaff = staffRepository.GetStaffList();

            var listSearch = lstStaff;

            if (searchName.Length > 0)
            {
                listSearch = listSearch.Where(p => p.Name.ToLower().Contains(searchName.ToLower()));
            }

         
            //MessageBox.Show($"Name : {searchName}  Role : {searchRole} and Orders : {searchOrders}");
            
            if(searchRole != 0)
            {
                listSearch = listSearch.Where( p => p.Role == searchRole );
            }
            else { };

           

            foreach (var item in listSearch)
            {
                var totalOrders = getTotalOrderByID(item.StaffId);
                countArr.Add(totalOrders);

            }

            


            var combineList = listSearch.Zip(countArr, (staff, count) => new Tuple<Estore.Models.Staff, int>(staff, count)).ToList();
            lstStaffs = combineList;

            if(searchOrders == 1)
            {

                lstStaffs = combineList.OrderBy(item => item.Item2).ToList();
            }
            else if(searchOrders == 2)
            {
                lstStaffs = combineList.OrderByDescending(item => item.Item2).ToList();
            }

            listViewStaff.ItemsSource = lstStaffs;
        }

        private void loadData()
        {
            List<int> countArr = new List<int>();
            //List<Tuple<Estore.Models.Staff, int>> combineList = new List<Tuple<Estore.Models.Staff, int>>();
            var lstStaff = staffRepository.GetStaffList();

            foreach (var item in lstStaff)
            {
                var totalOrders = getTotalOrderByID(item.StaffId);
                countArr.Add(totalOrders);

            }


            lstStaffs = lstStaff.Zip(countArr, (staff, count) => new Tuple<Estore.Models.Staff, int>(staff, count)).ToList();

            listViewStaff.ItemsSource = lstStaffs;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // MainFrame.Content = new StaffInsert();
            //new StaffUpdate().Show();
        }

        private void listViewStaff_selected(object sender, SelectionChangedEventArgs e)
        {

            if (listViewStaff.SelectedValue != null)
            {
                var selectedItem = (Tuple<Estore.Models.Staff, int>)listViewStaff.SelectedItem;
                int staffId = selectedItem.Item1.StaffId;

                StaffUpdate staffUpdate = new StaffUpdate(staffId);
                staffUpdate.Show();
            }
            else
            {
                // Xử lý trường hợp không có mục nào được chọn
            }
        }
    }
}
