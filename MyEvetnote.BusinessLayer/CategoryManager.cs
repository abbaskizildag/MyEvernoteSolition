
using MyEvernote.Entities;
using MyEvetnote.BusinessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvetnote.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        //public override int Delete(Category category)
        //{
        //    //Kategori ile ilişkili notların silinmesi gerekiyor.
        
        //    NoteManager noteManager = new NoteManager();
        //    LikedManager likedManager = new LikedManager();
        //    CommentManager commentManager = new CommentManager();
        //    foreach (Note note in category.Notes.ToList()) //list olarak dönerse her defasında o listeyi alırlar silinde hata vermez.
        //    {
        //        //Note ile ilişkili like'ları silinmeli
        //        foreach (Liked like in note.Likes.ToList())
        //        {
        //            likedManager.Delete(like);
        //        }
        //        //Note ile ilişkili comment'lerin silimesi
        //        foreach (Comment comment in note.Comments.ToList())
        //        {
        //            commentManager.Delete(comment);
        //        }

        //        noteManager.Delete(note); //en son not'ları silieriz.
        //    }
        //    return base.Delete(category);
        //}
    }
}
