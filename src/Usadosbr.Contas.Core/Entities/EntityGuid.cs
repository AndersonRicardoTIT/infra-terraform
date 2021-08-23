using System;
using MassTransit;

namespace Usadosbr.Contas.Core.Entities
{
    public abstract class EntityGuid : Entity<Guid>
    {
        protected EntityGuid()
        {
            Id = NewId.NextGuid();
        }
    }
}