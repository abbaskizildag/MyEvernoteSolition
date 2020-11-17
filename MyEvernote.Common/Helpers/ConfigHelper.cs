using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Common.Helpers
{
    public class ConfigHelper
    {
        public static T Get<T> (string key) //içerisinde bulunan static get metotu ile verilen anahtar değeri config içindeki yazdığımız kodda okuycak geri döndürcek.
        {

            //bu class ile appsetting ve connectionstring'i okunabilir.
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T)); //changtype ile dönüştürdük.
            //burada dönüştürme amacımız port numarasını int almak için. normalde string okuyorduk

        }
    }
}
