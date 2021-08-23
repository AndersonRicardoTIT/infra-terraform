using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Usadosbr.Contas.Core.Entities;

namespace Usadosbr.Contas.Core.Persistance.Configurations
{
    public class ModuloConfiguration : IEntityTypeConfiguration<Modulo> 
    {
        public void Configure(EntityTypeBuilder<Modulo> builder)
        {
            builder.ToTable("Modulo");

            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Descricao)
                .IsRequired()
                .HasColumnType("varchar(20)");
        }
    }
}