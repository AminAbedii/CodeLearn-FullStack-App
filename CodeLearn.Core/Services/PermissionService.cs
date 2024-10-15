using CodeLearn.Core.Services.Interfaces;
using CodeLearn.DataLayer.Context;
using CodeLearn.DataLayer.Entities.Permissions;
using CodeLearn.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLearn.Core.Services
{
    public class PermissionService :IPermissionService
    {
        private CodeLearnContext _context;
        public PermissionService(CodeLearnContext context)
        {
            _context = context;
        }

        public List<int> ActivePermissionsRole(int roleId)
        {
            return _context.RolePermission
                .Where(r => r.RoleId == roleId)
                .Select(r => r.PermissionId).ToList();
        }

        public void AddPermissionToRole(int roleId, List<int> Permissions)
        {
            foreach (var p in Permissions) 
            {
                _context.RolePermission.Add(new RolePermission()
                {
                    PermissionId = p,
                    RoleId = roleId
                });
            }
            _context.SaveChanges();
        }

        public int AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role.RoleId;
        }

        public void AddRolesToUser(List<int> roleIds, int userId)    //ezafe kardane Role be user sabtnam shavande tavasote admin
        {
            foreach (int roleId in roleIds) 
            {
                _context.UserRoles.Add(new UserRole()
                {
                    RoleId = roleId,
                    UserId = userId
                });
            }
            _context.SaveChanges();
        }

        public bool CheckPermission(int permissionId, string userName)
        {
            int userId=_context.Users.Single(u=>u.UserName == userName).UserId;

            List<int> UserRoles = _context.UserRoles
                .Where(r => r.UserId == userId).Select(r => r.RoleId).ToList();     //bedast avordane role haye user

            if(!UserRoles.Any())  //agar kaarbar nagshi nadasht
                return false;

            List<int> RolesPermission = _context.RolePermission         //bedast avordane dastresihaye nagsh morede nazar
                .Where(p => p.PermissionId == permissionId)
                .Select(p => p.RoleId).ToList();

            return RolesPermission.Any(p => UserRoles.Contains(p));
        }

        public void DeleteRole(Role role)
        {
            role.IsDelete = true;
            UpdateRole(role);
        }

        public void EditRolesUser(int userId, List<int> rolesId)
        {
            // Delete All Roles User
            _context.UserRoles.Where(r => r.UserId == userId).ToList().ForEach(r => _context.UserRoles.Remove(r));

            //Add New Roles
            AddRolesToUser(rolesId, userId);
        }

        public List<Permission> GetAllPermission()
        {
            return _context.Permission.ToList();
        }

        public Role GetRoleById(int roleId)
        {
            return _context.Roles.Find(roleId);
        }

        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public void UpdatePermissionsRole(int roleId, List<int> Permissions)
        {
            _context.RolePermission.Where(p => p.RoleId == roleId).ToList()
                .ForEach(p => _context.RolePermission.Remove(p));

            AddPermissionToRole(roleId, Permissions);

        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }
    }
}
