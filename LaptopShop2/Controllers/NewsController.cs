using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.AccessControl;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Azure;
using System.Drawing.Printing;

namespace LaptopShop2.Controllers
{
    public class NewsController : Controller
    {
        private readonly LaptopShopContext _context;
        private readonly ILogger<NewsController> _logger;

        public NewsController(LaptopShopContext context, ILogger<NewsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index(int? page, string searchString, int? categoryId, string tag)
        {
            ViewBag.Keyword = searchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            var news = _context.TbNews.Where(n => n.IsActive);

            if(!string.IsNullOrEmpty(searchString) )
            {
                searchString = searchString.ToLower();
                news = news.Where(n => n.Title.ToLower().Contains(searchString));
            }

            if(categoryId.HasValue )
            {
                news = news.Where(n => n.CategoryNewId == categoryId);
            }

            if (!string.IsNullOrEmpty(tag))
            {
                news = news.Where(n => n.Tags.Contains(tag));
            }

            var newsList = news.OrderByDescending(n => n.CreatedDate).ToPagedList(pageNumber, pageSize);

            var categoryNews = _context.TbCategoryNews
                                        .Select(c => new
                                        {
                                            CategoryNew = c,
                                            NewsCount = c.TbNews.Count(n => n.IsActive)
                                        })
                                        .ToList();

            ViewBag.CategoryNews = categoryNews;

            var tags = _context.TbNews
                                .Where(n => n.IsActive && n.Tags != null)
                                .AsEnumerable()
                                .Select(selector: n => n.Tags.Split([',']))
                                .SelectMany(tagsArray => tagsArray)
                                .Distinct()
                                .ToList();
            ViewBag.Tags = tags;
            return View(newsList);
        }

        [Route("/news-{slug}-{id:}.html", Name = "newDetail")]
        public ActionResult Details(int? id, string searchString, int? categoryId, string tag)
        {
            if(id == null)
            {
                return NotFound();
            }
            var newDetails = _context.TbNews.Where(n => n.NewId == id).FirstOrDefault();
            if(newDetails == null)
            {
                return NotFound();
            }

            var news = _context.TbNews.Where(n => n.IsActive); 
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                news = news.Where(n => n.Title.ToLower().Contains(searchString));
            }

            if (categoryId.HasValue)
            {
                news = news.Where(n => n.CategoryNewId == categoryId);
            }

            if (!string.IsNullOrEmpty(tag))
            {
                news = news.Where(n => n.Tags.Contains(tag));
            }

           

            var categoryNews = _context.TbCategoryNews
                                        .Select(c => new
                                        {
                                            CategoryNew = c,
                                            NewsCount = c.TbNews.Count(n => n.IsActive)
                                        })
                                        .ToList();

            ViewBag.CategoryNews = categoryNews;

            var tags = _context.TbNews
                                .Where(n => n.IsActive && n.Tags != null)
                                .AsEnumerable()
                                .Select(selector: n => n.Tags.Split([',']))
                                .SelectMany(tagsArray => tagsArray)
                                .Distinct()
                                .ToList();
            ViewBag.Tags = tags;
            return View(newDetails);
        }
    }
}
