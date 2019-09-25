
namespace RedisSentinel.Tests
{
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
    using StackExchange.Redis;

    [TestClass]
    public class RedisSentinelRepostoryTests
    {
        private const int dataBase = 0;

        private const string redisConn = @"127.0.0.1:7001,127.0.0.1:7002,127.0.0.1:7003,127.0.0.1:7004,127.0.0.1:7005,127.0.0.1:7006,connectTimeout=10000,configCheckSeconds=3,allowAdmin=true";

        private const string affixKey = "RedisSentinel";

        private RedisSentinelRepostory repo;

        [TestInitialize]
        public void Init()
        {
            var conn = ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(redisConn));
            this.repo = new RedisSentinelRepostory(conn, affixKey);
        }

        [TestMethod]
        public void SetValueTest()
        {
            var result = this.repo.SetValue("TT", "RR");
            Assert.IsNull(result.Item1);
        }

        [TestMethod]
        public void GetValueTest()
        {
            var setResult = this.repo.SetValue("TT", "RR");
            Assert.IsNull(setResult.Item1);

            var getResult = this.repo.GetValue("TT");
            Assert.IsNull(getResult.Item1);
            Assert.IsNotNull(getResult.Item2);
            Assert.AreEqual(getResult.Item2, "RR");
        }

        [TestMethod]
        public void DeleteTest()
        {
            var setResult = this.repo.SetValue("TT", "RR");
            Assert.IsNull(setResult.Item1);

            var getResult = this.repo.GetValue("TT");
            Assert.IsNull(getResult.Item1);
            Assert.IsNotNull(getResult.Item2);
            Assert.AreEqual(getResult.Item2, "RR");

            var deleteResult = this.repo.RemoveKey("TT");
            Assert.IsNull(deleteResult.Item1);
        }
    }
}
