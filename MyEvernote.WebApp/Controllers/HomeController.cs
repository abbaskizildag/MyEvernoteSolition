using MyEvernote.Entities;
using MyEvernote.Entities.ValueObject;
using MyEvernote.Entities.Messages;
using MyEvetnote.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEvernote.WebApp.ViewModels;
using MyEvetnote.BusinessLayer.Results;
using MyEvernote.WebApp.Models;
using MyEvernote.WebApp.Filters;

namespace MyEvernote.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryeManager = new CategoryManager();
        private EvetnoteUserManager evernoteUserManager = new EvetnoteUserManager();
        // GET: Home
        public ActionResult Index()
        {

            //CategoryController üzerinden gelen view talebi model
            //if (TempData["mm"]!=null)
            //{
            //    return View(TempData["mm"] as List<Note>);
            //}
            
            return View(noteManager.ListQueryable().Where(x=>x.IsDraft==false).OrderByDescending(x => x.ModifiedOn).ToList()); //bu kod c# tarafında sorgu yapıldıktan sonra 
            //return View(nm.GetAllNotesQuryable().OrderByDescending(x => x.ModifiedOn).ToList()); //bu kodda ise quaryable ile sql tarafında sorguyu çekiyoruz

        }
        public ActionResult ByCategory(int? id) //id boş geçilebilir
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            //}


            //Category cat = categoryeManager.Find(x=>x.Id==id.Value); //burada nullable değerin tipini vermek için value

            //if (cat == null)
            //{
            //    return HttpNotFound();
            //    //return RedirectToAction("Index", "Home");
            //}
            ////TempData["mm"] = cat.Notes; //bir aciton'dan diğerine geçerken hayatta kalabilen bir action çantası
            //return View("Index", cat.Notes.Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());

            List<Note> notes = noteManager.ListQueryable().Where(x => x.IsDraft == false && x.CategoryId == id).OrderByDescending(x => x.ModifiedOn).ToList();

            return View("Index", notes);
        }

        public ActionResult MostLiked()
        {
            return View("Index", noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList()); //likedcount'a göre descending yapıcaz.

        }
        public ActionResult About()
        {
            return View();
        }

        [Auth]
        public ActionResult ShowProfile()
        {
            //EvernoteUser currentUser = Session["login"] as EvernoteUser; //burada logindeki veriyi elde ettik //bu koda gerek kalmadı artık.
             
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(CurrentSession.User.Id); //üstteki session kodunu bu şekil revize ettik.
            if (res.Errors.Count > 0)
            {
                //TODO: Kullanıcı bir hata ekranına yönlendirmemiz gerekiyor.
                ErrorViewModel errornotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                //TempData["errors"] = res.Errors;
                return View("Error", errornotifyObj);
            }
            return View(res.Result);
        }
        [Auth]
        public ActionResult EditProfile()
        {
            //EvernoteUser currentUser = Session["login"] as EvernoteUser; //burada logindeki veriyi elde ettik
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(CurrentSession.User.Id);
            if (res.Errors.Count > 0)
            {
                //TODO: Kullanıcı bir hata ekranına yönlendirmemiz gerekiyor.
                ErrorViewModel errornotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                //TempData["errors"] = res.Errors;
                return View("Error", errornotifyObj);
            }
            return View(res.Result);
        }

        [Auth]
        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                //bize hep EvernoteUser nesnesi hemde profile resmi dosyası gelebilir
                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpg" ||
                    ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}"; //varsayılan isim olarak user kullanıcının id'si ve content type'nı split ile ayırdık ve türünü ekledik.
                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}")); //fiziksel olarak kaydediyor.
                    model.ProfileImageFileName = filename;
                }
                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.UpdataProfile(model);
                if (res.Errors.Count > 0)
                {
                    //TODO: Kullanıcı bir hata ekranına yönlendirmemiz gerekiyor.
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirectingUrl = "/Home/EditProfile"

                    };
                    //TempData["errors"] = res.Errors;
                    return View("Error", errorNotifyObj);
                }
                CurrentSession.Set<EvernoteUser>("login", res.Result); //profil güncellendiği için session güncellendi
                // Session["login"] = res.Result;  bu koduda reviz ettik.


                return RedirectToAction("ShowProfile");
            }
            return View(model);
        }
        [Auth]
        public ActionResult DeleteProfile()
        {
            //EvernoteUser currentUser = Session["login"] as EvernoteUser;
           
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.RemoveUserById(CurrentSession.User.Id);
            if (res.Errors.Count > 0)
            {
                
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Güncellenemedi",
                    RedirectingUrl = "/Home/ShowProfile"

                };
                //TempData["errors"] = res.Errors;
                return View("Error", errorNotifyObj);
            }
            Session.Clear(); 

            return RedirectToAction("Index");
        }
        public ActionResult TestNotify()
        {
            ErrorViewModel model = new ErrorViewModel()
            {
                Header = "Yönlendirme",
                Title = "Ok Test",
                RedirectingTimeout = 10000,
                Items = new List<ErrorMessageObj> { new ErrorMessageObj() { Message = "test başarılı1" } }
            };
            return View("Error", model);
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    //res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive); //bu kod ile hataları if ile kontrol ederek yönlendirme yapabiliyoruz

                    return View(model);
                }

                //session'a kullanıcı bilgi saklama
               CurrentSession.Set<EvernoteUser>("login", res.Result); //ilgili nesneyi sesion olarak login'e attık
                //Yönlendirme
                return RedirectToAction("Index");
            }



            return View(model);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) //eğer model geçerliyse
            {


                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.RegisterUser(model);


                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login"

                };
                notifyObj.Items.Add("Lütfen eposta adresinize gönderdiğimiz aktivasyon link'ine  tıklayarak hesabınızı aktive ediniz. Hesabınız aktive etmeden not ekleyemez ve beğenme yapamazsınız. ");
                return View("Ok", notifyObj);
            }

            return View(model);
            //EvernoteUser user = null;

            //try
            //{
            //    eum.RegisterUser(model);
            //}
            //catch (Exception ex)
            //{

            //    ModelState.AddModelError("", ex.Message);
            //}

            //if (user==null) //user nesnesi null'sa hata var demektir
            //{
            //    return View(model);
            //}
            //if (model.Username == "aaa")
            //{
            //    ModelState.AddModelError("", "Kullanıcı adı kullanılıyor");

            //}
            //foreach (var item in ModelState)
            //{
            //    if (item.Value.Errors.Count > 0)
            //    {
            //        return View(model);
            //    }
            //}




            //kullanıcı username kontrolü
            //kullanıcı e-posta kontrolü
            //kayıt işlemi
            //aktivasyon e-postası gönderimi

        }

        public ActionResult UserActivate(Guid id)
        {
            //kullanıcı aktivasyonu sağlanacak.
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errornotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
                //TempData["errors"] = res.Errors;
                return View("Error", errornotifyObj);
            }
            OkViewModel okNotifyObj = new OkViewModel()
            {
                Header = "Hesap Aktifleştirildi",
                RedirectingUrl = "/Home/Login",

            };
            okNotifyObj.Items.Add(" Hesabınızı Aktifleştirildi. Artık not paylaşabilir ve beğenme yapabilirsiniz.");
            return View("Ok", okNotifyObj);
        }
        public ActionResult Logout()
        {
            Session.Clear(); //session temizlenince kullanıcı çıkış yapmış oluyor
            return RedirectToAction("Index");
        }

        public ActionResult AccesDenied()
        {
            return View();
        }
        public ActionResult HasError()
        {
            return View();
        }
    }
}