﻿using Microsoft.AspNetCore.Http;
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
            if (Request.Cookies["user_id"] == null)
            {
                return RedirectToAction(nameof(LogIn));
            }
            var benutzer = await _dbSocialMediaSite.Benutzer.ToListAsync();
            return View(benutzer);
        }
        //Loads when Register pressed
        public async Task<IActionResult> AddUser(int? id_Benutzer)
        {
            ViewBag.PageName = id_Benutzer == null ? "Hinzufügen Besitzer" : "Bearbeiten Besitzer";
            ViewBag.IsEdit = id_Benutzer == null ? false : true;

            if (id_Benutzer == null)
            {
                return View();
            }
            else
            {
                var benutzer = await _dbSocialMediaSite.Benutzer.FirstOrDefaultAsync(b => b.id_Benutzer == id_Benutzer);

                if (benutzer == null)
                {
                    return NotFound();
                }

                return View(benutzer);
            }
        }

        //Runs after Signing up
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

            if (benutzerExist)
                _dbSocialMediaSite.Benutzer.Update(benutzer);
            else
                _dbSocialMediaSite.Benutzer.Add(benutzer);

            await _dbSocialMediaSite.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

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

        public IActionResult LogIn()
        {
            if (Request.Cookies["user_id"] != null)
            {
                Response.Cookies.Delete("user_id");
            }
            return View();
        }

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
                CookieOptions cookies = new CookieOptions();
                cookies.Expires = DateTime.Now.AddDays(2);
                Response.Cookies.Append("user_id", benutzer.id_Benutzer.ToString(), cookies);
                Console.WriteLine(Request.Cookies["user_id"]);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }
    }
}
