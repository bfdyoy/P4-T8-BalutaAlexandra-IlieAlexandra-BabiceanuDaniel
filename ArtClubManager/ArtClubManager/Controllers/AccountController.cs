using ArtClubManager.Data;
using ArtClubManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security; // authentification

namespace ArtClubManager.Controllers
{
    public class AccountController : Controller
    {
        ArtClubContext db = new ArtClubContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Membrii memb)
        {
            var result = db.Membriis.Where(a => a.ID_Username == memb.ID_Username && a.Parola == memb.Parola).ToList();
            if (result.Count() > 0)
            {
                Session["ID_Username"] = result[0].ID_Username;
                FormsAuthentication.SetAuthCookie(result[0].ID_Username, false);
                if (result[0].ID_NumeFunctie == "Administrator")
                {
                    return RedirectToAction("../Admin/Index");
                }
                else if (result[0].ID_NumeFunctie == "Membru" || result[0].ID_NumeFunctie == "Non-Membru")
                {
                    return RedirectToAction("../User/Index");
                }

            }
            else
            {
                ViewBag.Message = "User sau parola incorecta";
            }

            return View(memb);
        }
    }
}
