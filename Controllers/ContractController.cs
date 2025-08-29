using ABCRetailWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailWebApp.Controllers
{
    public class ContractController : Controller
    {
        private readonly AzureFileShareService _fileShareService;
        public ContractController(AzureFileShareService fileShareService)
        {
            _fileShareService = fileShareService;
        }
        
        public async Task<IActionResult> Index()
        {
            List<Models.Contract> contracts;
            try
            {
                contracts = await _fileShareService.ListFilesAsync("uploads");
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error retrieving contracts :{ ex.Message}";
                contracts = new List<Models.Contract>();
            }
            return View(contracts);
        }

        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Message"] = "Please select a file to upload.";
                return RedirectToAction("Index");
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    string directoryName = "uploads";
                    string fileName = file.FileName;
                    await _fileShareService.UploadFileAsync(directoryName, fileName, stream);
                }
                TempData["Message"] = $"File {file.FileName} uploaded successfully.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error uploading file: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
        [HttpGet]

        public async Task<IActionResult> DownloadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                
                return BadRequest("Filename must be provided.");
            }
            try
            {
                var fileStream = await _fileShareService.DownloadFileAsync("uploads", fileName);
                if (fileStream == null)
                {
                    return NotFound($"File '{fileName}' not found.");
                }
                return File(fileStream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                
                return BadRequest($"Error downloading file; {ex.Message}");
            }
        }


    }
}
