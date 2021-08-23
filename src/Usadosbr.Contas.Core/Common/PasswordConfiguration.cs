namespace Usadosbr.Contas.Core.Common
{
    public class PasswordConfiguration
    {
        public const bool RequireDigit = true;
        public const bool RequireLowercase = true;
        public const bool RequireUppercase = true;
        public const bool RequireNonAlphanumeric = true;
        public const int RequiredLength = 8;
        public const int RequiredUniqueChars = 1;
    }
}