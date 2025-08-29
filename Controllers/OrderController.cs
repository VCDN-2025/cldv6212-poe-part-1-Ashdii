using ABCRetailWebApp.Models;
using ABCRetailWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailWebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly TableStorageService _tableStorageService;
        private readonly QueueService _queueService;

        public OrderController(TableStorageService tableStorageService, QueueService queueService)
        {
            _tableStorageService = tableStorageService;
            _queueService = queueService;
        }

        // GET: Order Index
        public async Task<IActionResult> Index()
        {
            var orders = await _tableStorageService.GetAllOrdersAsync();
            var customers = await _tableStorageService.GetAllCustomersAsync();
            var products = await _tableStorageService.GetAllProductsAsync();

            ViewData["Customers"] = customers;
            ViewData["Products"] = products;

            return View(orders);
        }

        // GET: Order/AddOrder
        public async Task<IActionResult> AddOrder()
        {
            var customers = await _tableStorageService.GetAllCustomersAsync();
            var products = await _tableStorageService.GetAllProductsAsync();

            if (!customers.Any() || !products.Any())
            {
                ModelState.AddModelError("", "Please ensure there are customers and products before creating an order.");
            }

            ViewData["Customer"] = customers;
            ViewData["Product"] = products;

            return View();
        }

        // POST: Order/AddOrder
        [HttpPost]
        public async Task<IActionResult> AddOrder(Order order)
        {
            var customers = await _tableStorageService.GetAllCustomersAsync();
            var products = await _tableStorageService.GetAllProductsAsync();
            ViewData["Customer"] = customers;
            ViewData["Product"] = products;

            if (!ModelState.IsValid)
            {
                return View(order);
            }

            order.Order_Date = DateTime.SpecifyKind(order.Order_Date, DateTimeKind.Utc);
            order.PartitionKey = "OrderPartition";
            order.RowKey = Guid.NewGuid().ToString();

            await _tableStorageService.AddOrdersAsync(order);

            // Optional: send message to queue
            string message = $"New order placed by customer {order.Customer_Id} " +
                             $"for product {order.Product_Id} " +
                             $"total {order.TotalAmount} on {order.Order_Date}";
            await _queueService.SendMessage(message);

            return RedirectToAction("Index");
        }
    }
}
