using FluentAssertions;
using Usadosbr.Contas.Core.Entities;
using Xunit;

namespace Usadosbr.Contas.Tests.UnitTest.Entities
{
    public class ModuloTests
    {
        [Fact(DisplayName = "Deve criar o modulo")]
        [Trait("Categoria", "Negócio")]
        public void Modulo_Criar_DeveCriarModulo()
        {
            var modulo = new Modulo("Revenda");

            modulo.Should().NotBeNull();
            modulo.Descricao.Should().NotBeNullOrEmpty();
        }
    }
}