using System;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Serilog;

namespace Usadosbr.Contas.IntegrationTests.Shared.Containers
{
    public abstract class ContainerImage
    {
        protected ContainerImage(string imageName, string containerName, IDockerClient client)
        {
            ImageName = imageName;
            ContainerName = containerName;
            _client = client;
        }

        public async Task StartContainer()
        {
            Log.Information("Inicializando container {ContainerName}", ContainerName);

            await CreateImage();

            var container = await FindContainerByName(ContainerName) ?? await CreateContainer();

            if (container.State != "running")
            {
                var started =
                    await _client.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());

                if (!started)
                {
                    throw new InvalidOperationException(
                        $"Não foi possível iniciar o container '{ContainerName}' falhou");
                }
            }

            var i = 0;
            while (!await IsReady())
            {
                if (i > 10)
                {
                    throw new InvalidOperationException($"O container '{ContainerName}' demorou muito para responder");
                }

                i += 1;

                await Task.Delay(5000);
            }
        }

        public Task RemoveContainer()
        {
            return _client.Containers.RemoveContainerAsync(ContainerName,
                new ContainerRemoveParameters { Force = true, RemoveVolumes = true });
        }

        private readonly IDockerClient _client;

        private string ImageName { get; }
        private string ContainerName { get; }

        protected abstract HostConfig GetHostConfig();
        protected abstract Config GetConfig();
        protected abstract Task<bool> IsReady();

        private async Task CreateImage()
        {
            Log.Information("Baixando imagem do dockerhub (se necessário)");
            await _client.Images.CreateImageAsync(new ImagesCreateParameters
            {
                FromImage = ImageName,
            }, new AuthConfig(), new Progress<JSONMessage>());
        }

        private async Task<ContainerListResponse> CreateContainer()
        {
            Log.Information("Criando container {ContainerName}", ContainerName);

            var response = await _client.Containers.CreateContainerAsync(new CreateContainerParameters(GetConfig())
            {
                Image = ImageName,
                Name = ContainerName,
                HostConfig = GetHostConfig(),
                Tty = false,
            });

            var containers =
                await _client.Containers.ListContainersAsync(new ContainersListParameters { All = true });

            return containers.FirstOrDefault(x => x.ID == response.ID) ??
                   throw new InvalidOperationException($"Não foi possiver criar o container {ContainerName}");
        }

        private async Task<ContainerListResponse?> FindContainerByName(string name)
        {
            Log.Information("Buscando container {ContainerName}", ContainerName);

            var containers = await _client.Containers.ListContainersAsync(new ContainersListParameters { All = true });

            return containers.FirstOrDefault(x => x.Names.Contains(string.Concat("/", name)));
        }
    }
}