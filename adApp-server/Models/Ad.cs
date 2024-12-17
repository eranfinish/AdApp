using System.Security.AccessControl;
using System.Transactions;

namespace AdApp.Models
{
    public class Ad
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public DateTime createdAt { get; set; }
        public int ownerId { get; set; }
        public string imageBase64 { get; set; } // Base64 encoded image
        public decimal price { get; set; }
        public string currency { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
    }


}
