using System.Web.Mvc;


/*
todo:
- Ensure that we handle container names with a space in
- Limit to images only
- May have to change the size of the images - particularly the thumbnails

- To do in other project:
- Look at security
- Add styling (https://mva.microsoft.com/en-US/training-courses/building-responsive-ui-with-bootstrap-8378)
- Look at the way that instagram lays out pictures. It's fairly awesome.
- How do you unit test this?
- To abstract away the Azure references we can work with absolute URIs based on the following:
- http://stackoverflow.com/questions/29285239/delete-a-blob-from-windows-azure-in-c-sharp
- Deployment
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

            return RedirectToAction("Index");
        }
    }
}