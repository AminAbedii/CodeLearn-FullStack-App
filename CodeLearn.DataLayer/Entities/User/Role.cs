﻿using CodeLearn.DataLayer.Entities.Permissions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLearn.DataLayer.Entities.User
{
    public class Role
    {
        public Role()  //constractor lazem ast
        {
            
        }

        [Key]
        public int RoleId { get; set; }
        [Display(Name="عنوان نقش")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        [MaxLength(200,ErrorMessage ="{0}نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string RoleTitle { get; set; }

        public bool IsDelete { get; set; }

        #region Relations
        public virtual List<UserRole> UserRoles { get; set; }
        public List<RolePermission> RolePermissions { get; set; }
        #endregion
    }
}
