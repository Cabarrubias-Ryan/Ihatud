using SQLite;
using Ihatud.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ihatud.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            InitializeDatabase();
        }

        private async void InitializeDatabase()
        {
            // Create User, LostItem, and FoundItem tables asynchronously
            await _database.CreateTableAsync<User>();

            // Check if the Users table is empty and insert a default admin account if so
            await InitializeDefaultAdminAccountAsync();
        }

        private async Task InitializeDefaultAdminAccountAsync()
        {
            // Check if there are any users in the database
            var users = await _database.Table<User>().ToListAsync();

            if (!users.Any()) // If no users exist, add the default admin account
            {
                var defaultAdmin = new User
                {
                    FullName = "Admin",
                    PhoneNumber = "123-456-7890",
                    Email = "admin@domain.com",
                    Password = "adminpassword", // In a real app, hash this password
                    Role = "Admin", // Set the Role to Admin
                    Address = "123 Admin Street, Admin City, Admin Country" // Default address for the admin
                };

                // Insert the default admin account into the database
                await _database.InsertAsync(defaultAdmin);
            }
        }

        // User-related methods (existing)
        public Task<int> AddUserAsync(User user)
        {
            return _database.InsertAsync(user);
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return _database.Table<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            return _database.Table<User>().ToListAsync();
        }

        public Task<int> DeleteUserAsync(int userId)
        {
            return _database.Table<User>().Where(u => u.Id == userId).DeleteAsync();
        }

        public Task<int> UpdateUserAsync(User user)
        {
            return _database.UpdateAsync(user);
        }

    }
}