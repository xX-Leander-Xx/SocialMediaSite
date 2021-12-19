using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class KategorieController : Controller
    {
        private DbSocialMediaSite _dbSocialMediaSite;

        public KategorieController(DbSocialMediaSite dbSocialMediaSite)
        {
            _dbSocialMediaSite = dbSocialMediaSite;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var kategorie = await _dbSocialMediaSite.Kategorie.ToListAsync();
            
            return View(kategorie);
        }

        public async Task<IActionResult> FollowCategory(int id_kategorie)
        {
            var benutzerKategorie = await _dbSocialMediaSite.BenutzerKategorie.FirstOrDefaultAsync(b => b.fk_id_Benutzer == int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]) && b.fk_id_Kategorie == id_kategorie);

            Logs log = new Logs();

            if (benutzerKategorie == null)
            {
                benutzerKategorie = new BenutzerKategorie();
                benutzerKategorie.fk_id_Kategorie = id_kategorie;
                benutzerKategorie.fk_id_Benutzer = int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]);
                _dbSocialMediaSite.BenutzerKategorie.Add(benutzerKategorie);
                log.LogInfo = "Benutzer ID: " + int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]) + " hat Kategorie ID: " + benutzerKategorie.fk_id_Kategorie + " gefolgt";
            } 
            else
            {
                _dbSocialMediaSite.BenutzerKategorie.Remove(benutzerKategorie);
                log.LogInfo = "Benutzer ID: " + int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]) + " hat Kategorie ID: " + benutzerKategorie.fk_id_Kategorie + " entfolgt";
            }

            log.LogDate = DateTime.Now;
            _dbSocialMediaSite.Logs.Add(log);
            await _dbSocialMediaSite.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}

