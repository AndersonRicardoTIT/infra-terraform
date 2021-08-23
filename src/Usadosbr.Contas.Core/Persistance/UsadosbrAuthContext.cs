using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Usadosbr.Contas.Core.Common;
using Usadosbr.Contas.Core.Common.Services;
using Usadosbr.Contas.Core.Entities;
using Usadosbr.Contas.Core.Persistance.Models;

namespace Usadosbr.Contas.Core.Persistance
{
    public class UsadosbrAuthContext : IdentityDbContext<UsadosbrUser, IdentityRole<Guid>, Guid>
    {
        public UsadosbrAuthContext(DbContextOptions<UsadosbrAuthContext> options, IDateTimeService time)
            : base(options)
        {
            _time = time;
        }

        private readonly IDateTimeService _time;

        public DbSet<Auditoria> AuditHistory { get; set; } = null!;

        public DbSet<Modulo> Modulos { get; set; } = null!;

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new())
        {
            AttachAuditLogic();
            
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AttachAuditLogic()
        {
            // TODO: Adicionar dados de usuários.

            var entries = ChangeTracker.Entries().ToList();

            foreach (var entry in entries)
            {
                if (entry.Entity is not IAuditable || entry.State is EntityState.Detached or EntityState.Unchanged)
                {
                    continue;
                }

                var auditEntry = new AuditoriaEntry
                {
                    Tabela = entry.Metadata.GetTableName() ?? "*NO TABLE*",
                    Entidade = entry.Entity.GetType().Name,
                    DataOperacao = _time.Now,
                    Operacao = entry.State.ToString()
                };

                foreach (var property in entry.Properties)
                {
                    var name = property.Metadata.Name;

                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.Chaves[name] = property.CurrentValue!;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NovosValores[name] = property.CurrentValue!;
                            break;
                        case EntityState.Deleted:
                            auditEntry.ValoresOriginais[name] = property.OriginalValue!;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ValoresOriginais[name] = property.OriginalValue!;
                                auditEntry.NovosValores[name] = property.CurrentValue!;
                            }

                            break;
                    }
                }

                AuditHistory.Add(auditEntry.ToAuditoria());
            }
        }
    }
}