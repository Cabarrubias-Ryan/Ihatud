using Ihatud.Models;
using Ihatud.Services;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace Ihatud;

public partial class LoginRegisterPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private bool isLoginMode = true;

    // Constructor to initialize the database service and email service
    public LoginRegisterPage(DatabaseService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
    }

    private void OnToggleClicked(object sender, EventArgs e)
    {
        isLoginMode = !isLoginMode;

        TitleLabel.Text = isLoginMode ? "Login" : "Register";
        actionButton.Text = isLoginMode ? "Login" : "Register";
        toggleButton.Text = isLoginMode ? "No account? Register here" : "Have an account? Login here";

        nameEntry.IsVisible = !isLoginMode;
        phoneEntry.IsVisible = !isLoginMode;
    }

    private async void OnActionClicked(object sender, EventArgs e)
    {
        string email = emailEntry.Text?.Trim();
        string password = passwordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Please fill in all required fields.", "OK");
            return;
        }

        if (isLoginMode)
        {
            var user = await _dbService.GetUserByEmailAsync(email);
            if (user != null && user.Password == password)
            {
                await DisplayAlert("Success", $"Welcome back, {user.FullName}!", "OK");
                await Navigation.PushAsync(new HomePage(user));
            }
            else
            {
                await DisplayAlert("Login Failed", "Invalid email or password.", "OK");
            }
        }
        else
        {
            string fullName = nameEntry.Text?.Trim();
            string phone = phoneEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(phone))
            {
                await DisplayAlert("Error", "Please complete all fields.", "OK");
                return;
            }

            var existingUser = await _dbService.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                await DisplayAlert("Error", "Email already registered.", "OK");
                return;
            }

            var newUser = new User
            {
                FullName = fullName,
                PhoneNumber = phone,
                Email = email,
                Password = password,
                Role = "User" // Default
            };

            await _dbService.AddUserAsync(newUser); // Add user to the database
            await DisplayAlert("Success", "Account registered!", "OK");

            // Send registration notification email
            //string emailSubject = "Welcome to Our App!";
            //string emailContent = $"Hello {fullName},\n\nThank you for registering with us. We're excited to have you as a part of our community!\n\nBest regards,\nYour App Team";
            //await _emailService.SendEmailAsync(email, emailSubject, emailContent); // Send the email

            // Clear form fields
            emailEntry.Text = string.Empty;
            passwordEntry.Text = string.Empty;
            nameEntry.Text = string.Empty;
            phoneEntry.Text = string.Empty;

            // Switch to login mode after successful registration
            OnToggleClicked(null, null);
        }
    }
}
