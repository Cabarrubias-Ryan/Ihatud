using Ihatud.Models;
using Ihatud.Services;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ihatud.Views.ViewUsers
{
    public partial class ViewUsersPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<User> Users { get; set; }

        public ViewUsersPage()
        {
            InitializeComponent();

            // Directly create the DatabaseService by getting the DB path from the FileSystem.
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "myapp.db");
            _databaseService = new DatabaseService(dbPath);

            Users = new ObservableCollection<User>();
            BindingContext = this;
            LoadUsersAsync();  // Load the users when the page is initialized
        }

        // Function to load the users from the database and filter out admin users
        private async Task LoadUsersAsync()
        {
            var users = await _databaseService.GetAllUsersAsync();

            // Filter out the "Admin" users
            var nonAdminUsers = users.Where(user => user.Role != "Admin").ToList();

            Users.Clear();
            foreach (var user in nonAdminUsers)
            {
                Users.Add(user);
            }
        }

        // Delete user function
        private async void OnDeleteUserClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var userId = (int)button.CommandParameter;
            var result = await _databaseService.DeleteUserAsync(userId);
            if (result > 0)
            {
                // Reload users after deletion
                await LoadUsersAsync();  // Reload the list of users after deletion
            }
        }

        // Edit user function (You can directly update the user in the database)
        private async void OnEditUserClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var userToEdit = button.CommandParameter as User;
            if (userToEdit != null)
            {
                // Navigate to EditUserPage and pass the user to edit
                await Navigation.PushAsync(new EditUserPage(userToEdit, _databaseService));  // The EditUserPage handles DB changes
            }
        }

        // Reload users after the EditUserPage update (this should be triggered when you return to the ViewUsersPage)
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadUsersAsync();  // Refresh the users when the page is visible again
        }
    }
}
