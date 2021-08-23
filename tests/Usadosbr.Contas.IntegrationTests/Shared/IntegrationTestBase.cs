using System.Net.Http;
using System.Threading.Tasks;
using Usadosbr.Contas.IntegrationTests.Shared.Collections;
using Xunit;

namespace Usadosbr.Contas.IntegrationTests.Shared
{
    [Collection(CollectionsList.WebApi)]
    public class IntegrationTestBase : IAsyncLifetime
    {
        protected readonly ServerFixture Server;

        protected readonly HttpClient Client;

        protected IntegrationTestBase(ServerFixture server)
        {
            Server = server;
            Client = server.Api.CreateClient();
        }

        public async Task InitializeAsync()
        {
            await Server.Api.ResetDatabase();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}