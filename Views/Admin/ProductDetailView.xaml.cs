using Estore.Models;
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

namespace Estore.Views.Admin
{
    /// <summary>
    /// Interaction logic for ProductDetailView.xaml
    /// </summary>
    public partial class ProductDetailView : Page
    {
        public ProductDetailView()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id, price;
                var isInt = Int32.TryParse(productId.Text, out id);
                if (!isInt)
                    throw new Exception("Product ID is invalid!");
                isInt = Int32.TryParse(unitPrice.Text, out price);
                if (!isInt)
                    throw new Exception("Unit price must be a positive integer!");
                Product product = new Product()
                {
                    CategoryId = id,
                    ProductName = productName.Text,
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        static bool ValidateProduct(Product product)
        {
            if (string.IsNullOrEmpty(product.ProductName))
                throw new Exception("Product name is a required field");
            if (product.ProductName.Trim().Length < 6)
                throw new Exception("Product name must be at least 6-character long");
            if (!product.ProductName.All(o => char.IsLetter(o)))
                throw new Exception("Product name must contain letters only");
            if (product.UnitPrice <= 0)
                throw new Exception("Unit price must be bigger than 0");
            return true;
        }
    }
}
