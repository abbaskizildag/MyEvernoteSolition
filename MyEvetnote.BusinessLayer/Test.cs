using MyEvernote.DataAccessLayer;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvetnote.BusinessLayer
{
    public class Test
    {
        public Test()
        {
            // DatabaseContext db = new DatabaseContext();
            // db.Database.CreateIfNotExists(); //database yok ise oluştur.
            //db.Categories.ToList();

            Repository<Category> repo = new Repository<Category>();
           List<Category> categories = repo.List(x=> x.Id>5);
        }


    }
}
