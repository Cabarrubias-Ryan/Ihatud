using SQLite;

namespace Ihatud.Models
{
    public class Order
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int FoodProductId { get; set; }
        public string FoodProductName { get; set; }
        public double Price { get; set; }
        public string OrderedAt { get; set; } // Store as string for simplicity
                                              // Add UserId or other fields as needed
        public bool IsAccepted { get; set; } = false;

        [Ignore]
        public string DisplayStatus => IsAccepted ? "Wait in 20 mins your order will arrive" : "Waiting";
    }
}