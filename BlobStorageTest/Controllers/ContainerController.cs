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

        public ActionResult UploadImage(string container_name, HttpPostedFileBase image)
        {
            // Need something to handle HttpPostedFileBase, file, image manipulation etc..
            if (image != null && image.ContentLength > 0)
            {
                cloud_store.upload_blob(container_name, System.IO.Path.GetFileName(image.FileName), image.InputStream);
            }

            return RedirectToAction("Index", new { container_name = container_name });
        }

        public ActionResult RemoveImage(string container_name, string image_name)
        {
            if (container_name.Length > 0 && image_name.Length > 0)
            {
                cloud_store.remove_blob(container_name, image_name);
            }

            return RedirectToAction("Index", new { container_name = container_name });
        }
    }
}