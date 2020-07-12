using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Servers;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase database;
        private string listKey = "names"; // keyi sabit olarak yazıyorum cünkü sürekli aynısını kullanacağım.

        public ListTypeController(RedisService _redisService)
        {
            this._redisService = _redisService;
            database = _redisService.GetDB(2);
        }
        public IActionResult Index()
        {
            List<string> nameList = new List<string>();
            if (database.KeyExists(listKey))
            {
                database.ListRange(listKey).ToList().ForEach(x =>
                {
                    nameList.Add(x.ToString());
                });
            }

            return View(nameList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            database.ListRightPush(listKey, name);
            //database.ListRightPush(listKey,name); listenin basına ekler
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            database.ListRemoveAsync(listKey, name).Wait();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteFirstItem(string name)
        {
            database.ListLeftPop(listKey);
             //database.ListRightPop(listKey);
            return RedirectToAction("Index");
        }
    }
}