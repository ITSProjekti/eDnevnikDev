using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDnevnikDev.Controllers
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

        public ActionResult DodavanjeOcenePopup()
        {
            return View();
        }

        public ActionResult US84()
        {
            ViewBag.Message = "Story 84";
            return View();
        }

        public ActionResult US74()
        {
            ViewBag.Message = "Story 74";
            return View();
        }

        public ActionResult US82()
        {
            ViewBag.Message = "";
            return View();
        }

        public ActionResult TableTemplate()
        {
            ViewBag.Message = "";
            return View();
        }

        public ActionResult Collapse()
        {
            ViewBag.Message = "";
            return View();
        }
    }
}