
namespace RedisSentinel
{
    using System;
    using StackExchange.Redis;

    public class RedisSentinelRepostory
    {
        private ConnectionMultiplexer conn;

        private string affixKey;

        public RedisSentinelRepostory(ConnectionMultiplexer conn, string affixKey)
        {
            this.conn = conn;
            this.affixKey = affixKey;
        }

        public Tuple<Exception, string> GetValue(string key)
        {
            try
            {
                return this.UseConnection(redis =>
                {
                    var value = redis.StringGetAsync($"{this.affixKey}:{key}").Result;
                    return Tuple.Create<Exception, string>(null, value);
                });
            }
            catch (Exception ex)
            {
                return Tuple.Create<Exception, string>(ex, string.Empty);
            }
        }

        public Tuple<Exception> RemoveKey(string key)
        {
            try
            {
                return this.UseConnection(redis =>
                {
                    redis.KeyDelete($"{this.affixKey}:{key}");
                    return Tuple.Create<Exception>(null);
                });
            }
            catch (Exception ex)
            {
                return Tuple.Create<Exception>(ex);
            }
        }

        public Tuple<Exception> SetValue(string key, string value)
        {
            try
            {
                return this.UseConnection(redis =>
                {
                    redis.StringSetAsync($"{this.affixKey}:{key}", value).ConfigureAwait(false).GetAwaiter().GetResult();
                    return Tuple.Create<Exception>(null);
                });
            }
            catch (Exception ex)
            {
                return Tuple.Create<Exception>(ex);
            }
        }

        private T UseConnection<T>(Func<IDatabase, T> func)
        {
            var redis = this.conn.GetDatabase(0);
            return func(redis);
        }
    }
}
