using FluentValidation.TestHelper;
using Usadosbr.Contas.Core.Features.Usuarios.Commands.CriaUsuarioCommand;
using Xunit;

namespace Usadosbr.Contas.Tests.UnitTest.Validators
{
    public class CriaUsuarioTests
    {
        private readonly CriaUsuarioCommandValidator _validator;
        
        public CriaUsuarioTests()
        {
            _validator = new CriaUsuarioCommandValidator();
        }
        
        [Fact(DisplayName = "Deve validar um usuário válido")]
        [Trait("Categoria", "Validações")]
        public void Usuario_Validacoes_DeveValidarUsuarioValido()
        {
            var command = new CriaUsuarioCommand(new string('*', 20), "foo@example.org", "P@ssw0rd", "65999999999");

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
        
           
        [Fact(DisplayName = "Deve validar um usuário com nome muito grande")]
        [Trait("Categoria", "Validações")]
        public void Usuario_Validacoes_DeveValidarNomeInvalido()
        {
            var command = new CriaUsuarioCommand(new string('*', 201), "foo@example.org", "P@ssw0rd", "65999999999");

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Nome);
        }
        
        [Theory(DisplayName = "Deve validar um usuário com email inválido")]
        [Trait("Categoria", "Validações")]
        [InlineData("@")]
        [InlineData("foo@")]
        [InlineData("@foo")]
        [InlineData("foobar.com")]
        public void Usuario_Validacoes_DeveValidarEmailInvalido(string email)
        {
            var command = new CriaUsuarioCommand(new string('*', 100), email, "P@ssw0rd", "65999999999");

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
        
        [Theory(DisplayName = "Deve validar um usuário com telefone inválido")]
        [Trait("Categoria", "Validações")]
        [InlineData("")]
        [InlineData("1234")]
        [InlineData("99999999")]
        [InlineData("6599999999")]
        [InlineData("659999999999999999999")]
        [InlineData("6599ABCDEFG")]
        public void Usuario_Validacoes_DeveValidarTelefoneInvalido(string telefone)
        {
            var command = new CriaUsuarioCommand(new string('*', 100), "example.org", "P@ssw0rd", telefone);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Telefone);
        }
    }
}