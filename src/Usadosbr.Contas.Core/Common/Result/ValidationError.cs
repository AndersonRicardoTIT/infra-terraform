using System.Collections.Generic;

namespace Usadosbr.Contas.Core.Common.Result
{
    public class ValidationError
    {
        public string Key { get; }
        public IList<string> Messages { get; }

        public ValidationError(string key, IList<string> messages)
        {
            Key = key;
            Messages = messages;
        }
    }
}