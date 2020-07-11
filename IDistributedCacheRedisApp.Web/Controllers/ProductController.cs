using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            // complex typeların cachelenmesi
            Product product = new Product { Id = 1, Name = "Kalem", Price = 12 };
            string jsonProduct = JsonConvert.SerializeObject(product);
            await _distributedCache.SetStringAsync("product:1", jsonProduct, cacheEntryOptions); ;

            //complex typeların binary formata dönüştürüp cachelenmesi. İşlem kalabalıklığı olduğu için üstteki yöntem daha sık tercih edilebilir.
            
            Product byteProduct = new Product { Id = 2, Name = "Silgi", Price = 13 };
            string jsonproduct2 = JsonConvert.SerializeObject(byteProduct);
            Byte[] byteproduct = Encoding.UTF8.GetBytes(jsonproduct2);
            _distributedCache.Set("product:2",byteproduct);



            return View();
        }

        public IActionResult Show()
        {
            string name = _distributedCache.GetString("myname");
            string jsonProduct = _distributedCache.GetString("product:1");
            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);

            Byte[] byteProduct = _distributedCache.Get("product:2");
            string jsonproduct = Encoding.UTF8.GetString(byteProduct);
            Product pj = JsonConvert.DeserializeObject<Product>(jsonproduct);


            ViewBag.product = p;
            ViewBag.productpj = pj;
            ViewBag.name = name;

            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("myname");
            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/apple.jpg");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("picture",imageByte);
            
            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] imageByte = _distributedCache.Get("picture");
            return File(imageByte,"image/jpg");
            
        }
    }
}