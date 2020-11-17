using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using MyEvernote.Entities.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEvernote.Entities.Messages;
using MyEvernote.Common.Helpers;
using MyEvetnote.BusinessLayer.Results;
using MyEvetnote.BusinessLayer.Abstract;

namespace MyEvetnote.BusinessLayer
{
    public class EvetnoteUserManager : ManagerBase<EvernoteUser>
    {
        public BusinessLayerResult<EvernoteUser> RegisterUser(RegisterViewModel data)

        {
            EvernoteUser user = Find(x => x.Username == data.Username || x.Email == data.EMail);
            BusinessLayerResult<EvernoteUser> layerResult = new BusinessLayerResult<EvernoteUser>();
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    layerResult.AddError(ErrorMessageCode.UsernameAlreadyExits, "Kullanıcı adı kayıtlı");
                }
                if (user.Email == data.EMail)
                {
                    layerResult.AddError(ErrorMessageCode.EmailAlreadyExits, "E-posta adı kayıtlı");

                }
            }
            else
            {
                int dbResult = base.Insert(new EvernoteUser()
                {
                    Username = data.Username,
                    Email = data.EMail,
                    ProfileImageFileName="user_boy.png",
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false

                });

                if (dbResult > 0) //kullanıcı insert olmuştur demektir.
                {
                    layerResult.Result = Find(x => x.Email == data.EMail && x.Username == data.Username);
                    //TODA: akviasyon maili'ı atılacak. 
                    //layerResult.Result.ActivateGuid
                    string siteUrl = ConfigHelper.Get<string>("SiteRootUrl");
                    string activateUrl = $"{siteUrl}/Home/UserActivate/{layerResult.Result.ActivateGuid}";
                    string body = $"Merhaba {layerResult.Result.Username } <br><br> Hesabınızı aktifleştirmek için <a href='{activateUrl}' target='_blank'> tıklayınız </a>";
                    MailHelper.SendMail(body, layerResult.Result.Email, "MyEverNote Hesap Aktifleştirme");


                }
            }
            return layerResult; //geriye layerresult dönceğimiz için tipi layerresult olmalı
        }

        public BusinessLayerResult<EvernoteUser> GetUserById(int id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = Find(x => x.Id == id);
            if (res.Result==null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı");
            }
            return res;
        }

        public BusinessLayerResult<EvernoteUser> LoginUser(LoginViewModel data)
        {
            //Giriş kontrolü
            //Hesap aktive edilmiş mi
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);



            if (res.Result != null)
            {
                if (!res.Result.IsActive) //kullanıcı aktif değilse
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen eposta adresinizi kontrol ediniz");
                }

            }
            else //kullanıcı veya şifre null ise
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı ya da şifre uyuşmuyor");
            }
            return res;
        }

        public BusinessLayerResult<EvernoteUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = Find(x => x.ActivateGuid == activateId);
            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir");
                    return res;
                }
                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExits, "Aktifleştirilecek kullanıcı bulunamadı");
            }
            return res;
        }

        public BusinessLayerResult<EvernoteUser> UpdataProfile(EvernoteUser data)
        {
            EvernoteUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();

            if (db_user !=null && db_user.Id!= data.Id) //update işlemini yapan kişi eşleşmiyorsa
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExits, "Kullanıcı adı kayıtlı");
                }
                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExits, "E-posta adresi kayıtlı");
                }
                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;

            if (string.IsNullOrEmpty(data.ProfileImageFileName)==false) //bir dosya adı gelmişsse
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }
            if (base.Update(res.Result)==0) //eğer update işlemi 0 geldiyse
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profile Güncellenemedi");
            }
            return res;
        }

        public BusinessLayerResult<EvernoteUser> RemoveUserById(int id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            EvernoteUser user = Find(x => x.Id == id);
            if (user!=null)
            {
                if (Delete(user)==0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı");
            }
            return res;
            
        }

        public new BusinessLayerResult<EvernoteUser> Insert(EvernoteUser data)
        {
            //method hiding.
            EvernoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> layerResult = new BusinessLayerResult<EvernoteUser>();
            layerResult.Result = data;
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    layerResult.AddError(ErrorMessageCode.UsernameAlreadyExits, "Kullanıcı adı kayıtlı");
                }
                if (user.Email == data.Email)
                {
                    layerResult.AddError(ErrorMessageCode.EmailAlreadyExits, "E-posta adı kayıtlı");

                }
            }
            else
            {
                layerResult.Result.ProfileImageFileName = "user_boy.png";
                layerResult.Result.ActivateGuid = Guid.NewGuid();

                if (base.Insert(layerResult.Result)==0) //eğer insert kısmından 0 gelirse yani eklenemediyse.
                {
                    layerResult.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı eklenemedi");
                }
             //geriye aktivasyon maili döndürmüyoruz.

            }
            return layerResult; //geriye layerresult dönceğimiz için tipi layerresult olmalı
        }

        public new BusinessLayerResult<EvernoteUser> Update(EvernoteUser data)
        {
            EvernoteUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = data;
            if (db_user != null && db_user.Id != data.Id) //update işlemini yapan kişi eşleşmiyorsa
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExits, "Kullanıcı adı kayıtlı");
                }
                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExits, "E-posta adresi kayıtlı");
                }
                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;
            res.Result.Username = data.Username;

 
            if (base.Update(res.Result) == 0) //eğer update işlemi 0 geldiyse
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı Güncellenemedi");
            }
            return res;
        }
    }
}
