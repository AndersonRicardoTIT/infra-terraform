using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usadosbr.Contas.Core.Entities;
using Usadosbr.Contas.Core.Features.Usuarios.Commands.CriaUsuarioCommand;
using Usadosbr.Contas.IntegrationTests.Shared;
using Usadosbr.Contas.WebApi.Shared.Result;
using Xunit;

namespace Usadosbr.Contas.IntegrationTests.Usuarios
{
    public class UsuarioTests : IntegrationTestBase
    {
        public UsuarioTests(ServerFixture server) : base(server)
        {
        }

        [Fact(DisplayName = "Deve criar usuário válido")]
        [Trait("Categoria", "Negócio")]
        public async Task Usuario_Criar_DeveCriarUsuario()
        {
            var command = new CriaUsuarioCommand("Foo Silva", "foo@example.org", "P@ssw0rd", "65999999999");

            var response = await Client.PostAsJsonAsync("api/Usuarios", command);

            var json = await response.Content.ReadFromJsonAsync<Guid>();

            // Assert
            ((int) response.StatusCode).Should().Be(StatusCodes.Status201Created);

            json.Should().NotBeEmpty();

            var user = await Server.Api.FirstOrDefault<UsadosbrUser>(x => x.Email == command.Email);

            user.Should().NotBeNull();

            user!.Nome.Should().Be("Foo Silva");
            user!.Email.Should().Be("foo@example.org");
            user!.Telefone.Should().Be("65999999999");
        }

        [Fact(DisplayName = "Não deve criar usuário que já existe")]
        [Trait("Categoria", "Negócio")]
        public async Task Usuario_Criar_NaoDeveCriarUsuarioJaExiste()
        {
            Server.Api.Add(new UsadosbrUser
            {
                Nome = "Foo Silva",
                UserName = "foo@example.org",
                NormalizedUserName = "FOO@EXAMPLE.ORG",
                Email = "foo@example.org",
                NormalizedEmail = "FOO@EXAMPLE.ORG",
                NomeNormalizado = "FOO SILVA",
                Telefone = "65999999999"
            });

            var command = new CriaUsuarioCommand("Foo Silva", "foo@example.org", "P@ssw0rd", "65999999999");

            var response = await Client.PostAsJsonAsync("api/Usuarios", command);

            // Assert
            ((int) response.StatusCode).Should().Be(StatusCodes.Status422UnprocessableEntity);

            var json = await response.Content.ReadFromJsonAsync<ValidationErrorDetails>();

            json!.Status.Should().Be(422);
        }

        [Fact(DisplayName = "Não deve criar usuário com email inválido")]
        [Trait("Categoria", "Negócio")]
        public async Task Usuario_Criar_NaoDeveCriarUsuarioInvalido()
        {
            var command = new CriaUsuarioCommand("Baz Jorge", "example.org", "P@ssw0rd", "65999999999");

            var response = await Client.PostAsJsonAsync("api/Usuarios", command);

            await Client.PostAsJsonAsync("api/Usuarios", command);


            // Assert
            ((int) response.StatusCode).Should().Be(StatusCodes.Status400BadRequest);

            var json = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            json!.Status.Should().Be(400);

            var user = await Server.Api.FirstOrDefault<UsadosbrUser>(x => x.Email == command.Email);

            user.Should().BeNull();
        }

        [Fact(DisplayName = "Não deve criar usuário com senha inválida")]
        [Trait("Categoria", "Negócio")]
        public async Task Usuario_Criar_NaoDeveCriarUsuarioSenhaInvalida()
        {
            var command = new CriaUsuarioCommand("Baz Jorge", "example@org", "1234", "65999999999");

            var response = await Client.PostAsJsonAsync("api/Usuarios", command);

            // Assert
            ((int) response.StatusCode).Should().Be(StatusCodes.Status422UnprocessableEntity);

            var json = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            json!.Status.Should().Be(422);

            var user = await Server.Api.FirstOrDefault<UsadosbrUser>(x => x.Email == command.Email);

            user.Should().BeNull();
        }
    }
}