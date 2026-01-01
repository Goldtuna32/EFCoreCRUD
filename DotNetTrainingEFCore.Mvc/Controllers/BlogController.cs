using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DotNetTrainingEFCore.Mvc.Controllers
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
            var blogs = await _db.BlogTables
                        .AsNoTracking()
                        .OrderByDescending(x=> x.BlogId)
                        .ToListAsync();

            return View(blogs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(BlogTable blogTable)
        {
            if (blogTable == null) 
            {
                return View("Blog table can't be null");
            }
            await _db.BlogTables.AddAsync(blogTable);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _db.BlogTables
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.BlogId == id);
            if (blog == null)
            {
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,BlogTable blog)
        {
            var item = await _db.BlogTables
                        .FirstOrDefaultAsync(x=> x.BlogId ==id); 
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
            var item = await _db.BlogTables
                            .FirstOrDefaultAsync(x=> x.BlogId ==id);
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
