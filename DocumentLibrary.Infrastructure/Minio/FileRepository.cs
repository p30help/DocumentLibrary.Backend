using DocumentLibrary.Domain.Contracts;
using Minio;
using Minio.DataModel.Args;

namespace DocumentLibrary.Infrastructure.Minio;

public class FileRepository : IFileRepository
{
    private readonly IMinioClient minioClient;
    private readonly MinioConfiguration minioConfiguration;

    public FileRepository(IMinioClient minioClient, MinioConfiguration minioConfiguration)
    {
        this.minioClient = minioClient;
        this.minioConfiguration = minioConfiguration;
    }

    public async Task UploadFile(string fileNameWithExtention, string contentType, Stream fileStreamData, DocumentAccessPolicy accessPolicy)
    {
        await TryToCreateBucket(accessPolicy);

        var putObjectArgs = new PutObjectArgs()
                .WithBucket(GetBucketName(accessPolicy).Name)
                .WithObject(fileNameWithExtention)
                .WithStreamData(fileStreamData)
                .WithObjectSize(fileStreamData.Length)
                .WithContentType(contentType);

        await minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
    }

    public async Task<Stream> GetFileStream(Guid fileId, DocumentAccessPolicy accessPolicy)
    {
        var bucket = GetBucketName(accessPolicy);

        // confirm object exist before attempt to get
        var stat = new StatObjectArgs()
            .WithBucket(bucket.Name)
            .WithObject(fileId.ToString());
        var statObj = await minioClient.StatObjectAsync(stat).ConfigureAwait(false);

        MemoryStream memoryStream = new MemoryStream();
        byte[] fileArray = new byte[statObj.Size];

        // get stream
        GetObjectArgs getObjectArgs = new GetObjectArgs()
            .WithBucket(bucket.Name)
            .WithObject(fileId.ToString())
            .WithCallbackStream(stream =>
            {
                //fileArray
                stream.CopyTo(memoryStream);
            });

        await minioClient.GetObjectAsync(getObjectArgs);

        memoryStream.Position = 0;

        return memoryStream;

    }    

    public string GetDirectFileUrl(Guid fileId, DocumentAccessPolicy accessPolicy)
    {
        var bucket = GetBucketName(accessPolicy);

        return $"http://{minioConfiguration.ServiceUrl}/{bucket.Name}/{fileId}";
    }

    private async Task TryToCreateBucket(DocumentAccessPolicy bucketType)
    {
        var bucket = GetBucketName(bucketType);

        var beArgs = new BucketExistsArgs()
            .WithBucket(bucket.Name);

        bool found = await minioClient.BucketExistsAsync(beArgs).ConfigureAwait(false);
        if (!found)
        {
            var mbArgs = new MakeBucketArgs()
                .WithBucket(bucket.Name);
            await minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);

            if (bucket.Public)
            {
                await MakeBucketPublic(bucket.Name);
            }
        }
    }

    private async Task MakeBucketPublic(string bucketName)
    {
        var policyJson = $@"{{
                            ""Version"": ""2012-10-17"",
                            ""Statement"": [
                                {{
                                    ""Effect"": ""Allow"",
                                    ""Principal"": {{
                                        ""AWS"": [
                                            ""*""
                                        ]
                                    }},
                                    ""Action"": [
                                        ""s3:GetObject""
                                    ],
                                    ""Resource"": [
                                        ""arn:aws:s3:::{bucketName}/*""
                                    ]
                                }}
                            ]
                        }}";

        var args = new SetPolicyArgs()
            .WithBucket(bucketName)
            .WithPolicy(policyJson);
        await minioClient.SetPolicyAsync(args).ConfigureAwait(false);
    }

    private (string Name, bool Public) GetBucketName(DocumentAccessPolicy accessPolicy)
    {
        if (accessPolicy == DocumentAccessPolicy.Private)
            return (minioConfiguration.DocumentsBucketName, false);
        else
            return (minioConfiguration.ThumbnailsBucketName, true);
    }
}
