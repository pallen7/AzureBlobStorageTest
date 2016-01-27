using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BlobStorageTest
{
    public class AzureCloudStorage
    {
        // List images (container)
        // Delete image (container and name)

        public IEnumerable<CloudBlockBlob> list_blobs(string container_name)
        {
            // todo: no handling for other blob types

            var container = blob_client.GetContainerReference(container_name);

            if (!container.Exists())
            {
                throw new StorageException(string.Format("Invalid container name: {0}", container_name));
            }

            return container
                .ListBlobs()
                .Where(x => x.GetType() == typeof(CloudBlockBlob))
                .Select(x => (CloudBlockBlob)x)
                ;

        }

        public void upload_blob(string container_name, string file_name, System.IO.Stream image)
        {
            var container = blob_client.GetContainerReference(container_name);

            if (!container.Exists())
            {
                throw new StorageException(string.Format("Invalid container name: {0}", container_name));
            }

            if (image != null && image.Length > 0)
            {
                var blob = container.GetBlockBlobReference(file_name);
                blob.UploadFromStream(image);
            }
        }

        public CloudBlobContainer create_container(string container_name)
        {
            // todo:
            // Can currently only create container in root

            var container = blob_client.GetContainerReference(container_name);

            if (container.Exists())
            {
                throw new StorageException(string.Format("Container already exists: {0}", container_name));
            }

            container.Create();

            // Make the container accessible to anybody
            container.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            return container;
        }

        public IEnumerable<string> list_containers()
        {
            // todo:
            // Currently only lists containers in root

            return blob_client
                        .ListContainers()
                        .Select(x => x.Name)
                        ;
        }
        
        public void delete_container(string container_name)
        {
            blob_client.GetContainerReference(container_name).Delete();
        }

        public AzureCloudStorage()
        {
            var storage_account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            blob_client = storage_account.CreateCloudBlobClient();
        }

        private CloudBlobClient blob_client;
    }
}