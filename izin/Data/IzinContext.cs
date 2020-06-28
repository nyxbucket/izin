using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace izin.Data
{
    public class IzinContext : DbContext
    {
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Rol> Roller { get; set; }
        public DbSet<Departman> Departmanlar { get; set; }
        public DbSet<Izin> Izinler { get; set; }
        public DbSet<Durum> Durumlar { get; set; }
        public DbSet<IzinTip> IzinTipler { get; set; }


        public IzinContext() : base("IzinContextConStr")
        {

        }

        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kullanici>()
               .HasMany(m => m.Roller)
               .WithMany(m2 => m2.Kullanicilar)
               .Map(f =>
               {
                   f.ToTable("KullaniciRol");
                   f.MapLeftKey("KullaniciId");
                   f.MapRightKey("RolId");
               }
               );

            modelBuilder.Entity<Departman>()
                .HasRequired(m => m.YoneticiKullanici)
                .WithMany(m2 => m2.Departmanlar)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);

            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<Kullanici>()
            //    .HasMany(m => m.Roller)
            //    .WithMany(m2 => m2.Kullanicilar)
            //    .Map(f =>
            //    {
            //        f.ToTable("KullaniciRol");
            //        f.MapLeftKey("KullaniciId");
            //        f.MapRightKey("RolId");
            //    }
            //    );
        }
    }
}