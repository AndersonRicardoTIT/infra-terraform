using System;

namespace Usadosbr.Contas.Core.Common.Services
{
    public interface IDateTimeService
    {
        DateTimeOffset Now { get; }
    }
}