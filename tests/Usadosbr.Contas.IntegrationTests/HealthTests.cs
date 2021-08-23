using System.Net.Http.Json;
using System.Threading.Tasks;
using Usadosbr.Contas.IntegrationTests.Shared;
using Xunit;

namespace Usadosbr.Contas.IntegrationTests
{
    public class HeathCheckTestsBase : IntegrationTestBase
    {
        public HeathCheckTestsBase(ServerFixture server) : base(server)
        {
        }

        [Fact]
        public async Task CheckHealth()
        {
            var healthCheck = await Client.GetAsync("api/HealthCheck");

            var content = await healthCheck.Content.ReadFromJsonAsync<string>();
            
            Assert.Equal("I'm alive", content);
        }
    }
}