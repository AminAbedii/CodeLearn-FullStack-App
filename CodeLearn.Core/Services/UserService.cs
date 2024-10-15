using CodeLearn.Core.Convertors;
using CodeLearn.Core.DTOs;
using CodeLearn.Core.Generator;
using CodeLearn.Core.Security;
using CodeLearn.Core.Services.Interfaces;
using CodeLearn.DataLayer.Context;
using CodeLearn.DataLayer.Entities.User;
using CodeLearn.DataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodeLearn.Core.Services
{
    public class UserService : IUserService
    {
        private CodeLearnContext _context;

        public UserService(CodeLearnContext context)      //tazrige vaabastegi
        {
            _context = context;
        }

        public bool IsExistUserName(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }
        public bool IsExistUserEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

   
        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public User LoginUser(LoginViewModel login)
        {
            string hashPassword = PasswordHelper.EncodePasswordMd5(login.Password);  //hash kardane password
            string email = FixedText.FixEmail(login.Email);                          //fixed kardane formate email
            return _context.Users.SingleOrDefault(u => u.Email == email && u.Password == hashPassword);  //check kardane vujud kaarbar ya xeyr
        }

        public bool ActiveAccount(string activeCode)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
            if(user == null || user.IsActive) 
            {
                return false;
            }
            user.IsActive = true;
            user.ActiveCode = NameGenerator.GeneratorUniqCode();  //avazkardane active code baraye amniyat
            _context.SaveChanges();
            return true;

        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public User GetUserByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName);
        }

        public User GetUserByActiveCode(string activecode)
        {
            return _context.Users.SingleOrDefault(u => u.ActiveCode == activecode);
        }

        public User GetUserByUserId(int userId)
        {
            return _context.Users.Find(userId);
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public InformationUserViewModel GetUserInformation(string username)
        {
            var user = GetUserByUserName(username);
            InformationUserViewModel information = new InformationUserViewModel();
            information.UserName = user.UserName;
            information.RegisterDate = user.RegisterDate;
            information.Email = user.Email;
            information.Wallet = BalanceUserWallet(username);

            return information;
        }
        public InformationUserViewModel GetUserInformation(int userId)
        {
            var user = GetUserByUserId(userId);
            InformationUserViewModel information = new InformationUserViewModel();
            information.UserName = user.UserName;
            information.RegisterDate = user.RegisterDate;
            information.Email = user.Email;
            information.Wallet = BalanceUserWallet(user.UserName);

            return information;
        }

        public SideBarUSerPanelViewModel GetSidebarUSerPanelData(string username)
        {
            return _context.Users.Where(u => u.UserName == username).Select(u => new SideBarUSerPanelViewModel()
            {
                UserName = u.UserName,
                RegisterDate=u.RegisterDate,
                ImageName=u.UserAvatar
            }).Single();
        }

        public EditProfileModelView GetDataForEditProfileUser(string username)
        {
            return _context.Users.Where(u => u.UserName == username).Select(u => new EditProfileModelView()
            {
                AvatarName = u.UserAvatar,
                Email=u.Email,
                UserName = u.UserName,
            }).Single();
        }

        public void EditProfile(string username, EditProfileModelView profile)
        {
            #region Updating AvatarImage

            if (profile.UserAvatar != null) 
            {
                string imagePath = "";
                if (profile.AvatarName != "Default.jpg")
                {
                    imagePath=Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/USerAvatar",profile.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath );
                    }
                }
                profile.AvatarName = NameGenerator.GeneratorUniqCode() + Path.GetExtension(profile.UserAvatar.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/USerAvatar", profile.AvatarName);
                using (var stream = new FileStream(imagePath,FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }
            }
            #endregion

            var user =GetUserByUserName(username);
            user.UserName = profile.UserName;
            user.Email = profile.Email;
            user.UserAvatar = profile.AvatarName;

            UpdateUser(user);
        }

        public bool CompareOldPassword(string oldPassword, string username)
        {
            string hashOldPassword = PasswordHelper.EncodePasswordMd5(oldPassword);
            return _context.Users.Any(u=>u.UserName == username && u.Password==hashOldPassword);
        }

        public void ChangeUserPassword(string username, string newPassword)
        {
            var user = GetUserByUserName(username);
            user.Password = PasswordHelper.EncodePasswordMd5(newPassword);
            UpdateUser(user);
        }

        public int BalanceUserWallet(string username)
        {
            int userId=GetUserIdByUserName(username);
            var enter = _context.Wallets.Where(w=>w.UserId==userId && w.TypeId==1 && w.IsPay)
                .Select(w=>w.Amount).ToList();
            var exit = _context.Wallets.Where(w => w.UserId == userId && w.TypeId == 2)
                .Select(w => w.Amount).ToList();

            return(enter.Sum() - exit.Sum());
        }

        public int GetUserIdByUserName(string userName)
        {
            return _context.Users.Single(u => u.UserName == userName).UserId;
        }

        public List<WalletViewModel> GetWalletsUser(string username)
        {
            var userId=GetUserIdByUserName(username);
            return _context.Wallets.Where(w=>w.IsPay && w.UserId==userId)
                .Select(w=> new WalletViewModel
                {
                    Amount= w.Amount,
                    dateTime=w.CreateDate,
                    Description=w.Description,
                    Type=w.TypeId
                }).ToList();
        }

        public int ChargeWallet(string username, int amount,string description, bool isPay = false)
        {
            Wallet wallet=new Wallet()
            {
                Amount= amount,
                Description= description,
                IsPay= isPay,
                CreateDate= DateTime.Now,
                TypeId= 1,
                UserId=GetUserIdByUserName(username)
            };
            return AddWallet(wallet);
        }

        public int AddWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            _context.SaveChanges();
            return wallet.WalletId;
        }

        public Wallet GetWalletByWalletId(int walletId)
        {
            return _context.Wallets.Find(walletId);
        }

        public void UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            _context.SaveChanges();
        }


        public UserForAdminViewModel GetUsers(int pageId = 1, string filterEmail = "", string filterUserName = "")  //baraye pageing va emake filter ha
        {
            IQueryable<User> result = _context.Users;

            if(!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }
            if (!string.IsNullOrEmpty(filterUserName))
            {
                result=result.Where(u=>u.UserName.Contains(filterUserName));
            }

            //show Item in Page
            int take = 10;
            int skip = (pageId - 1) * take;

            UserForAdminViewModel list = new UserForAdminViewModel();
            list.CorrentPage = pageId;
            list.PageCounte = result.Count() / take;
            list.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();

            return list;
        }

        public int AddUserFromAdmin(CreateUserViewModel user)
        {
            User addUser=new User();
            addUser.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            addUser.ActiveCode = NameGenerator.GeneratorUniqCode();
            addUser.Email = user.Email;
            addUser.UserName = user.UserName;
            addUser.IsActive = true;
            addUser.RegisterDate = DateTime.Now;

            #region Uploading AvatarImage

            if (user.UserAvatar != null)
            {
                string imagePath = "";
                addUser.UserAvatar = NameGenerator.GeneratorUniqCode() + Path.GetExtension(user.UserAvatar.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/USerAvatar", addUser.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    user.UserAvatar.CopyTo(stream);
                }
            }
            #endregion

            return AddUser(addUser);
        }

        public EditUserViewModel GetUserForShowInEditMode(int userId)
        {
            return _context.Users.Where(u => u.UserId == userId)
                .Select(u => new EditUserViewModel()
                {
                    UserId = u.UserId,
                    AvatarName = u.UserAvatar,
                    Email = u.Email,
                    UserName = u.UserName,
                    UserRoles =u.UserRoles.Select(r=>r.RoleId).ToList()
                }).Single();
        }

        public void EditUserFromAdmin(EditUserViewModel editUser)
        {
            User user = GetUserByUserId(editUser.UserId);
            user.Email=editUser.Email;
            if(!string.IsNullOrEmpty(editUser.Password)) 
            {
                user.Password = PasswordHelper.EncodePasswordMd5(editUser.Password);
            }
            if (editUser.UserAvatar != null)
            {
                //delete old image
                if (editUser.AvatarName != "Default.jpg")
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editUser.AvatarName);
                    if (File.Exists(deletePath))
                    {
                        File.Delete(deletePath);
                    }
                }

                #region Uploading new AvatarImage
                user.UserAvatar = NameGenerator.GeneratorUniqCode() + Path.GetExtension(editUser.UserAvatar.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", user.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editUser.UserAvatar.CopyTo(stream);
                }
                #endregion
            }
            _context.Update(user);
            _context.SaveChanges();
        }

        public UserForAdminViewModel GetDeleteUsers(int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(u=>u.IsDelete);

            if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }
            if (!string.IsNullOrEmpty(filterUserName))
            {
                result = result.Where(u => u.UserName.Contains(filterUserName));
            }

            //show Item in Page
            int take = 10;
            int skip = (pageId - 1) * take;

            UserForAdminViewModel list = new UserForAdminViewModel();
            list.CorrentPage = pageId;
            list.PageCounte = result.Count() / take;
            list.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();

            return list;
        }

        public void DeleteUser(int userId)
        {
            User user = GetUserByUserId(userId);
            user.IsDelete = true;
            UpdateUser(user);
        }

        
    }
}
