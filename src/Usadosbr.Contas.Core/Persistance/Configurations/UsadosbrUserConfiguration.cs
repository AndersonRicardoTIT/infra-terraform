using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Usadosbr.Contas.Core.Entities;

namespace Usadosbr.Contas.Core.Persistance.Configurations
{
    public class UsadosbrUserConfiguration : IEntityTypeConfiguration<UsadosbrUser>
    {
        public void Configure(EntityTypeBuilder<UsadosbrUser> builder)
        {
            builder.Property(x => x.Nome)
                .HasColumnType("NVARCHAR(200)")
                .HasMaxLength(200);
            
            builder.Property(x => x.NomeNormalizado)
                .HasColumnType("NVARCHAR(200)")
                .HasMaxLength(200);
            
            builder.HasIndex(x => x.NomeNormalizado);
            
            builder.Property(x => x.Telefone)
                .HasColumnType("NVARCHAR(20)")
                .HasMaxLength(20);
        }
    }
}