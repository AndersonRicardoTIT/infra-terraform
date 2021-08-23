using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Data.SqlClient;
using Xunit;

namespace Usadosbr.Contas.IntegrationTests.Shared.Containers
{
    public class SqlServerContainer : ContainerImage
    {
        public const string Image = "mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04";

        public const string ContainerName = "usadosbr-integration-tests-sqlserver";

        public readonly string ConnectionString;

        public readonly string HostPort;

        public readonly string HostIp;

        public readonly string Password;

        public SqlServerContainer(IDockerClient client, string connectionString) : base(Image, ContainerName, client)
        {
            ConnectionString = connectionString;

            var parsed = new SqlConnectionStringBuilder(ConnectionString);

            var host = parsed.DataSource.Split(',');

            HostIp = host[0];

            HostPort = host[1];

            Password = parsed.Password;
        }

        protected override HostConfig GetHostConfig()
        {
            return new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    {
                        "1433/tcp",
                        new List<PortBinding>
                        {
                            new()
                            {
                                HostPort = HostPort,
                                HostIP = HostIp
                            }
                        }
                    }
                }
            };
        }

        protected override Config GetConfig()
        {
            return new Config
            {
                Env = new List<string> { "ACCEPT_EULA=Y", $"SA_PASSWORD={Password}", "MSSQL_PID=Developer" }
            };
        }

        protected override async Task<bool> IsReady()
        {
            try
            {
                var parsed = new SqlConnectionStringBuilder(ConnectionString);

                parsed.InitialCatalog = string.Empty;

                await using var conn = new SqlConnection(parsed.ConnectionString);

                await conn.OpenAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}