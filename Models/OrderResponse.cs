namespace OrderManagement.Models
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}
