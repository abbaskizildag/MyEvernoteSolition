using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.Entities;
using MyEvernote.WebApp.Filters;
using MyEvetnote.BusinessLayer;
using MyEvetnote.BusinessLayer.Results;

namespace MyEvernote.WebApp.Controllers
{
    [Auth]

    public class EvernoteUserController : Controller
    {
        private EvetnoteUserManager evetnoteUserManager = new EvetnoteUserManager();
        // GET: EvernoteUser
        public ActionResult Index()
        {
            return View(evetnoteUserManager.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvernoteUser evernoteUser = evetnoteUserManager.Find(x=>x.Id==id.Value);
            if (evernoteUser == null)
            {
                return HttpNotFound();
            }
            return View(evernoteUser);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EvernoteUser evernoteUser)
        {
            ModelState.Remove("CratedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                //Todo : düzeltilecek
                BusinessLayerResult<EvernoteUser> res = evetnoteUserManager.Insert(evernoteUser);

                if (res.Errors.Count>0) //hata var demektir
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    //bu kod view'de summary kısmında hatanın görünmesini sağlıyord
                    return View(evernoteUser);
                }
                return RedirectToAction("Index");
            }

            return View(evernoteUser);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvernoteUser evernoteUser = evetnoteUserManager.Find(x => x.Id == id.Value);
            if (evernoteUser == null)
            {
                return HttpNotFound();
            }
            return View(evernoteUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EvernoteUser evernoteUser)
        {
            ModelState.Remove("CratedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                //Todo : düzeltilecek
                BusinessLayerResult<EvernoteUser> res = evetnoteUserManager.Update(evernoteUser);

                if (res.Errors.Count > 0) //hata var demektir
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    //bu kod view'de summary kısmında hatanın görünmesini sağlıyord
                    return View(evernoteUser);
                }
                return RedirectToAction("Index");
            }

            return View(evernoteUser);
        }

        // GET: EvernoteUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvernoteUser evernoteUser = evetnoteUserManager.Find(x => x.Id == id.Value);
            if (evernoteUser == null)
            {
                return HttpNotFound();
            }
            return View(evernoteUser);
        }

        // POST: EvernoteUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EvernoteUser evernoteUser = evetnoteUserManager.Find(x => x.Id == id);
            evetnoteUserManager.Delete(evernoteUser);
            return RedirectToAction("Index");
        }
    }
}
