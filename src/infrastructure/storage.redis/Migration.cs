using System;
using System.Linq;
using System.Threading.Tasks;
using core.Options;
using StackExchange.Redis;

namespace storage.redis
{
    public class Migration
    {
        protected ConnectionMultiplexer _redis;

        public Migration(string redisCnn)
        {
            _redis = ConnectionMultiplexer.Connect(redisCnn);
        }

        public async Task FixMinValueDates(string entity, string userId)
        {
            var redisKey = entity + ":" + userId;

            var db = _redis.GetDatabase();

            var keys = await db.SetMembersAsync(redisKey);

            var events = keys.Select(async k => await db.HashGetAllAsync(k.ToString()))
                .Select(e => AggregateStorage.ToEvent(entity, userId, e.Result));

            foreach(var e in events)
            {
                if (e.Event.When == DateTime.MinValue)
                {
                    e.Event.When = e.Created;

                    var fields = new HashEntry[] {
                        new HashEntry("created", e.Created.ToString("o")),
                        new HashEntry("entity", entity),
                        new HashEntry("event", e.EventJson),
                        new HashEntry("key", e.Key),
                        new HashEntry("userId", e.UserId),
                        new HashEntry("version", e.Version),
                    };

                    var keyToStore = $"{entity}:{e.UserId}:{e.Key}:{e.Version}";

                    await db.HashSetAsync(keyToStore, fields);
                }
            }
        }

        public async Task<int> FixMistakenEntry()
        {
            const string entity = "soldoption";
            const string userId = "laimis@gmail.com";

            var redisKey = entity + ":" + userId;

            var db = _redis.GetDatabase();

            var keys = await db.SetMembersAsync(redisKey);

            var events = keys.Select(async k => await db.HashGetAllAsync(k.ToString()))
                .Select(e => AggregateStorage.ToEvent(entity, userId, e.Result));
            
            var fixedRecords = 0;

            foreach(var e in events)
            {
                if (e.Event is OptionClosed c && c.Money == 140 && e.Key.Contains("WORK"))
                {
                    c.Money = 40;

                    var fields = new HashEntry[] {
                        new HashEntry("created", e.Created.ToString("o")),
                        new HashEntry("entity", entity),
                        new HashEntry("event", e.EventJson),
                        new HashEntry("key", e.Key),
                        new HashEntry("userId", e.UserId),
                        new HashEntry("version", e.Version),
                    };

                    var keyToStore = $"{entity}:{e.UserId}:{e.Key}:{e.Version}";

                    await db.HashSetAsync(keyToStore, fields);

                    fixedRecords++;
                }
            }

            return fixedRecords;
        }
    }
}