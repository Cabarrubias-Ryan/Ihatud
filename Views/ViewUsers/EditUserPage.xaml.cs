using Ihatud.Models;
using Ihatud.Services;

namespace Ihatud.Views.ViewUsers
{
    public partial class EditUserPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        private User _userToEdit;

        public EditUserPage(User user, DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _userToEdit = user;

            // Populate the entry fields with the user's current information
            FullNameEntry.Text = _userToEdit.FullName;
            EmailEntry.Text = _userToEdit.Email;
            PhoneNumberEntry.Text = _userToEdit.PhoneNumber;
        }

        private async void OnSaveChangesClicked(object sender, EventArgs e)
        {
            // Validate the fields
            if (string.IsNullOrEmpty(FullNameEntry.Text) || string.IsNullOrEmpty(EmailEntry.Text) || string.IsNullOrEmpty(PhoneNumberEntry.Text))
            {
                await DisplayAlert("Validation Error", "All fields must be filled out.", "OK");
                return;
            }

            // Update the user object with new values
            _userToEdit.FullName = FullNameEntry.Text;
            _userToEdit.Email = EmailEntry.Text;
            _userToEdit.PhoneNumber = PhoneNumberEntry.Text;

            // Save the changes to the database
            var result = await _databaseService.UpdateUserAsync(_userToEdit);
            if (result > 0)
            {
                await DisplayAlert("Success", "User details updated successfully.", "OK");
                await Navigation.PopAsync(); // Go back to the previous page
            }
            else
            {
                await DisplayAlert("Error", "Failed to update user details.", "OK");
            }
        }
    }
}
