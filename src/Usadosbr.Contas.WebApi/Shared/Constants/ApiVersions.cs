using Microsoft.AspNetCore.Mvc;

namespace Usadosbr.Contas.WebApi.Shared.Constants
{
    public class ApiVersions
    {
        public const string V1 = "1.0";

        public static readonly ApiVersion V1ApiVersion = ApiVersion.Parse(V1);
    }
}