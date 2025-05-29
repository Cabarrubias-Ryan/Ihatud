using Ihatud.Models;
using Ihatud.Services;
using Ihatud.Views.ViewUsers;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace Ihatud;

public partial class HomePage : ContentPage
{
    private readonly User _loggedInUser;
    private DatabaseService _databaseService;

    public HomePage(User user)
    {
        InitializeComponent();

        _loggedInUser = user;

        // Create the SQLite database path (local path for each platform)
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "myapp.db");

        // Initialize the DatabaseService
        _databaseService = new DatabaseService(dbPath);

        viewUsersButton.IsVisible = _loggedInUser.Role == "Admin";

        // Binding context for data binding (if needed)
        BindingContext = this;
        LoadFoodProducts();
    }

    // Event handler for Menu Click
    private void OnMenuClicked(object sender, EventArgs e)
    {
        Sidebar.IsVisible = true;
        SidebarOverlay.IsVisible = true;
    }

    // Event handler for Overlay Tap
    private void OnOverlayTapped(object sender, EventArgs e)
    {
        Sidebar.IsVisible = false;
        SidebarOverlay.IsVisible = false;
    }

    // Event handler for Home button click
    private async void OnHomeClicked(object sender, EventArgs e)
    {
        Sidebar.IsVisible = false;
        SidebarOverlay.IsVisible = false;
        await DisplayAlert("Info", "You are already on the home page.", "OK");
    }
    private async void LoadFoodProducts()
    {
        // Fetch food products from the database
        var foodProducts = await _databaseService.GetAllFoodProductsAsync();

        // Bind the food products to the UI dynamically
        FoodProductCollectionView.ItemsSource = foodProducts;
    }
    
    // Event handler for Track Delivery button
    private async void OnTrackDeliveryClicked(object sender, EventArgs e)
    {
        // Logic for tracking delivery (show tracking page or service)
        // Example navigation from HomePage.xaml.cs
        await Navigation.PushAsync(new Ihatud.Views.ViewOrder.OrderFoods(_loggedInUser));
    }
     private async void OnOrderClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var foodProduct = button?.BindingContext as FoodProduct;
        if (foodProduct != null)
        {
            var order = new Order
            {
                FoodProductId = foodProduct.Id,
                FoodProductName = foodProduct.Name,
                Price = foodProduct.Price,
                OrderedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            await _databaseService.AddOrderAsync(order);
            await DisplayAlert("Order Placed", $"You ordered: {foodProduct.Name}", "OK");
        }
    }

    // Event handler for Add Product button (Admin Only)
    private async void OnViewUsersClicked(object sender, EventArgs e)
    {
        Sidebar.IsVisible = false;
        SidebarOverlay.IsVisible = false;

        if (_loggedInUser.Role == "Admin")
        {
            await Navigation.PushAsync(new ViewUsersPage()); 
        }
        else
        {
            await DisplayAlert("Access Denied", "Only admins can view users.", "OK");
        }
    }

    // Event handler for Logout button click
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        Sidebar.IsVisible = false;
        SidebarOverlay.IsVisible = false;
        await Navigation.PopToRootAsync(); // Pops to the root page (likely login page)
    }
}
