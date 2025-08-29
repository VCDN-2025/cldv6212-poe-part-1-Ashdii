﻿using ABCRetailWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly BlobService _blobService;
        private readonly TableStorageService _tableStorageService;

        public ProductController(BlobService blobService, TableStorageService tableStorageService)
        {
            _blobService = blobService;
            _tableStorageService = tableStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _tableStorageService.GetAllProductsAsync();
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Models.Product product, IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                var imageUrl = await _blobService.UploadsAsync(stream, file.FileName);
                product.ImageUrl = imageUrl;
            }

            if (!ModelState.IsValid)
            {
                product.PartitionKey = "ProductPartition";
                product.RowKey = Guid.NewGuid().ToString();
                await _tableStorageService.AddProductAsync(product);
                return RedirectToAction("Index");
            }
            return View(product);

        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(string partitionKey, string rowKey, Models.Product product)
        {
            if (product !=null && !string.IsNullOrEmpty(product.ImageUrl))
            {
                await _blobService.DeleteBlobAsync(product.ImageUrl);
            }

            await _tableStorageService.DeleteProductAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }
    }
}
