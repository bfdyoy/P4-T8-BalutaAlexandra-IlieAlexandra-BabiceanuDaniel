using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArtClubManager;
using System.Linq.Dynamic; //sort
using System.Web.Helpers; //gridView
using ArtClubManager.Data;
using ArtClubManager.Models;

namespace ArtClubManager.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class MembriiController : Controller
    {

        private ArtClubContext db = new ArtClubContext();

        public ActionResult Index(int page = 1, string sort = "Nume", string sortdir = "asc", string search = "")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetTables(search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            return View(data);
        }

        [HttpPost]
        public ActionResult SaveData(List<Membrii> Membrii)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (ArtClubContext dc = new ArtClubContext())
                //using()
                {
                    foreach (var i in Membrii)
                    {

                        dc.Membriis.Add(i);
                    }
                    dc.SaveChanges();
                    status = true;
                    Console.WriteLine(status);
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        public List<Membrii> GetTables(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            using (ArtClubContext dc = new ArtClubContext())
            {
                var v = (from a in dc.Membriis
                         where
                                 a.Nume.Contains(search) ||
                                 a.Prenume.Contains(search) ||
                                 a.Email.Contains(search) ||
                                 a.ID_NumeFunctie.Contains(search) ||
                                 a.ID_Username.Contains(search)
                         select a
                               );
                totalRecord = v.Count();
                v = v.OrderBy(sort + " " + sortdir);   //linq.dynamic
                if (pageSize > 0)
                {
                    v = v.Skip(skip).Take(pageSize);
                }
                return v.ToList();
            }
        }

        public void GetExcel()
        {
            List<Membrii> allCust = new List<Membrii>();
            using (ArtClubContext dc = new ArtClubContext())
            {
                allCust = dc.Membriis.ToList();
            }

            WebGrid grid = new WebGrid(source: allCust, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
                columns: grid.Columns(
                             grid.Column("ID_Username", "Username"),
                             grid.Column("Nume", "Nume"),
                             grid.Column("Prenume", "Prenume"),
                             grid.Column("Email", "Email"),
                             grid.Column("Parola", "Parola"),
                             grid.Column("ID_NumeFunctie", "NumeFunctie")
                        )
                    ).ToString();


            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Membrii.xls");
            Response.ContentType = "application/vnd.ms-excel"; //excel

            Response.Write(gridData);
            Response.End();
        }

        // GET: /Membrii/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Membrii membrii = db.Membriis.Find(id);
            if (membrii == null)
            {
                return HttpNotFound();
            }
            return View(membrii);
        }

        // GET: /Membrii/Create
        public ActionResult Create()
        {
            ViewBag.ID_NumeFunctie = new SelectList(db.Functies, "ID_NumeFunctie", "ID_NumeFunctie");
            return View();
        }

        // POST: /Membrii/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Username,Nume,Prenume,Email,Parola,ID_NumeFunctie")] Membrii membrii)
        {
            if (ModelState.IsValid)
            {
                db.Membriis.Add(membrii);
                db.SaveChanges();
                return RedirectToAction("../Membrii/Index");
            }

            ViewBag.ID_NumeFunctie = new SelectList(db.Functies, "ID_NumeFunctie", "ID_NumeFunctie", membrii.ID_NumeFunctie);
            return View(membrii);
        }

        // GET: /Membrii/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Membrii membrii = db.Membriis.Find(id);
            if (membrii == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_NumeFunctie = new SelectList(db.Functies, "ID_NumeFunctie", "ID_NumeFunctie", membrii.ID_NumeFunctie);
            return View(membrii);
        }

        // POST: /Membrii/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Username,Nume,Prenume,Email,Parola,ID_NumeFunctie")] Membrii membrii)
        {
            if (ModelState.IsValid)
            {
                db.Entry(membrii).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Membrii/Index");
            }
            ViewBag.ID_NumeFunctie = new SelectList(db.Functies, "ID_NumeFunctie", "ID_NumeFunctie", membrii.ID_NumeFunctie);
            return View(membrii);
        }

        // GET: /Membrii/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Membrii membrii = db.Membriis.Find(id);
            if (membrii == null)
            {
                return HttpNotFound();
            }
            return View(membrii);
        }

        // POST: /Membrii/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Membrii membrii = db.Membriis.Find(id);
            db.Membriis.Remove(membrii);
            db.SaveChanges();
            return RedirectToAction("../Membrii/Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
