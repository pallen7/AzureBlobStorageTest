using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;



/*
todo:
- Create remove link with image
- Functionality to upload image
- Tidy up code a bit
- Add styling (https://mva.microsoft.com/en-US/training-courses/building-responsive-ui-with-bootstrap-8378)
- Create container (everything currently in images)
- List containers
- Remove container
- Drill through to containers and display images
- Look at security
- How do you unit test this?
*/

namespace BlobStorageTest.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            // Get the connection string
            CloudStorageAccount storage_account = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]
                );

            // Create the blob clien
            CloudBlobClient blob_client = storage_account.CreateCloudBlobClient();

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

            var view_model = GetBlobViewModel(containers);

            return View(view_model);
        }

        private static BlobViewModel GetBlobViewModel(IEnumerable<CloudBlobContainer> containers)
        {
            var view_model = new BlobViewModel();

            foreach (var container in containers)
            {
                var blobs = container.ListBlobs();

                foreach (var item in blobs)
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        var blob = (CloudBlockBlob)item;
                        view_model
                            .blobs
                            .Add(new BlobInfo
                            {
                                blob_uri = blob.Uri.ToString(),
                                blob_name = blob.Name.ToString()
                            });
                    }
                    else
                    {
                        view_model.blobs.Add(new BlobInfo { blob_uri = "", blob_name = "Not a CloudBlockBlob" + item.GetType().Name });
                    }
                }
            }

            return view_model;
        }

        public ActionResult Remove(string item_name)
        {
            return RedirectToAction("Index");
        }
    }

    public class BlobViewModel
    {
        public List<BlobInfo> blobs { get; set; }

        public BlobViewModel()
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