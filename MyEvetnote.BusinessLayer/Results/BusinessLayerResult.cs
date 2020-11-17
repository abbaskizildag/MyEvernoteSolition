using MyEvernote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvetnote.BusinessLayer.Results
{
    public class BusinessLayerResult<T> where T : class
    {
        public BusinessLayerResult()
        {
            Errors = new List<ErrorMessageObj>();
        }
        public List<ErrorMessageObj> Errors { get; set; } 

        public T Result { get; set; }

        public void AddError(ErrorMessageCode code, string message) //mesajı ve bu mesaja uygun code u bu şekilde ekliyoruz
        {
            Errors.Add(new ErrorMessageObj() {
                Code=code,
                Message=message
             });
        }
    }
}
