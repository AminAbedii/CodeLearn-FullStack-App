using CodeLearn.DataLayer.Entities.Course;
using CodeLearn.DataLayer.Entities.Order;
using CodeLearn.DataLayer.Entities.Permissions;
using CodeLearn.DataLayer.Entities.User;
using CodeLearn.DataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLearn.DataLayer.Context
{
    public class CodeLearnContext : DbContext
    {
        public CodeLearnContext(DbContextOptions<CodeLearnContext> options) : base(options)  //tanzimate lazem baraye context ke az web dastur begirad
        {

        }

        #region User
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        #endregion

        #region Wallet
        public DbSet<WalletType> WalletTypes { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        #endregion

        #region Permission
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        #endregion

        #region Course
        public DbSet<CourseGroup> CourseGroups { get; set; }
        public DbSet<CourseLevel> CourseLevels { get; set; }
        public DbSet<CourseStatus> CourseStatuses { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseEpisode> CourseEpisodes { get; set; }
        #endregion

        #region Order

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)    //query filter baraye inke harjayi agar IsDelete true bood digar an neshandade nashavad
        {

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Role>()
                .HasQueryFilter(r => !r.IsDelete);
            modelBuilder.Entity<CourseGroup>()
                .HasQueryFilter(g => !g.IsDelete);
            modelBuilder.Entity<Course>()
                .HasQueryFilter(c => !c.IsDelete);

            base.OnModelCreating(modelBuilder);
        }

    }
}
