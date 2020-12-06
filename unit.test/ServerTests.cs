using Common;

using Server;
using NUnit.Framework;
using System.Threading.Tasks;
using System;

namespace unit.test
{
    [TestFixture]
    public class ServerTests
    {
        private const int port = 50001;
        private ClientServer server;

        [OneTimeSetUp]
        public void Init()
        {
            var N = 10;
            server = new ClientServer(N);

            Assert.IsNotNull(server);
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            Assert.IsNotNull(server);

            server.Stop();
        }

        [Test]
        public async Task Connection()
        {
            Assert.IsNotNull(server);

            ClientTcp client = new ClientTcp();
            Assert.IsNotNull(client);

            var isConnected = await client.Connect("127.0.0.1", port);

            Assert.IsFalse(isConnected);
            Assert.IsFalse(client.IsConnected);

            _ = Task.Run(server.StartListen);

            isConnected = await client.Connect("127.0.0.1", port);

            Assert.IsTrue(isConnected);
            Assert.IsTrue(client.IsConnected);
        }
    }
}
