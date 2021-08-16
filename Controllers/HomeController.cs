﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RomanWrites.Data;
using RomanWrites.Models;
using RomanWrites.Services;
using RomanWrites.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace RomanWrites.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogEmailSender _emailSender;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, IBlogEmailSender emailSender, ApplicationDbContext context)
        {
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 6;

            //var blogs = await _context.Blogs.Where(b => b.Posts.Any(p => p.ProductionStatus == Enums.ProductionStatus.PreviewReady))
            //            .OrderByDescending(b => b.Created).ToPagedListAsync(pageNumber, pageSize);

            var blogs = await _context.Blogs.Include(b => b.Author).OrderByDescending(b => b.Created).ToPagedListAsync(pageNumber, pageSize);

            return View(blogs);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactMe model)
        {
            model.Message = $"{model.Message} <hr/> Email Address : {model.Email}";
            try
            {
                await _emailSender.SendContactEmailAsync(model.Email, model.Name, model.Subject, model.Message);
                return View("ContactSuccess");
            }
            catch
            {
                return View("ContactFailed");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
