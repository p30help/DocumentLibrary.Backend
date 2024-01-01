namespace DocumentLibrary.Infrastructure.Minio
{
    public class JwtConfiguration
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpireIn { get; set; }
    }
}
