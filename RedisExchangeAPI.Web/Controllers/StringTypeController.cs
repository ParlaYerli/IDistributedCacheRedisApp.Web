using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Servers;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase database;

       
        public StringTypeController(RedisService redisService)
        {
           _redisService = redisService;
           database = _redisService.GetDB(1);
        }
        public IActionResult Index()
        {

            database.StringSet("name", "Parla");

            database.StringSet("ziyaretci", 100);
            return View();
        }

        public IActionResult Show()
        {
            var value = database.StringGet("name");
            var length = database.StringLength("name");
            //var value = database.StringGetRange("name", 0, 2); // datanın 0 il 2 index arasında veriyi gösterir.
            // database.StringIncrement("ziyaretci", 1);
            //  var count = database.StringDecrementAsync("ziyaretci").Result; //async metodunu kullanınca geriye bir data döndürmek istiyorsam .Result metodunu kullanmak gerekiyor.eğer metod çalıştıktan sonra geriye bir data dönmeyecekse .Wait() çalıştırmak yeterli olacaktır.
           
            database.StringDecrementAsync("ziyaretci",10).Wait();
            if (value.HasValue)
            {
                ViewBag.value = value.ToString();
            }

            ViewBag.length = length;
            return View();
        }
    }
}