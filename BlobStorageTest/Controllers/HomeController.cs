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

            // Retrieve a reference to the container and create if it doesn't exist
            CloudBlobContainer container = blob_client.GetContainerReference("images");
            container.CreateIfNotExists();


            view_model.containers = blob_client.ListContainers().Select(x => x.Name).ToList();

            return View(view_model);
        }
    }

    public class BlobViewModel
    {
        public IEnumerable<string> containers { get; set; }
    }
}