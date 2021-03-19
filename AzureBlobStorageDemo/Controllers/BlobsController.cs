using AzureBlobStorageDemo.Models;
using AzureBlobStorageDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AzureBlobStorageDemo.Controllers
{

    [Route("/Containers/{containerName}/Blobs")]
    public class BlobsController : Controller
    {

        private readonly StorageDemoService _storageDemoService;

        public BlobsController(StorageDemoService storageDemoService)
        {
            _storageDemoService = storageDemoService;
        }



        // GET: StorageDemoBlobsController
        public ActionResult Index(string containerName)
        {
            var containerExists = _storageDemoService.CheckContainerExists(containerName);
            
            if ( !containerExists )
            {
                TempData["Message"] = new MessageModel() { Level = MessageLevel.Danger, Message = $"Unable to list blobs for container {containerName} as no container exists with that name" };
                RedirectToAction("Index", "Home");
            }

            var blobs = _storageDemoService.GetBlobs(containerName)
                .OrderBy(c => c.Name)
                .ToList();

            BlobsPageModel model = new BlobsPageModel()
            {
                StorageAccountName = _storageDemoService.GetStorageAccountName(),
                ContainerName = containerName,
                Blobs = blobs
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(string containerName, BlobUploadModel uploadModel)
        {
            using (Stream content = uploadModel.UploadFile.OpenReadStream())
            {
                _storageDemoService.UploadBlob(containerName, uploadModel.Name, uploadModel.UploadFile.ContentType, content);
            }
            return RedirectToAction("Index", new { containerName = containerName });
        }


        [Route("[action]")]
        public ActionResult Download(string containerName, string blobName)
        {
            var model = _storageDemoService.GetBlobContents(containerName, blobName);

            var result = new FileStreamResult(model.Content, model.ContentType) { FileDownloadName = model.Name} ;            
            return result;
        }



        [Route("[action]/{blobName}")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete(string containerName, string blobName)
        {
            _storageDemoService.DeleteBlob(containerName, blobName);

            return RedirectToAction("Index", new { containerName = containerName });
        }

    }
}
