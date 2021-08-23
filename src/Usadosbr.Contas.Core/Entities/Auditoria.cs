using System;
using Microsoft.EntityFrameworkCore;
using Usadosbr.Contas.Core.Persistance.Configurations;

namespace Usadosbr.Contas.Core.Entities
{
    [EntityTypeConfiguration(typeof(AuditoriaConfiguration))]
    public class Auditoria : EntityGuid
    {
        public DateTimeOffset DataOperacao { get; set; }
        public string Operacao { get; set; } = null!;
        public string Entidade { get; set; } = null!;
        public string Tabela { get; set; } = null!;
        public string? Chaves { get; set; }
        public string? ValoresOriginais { get; set; }
        public string? ValoresNovos { get; set; }
    }
}