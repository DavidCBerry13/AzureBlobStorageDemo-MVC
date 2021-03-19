using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using Azure.ResourceManager.Storage;
using Azure.ResourceManager.Storage.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobStorageDemo.Models;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageDemo.Services
{
    public class StorageDemoService
    {

        public StorageDemoService(ResourcesManagementClient resourceManagementClient, StorageManagementClient storageManagementClient,
            BlobServiceClient blobServiceClient)
        {
            _resourceManagementClient = resourceManagementClient;
            _storageManagementClient = storageManagementClient;
            _blobServiceClient = blobServiceClient;
        }



        private ResourcesManagementClient _resourceManagementClient;
        private StorageManagementClient _storageManagementClient;
        private BlobServiceClient _blobServiceClient;



        public string GetStorageAccountName()
        {
            return _blobServiceClient.AccountName;
        }


        public IEnumerable<StorageContainerModel> GetContainers()
        {
            var response = _blobServiceClient.GetBlobContainers();

            return response.Select(c => new StorageContainerModel() { Name = c.Name });
        }


        public MessageModel CreateContainer(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            if (containerClient.Exists())
            {
                // Container already exists
                return new MessageModel() { Level = MessageLevel.Warning, Message = $"Could not create container {containerName} as it a container with that name already exists in this storage account" };
            }
            else
            {
                containerClient.Create();
                return new MessageModel() { Level = MessageLevel.Success, Message = $"Container {containerName} created" };
            }
        }


        public MessageModel DeleteContainer(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            if (containerClient.Exists())
            {
                _blobServiceClient.DeleteBlobContainer(containerName);
                return new MessageModel() { Level = MessageLevel.Success, Message = $"Container {containerName} deleted" };
            }
            else
            {
                // Can't delete a container that does not exist
                return new MessageModel() { Level = MessageLevel.Warning, Message = $"Could not delete container {containerName} as no container with that name exists in this storage account" };
            }
        }


        public bool CheckContainerExists(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return containerClient.Exists();
        }


        public IEnumerable<BlobModel> GetBlobs(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobs = containerClient.GetBlobs();
            var models = blobs.Select(b => new BlobModel()
            {
                Name = b.Name,
                Tags = b.Tags,
                ContentEncoding = b.Properties.ContentEncoding,
                ContentType = b.Properties.ContentType,
                Size = b.Properties.ContentLength,
                CreatedOn = b.Properties.CreatedOn,
                AccessTier = b.Properties.AccessTier?.ToString(),
                BlobType = b.Properties.BlobType?.ToString()
            });

            return models;
        }


        public MessageModel UploadBlob(string containerName, string blobName, String contentType, Stream content)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            if (!containerClient.Exists())
            {
                return new MessageModel() { Level = MessageLevel.Danger, Message = $"Could not upload blob as no container with the name {containerName} exists in this storage account" };
            }
            else
            {
                //var blobInfo = containerClient.UploadBlob(blobName, content);
                
                var blobClient = containerClient.GetBlobClient(blobName);
                var options = new BlobUploadOptions() { HttpHeaders = new BlobHttpHeaders() { ContentType = contentType }  } ;              
                blobClient.Upload(content, options);

                return new MessageModel() { Level = MessageLevel.Success, Message = $"Blob {blobName} uploaded to container {containerName}" };
            }
        }

        public void DeleteBlob(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            containerClient.DeleteBlob(blobName);
        }


        public BlobContentModel GetBlobContents(String containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);            
            var blobClient = containerClient.GetBlobClient(blobName);

            return new BlobContentModel() {
                Name = blobName,
                ContentType = containerClient.GetBlobs().FirstOrDefault()?.Properties.ContentType,
                Content = blobClient.OpenRead()
            };
        }

    }
}
