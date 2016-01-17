using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlobStorageTest.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var view_model = new BlobViewModel();

            // Get the connection string
            CloudStorageAccount storage_account = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]
                );

            // Create the blob client
            CloudBlobClient blob_client = storage_account.CreateCloudBlobClient();

            //view_model.containers = blob_client.ListContainers()`.Select(x => x.Name).ToList();

            // Retrieve a reference to the container and create if it doesn't exist
            CloudBlobContainer container = blob_client.GetContainerReference("images");
            container.CreateIfNotExists();

            // Make the container accessible to anybody
            container.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            using (var image = System.IO.File.OpenRead(Server.MapPath("~/test_images/All Pictures 20141025 229.jpg")))
            {
                var blob = container.GetBlockBlobReference("picture1.jpg");
                blob.UploadFromStream(image);
            }

            var containers = blob_client.ListContainers().Select(x => x);

            foreach (var item in containers)
            {
                view_model.blobs.AddRange(item.ListBlobs().Select(x => x.Uri.ToString()));
            }

            return View(view_model);
        }
    }

    public class BlobViewModel
    {
        public BlobViewModel()
        {
            blobs = new List<string>();
        }
        public List<string> blobs { get; set; }
    }
}