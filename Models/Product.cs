using Azure;
using Azure.Data.Tables;

namespace ABCRetailWebApp.Models;
public class Product : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public string ProductId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string ImageUrl { get; set; }
    public string Category { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;   
}
