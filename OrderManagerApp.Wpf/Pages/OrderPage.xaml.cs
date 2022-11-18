using OrderManagerApp.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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

namespace OrderManagerApp.Wpf.Pages
{
    /// <summary>
    /// Interaction logic for OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public OrderPage()
        {
            InitializeComponent();
            PopulateCustomersAndProducts().ConfigureAwait(false);
            lv_Order.ItemsSource = products;
        }

        private ObservableCollection<ProductModel> products = new ObservableCollection<ProductModel>();
        public async Task PopulateCustomersAndProducts()
        {
            try
            {
                using var client = new HttpClient();
                var customers = await client.GetFromJsonAsync<IEnumerable<CustomerModel>>("https://localhost:7131/api/Customers");


                var c_collection = new ObservableCollection<CustomerModel>();

                foreach (var customer in customers)
                {
                    c_collection.Add(customer);
                }
                cb_Customers.ItemsSource = c_collection;


                using var client2 = new HttpClient();
                var products = await client2.GetFromJsonAsync<IEnumerable<ProductModel>>("https://localhost:7131/api/Products");

                var p_collection = new ObservableCollection<ProductModel>();

                foreach (var product in products)
                {
                    p_collection.Add(product);
                }
                cb_Products.ItemsSource = p_collection;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Quantity.Text == "" || tb_Quantity.Text == "0")
            {
                MessageBox.Show("Inte tillåtet att välja antal 0 av en produkt, försök igen");
                tb_Quantity.Text = "";
                cb_Customers.SelectedIndex = -1;
                cb_Products.SelectedIndex = -1;
            }
            else
            {
                try
                {
                    var product = (ProductModel)cb_Products.SelectedItem;
                    product.Quantity = int.Parse(tb_Quantity.Text);
                    products.Add(product);
                    lv_Order.ItemsSource = products;

                    tb_Quantity.Text = "";
                    cb_Products.SelectedIndex = -1;
                }
                catch
                {
                    MessageBox.Show("Något gick fel, försök igen");
                    tb_Quantity.Text = "";
                    cb_Customers.SelectedIndex = -1;
                    cb_Products.SelectedIndex = -1;
                }
            }

        }
        private async void btn_Creat_Order_Click(object sender, RoutedEventArgs e)
        {
            var url = "https://localhost:7131/api/Order";
            using var client = new HttpClient();

            var customer = (CustomerModel)cb_Customers.SelectedItem;
            decimal orderTotalPrice = 0;
            decimal unitPrice = 0;

            foreach (var item in products)
            {
                unitPrice += item.Price;
            }

            foreach (var item in products)
            {
                orderTotalPrice += item.Price * item.Quantity;
            }

            if (customer != null)
            {
                try
                {
                    await client.PostAsJsonAsync($"{url}", new OrderRequest
                    {
                        CustomerId = customer.CustomerId,
                        OrderDate = DateTime.Now,
                        DueDate = DateTime.Now.AddDays(30),
                        Products = products,
                        Quantity = products.Count,
                        Price = unitPrice,
                        TotalPrice = orderTotalPrice

                    });

                    MessageBox.Show($"Order för kund {customer.Name} är skapad");
                    products.Clear();
                    tb_Quantity.Text = "";
                    cb_Customers.SelectedIndex = -1;
                    cb_Products.SelectedIndex = -1;
                }


                catch
                {
                    MessageBox.Show("Något gick fel, försök igen");
                    tb_Quantity.Text = "";
                    cb_Customers.SelectedIndex = -1;
                    cb_Products.SelectedIndex = -1;
                }
            }

        }

    }
}
