using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageDemo.Models
{
    public class StorageDemoIndexModel
    {


        public string StorageAccountName { get; set; }

        public List<StorageContainerModel> Containers { get; set; }


    }
}
