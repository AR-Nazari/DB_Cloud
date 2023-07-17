using Minio.Exceptions;
using Minio;
using System;
using Minio.DataModel;
using System.Threading.Tasks;
namespace WebApi
{



namespace FileUploader
    {
        class FileUpload
        {
            static void Main(string[] args)
            {
                var endpoint = "http://127.0.0.1:9000";
                var accessKey = "iT9aL0lOxwrZeKk7";
                var secretKey = "QX5HSRuopY0RXNXWV5Jk9FYpbihrVTf2";
                try
                {
                    var minio = new MinioClient()
                                        .WithEndpoint(endpoint)
                                        .WithCredentials(accessKey, secretKey)
                                        .WithSSL()
                                        .Build();
                    FileUpload.Run(minio).Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }

            // File uploader task.
            private async static Task Run(MinioClient minio)
            {
                var bucketName = "test";
                var location = "us-east-1";
                var objectName = "golden-oldies.zip";
                var filePath = "C:\\Users\\administrator\\Downloads\\amin.txt";
                var contentType = "application/zip";

                try
                {
                    // Make a bucket on the server, if not already present.
                    var beArgs = new BucketExistsArgs()
                        .WithBucket(bucketName);
                    bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);
                    if (!found)
                    {
                        var mbArgs = new MakeBucketArgs()
                            .WithBucket(bucketName);
                        await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                    }
                    // Upload a file to bucket.
                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(objectName)
                        .WithFileName(filePath)
                        .WithContentType(contentType);
                    await minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
                    Console.WriteLine("Successfully uploaded " + objectName);
                }
                catch (MinioException e)
                {
                    Console.WriteLine("File Upload Error: {0}", e.Message);
                }
            }
        }
    }
}

