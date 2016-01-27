using BlobStorageTest.ViewModels;
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
- To abstract away the Azure references we can work with absolute URIs based on the following:
- http://stackoverflow.com/questions/29285239/delete-a-blob-from-windows-azure-in-c-sharp
*/

namespace BlobStorageTest.Controllers
{
    public class HomeController : Controller
    {
        AzureCloudStorage cloud_store;

        public HomeController()
        {
            cloud_store = new AzureCloudStorage();
        }

        public ActionResult Index()
        {
            var containers = cloud_store.list_containers();

            return View(containers);
        }

        public ActionResult CreateContainer(string container_name)
        {
            cloud_store.create_container(container_name);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveContainer(string item_name)
        {
            cloud_store.delete_container(item_name);

            // Retrieve a reference to the container and create if it doesn't exist
           // CloudBlobContainer container = cloud_store.blob_client.GetContainerReference("images");

            //var blob = container.GetBlockBlobReference(item_name);
            //blob.Delete();

            return RedirectToAction("Index");
        }
    }
}