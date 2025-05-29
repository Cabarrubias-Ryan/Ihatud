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

        // Show "Track Delivery" for all users
        // trackDeliveryButton.IsVisible = true;

        // Show "Add Product" button only for admins
        addProductButton.IsVisible = _loggedInUser.Role == "Admin";
        viewUsersButton.IsVisible = _loggedInUser.Role == "Admin";

        // Binding context for data binding (if needed)
        BindingContext = this;
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

    // Event handler for Track Delivery button
    private async void OnTrackDeliveryClicked(object sender, EventArgs e)
    {
        // Logic for tracking delivery (show tracking page or service)
        await DisplayAlert("Track Delivery", "Here you can track your deliveries.", "OK");
    }
     private async void OnOrderClicked(object sender, EventArgs e)
    {
        // Logic for tracking delivery (show tracking page or service)
        await DisplayAlert("Order Food", "Order Food Successfully.", "OK");
    }

    // Event handler for Add Product button (Admin Only)
    private async void OnAddProductClicked(object sender, EventArgs e)
    {
        // Only for Admin: Navigate to the Product Addition page
        if (_loggedInUser.Role == "Admin")
        {
            await DisplayAlert("Add Product", "Here you can add new products.", "OK");
            // Navigate to a page for adding products if needed
        }
        else
        {
            await DisplayAlert("Access Denied", "Only admins can add products.", "OK");
        }
    }
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
