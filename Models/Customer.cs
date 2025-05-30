using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OrderManagement.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CustomerSegment Segment { get; set; }
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
