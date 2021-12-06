using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialMediaSite.Helpers;
using SocialMediaSite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaSite.Controllers
{
    public class BenutzerController : Controller
    {
        private DbSocialMediaSite _dbSocialMediaSite;
        
        public BenutzerController(DbSocialMediaSite dbSocialMediaSite)
        {
            _dbSocialMediaSite = dbSocialMediaSite;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var benutzer = await _dbSocialMediaSite.Benutzer.ToListAsync();

            return View(benutzer);
        }
    }
}
