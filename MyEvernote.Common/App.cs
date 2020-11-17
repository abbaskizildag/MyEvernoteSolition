using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Common
{
    public static class App //dışardan erişilecek sınıf bu.
    {
        public static ICommon common = new DefaultCommon(); //defaultcommon ile çalışıyoruz artık.
    }
}
