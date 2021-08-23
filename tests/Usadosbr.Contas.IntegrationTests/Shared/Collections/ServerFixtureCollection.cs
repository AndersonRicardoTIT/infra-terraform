using Xunit;

namespace Usadosbr.Contas.IntegrationTests.Shared.Collections
{
    [CollectionDefinition(CollectionsList.WebApi)]
    public class ServerFixtureCollection : ICollectionFixture<ServerFixture>
    {
    }
}