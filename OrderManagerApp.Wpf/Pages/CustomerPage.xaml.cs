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
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        public CustomerPage()
        {
            InitializeComponent();
            PopulateCustomers().ConfigureAwait(false);
        }

        public async Task PopulateCustomers()
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void btn_Create_Customer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var client = new HttpClient();

                await client.PostAsJsonAsync("https://localhost:7131/api/Customers", new CustomerRequest
                {
                    Name = tb_Customer_Name.Text
                });

                MessageBox.Show("Kund skapad");
                tb_Customer_Name.Text = "";
                cb_Customers.SelectedIndex = -1;
                await PopulateCustomers();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void btn_Update_Customer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var uRL = "https://localhost:7131/api/Customers";
                using var client = new HttpClient();
                var customer = (CustomerModel)cb_Customers.SelectedItem;

                customer.Name = tb_Customer_Name.Text;

                await client.PutAsJsonAsync($"{uRL}?id={customer.CustomerId}", customer);

                MessageBox.Show("Kund uppdaterad");
                tb_Customer_Name.Text = "";
                cb_Customers.SelectedIndex = -1;
                await PopulateCustomers();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void cb_Customers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var customer = (CustomerModel)cb_Customers.SelectedItem;

                if (customer != null)
                {
                    tb_Customer_Name.Text = customer.Name;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void btn_Delete_Customer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var uRL = "https://localhost:7131/api/Customers/id";
                using var client = new HttpClient();
                var customer = (CustomerModel)cb_Customers.SelectedItem;
                var customers = new ObservableCollection<CustomerModel>();

                customers.Remove(customer);

                await client.DeleteAsync($"{uRL}?id={customer.CustomerId}");

                MessageBox.Show("Kund borttagen");
                tb_Customer_Name.Text = "";
                cb_Customers.SelectedIndex = -1;
                await PopulateCustomers();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
