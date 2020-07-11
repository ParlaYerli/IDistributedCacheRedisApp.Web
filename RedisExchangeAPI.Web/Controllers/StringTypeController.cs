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

            if (value.HasValue)
            {
                ViewBag.value = value.ToString();
            }
            return View();
        }
    }
}