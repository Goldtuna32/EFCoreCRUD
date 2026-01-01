using DotNetTrainingEFCore.Database1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetTrainingEFCore.Mvc1.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _db;

        public BlogController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var blog = await _db.BlogTables
                        .AsNoTracking()
                        .OrderByDescending(x => x.BlogId)
                        .ToListAsync();
            return View(blog);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(BlogTable blog)
        {
            _db.BlogTables.Add(blog);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.BlogTables.FirstOrDefaultAsync(x=> x.BlogId == id);
            if (item is null)
            {
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, BlogTable blog)
        {
            var item = await _db.BlogTables.FirstOrDefaultAsync(x => x.BlogId == id);
            if (item is null)
            {
                return RedirectToAction("Index");
            }
            item.BlogTitle = blog.BlogTitle;
            item.BlogAuthor = blog.BlogAuthor;
            item.BlogContent = blog.BlogContent;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.BlogTables.FirstOrDefaultAsync(x => x.BlogId == id);
            if (item is null)
            {
                return RedirectToAction("Index");
            }
            _db.BlogTables.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
