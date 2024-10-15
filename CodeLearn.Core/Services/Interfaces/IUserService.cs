using CodeLearn.Core.DTOs;
using CodeLearn.DataLayer.Entities.User;
using CodeLearn.DataLayer.Entities.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLearn.Core.Services.Interfaces
{
    public interface IUserService
    {
        bool IsExistUserName(string userName);   //baraye check kardane vujud ya adame vujude etelaat sabt shode dar database 
        bool IsExistUserEmail(string email);

        int AddUser(User user);       //gereftane yek user va insert an

        User LoginUser(LoginViewModel login);    //az LoginViewModel migirad va check mikonak ke kaarbar vujud darad ya xeyr

        User GetUserByEmail(string email);     //baraye baazyabiye kalameye ubur
        User GetUserByUserId(int userId);
        User GetUserByUserName(string userName);
        User GetUserByActiveCode(string activecode);  //in 2 baraye taaghir kalameye ubur ast

        int GetUserIdByUserName(string userName);
        void UpdateUser(User user);

        bool ActiveAccount(string activeCode);   //baraye active kardane kaarbar activecode ra migirad

        void DeleteUser(int userId);

        #region UserPanel
        InformationUserViewModel GetUserInformation(string username);
        InformationUserViewModel GetUserInformation(int userId);
        SideBarUSerPanelViewModel GetSidebarUSerPanelData(string username);
        EditProfileModelView GetDataForEditProfileUser(string username);
        void EditProfile(string username, EditProfileModelView profile);

        bool CompareOldPassword(string oldPassword, string username);   //chek kardane vurude sahihe passworde gabli jahate raghire password
        void ChangeUserPassword(string username, string newPassword);
        #endregion

        #region Wallet
        int BalanceUserWallet(string username);
        List<WalletViewModel> GetWalletsUser(string username);
        int ChargeWallet(string username,int amount,string description,bool isPay=false);
        int AddWallet(Wallet wallet);
        Wallet GetWalletByWalletId(int walletId);
        void UpdateWallet(Wallet wallet);
        #endregion

        #region AdminPanel
        UserForAdminViewModel GetUsers(int pageId = 1, string filterEmail = "", string filterUserName="");
        UserForAdminViewModel GetDeleteUsers(int pageId = 1, string filterEmail = "", string filterUserName = "");

        int AddUserFromAdmin(CreateUserViewModel user);
        EditUserViewModel GetUserForShowInEditMode(int userId);
        void EditUserFromAdmin(EditUserViewModel editUser);
        #endregion
    }
}
