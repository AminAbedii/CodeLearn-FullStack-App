﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLearn.DataLayer.Entities.User
{
    public class User
    {
        public User()
        {
            
        }

        [Key]
        public int UserId { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; }

        [Display(Name = "کد فعال سازی")]
        [MaxLength(50, ErrorMessage = "{0}نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string ActiveCode { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        [Display(Name = "آواتار")]

        public string UserAvatar { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتر از {1} کاراکتر باشد")]
        public DateTime RegisterDate { get; set; }

        public bool IsDelete { get; set; }




        #region Relations
        public virtual List<UserRole> UserRoles { get; set; }

        public virtual List<Wallet.Wallet> Wallets { get; set; }     //list gozashtim chon yek USer mitavanad chandino chand tarakonesh dashte bashad

        public virtual List<Course.Course> Courses { get; set; }

        public virtual List<Order.Order> Orders { get; set; }

        #endregion
    }
}
