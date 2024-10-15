using CodeLearn.DataLayer.Entities.Permissions;
using CodeLearn.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLearn.Core.Services.Interfaces
{
    public interface IPermissionService
    {
        #region Roles
        List<Role> GetRoles();
        int AddRole(Role role);
        Role GetRoleById(int roleId);
        void UpdateRole(Role role);
        void DeleteRole(Role role);
        void AddRolesToUser(List<int> roleIds,int userId);     //ezafe kardane Role be user sabtnam shavande tavasote admin
        void EditRolesUser(int userId,List<int> rolesId);

        #endregion

        #region Permissions
        List<Permission> GetAllPermission();
        void AddPermissionToRole(int roleId, List<int> Permissions);
        List<int> ActivePermissionsRole(int roleId);
        void UpdatePermissionsRole(int roleId,List<int> Permissions);

        bool CheckPermission(int permissionId, string userName);
        #endregion
    }
}
