using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace ABCRetailWebApp.Models 
{ public class Customer :ITableEntity 
    {
        public string PartitionKey { get; set; } 
        public string RowKey { get; set; } 
        public DateTimeOffset? Timestamp { get; set; } 
        
        public ETag ETag { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    } 
}

