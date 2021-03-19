using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageDemo.Models
{
    public class BlobContentModel
    {

        public string Name { get; set; }

        public string ContentType { get; set; }

        public Stream Content { get; set; }

    }
}
