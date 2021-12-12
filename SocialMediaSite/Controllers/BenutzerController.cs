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
            Console.WriteLine(User.IsInRole("Admin"));
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
        public async Task<IActionResult> AddUser(int id_Benutzer, [Bind("Benutzername, Passwort")] Benutzer benutzerData)
        {
            var benutzer = await _dbSocialMediaSite.Benutzer.FirstOrDefaultAsync(b => b.id_Benutzer == id_Benutzer);

            bool benutzerExist = false;

            if (benutzer == null)
                benutzer = new Benutzer();
            else
                benutzerExist = true;

            benutzer.Benutzername = benutzerData.Benutzername;
            benutzer.Passwort = benutzerData.Passwort;
            benutzer.isAdmin = "User";

            if (benutzerExist)
                _dbSocialMediaSite.Benutzer.Update(benutzer);
            else
                _dbSocialMediaSite.Benutzer.Add(benutzer);

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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id_benutzer)
        {
            var benutzer = await _dbSocialMediaSite.Benutzer.FirstOrDefaultAsync(b => b.id_Benutzer == id_benutzer);

            if (benutzer == null)
            {
                return NotFound();
            }

            _dbSocialMediaSite.Benutzer.Remove(benutzer);
            await _dbSocialMediaSite.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

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

            if (benutzer.Passwort.Trim().Equals(logInData.Passwort))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, benutzer.Benutzername.Trim()),
                    new Claim(ClaimTypes.Role, benutzer.isAdmin.Trim())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                HttpContext.Response.Cookies.Append("id_LoggedIn", benutzer.id_Benutzer.ToString());

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.LogInError = "Benutzername oder Passwort falsch";
                return View();
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(LogIn));
        }

        public async Task<IActionResult> FollowUser(int id_Benutzer)
        {
            var benutzerBenutzer = await _dbSocialMediaSite.BenutzerBenutzer.FirstOrDefaultAsync(b => b.fk_id_BenutzerFolgen == int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]) && b.fk_id_BenutzerGefolgt == id_Benutzer);
            


            if (benutzerBenutzer == null)
            {
                benutzerBenutzer = new BenutzerBenutzer();
                benutzerBenutzer.fk_id_BenutzerGefolgt = id_Benutzer;
                benutzerBenutzer.fk_id_BenutzerFolgen = int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]);
                _dbSocialMediaSite.BenutzerBenutzer.Add(benutzerBenutzer);
                await _dbSocialMediaSite.SaveChangesAsync();
            }
            else
            {
                _dbSocialMediaSite.BenutzerBenutzer.Remove(benutzerBenutzer);
                await _dbSocialMediaSite.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
