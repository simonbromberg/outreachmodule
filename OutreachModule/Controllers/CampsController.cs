using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OutreachModule.Models;
using System.Diagnostics;

namespace OutreachModule.Controllers
{
    public class CampsController : Controller
    {        
        private ModelManager manager = new ModelManager();
        // GET: Camps
        public ActionResult Index()
        {
            return View(manager.campList);
        }

        // GET: Camps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Camp camp = manager.getCampWithId((int)id);
            if (camp == null)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Camp", new { campId = id });
        }

        // GET: Camps/Create
        public ActionResult Create()
        {
            SetCampTypeViewBag();
            return View();
        }

        // POST: Camps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Camp campToCreate)
        {
            if (ModelState.IsValid)
            {
                var success = manager.addCamp(campToCreate);
                
                if (success)
                {
                    return RedirectToAction("Details", new { id = campToCreate.Id });
                }
            }

            ViewBag.Message = "Saving was unsuccessful";
            SetCampTypeViewBag();
            return View(campToCreate);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var camp = manager.getCampWithId((int)id);
            if (camp == null)
            {
                return RedirectToAction("Index");
            }
            SetCampTypeViewBag(camp.type);
            return View(camp);
        }

        // POST: Camps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Camp camp)
        {
            if (ModelState.IsValid)
            {
                if (manager.editCamp(camp)) { 
                    return RedirectToAction("Index");
                }
            }
            SetCampTypeViewBag(camp.type);
            return View(camp);
        }

        // GET: Camps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Camp camp = manager.getCampWithId((int)id);
            if (camp == null)
            {
                return RedirectToAction("Index");
            }
            return View(camp);
        }

        // POST: Camps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (manager.removeCamp(id))
            {
                return RedirectToAction("Index");
            }
            ViewBag.Message = "Deletion unsuccessful";
            return View(manager.getCampWithId(id));
        }
        private void SetCampTypeViewBag(string type = null)
        {
            ViewBag.TypeSelect = manager.CampTypeSelect(type);
        }
    }
}
