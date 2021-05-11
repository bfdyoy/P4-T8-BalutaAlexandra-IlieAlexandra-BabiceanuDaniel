using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtClubManager.Controllers
{
    [Authorize(Roles = "Membru,Non-Membru")]
    public class UserController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult secondPage()
        {
            return View();
        }
    }
}