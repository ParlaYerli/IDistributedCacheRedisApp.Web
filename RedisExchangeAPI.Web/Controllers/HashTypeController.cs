using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Servers;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class HashTypeController : Controller
    {
        //hash veri tipi, key value değerlerini array olarak tutar.c# da dictionary sınıfına denk gelir.
        private readonly RedisService _redisService;
        private readonly IDatabase database;
        private string hashKey = "dictionary"; // keyi sabit olarak yazıyorum cünkü sürekli aynısını kullanacağım.

        public HashTypeController(RedisService _redisService)
        {
            this._redisService = _redisService;
            database = _redisService.GetDB(5);
        }
        public IActionResult Index()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            if (database.KeyExists(hashKey))
            {
                database.HashGetAll(hashKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name, x.Value);
                });
            }
            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string name, string value)
        {
            database.HashSet(hashKey,name,value);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteItem(string name)
        {
            database.HashDelete(hashKey,name);
            return RedirectToAction("Index");
        }
    }
}