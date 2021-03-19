using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageDemo.Models
{

    /// <summary>
    /// Represents the data for the Razor page that lists the blobs in a container
    /// </summary>
    public class BlobsPageModel
    {
        /// <summary>
        /// The name of the storage account the blobs shown on this page are in
        /// </summary>
        public string StorageAccountName { get; set; }

        /// <summary>
        /// The name of the container in the storage account the blobs for this page are in
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// The list of blobs
        /// </summary>
        public List<BlobModel> Blobs { get; set; }


    }
}
