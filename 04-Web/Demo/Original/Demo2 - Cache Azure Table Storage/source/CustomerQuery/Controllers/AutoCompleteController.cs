using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CustomerQuery.Controllers
{
    public class AutoCompleteController : ApiController
    {
        public List<string> Get(string prefix)
        {
            List<string> ret = new List<string>();
            BookSleeve.RedisConnection connection = new BookSleeve.RedisConnection("querydemo.redis.cache.windows.net",
                password:"AwMLfPaovSxdkXv+sOWx4gf0GBRqKiMcZMwsvW8Ig+E=");
            connection.Open();
            var list = connection.Wait(connection.Keys.Find(0, "cust:" + prefix.Replace(' ', ':') + "*"));
            for (int i = 0; i < Math.Min(5, list.Length); i++)
            {
                ret.Add(list[i].Substring(5).Replace(':',' '));
            }
            connection.Close(false);
            return ret;
        }
    }
}
