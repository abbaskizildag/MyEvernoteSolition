using MyEvernote.Common;
using MyEvernote.Core.DataAccess;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class Repository<T>:RepositoryBase, IDataAccess<T> where T:class
    {
        //buradaki db repositorybase'den geliyor.
        private DbSet<T> _objectSet; //DbSet Object de olabilir.
        public Repository()
        {
           
            _objectSet = context.Set<T>(); //alt tarafta devamllı oluşturmak yerine bu şekil yaptık.
        }
        public List<T> List()
        {
          return  _objectSet.ToList(); //Burada normalde altını çiziyordu. Bunu çözmek için T'nin class olması gerektiğini belirttik.
        }
        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>(); //ıquaryable dönecek.
        }
        public List<T> List(Expression<Func<T, bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }
        public int Insert(T obj)
        {
            _objectSet.Add(obj);
            //aşağıdaki işlemi yapmamızın nedeni Createon, modfioOn ve modifionusername'in 3'nünde otomatik burada değer alıp atanmasını sağlamak.
            if (obj is MyEntityBase) //obj MyEntityBase ise diyoruz
            {
                MyEntityBase o = obj as MyEntityBase; //dönüştürme işlemi set etmek dedik
                DateTime now = DateTime.Now;

                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.common.GetCurrentUsername(); //TODO : İşlem yapan kullanıcı adı yazılmalı
            }
            return Save();
        }
        public int Update(T obj)
        {
            if (obj is MyEntityBase) //obj MyEntityBase ise diyoruz
            {
                MyEntityBase o = obj as MyEntityBase; //dönüştürme işlemi set etmek dedik
               

                //o.CreatedOn = now; //bu insert'te oluştu bu yüzden kaldırdık update'de
                o.ModifiedOn = DateTime.Now;
                o.ModifiedUsername = App.common.GetCurrentUsername(); //TODO : İşlem yapan kullanıcı adı yazılmalı
            }
            return Save();
        }
        public int Delete(T obj)
        {
            //Remove etmek istemiyorsak verileri tablomuza IsDeleted ekler. Buradan onu aktif ederiz. aşağıdaki gibi kodları yazarız.
            //if (obj is MyEntityBase) //obj MyEntityBase ise diyoruz
            //{
            //    MyEntityBase o = obj as MyEntityBase; //dönüştürme işlemi set etmek dedik

            //    //o.CreatedOn = now; //bu insert'te oluştu bu yüzden kaldırdık update'de
            //    o.ModifiedOn = DateTime.Now;
            //    o.ModifiedUsername = "system"; //TODO : İşlem yapan kullanıcı adı yazılmalı
            //}
            _objectSet.Remove(obj);
            return Save();
        }

        public int Save()
        {
            return context.SaveChanges();
        }
        public T Find(Expression<Func<T,bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }
    }
}
