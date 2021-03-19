using AzureBlobStorageDemo.Models;
using AzureBlobStorageDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AzureBlobStorageDemo.Utilities;

namespace AzureBlobStorageDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StorageDemoService _storageDemoService;

        public HomeController(ILogger<HomeController> logger, StorageDemoService storageDemoService)
        {
            _storageDemoService = storageDemoService;
        }

        public IActionResult Index()
        {
            var containers = _storageDemoService.GetContainers()
                .OrderBy(c => c.Name)
                .ToList();

            StorageDemoIndexModel model = new StorageDemoIndexModel()
            {
                StorageAccountName = _storageDemoService.GetStorageAccountName(),
                Containers = containers
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string containerName)
        {
            var result = _storageDemoService.CreateContainer(containerName);
            TempData.Put<MessageModel>("Message", result);

            return RedirectToAction("Index");
        }



        [Route("[action]/{containerName}")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete(string containerName)
        {
            _storageDemoService.DeleteContainer(containerName);

            return RedirectToAction("Index");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }







    }
}
