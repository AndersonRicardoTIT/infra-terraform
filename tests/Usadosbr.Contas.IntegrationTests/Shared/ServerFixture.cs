using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Docker.DotNet;
using Microsoft.Extensions.Configuration;
using Usadosbr.Contas.IntegrationTests.Shared.Containers;
using Xunit;

namespace Usadosbr.Contas.IntegrationTests.Shared
{
    public sealed class ServerFixture : IDisposable, IAsyncLifetime
    {
        public const int Timeout = 60 * 1000;

        public readonly WebApiFactory Api;

        private readonly DockerClient _docker;

        private SqlServerContainer? _sql;

        private IConfiguration _configuration;

        public ServerFixture()
        {
            Api = new WebApiFactory();

            var pipe = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? new Uri("npipe://./pipe/docker_engine")
                : new Uri("unix:///var/run/docker.sock");

            _docker = new DockerClientConfiguration(pipe)
                .CreateClient();

            _configuration = GetConfiguration();
        }

        public async Task InitializeAsync()
        {
            _sql = new SqlServerContainer(_docker, _configuration.GetConnectionString("SqlServer"));

            await _sql.StartContainer();
        }

        public async Task DisposeAsync()
        {
            if (_sql != null)
            {
                await _sql.RemoveContainer();
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _docker.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ServerFixture()
        {
            Dispose(false);
        }

        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.IntegrationTest.json")
                .Build();
        }
    }
}