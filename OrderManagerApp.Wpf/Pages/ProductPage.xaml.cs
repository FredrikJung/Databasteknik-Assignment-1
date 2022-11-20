using OrderManagerApp.WebApi.Models;
using OrderManagerApp.WebApi.Models.Entities;
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
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();
            PopulateProducts().ConfigureAwait(false);
        }
        public async Task PopulateProducts()
        {
            try
            {
                using var client = new HttpClient();
                var products = await client.GetFromJsonAsync<IEnumerable<ProductModel>>("https://localhost:7131/api/Products");

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
        private async void btn_Create_Product_Click(object sender, RoutedEventArgs e)
        {
            if( tb_Product_Name.Text != "" && tb_Product_Price.Text != "")
            {
                try
                {
                    using var client = new HttpClient();

                    await client.PostAsJsonAsync("https://localhost:7131/api/Products", new ProductRequest
                    {
                        Name = tb_Product_Name.Text,
                        Price = decimal.Parse(tb_Product_Price.Text)
                    });

                    MessageBox.Show("Produkt skapad");
                    tb_Product_Name.Text = "";
                    tb_Product_Price.Text = "";
                    cb_Products.SelectedIndex = -1;
                    await PopulateProducts();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Namn och pris krävs att fylla i för att skapa ny produkt");
            }
        }
        private async void btn_Update_Product_Click(object sender, RoutedEventArgs e)
        {
            if( tb_Product_Name.Text != "" && tb_Product_Price.Text != "")
            {
                try
                {
                    var uRL = "https://localhost:7131/api/Products";
                    using var client = new HttpClient();
                    var product = (ProductModel)cb_Products.SelectedItem;

                    product.Name = tb_Product_Name.Text;
                    product.Price = decimal.Parse(tb_Product_Price.Text);

                    await client.PutAsJsonAsync($"{uRL}?id={product.ProductId}", product);

                    MessageBox.Show("Produkt uppdaterad");
                    tb_Product_Name.Text = "";
                    tb_Product_Price.Text = "";
                    cb_Products.SelectedIndex = -1;
                    await PopulateProducts();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Namn och pris krävs att fylla i för att uppdatera produkt");
            }
        }
        private void cb_Products_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var product = (ProductModel)cb_Products.SelectedItem;

                if (product != null)
                {
                    tb_Product_Name.Text = product.Name;
                    tb_Product_Price.Text = product.Price.ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void btn_Delete_Product_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var uRL = "https://localhost:7131/api/Products/id";
                using var client = new HttpClient();
                var product = (ProductModel)cb_Products.SelectedItem;

                var products = new ObservableCollection<ProductModel>();

                products.Remove(product);

                await client.DeleteAsync($"{uRL}?id={product.ProductId}");

                MessageBox.Show("Produkt borttagen");
                tb_Product_Name.Text = "";
                tb_Product_Price.Text = "";
                cb_Products.SelectedIndex = -1;
                await PopulateProducts();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
