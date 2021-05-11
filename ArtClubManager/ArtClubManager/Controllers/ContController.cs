using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArtClubManager;
using System.Web.Security;
using ArtClubManager.Data;
using ArtClubManager.Models;

namespace ArtClubManager.Controllers
{
    public class ContController : Controller
    {
        private ArtClubContext db = new ArtClubContext();
        
        public ActionResult UserRegistration()
        {
            ViewBag.ID_NumeFunctie = new SelectList(db.Functies, "ID_NumeFunctie", "ID_NumeFunctie");
            return View();
        }

        [HttpPost]
        public ActionResult UserRegistration(MembriiMetadataClass userdet)
        {

            if (ModelState.IsValid)
            {
                Membrii membrii = new Membrii();
                membrii.ID_Username = userdet.Username;
                membrii.Nume = userdet.Nume;
                membrii.Prenume = userdet.Prenume;
                membrii.Email = userdet.Email;
                membrii.Parola = userdet.Password;
                membrii.ID_NumeFunctie = userdet.ID_NumeFunctie;

                //userdet.ID_Username = userdet.Username;
                //userdet.Parola = userdet.Password;
                db.Membriis.Add(membrii);
                db.SaveChanges();


                if (userdet.ID_NumeFunctie == "Administrator")
                {
                    Session["ID_Username"] = membrii.ID_Username;
                    FormsAuthentication.SetAuthCookie(membrii.ID_Username, false);
                    return RedirectToAction("../Admin/Index");
                }
                else if (userdet.ID_NumeFunctie == "Membru" || userdet.ID_NumeFunctie == "Non-Membru")
                {
                    Session["ID_Username"] = membrii.ID_Username;
                    FormsAuthentication.SetAuthCookie(membrii.ID_Username, false);
                    return RedirectToAction("../User/Index");
                }
                else
                {
                    return RedirectToAction("UserRegistration");
                }
            }
            ViewBag.ID_NumeFunctie = new SelectList(db.Functies, "ID_NumeFunctie", "ID_NumeFunctie");

            return View(userdet);

        }

        //public JsonResult isUsernameAvaliable(string Username)   6/9v
        //{
        //    return Json(!db.Membrii.Any(x => x.ID_Username == Username ), JsonRequestBehavior.AllowGet);
        //}

    }
}
