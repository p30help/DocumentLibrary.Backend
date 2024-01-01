namespace DocumentLibrary.Infrastructure.Minio
{
    public class MinioConfiguration
    {
        public string ServiceUrl { get; set; }
        public string DocumentsBucketName { get; set; }
        public string ThumbnailsBucketName { get; set; }
        public bool SslSupport { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}
