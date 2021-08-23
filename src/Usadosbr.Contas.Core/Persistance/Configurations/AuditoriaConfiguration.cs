using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Usadosbr.Contas.Core.Entities;

namespace Usadosbr.Contas.Core.Persistance.Configurations
{
    public class AuditoriaConfiguration : IEntityTypeConfiguration<Auditoria>
    {
        public void Configure(EntityTypeBuilder<Auditoria> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Auditoria");

            builder.Property(x => x.Operacao)
                .HasColumnType("nvarchar(200)")
                .HasMaxLength(200);

            builder.Property(x => x.Entidade)
                .HasColumnType("nvarchar(200)")
                .HasMaxLength(200);

            builder.Property(x => x.Tabela)
                .HasColumnType("nvarchar(200)")
                .HasMaxLength(200);

            builder.HasIndex(x => x.Tabela);

            builder.Property(x => x.Chaves)
                .HasColumnType("nvarchar(max)");
            
            builder.Property(x => x.ValoresOriginais)
                .HasColumnType("nvarchar(max)");
            
            builder.Property(x => x.ValoresNovos)
                .HasColumnType("nvarchar(max)");
        }
    }
}