using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArtClubActivityManagement;
using System.Linq.Dynamic; //sort
using System.Web.Helpers; //gridView
using ArtClubManager.Data;
using ArtClubManager.Models;

namespace ArtClubActivityManagement.Controllers
{
    [Authorize(Roles = "Membru,Non-Membru,Administrator")]
    public class ResurseController : Controller
    {
        private ArtClubContext db = new ArtClubContext();

        // GET: /Resurse/
        public ActionResult Index(int page = 1, string sort = "ID_Status", string sortdir = "asc", string search = "")
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

        public List<Resurse> GetTables(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            using (ArtClubContext dc = new ArtClubContext())
            {
                var v = (from a in dc.Resurses
                         where
                                 a.ID_NumeResursa.Contains(search) ||
                                 a.ID_Locatie.Contains(search) ||
                                 a.CostZi.Contains(search) ||
                                 a.ID_Status.Contains(search)
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

        // GET: /Resurse/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resurse resurse = db.Resurses.Find(id);
            if (resurse == null)
            {
                return HttpNotFound();
            }
            return View(resurse);
        }

        // GET: /Resurse/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.ID_Locatie = new SelectList(db.Locaties, "ID_Locatie", "ID_Locatie");
            ViewBag.ID_Status = new SelectList(db.Status, "ID_Status", "ID_Status");
            return View();
        }

        // POST: /Resurse/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_NumeResursa,ID_Locatie,CostZi,ID_Status")] Resurse resurse)
        {
            if (ModelState.IsValid)
            {
                db.Resurses.Add(resurse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Locatie = new SelectList(db.Locaties, "ID_Locatie", "ID_Locatie", resurse.ID_Locatie);
            ViewBag.ID_Status = new SelectList(db.Status, "ID_Status", "ID_Status", resurse.ID_Status);
            return View(resurse);
        }

        // GET: /Resurse/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resurse resurse = db.Resurses.Find(id);
            if (resurse == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Locatie = new SelectList(db.Locaties, "ID_Locatie", "ID_Locatie", resurse.ID_Locatie);
            ViewBag.ID_Status = new SelectList(db.Status, "ID_Status", "ID_Status", resurse.ID_Status);
            return View(resurse);
        }

        // POST: /Resurse/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_NumeResursa,ID_Locatie,CostZi,ID_Status")] Resurse resurse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resurse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Locatie = new SelectList(db.Locaties, "ID_Locatie", "ID_Locatie", resurse.ID_Locatie);
            ViewBag.ID_Status = new SelectList(db.Status, "ID_Status", "ID_Status", resurse.ID_Status);
            return View(resurse);
        }

        // GET: /Resurse/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resurse resurse = db.Resurses.Find(id);
            if (resurse == null)
            {
                return HttpNotFound();
            }
            return View(resurse);
        }

        // POST: /Resurse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Resurse resurse = db.Resurses.Find(id);
            db.Resurses.Remove(resurse);
            db.SaveChanges();
            return RedirectToAction("Index");
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
