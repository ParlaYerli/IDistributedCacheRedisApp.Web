using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Servers;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {

        private readonly RedisService _redisService;
        private readonly IDatabase database;
        private string listKey = "sortedsetnames"; // keyi sabit olarak yazıyorum cünkü sürekli aynısını kullanacağım.

        public SortedSetTypeController(RedisService _redisService)
        {
            this._redisService = _redisService;
            database = _redisService.GetDB(4);
        }
        public IActionResult Index()
        {
            HashSet<string> list = new HashSet<string>();
            if (database.KeyExists(listKey))
            {
                database.SortedSetScan(listKey).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });

                /*database.SortedSetRangeByRank(listKey, order: Order.Descending).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                }); */ // verilerin scorelarına göre sıralanır ve scoreları göstermez.
            }
            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string name, int score)
        {
            database.SortedSetAdd(listKey, name, score);
            database.KeyExpire(listKey, DateTime.Now.AddMinutes(1));
            return RedirectToAction("Index");
        }


        public IActionResult DeleteItem(string name)
        {
            database.SortedSetRemove(listKey, name);
            return RedirectToAction("Index");
        }
    }
}