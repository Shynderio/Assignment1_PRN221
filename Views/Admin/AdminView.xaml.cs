using Estore.Models;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;

namespace Estore.Views.Admin
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : Window
    {
        private readonly MyStoreContext context;
        public AdminView()
        {
            InitializeComponent();
            context = new MyStoreContext();
            loadProduct();
        }
        public void loadProduct()
        {
            List<Product> list = context.Products.Include(x => x.Category).ToList();
            //Include giong kieu join bang
            lvProduct.ItemsSource = list;
        }
    }
}
