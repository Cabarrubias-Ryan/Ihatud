using Ihatud.Models;
using Ihatud.Services;
using Microsoft.Maui.Controls;

namespace Ihatud;

public partial class AdminLoginPage : ContentPage
{
    private readonly DatabaseService _dbService;

    public AdminLoginPage(DatabaseService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Retrieve email and password from the UI
        string email = emailEntry.Text?.Trim();
        string password = passwordEntry.Text;

        // Check if email and password are not empty or null
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Please enter both email and password.", "OK");
            return;
        }

        // Check if user exists in the database
        var user = await _dbService.GetUserByEmailAsync(email);
        if (user != null && user.Password == password)
        {
            // Check if the user has the "Admin" role
            if (user.Role == "Admin")
            {
                // If the user is an Admin, navigate to the HomePage with the logged-in user
                await Navigation.PushAsync(new HomePage(user)); // Pass the user to HomePage
            }
            else
            {
                // If the user is not an Admin, show an access denied message
                await DisplayAlert("Access Denied", "You are not an Admin.", "OK");
            }
        }
        else
        {
            // Show login failure message
            await DisplayAlert("Login Failed", "Invalid credentials.", "OK");
        }
    }
}
