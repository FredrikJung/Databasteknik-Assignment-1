using OrderManagerApp.WebApi.Models;
using OrderManagerApp.Wpf.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace OrderManagerApp.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new OrderPage();
        }

        private void btn_Product_Page_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ProductPage();  
        }

        private void btn_Customer_Page_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new CustomerPage();
        }

        private void btn_Order_Page_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new OrderPage();
        }
    }
}
