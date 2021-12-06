using Microsoft.EntityFrameworkCore;
using SocialMediaSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaSite.Helpers
{
    public class DbSocialMediaSite: DbContext
    {
        public DbSocialMediaSite(DbContextOptions<DbSocialMediaSite> options) : base(options)
        {

        }

        public DbSet<Benutzer> Benutzer { get; set; }
    }
}
