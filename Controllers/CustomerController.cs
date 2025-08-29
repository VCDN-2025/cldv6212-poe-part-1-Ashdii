using Microsoft.AspNetCore.Mvc;

namespace ABCRetailWebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly Services.TableStorageService _tableStorageService;

        public CustomerController(Services.TableStorageService tableStorageService)
        {
            _tableStorageService = tableStorageService;

        }

        public async Task<IActionResult> Index()
        {
            var customers = await _tableStorageService.GetAllCustomersAsync();
            return View(customers);
        }

        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            await _tableStorageService.DeleteCustomerAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }
        [HttpPost]

        public async Task<IActionResult> AddCustomer( Models.Customer customer)
        {
            customer.PartitionKey = "CustomerPartition"; // Static PartitionKey for simplicity
            customer.RowKey = Guid.NewGuid().ToString(); // Unique RowKey

            await _tableStorageService.AddCustomerAsync(customer);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }   
    }
}
