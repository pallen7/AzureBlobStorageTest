using System.Collections.Generic;

namespace BlobStorageTest.ViewModels
{
    public class ContainerViewModel
    {
        public string container_name { get; set; }
        public List<BlobInfo> blobs { get; set; }

        public ContainerViewModel()
        {
            blobs = new List<BlobInfo>();
        }
    }

    public class BlobInfo
    {
        public string blob_uri { get; set; }
        public string blob_name { get; set; }
    }
}