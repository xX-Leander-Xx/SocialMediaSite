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
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaSite.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private DbSocialMediaSite _dbSocialMediaSite;

        public PostController(DbSocialMediaSite dbSocialMediaSite)
        {
            _dbSocialMediaSite = dbSocialMediaSite;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var LoggedInBenutzer = await _dbSocialMediaSite.Benutzer.FirstOrDefaultAsync(b => b.id_Benutzer == int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]));

            ViewBag.id_Benutzer = int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]);

            var postList = await _dbSocialMediaSite.Post
                                    .Include(k => k.kategorie)
                                        .ThenInclude(bk => bk.BenutzerKategorie)
                                        .ThenInclude(bkb => bkb.benutzer)
                                    .Include(b => b.benutzer)
                                        .ThenInclude(bb => bb.BenutzerBenutzerFolgen)
                                        .ThenInclude(bbb => bbb.benutzerFolgen)
                                    .Where(p => p.kategorie.BenutzerKategorie.benutzer == LoggedInBenutzer)
                                    .OrderByDescending(p => p.PostDate)
                                    .ToListAsync();
            /*.Include(b => b.benutzer)
                .ThenInclude(bb => bb.BenutzerBenutzerGefolgt)
                .ThenInclude(bbb => bbb.benutzerFolgen)
            .Where(p => p.kategorie.BenutzerKategorie.benutzer == LoggedInBenutzer || p.benutzer.BenutzerBenutzerFolgt.benutzerFolgen == LoggedInBenutzer)
            .ToListAsync();*/
            return View(postList);
        }

        //Searching DB so no async needed
        public IActionResult AddPost()
        {
            ViewBag.PageName = "Neuer Post";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([Bind("Titel, Inhalt, fk_id_Kategorie")] Post postData)
        {
            var post = new Post();

            if(postData.Titel == null || postData.Inhalt == null)
            {
                ViewBag.PostError = "Bitte füllen Sie alle Felder aus!";
                return View();
            }

            post.Titel = postData.Titel;
            post.Inhalt = postData.Inhalt;
            post.PostDate = DateTime.Now;
            post.fk_id_Kategorie = postData.fk_id_Kategorie;
            post.fk_id_Benutzer = int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]) ;

            _dbSocialMediaSite.Post.Add(post);

            await _dbSocialMediaSite.SaveChangesAsync();

            /* int id_post = _dbSocialMediaSite.Post.FirstOrDefault(b => b.PostDate == post.PostDate && b.Titel == postData.Titel).id_Post;

            Logs log = new Logs();
            log.LogInfo = "Benutzer ID: " + int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]) + " hat Post ID: " + id_post + " erstellt.";
            log.LogDate = DateTime.Now;
            _dbSocialMediaSite.Logs.Add(log);
            await _dbSocialMediaSite.SaveChangesAsync();*/

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeletePost(int? id_Post)
        {
            if (id_Post == null)
            {
                return NotFound();
            }

            var post = await _dbSocialMediaSite.Post.FirstOrDefaultAsync(b => b.id_Post == id_Post);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id_Post)
        {
            var post = await _dbSocialMediaSite.Post.FirstOrDefaultAsync(b => b.id_Post == id_Post);

            if (post == null)
            {
                return NotFound();
            }

            Logs log = new Logs();
            log.LogInfo = "Benutzer ID: " + int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]) + " hat Post ID: " + id_Post + " gelöscht.";
            log.LogDate = DateTime.Now;
            _dbSocialMediaSite.Logs.Add(log);
            await _dbSocialMediaSite.SaveChangesAsync();

            _dbSocialMediaSite.Post.Remove(post);
            await _dbSocialMediaSite.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> BenutzerPost(int? id_Benutzer)
        {
            if (id_Benutzer == null)
            {
                return NotFound();
            }

            ViewData["id_Benutzer"] = id_Benutzer;

            if (_dbSocialMediaSite.BenutzerBenutzer.FirstOrDefault(b => b.fk_id_BenutzerGefolgt == id_Benutzer && b.fk_id_BenutzerFolgen == int.Parse(HttpContext.Request.Cookies["id_LoggedIn"])) != null)
            {
                ViewBag.Follow = "Entfolgen";
            }
            else
            {
                ViewBag.Follow = "Folgen";
            }

            var postList = await _dbSocialMediaSite.Post.Include(post => post.benutzer).Include(post => post.kategorie).OrderByDescending(p => p.PostDate).ToListAsync();
            IEnumerable<Post> posts = postList.Where(b => b.fk_id_Benutzer == id_Benutzer);

            if (posts == null)
            {
                return NotFound();
            }
            
            return View(posts);
        }

        public async Task<IActionResult> KategoriePost(int? id_Kategorie)
        {
            if (id_Kategorie == null)
            {
                return NotFound();
            }

            ViewData["id_Kategorie"] = id_Kategorie;
            
            if (_dbSocialMediaSite.BenutzerKategorie.FirstOrDefault(b => b.fk_id_Kategorie == id_Kategorie && b.fk_id_Benutzer == int.Parse(HttpContext.Request.Cookies["id_LoggedIn"])) != null) {
                ViewBag.Follow = "Entfolgen";
            }
            else
            {
                ViewBag.Follow = "Folgen";
            }

            var postList = await _dbSocialMediaSite.Post.Include(post => post.benutzer).Include(post => post.kategorie).OrderByDescending(p => p.PostDate).ToListAsync();
            IEnumerable<Post> posts = postList.Where(b => b.fk_id_Kategorie == id_Kategorie);

            if (posts == null)
            {
                return NotFound();
            }

            return View(posts);
        }
    }
}

