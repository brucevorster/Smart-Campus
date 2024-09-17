namespace UmojaCampus.Shared.Configuration
{
    public class JwtConfiguration
    {
        public string SecretKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public int ExpiresIn { get; set; }
    }
}
