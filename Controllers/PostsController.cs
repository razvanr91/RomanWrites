using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RomanWrites.Data;
using RomanWrites.Models;
using RomanWrites.Services;
using RomanWrites.Enums;
using X.PagedList;

namespace RomanWrites.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISlugService _slugService;
        private readonly IImageService _imageService;
        private readonly UserManager<BlogUser> _userManager;
        private readonly BlogSearchService _blogSearchService;

        public PostsController(ApplicationDbContext context, ISlugService slugService, IImageService imageService, UserManager<BlogUser> userManager, BlogSearchService blogSearchService)
        {
            _context = context;
            _slugService = slugService;
            _imageService = imageService;
            _userManager = userManager;
            _blogSearchService = blogSearchService;
        }

        // Search Action
        public async Task<IActionResult> SearchIndex(int? page, string searchTerm)
        {
            ViewData["SearchTerm"] = searchTerm;

            var pageNumber = page ?? 1;
            var pageSize = 5;

            var posts = _blogSearchService.Search(searchTerm);

            return View(await posts.ToPagedListAsync(pageNumber, pageSize));

        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Author).Include(p => p.Blog);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BlogPosts

        public async Task<IActionResult> BlogPost(int? id, int? page)
        {
            if ( id is null )
            {
                return NotFound();
            }

            var pageNumber = page ?? 1;
            var pageSize = 6;

            var posts = await _context.Posts.Where(p => p.BlogId == id && p.ProductionStatus == ProductionStatus.ProductionReady)
                .OrderByDescending(p => p.Created)
                .ToPagedListAsync(pageNumber, pageSize);

            return View(posts);

        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(string slug)
        {
            if ( string.IsNullOrEmpty(slug) )
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Blog)
                .Include(p => p.Tags)
                .Include(p => p.Comments)
                //.ThenInclude(c => c.Author) if the comment author is not displayed or we get an Null Exception for the author
                .FirstOrDefaultAsync(p => p.Slug == slug);

            if ( post == null )
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Description");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Abstract,Content,Image,ProductionStatus")] Post post, List<string> tagValues)
        {
            if ( ModelState.IsValid )
            {
                post.Created = DateTime.Now;

                var authorId = _userManager.GetUserId(User);
                post.AuthorId = authorId;

                post.ImageData = await _imageService.EncodeImageAsync(post.Image);
                post.ContentType = _imageService.ContentType(post.Image);

                var slug = _slugService.UrlFriendly(post.Title);

                var badSlug = false;

                if ( !_slugService.IsUnique(slug) )
                {
                    badSlug = true;
                    ModelState.AddModelError("Title", "The title you provided can not be used as it already exists.");
                }

                if ( string.IsNullOrEmpty(slug) )
                {
                    badSlug = true;
                    ModelState.AddModelError("Title", "The title field is empty. Please add a title.");
                }

                if ( badSlug )
                {
                    ViewData["TagValues"] = string.Join(",", tagValues);
                    return View(post);
                }

                post.Slug = slug;

                _context.Add(post);
                await _context.SaveChangesAsync();

                foreach ( var tag in tagValues )
                {
                    _context.Add(new Tag()
                    {
                        PostId = post.Id,
                        AuthorId = authorId,
                        Text = tag

                    });
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorId);
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Description", post.BlogId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if ( id is null )
            {
                return NotFound();
            }

            var post = await _context.Posts.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == id);

            if ( post == null )
            {
                return NotFound();
            }

            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorId);
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Description", post.BlogId);
            ViewData["TagValues"] = string.Join(",", post.Tags.Select(t => t.Text));

            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Content,ProductionStatus")] Post post, IFormFile newImage, List<string> tagValues)
        {
            if ( id != post.Id )
            {
                return NotFound();
            }

            if ( ModelState.IsValid )
            {
                try
                {
                    var newPost = await _context.Posts.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == post.Id);

                    newPost.Updated = DateTime.Now;

                    if ( newImage is not null )
                    {
                        newPost.ImageData = await _imageService.EncodeImageAsync(newImage);
                        newPost.ContentType = _imageService.ContentType(newImage);
                    }

                    if ( newPost.Title != post.Title )
                    {
                        //newPost.Title = post.Title;
                        var newSlug = _slugService.UrlFriendly(newPost.Title);
                        if ( _slugService.IsUnique(newSlug) )
                        {
                            newPost.Title = post.Title;
                            newPost.Slug = newSlug;
                        }
                        else
                        {
                            ModelState.AddModelError("Title", "The title already exists. Please use a different title.");
                            ViewData["TagValues"] = string.Join(",", tagValues);
                            return View(post);
                        }

                    }


                    if ( newPost.Abstract != post.Abstract )
                        newPost.Abstract = post.Abstract;

                    if ( newPost.Content != post.Content )
                        newPost.Content = post.Content;

                    if ( newPost.ProductionStatus != post.ProductionStatus )
                        newPost.ProductionStatus = post.ProductionStatus;

                    _context.Tags.RemoveRange(newPost.Tags);

                    foreach ( var tagText in tagValues )
                    {
                        _context.Add(new Tag()
                        {
                            PostId = post.Id,
                            AuthorId = newPost.AuthorId,
                            Text = tagText
                        });
                    }

                    await _context.SaveChangesAsync();
                }
                catch ( DbUpdateConcurrencyException )
                {
                    if ( !PostExists(post.Id) )
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorId);
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Description", post.BlogId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if ( id == null )
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Blog)
                .FirstOrDefaultAsync(m => m.Id == id);
            if ( post == null )
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
