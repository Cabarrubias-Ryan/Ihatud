using Ihatud.Models;
using Ihatud.Services;
using Microsoft.Maui.Controls;
using System.IO;

namespace Ihatud.Views.ViewOrder
{
    public partial class OrderFoods : ContentPage
    {
        private readonly User _loggedInUser;
        private readonly DatabaseService _databaseService;
        public bool IsAdmin => _loggedInUser?.Role == "Admin";

        public OrderFoods(User user)
        {
            InitializeComponent();
            _loggedInUser = user;
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "myapp.db");
            _databaseService = new DatabaseService(dbPath);

            BindingContext = this; // For IsAdmin binding
            LoadOrders();
        }

        private async void LoadOrders()
        {
            var orders = await _databaseService.GetAllOrdersAsync();
            OrdersCollectionView.ItemsSource = orders;
        }

        private async void OnAcceptOrderClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var order = button?.BindingContext as Order;
            if (order != null && IsAdmin && !order.IsAccepted)
            {
                order.IsAccepted = true;
                await _databaseService.UpdateOrderAsync(order);
                await DisplayAlert("Order Accepted", $"Order for {order.FoodProductName} accepted.", "OK");
                LoadOrders();
            }
        }
    }
}