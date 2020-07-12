using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Servers;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        // Set türünde eklenen veriler random ve unique seklinde kaydedilir.
        private readonly RedisService _redisService;
        private readonly IDatabase database;
        private string listKey = "setnames"; // keyi sabit olarak yazıyorum cünkü sürekli aynısını kullanacağım.

        public SetTypeController(RedisService _redisService)
        {
            this._redisService = _redisService;
            database = _redisService.GetDB(3);
        }
        public IActionResult Index()
        {
            HashSet<string> nameList = new HashSet<string>();//Set verileri unique olarak ve sırasız bir sekilde tutar.
            if (database.KeyExists(listKey))
            {
                database.SetMembers(listKey).ToList().ForEach(x=>
                {
                    nameList.Add(x.ToString());
                });
            }
            return View(nameList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            database.KeyExpire(listKey, DateTime.Now.AddMinutes(5)); //slidingexpiration özelliği bulunmamaktadır ama bu sekilde eklenmiş oldu. metod her çalıştığında verinin cache ömrü belirlenen dakika kadar olacak. Eğer slidingexpiration olmasın dersem if (!database.KeyExists(listKey)) içine yazarım.
            database.SetAdd(listKey, name);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteItem(string name)
        {
            await database.SetRemoveAsync(listKey, name);
            return RedirectToAction("Index");
        }
    }
}