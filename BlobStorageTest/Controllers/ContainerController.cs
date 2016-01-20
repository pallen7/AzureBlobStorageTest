using BlobStorageTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlobStorageTest.Controllers
{
    public class ContainerController : Controller
    {

        AzureCloudStorage cloud_store;

        public ContainerController()
        {
            cloud_store = new AzureCloudStorage();
        }

        public ActionResult Index(string container_name)
        {
            var view_model = new ContainerViewModel();
            view_model.container_name = container_name;

            view_model
                .blobs
                .AddRange(
                    cloud_store
                        .list_blobs(container_name)
                        .Select
                        (
                            x => new BlobInfo
                            {
                                blob_name = x.Name,
                                blob_uri = x.Uri.ToString()
                            }
                        ));

            return View(view_model);
        }
    }
}