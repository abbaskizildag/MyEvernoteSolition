using MyEvernote.Entities;
using MyEvetnote.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MyEvernote.WebApp.Models
{
    public class CacheHelper
    {
        public static List<Category> GetCategoriesFromCache()
        {
            //cache'den okuyan olmaması durumda oluşturan bir metot yazdık. static bir metot
            var result = WebCache.Get("category-cashe");
            if (result==null)
            {
                CategoryManager  categoryManager = new CategoryManager();
                result = categoryManager.List();
                WebCache.Set("category-cashe",result, 20, true);

            }
            return result;
        }

        public static void RemoveCategoriesFromChace()
        {
            Remove("category-cache");
        }
        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }
    }
}