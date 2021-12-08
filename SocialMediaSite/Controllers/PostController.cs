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
    public class PostController : Controller
    {
        private DbSocialMediaSite _dbSocialMediaSite;

        public PostController(DbSocialMediaSite dbSocialMediaSite)
        {
            _dbSocialMediaSite = dbSocialMediaSite;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var post = await _dbSocialMediaSite.Post.ToListAsync();
            return View(post);
        }

        //Searching DB so no async needed
        public IActionResult AddPost()
        {
            ViewBag.PageName = "Neuer Post";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([Bind("Titel, Inhalt, fk_Kategorie")] Post postData)
        {
            var post = new Post();

            post.Titel = postData.Titel;
            post.Inhalt = postData.Inhalt;
            post.fk_Kategorie = postData.fk_Kategorie;

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
    }
}

