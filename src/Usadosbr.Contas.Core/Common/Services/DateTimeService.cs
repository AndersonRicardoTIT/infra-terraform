using System;

namespace Usadosbr.Contas.Core.Common.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTimeOffset Now => DateTimeOffset.Now;
    }
}