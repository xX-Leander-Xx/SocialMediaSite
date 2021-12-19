using Microsoft.EntityFrameworkCore;
using SocialMediaSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaSite.Helpers
{
    public class DbSocialMediaSite : DbContext
    {
        public DbSocialMediaSite(DbContextOptions<DbSocialMediaSite> options) : base(options)
        {

        }

        public DbSet<Benutzer> Benutzer { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Kategorie> Kategorie { get; set; }
        public DbSet<BenutzerKategorie> BenutzerKategorie { get; set; }
        public DbSet<BenutzerBenutzer> BenutzerBenutzer { get; set; }
        public DbSet<Logs> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BenutzerBenutzer>()
              .HasOne(x => x.benutzerFolgen)
              .WithMany(x => x.BenutzerBenutzerGefolgt)
              .HasForeignKey(x => x.fk_id_BenutzerFolgen);

            modelBuilder.Entity<BenutzerBenutzer>()
              .HasOne(x => x.benutzerGefolgt)
              .WithMany(x => x.BenutzerBenutzerFolgen)
              .HasForeignKey(x => x.fk_id_BenutzerGefolgt);
        }
    }
}
