using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VcapServicesExample.ViewModel;

namespace VcapServicesExample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult CloudFoundry()
        {

            var cloudFoundryApplication = ServerConfig.CloudFoundryApplication;
            var cloudFoundryServices = ServerConfig.CloudFoundryServices;
            return View(new CloudFoundryViewModel(
                cloudFoundryApplication == null ? new CloudFoundryApplicationOptions() : cloudFoundryApplication,
                cloudFoundryServices == null ? new CloudFoundryServicesOptions() : cloudFoundryServices));
        }
    }
}