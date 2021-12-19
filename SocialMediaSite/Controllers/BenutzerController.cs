using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaSite.Helpers;
using SocialMediaSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace SocialMediaSite.Controllers
{
    [Authorize]
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

        //Loads when Register pressed
        [AllowAnonymous]
        public IActionResult AddUser()
        {
            return View();
        }

        //Runs after Signing up
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddUser([Bind("Benutzername, Passwort")] Benutzer benutzerData)
        {
            Benutzer benutzer = new Benutzer();

            if (benutzerData.Benutzername == null || benutzerData.Passwort == null)
            {
                ViewBag.CreateError = "Bitte füllen Sie alle Felder aus";
                return View();
            }

            if (benutzerData.Benutzername.Length > 20 || benutzerData.Passwort.Length > 20)
            {
                ViewBag.CreateError = "Benutzername und Passwort dürfen nur 20 Charakter lang sein";
                return View();
            }

            if(_dbSocialMediaSite.Benutzer.FirstOrDefault(b => b.Benutzername == benutzerData.Benutzername) != null)
            {
                ViewBag.CreateError = "Benutzername existiert schon";
                return View();
            }
            

            benutzer.Benutzername = benutzerData.Benutzername;
            benutzer.Passwort = Crypto.HashPassword(benutzerData.Passwort); ;
            benutzer.isAdmin = "User";

            _dbSocialMediaSite.Benutzer.Add(benutzer);

            await _dbSocialMediaSite.SaveChangesAsync();

            int id_benutzer = _dbSocialMediaSite.Benutzer.FirstOrDefault(b => b.Benutzername == benutzerData.Benutzername).id_Benutzer;

            Logs log = new Logs();
            log.LogInfo = "Benutzer ID: " + id_benutzer + " wurde erstellt.";
            log.LogDate = DateTime.Now;
            _dbSocialMediaSite.Logs.Add(log);

            await _dbSocialMediaSite.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int? id_benutzer)
        {
            if (id_benutzer == null)
            {
                return NotFound();
            }

            var benutzer = await _dbSocialMediaSite.Benutzer.FirstOrDefaultAsync(b => b.id_Benutzer == id_benutzer);

            if (benutzer == null)
            {
                return NotFound();
            }

            return View(benutzer);
        }

        /*
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id_benutzer)
        {
            var benutzer = await _dbSocialMediaSite.Benutzer.FirstOrDefaultAsync(b => b.id_Benutzer == id_benutzer);

            if (benutzer == null)
            {
                return NotFound();
            }

            Logs log = new Logs();
            log.LogInfo = "Benutzer ID: " + int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]) + " hat Benutzer ID: " + id_benutzer + " gelöscht";
            log.LogDate = DateTime.Now;
            _dbSocialMediaSite.Logs.Add(log);

            _dbSocialMediaSite.Benutzer.Remove(benutzer);
            await _dbSocialMediaSite.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }*/

        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogIn([Bind("Benutzername, Passwort")] Benutzer logInData)
        {
            var benutzer = await _dbSocialMediaSite.Benutzer.FirstOrDefaultAsync(b => b.Benutzername == logInData.Benutzername);

            if (benutzer == null)
            {
                return NotFound();
            }

            if (Crypto.VerifyHashedPassword(benutzer.Passwort.Trim(), logInData.Passwort))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, benutzer.Benutzername.Trim()),
                    new Claim(ClaimTypes.Role, benutzer.isAdmin.Trim())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                Logs log = new Logs();
                log.LogInfo = "Benutzer ID: " + int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]) + " hat sich eingeloggt.";
                log.LogDate = DateTime.Now;
                _dbSocialMediaSite.Logs.Add(log);
                await _dbSocialMediaSite.SaveChangesAsync();

                await _dbSocialMediaSite.SaveChangesAsync();

                HttpContext.Response.Cookies.Append("id_LoggedIn", benutzer.id_Benutzer.ToString());

                return RedirectToAction(nameof(Index), nameof(Post));
            }
            else
            {
                ViewBag.LogInError = "Benutzername oder Passwort falsch";
                return View();
            }
        }

        public async Task<IActionResult> LogOut()
        {
            Logs log = new Logs();
            log.LogInfo = "Benutzer ID: " + int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]) + " hat sich ausgeloggt.";
            log.LogDate = DateTime.Now;
            _dbSocialMediaSite.Logs.Add(log);
            await _dbSocialMediaSite.SaveChangesAsync();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(LogIn));
        }

        public async Task<IActionResult> FollowUser(int id_Benutzer)
        {
            var benutzerBenutzer = await _dbSocialMediaSite.BenutzerBenutzer.FirstOrDefaultAsync(b => b.fk_id_BenutzerFolgen == int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]) && b.fk_id_BenutzerGefolgt == id_Benutzer);

            Logs log = new Logs();
            
            

            if (benutzerBenutzer == null)
            {
                benutzerBenutzer = new BenutzerBenutzer();
                benutzerBenutzer.fk_id_BenutzerFolgen = int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]);
                benutzerBenutzer.fk_id_BenutzerGefolgt = id_Benutzer;
                _dbSocialMediaSite.BenutzerBenutzer.Add(benutzerBenutzer);

                log.LogInfo = "Benutzer ID: " + int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]) + " hat Benutzer ID: " + benutzerBenutzer.fk_id_BenutzerGefolgt + " gefolgt";
            }
            else
            {
                _dbSocialMediaSite.BenutzerBenutzer.Remove(benutzerBenutzer);

                log.LogInfo = "Benutzer ID: " + int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]) + " hat Benutzer ID: " + benutzerBenutzer.fk_id_BenutzerGefolgt + " entfolgt";

            }

            log.LogDate = DateTime.Now;
            _dbSocialMediaSite.Logs.Add(log);

            await _dbSocialMediaSite.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ChangeUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser([Bind("Passwort")] Benutzer benutzerData)
        {
            string altesPasswort = Request.Form["oldPassword"];

            var benutzer = await _dbSocialMediaSite.Benutzer.FirstOrDefaultAsync(b => b.id_Benutzer == int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]));

            if(benutzer == null)
            {
                return NotFound();
            }
            
            if (Crypto.VerifyHashedPassword(benutzer.Passwort.Trim(), altesPasswort))
            {
                benutzer.Passwort = Crypto.HashPassword(benutzerData.Passwort);
                _dbSocialMediaSite.Benutzer.Update(benutzer);

                Logs log = new Logs();
                log.LogInfo = "Benutzer ID: " + int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]) + " hat das Passwort geändert.";
                log.LogDate = DateTime.Now;
                _dbSocialMediaSite.Logs.Add(log);

                await _dbSocialMediaSite.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            ViewBag.Error = "Falsches Passwort";
            return View();
        }
    }
}
