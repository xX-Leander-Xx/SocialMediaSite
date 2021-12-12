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
            var post = await _dbSocialMediaSite.Post.Include(post => post.benutzer).ToListAsync();
            return View(post);
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

            post.Titel = postData.Titel;
            post.Inhalt = postData.Inhalt;
            post.PostDate = DateTime.Now;
            post.fk_id_Kategorie = postData.fk_id_Kategorie;
            post.fk_id_Benutzer = int.Parse(HttpContext.Request.Cookies["id_LoggedIn"]) ;

            _dbSocialMediaSite.Post.Add(post);

            await _dbSocialMediaSite.SaveChangesAsync();

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

            var postList = await _dbSocialMediaSite.Post.ToListAsync();
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
            /*
            if (_dbSocialMediaSite.BenutzerKategorie.FirstOrDefaultAsync(b => b.fk_id_Kategorie == id_Kategorie && b.fk_id_Benutzer == int.Parse(HttpContext.Request.Cookies["id_LoggedIn"])) != null) {
                ViewBag.Follow = "Entfolgen";
            }
            else
            {
                ViewBag.Follow = "Folgen";
            }*/

            ViewBag.Follow = "Folgen"; //PlaceHolder

            var postList = await _dbSocialMediaSite.Post.ToListAsync();
            IEnumerable<Post> posts = postList.Where(b => b.fk_id_Kategorie == id_Kategorie);

            if (posts == null)
            {
                return NotFound();
            }

            return View(posts);
        }
    }
}

