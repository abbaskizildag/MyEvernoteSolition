using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
   public class RepositoryBase
    {
        protected static DatabaseContext context;
        private static object _lockSync = new object();

        protected RepositoryBase() //miras alan sadece bunu new'leyebilir demek
        {
            CreateContext();
        }
        private static void CreateContext()
        {
            if(context == null)
            {
                lock (_lockSync) //lock ile tek bir iş yapmasını sağlar
                {
                    context = new DatabaseContext();
                }
            }
        }
        
    }
}
