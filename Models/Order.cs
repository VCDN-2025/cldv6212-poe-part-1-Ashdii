using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace ABCRetailWebApp.Models
{
    public class Order : ITableEntity
    {
        public int Order_Id { get; set; }
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        //Introduce validation sample
        [Required(ErrorMessage = "Please select a customer.")]
        public string Customer_Id { get; set; }

        [Required(ErrorMessage = "Please select a product.")]
        public string Product_Id { get; set; }

        public DateTime Order_Date { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public double TotalAmount { get; set; }
    }
}