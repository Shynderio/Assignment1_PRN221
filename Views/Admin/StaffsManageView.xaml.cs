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
using System.Diagnostics.Metrics;

namespace Estore.Views.Admin
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    /// 
    public partial class StaffsManageView : Page
    {
        IStaffRepository staffRepository;
        private int indexNow;
        private int totalIndex;
        private readonly Frame _contentFrame;
        List<Tuple<Estore.Models.Staff, int>> lstStaffs;
        public StaffsManageView(Frame content)
        {   
            _contentFrame = content;
            indexNow = 1;
            totalIndex = 1;
            InitializeComponent();
        MyStoreContext dbContext = new MyStoreContext();
        staffRepository = new StaffRepository(dbContext);
        loadData();
        }

        //Action to tap

        


        private int getTotalOrderByID(int idStaff)
        {
            var total = 0;
            using (var dbContext = new MyStoreContext())
            {
                total = dbContext.Orders.Count(o => o.StaffId == idStaff);
            }
            return total;
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

            totalIndex = lstStaff.Count() / 5;
            lstStaffs = lstStaff.Zip(countArr, (staff, count) => new Tuple<Estore.Models.Staff, int>(staff, count)).ToList();
            var listTem = lstStaffs.ToList();
           listTem = listTem.Skip((indexNow - 1) * 5).Take(5).ToList();
            dataGrid.ItemsSource = listTem;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
           var ok =  MessageBox.Show("Do you want to delete?", "Delete", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if(ok == MessageBoxResult.OK)
            {
                var button = sender as Button;
                var dataId = button?.Tag as dynamic;
                if(dataId != null)
                {
                    var staffId = dataId;
                    var staffObj = staffRepository.GetStaffById(staffId);
                    if(staffObj != null)
                    {
                        staffRepository.DeleteStaff(staffObj);
                        loadData();
                    }
                    else
                    {
                        MessageBox.Show("Error, Can't delete this employee!", "Error", MessageBoxButton.OK,MessageBoxImage.Error);
                    }
                }
            }
            else
            {

            }
        }

        private void PaddingNext_Click(object sender, RoutedEventArgs e)
        {
            if (indexNow < totalIndex)
            {
                indexNow++;
                var listTemp = lstStaffs.Skip((indexNow-1)*5).Take(5).ToList();
                dataGrid.ItemsSource = listTemp;
            }
        }

        private void PaddingPrevod_Click(object sender, RoutedEventArgs e)
        {
            if (indexNow > 1)
            {
                indexNow--;
                var listTemp = lstStaffs.Skip((indexNow - 1) * 5).Take(5).ToList();
                dataGrid.ItemsSource = listTemp;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

            if (dataGrid.SelectedValue != null)
            {
                var selectedItem = (Tuple<Estore.Models.Staff, int>)dataGrid.SelectedItem;
                int staffId = selectedItem.Item1.StaffId;

                StaffUpdate staffUpdate = new StaffUpdate(staffId, _contentFrame);

                _contentFrame.Content = staffUpdate;
            }
            else
            {
                // Xử lý trường hợp không có mục nào được chọn
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            searchFunc();

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddStaffView addView = new AddStaffView(_contentFrame);
            _contentFrame.Content = addView;
        }


        private void searchFunc()
        {
            var name = txtName.Text;
            var role = cbxRole.SelectedIndex;
            var orders = cbxOrders.SelectedIndex;
            indexNow = 1;
            var listStaff = staffRepository.GetStaffList();
            List<int> countArr = new List<int>();

            if (name != null)
            {
                listStaff = listStaff.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();
            }

            if (role != 0)
            {
                listStaff = listStaff.Where(p => p.Role == role).ToList();
            }



            foreach (var item in listStaff)
            {
                var totalOrders = getTotalOrderByID(item.StaffId);
                countArr.Add(totalOrders);

            }

            totalIndex = listStaff.Count() / 5;
            lstStaffs = listStaff.Zip(countArr, (staff, count) => new Tuple<Estore.Models.Staff, int>(staff, count)).ToList();
            var listTem = lstStaffs.ToList();
            listTem = listTem.Skip((indexNow - 1) * 5).Take(5).ToList();
            dataGrid.ItemsSource = listTem;
        }
       

     
    }
}
