﻿using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Infrastructure.Minio;
using Minio;
using Minio.DataModel.Args;

namespace DocumentLibrary.Infrastructure.Repositories
{
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

            // Upload a file to bucket.
            var putObjectArgs = new PutObjectArgs()
                    .WithBucket(GetBucketName(accessPolicy).Name)
                    .WithObject(fileNameWithExtention)
                    .WithStreamData(fileStreamData)
                    .WithObjectSize(fileStreamData.Length)
                    .WithContentType(contentType);

            await minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
        }

        private async Task TryToCreateBucket(DocumentAccessPolicy bucketType)
        {
            var bucket = GetBucketName(bucketType); 

            // Make a bucket on the server, if not already present.
            var beArgs = new BucketExistsArgs()
                .WithBucket(bucket.Name);
                        
            bool found = await minioClient.BucketExistsAsync(beArgs).ConfigureAwait(false);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs()
                    .WithBucket(bucket.Name);
                await minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);

                if(bucket.Public)
                {
                    await MakeBucketPublic(bucket.Name);
                }
            }
        }

        private async Task MakeBucketPublic(string bucketName)
        {
            //var policyJson2 = $@"{{""Version"":""2012-10-17"",""Statement"":[{{""Action"":[""s3:GetBucketLocation""],""Effect"":""Allow"",""Principal"":{{""AWS"":[""*""]}},""Resource"":[""arn:aws:s3:::{bucket.Name}""],""Sid"":""""}},{{""Action"":[""s3:ListBucket""],""Condition"":{{""StringEquals"":{{""s3:prefix"":[""foo"",""prefix/""]}}}},""Effect"":""Allow"",""Principal"":{{""AWS"":[""*""]}},""Resource"":[""arn:aws:s3:::{bucket.Name}""],""Sid"":""""}},{{""Action"":[""s3:GetObject""],""Effect"":""Allow"",""Principal"":{{""AWS"":[""*""]}},""Resource"":[""arn:aws:s3:::{bucket.Name}/foo*"",""arn:aws:s3:::{bucket.Name}/prefix/*""],""Sid"":""""}}]}}";

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

        //public async Task<string> GenerateTemporaryFileLink(Guid fileId, TimeSpan expireTime)
        //{
        //    var obj = new PresignedGetObjectArgs()
        //        .WithBucket(minioConfiguration.DocumentsBucketName)
        //        .WithObject(fileId.ToString())
        //        .WithExpiry((int)expireTime.TotalSeconds);
        //
        //    return await minioClient.PresignedGetObjectAsync(obj).ConfigureAwait(false);
        //}

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

        private (string Name, bool Public) GetBucketName(DocumentAccessPolicy accessPolicy)
        {
            if (accessPolicy == DocumentAccessPolicy.Private)
                return (minioConfiguration.DocumentsBucketName, false);
            else
                return (minioConfiguration.ThumbnailsBucketName, true);
        }

        public string GetDirectFileUrl(Guid fileId, DocumentAccessPolicy accessPolicy)
        {
            var bucket = GetBucketName(accessPolicy);

            return $"http://{minioConfiguration.ServiceUrl}/{bucket.Name}/{fileId}";
        }
    }
}
