using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Usadosbr.Contas.Core.Common;
using Usadosbr.Contas.Core.Persistance.Configurations;

namespace Usadosbr.Contas.Core.Entities
{
    [EntityTypeConfiguration(typeof(UsadosbrUserConfiguration))]
    public class UsadosbrUser : IdentityUser<Guid>, IAuditable
    {
        public string Nome { get; set; } = null!;
        public string NomeNormalizado { get; set; } = null!;
        public string Telefone { get; set; } = null!;
    }
}