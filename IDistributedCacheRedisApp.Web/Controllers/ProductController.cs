using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IDistributedCache _distributedCache;
        public ProductController(IDistributedCache _distributedCache)
        {
            this._distributedCache = _distributedCache;
        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(2);
             _distributedCache.SetString("myname","Parla",cacheEntryOptions);
             await _distributedCache.SetStringAsync("lastname", "Yerli", cacheEntryOptions);

            Product product = new Product { Id = 1, Name = "Kalem", Price = 12 };
            string jsonProduct = JsonConvert.SerializeObject(product);

            await _distributedCache.SetStringAsync("product:1", jsonProduct, cacheEntryOptions); ;
            return View();
        }

        public IActionResult Show()
        {
            string name = _distributedCache.GetString("myname");
            string product = _distributedCache.GetString("product:1");
            ViewBag.product = product;
            ViewBag.name = name;
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("myname");
            return View();
        }
    }
}