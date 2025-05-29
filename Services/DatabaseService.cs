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

        // Initialize the database and create tables for User and FoodProduct
        private async void InitializeDatabase()
        {
            // Create User and FoodProduct tables asynchronously
            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<FoodProduct>();
            await _database.CreateTableAsync<Order>();

            // Initialize default admin account if the Users table is empty
            await InitializeDefaultAdminAccountAsync();

            // Initialize default food products if the FoodProduct table is empty
            await InitializeDefaultFoodProductsAsync();

        }

        // Initialize default admin account if there are no users in the database
        private async Task InitializeDefaultAdminAccountAsync()
        {
            var users = await _database.Table<User>().ToListAsync();

            if (!users.Any()) // If no users exist, add the default admin account
            {
                var defaultAdmin = new User
                {
                    FullName = "Admin",
                    PhoneNumber = "123-456-7890",
                    Email = "admin@domain.com",
                    Password = "adminpassword", // In a real app, hash this password
                    Role = "Admin",
                    Address = "123 Admin Street, Admin City, Admin Country"
                };

                await _database.InsertAsync(defaultAdmin);
            }
        }

        // Initialize some default food products if the FoodProduct table is empty
        private async Task InitializeDefaultFoodProductsAsync()
        {
            var products = await _database.Table<FoodProduct>().ToListAsync();

            if (!products.Any()) // If no food products exist, add some default ones
            {
                var defaultProducts = new List<FoodProduct>
                {
                    new FoodProduct { Name = "Pizza", Category = "Main Course", Price = 12.99, Description = "Delicious cheese pizza", ImageUrl = "https://example.com/pizza.jpg" },
                    new FoodProduct { Name = "Burger", Category = "Main Course", Price = 8.49, Description = "Juicy beef burger", ImageUrl = "https://example.com/burger.jpg" },
                    new FoodProduct { Name = "Ice Cream", Category = "Dessert", Price = 4.99, Description = "Sweet and creamy ice cream", ImageUrl = "https://example.com/icecream.jpg" }
                };

                foreach (var product in defaultProducts)
                {
                    await _database.InsertAsync(product);
                }
            }
        }

        // User-related methods
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

        // FoodProduct-related methods
        public Task<int> AddFoodProductAsync(FoodProduct product)
        {
            return _database.InsertAsync(product);
        }

        public Task<List<FoodProduct>> GetAllFoodProductsAsync()
        {
            return _database.Table<FoodProduct>().ToListAsync();
        }

        public Task<int> DeleteFoodProductAsync(int productId)
        {
            return _database.Table<FoodProduct>().Where(p => p.Id == productId).DeleteAsync();
        }

        public Task<int> UpdateFoodProductAsync(FoodProduct product)
        {
            return _database.UpdateAsync(product);
        }
        public Task<int> AddOrderAsync(Order order)
        {
            return _database.InsertAsync(order);
        }

        public Task<List<Order>> GetAllOrdersAsync()
        {
            return _database.Table<Order>().ToListAsync();
        }
        public Task<int> UpdateOrderAsync(Order order)
        {
            return _database.UpdateAsync(order);
        }
    }
}
