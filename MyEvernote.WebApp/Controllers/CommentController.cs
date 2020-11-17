using MyEvernote.Entities;
using MyEvernote.WebApp.Filters;
using MyEvernote.WebApp.Models;
using MyEvetnote.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    public class CommentController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CommentManager commentManager = new CommentManager();
        // GET: Comment
        public ActionResult ShowNoteComments(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  Note note = noteManager.Find(x => x.Id == id);
            Note note = noteManager.ListQueryable().Include("Comments").FirstOrDefault(x => x.Id == id);
            if (note==null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialComments",note.Comments);
        }

        [Auth]
        [HttpPost]
        public ActionResult Edit(int? id, string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //comment'i elde etmem lazım update etmek için.
            Comment comment = commentManager.Find(x => x.Id == id);

            if (comment==null)
            {
                return new HttpNotFoundResult();
            }
            comment.Text = text;
            if (commentManager.Update(comment)>0)
            {
                //result js'den gelen bir alan
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
      
        }
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //comment'i elde etmem lazım update etmek için.
            Comment comment = commentManager.Find(x => x.Id == id);

            if (comment == null)
            {
                return new HttpNotFoundResult();
            }
            if (commentManager.Delete(comment) > 0)
            {
                //result js'den gelen bir alan
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);

        }
        [Auth]
        [HttpPost]
        public ActionResult Create(Comment comment, int? noteid)
        {
            ModelState.Remove("CratedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                if (noteid == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //comment'i elde etmem lazım update etmek için.
                Note note = noteManager.Find(x => x.Id == noteid);

                if (note == null)
                {
                    return new HttpNotFoundResult();
                }

                comment.Note = note;
                comment.Owner = CurrentSession.User;
                if (commentManager.Insert(comment) > 0)
                {
                    //result js'den gelen bir alan
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
                
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);

        }
    }
}