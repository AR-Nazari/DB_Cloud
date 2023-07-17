using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.AspNetCore;
using Minio.DataModel;
using Minio.Exceptions;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Xml.Linq;
using WebApi.FileUploader;
using WebApi.Models;

namespace WebApi.Controllers

{
    [Route("api/[controller]/")]
    [ApiController]
    public class minioController : ControllerBase
    {
        MinioClient minioClient = new MinioClient()
                              .WithEndpoint("localhost:9000")
                              .WithCredentials("minioadmin", "minioadmin")
                              .Build();
        [HttpGet]
        [Authorize]
        [Route("makebucket/{bkname}")]
        public async Task<IActionResult> MakeBucket(string bkname)
        {
            object result = new();
            bkname = bkname.ToLower();
            if (!string.IsNullOrEmpty(bkname)) {
                try
                {
                    var mbArgs = new MakeBucketArgs()
                            .WithBucket(bkname);
                    await minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                }
            }
            else { return BadRequest("Empty Bucket name not Allowed"); }
        }


        [HttpGet]
        [Authorize]
        [Route("ListBuckets")]
        public async Task<IActionResult> ListBucket()
        {
            object result = new();
            try
            {
                result = await minioClient.ListBucketsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// توضیحات
        /// </summary>
        /// <param name="bkname"></param>
        /// <returns>خروجی</returns>
        [HttpGet]
        [Authorize]
        [Route("RemoveBucket/{bkname}")]
        public async Task<IActionResult> RemoveBucket(string bkname)
        {
            object result = new();
            bkname = bkname.ToLower();
            if (!string.IsNullOrEmpty(bkname))
            {
                try
                {
                    var mbArgs = new RemoveBucketArgs()
                            .WithBucket(bkname);
                    await minioClient.RemoveBucketAsync(mbArgs).ConfigureAwait(false);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                }
            }
            else { return BadRequest("Empty Bucket name not Allowed"); }
        }

        [HttpPost]
        [Authorize]
        [Route("PutObject")]
        public async Task<object> PutObject(FileUpload_Model file)
        {
          

            try
            {
                Aes aesEncryption = Aes.Create();
                aesEncryption.KeySize = 256;
                aesEncryption.GenerateKey();
                var ssec = new SSEC(aesEncryption.Key);
                //byte[] ByteFile = Convert.FromBase64String(file.File);
                PutObjectArgs putObjectArgs = new PutObjectArgs()
                                                  .WithBucket(file.BucketName)
                                                  .WithObject(file.FileObject)
                                                  .WithFileName(file.FileName)
                                                  .WithContentType(file.ContentType);
                                                 //.WithServerSideEncryption(ssec);
              return await minioClient.PutObjectAsync(putObjectArgs);
                
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("getObject")]
        public async Task<object> GetObject(FileUpload_Model file)
        {
            

            try
            {
                // Check whether the object exists using statObjectAsync().
                // If the object is not found, statObjectAsync() throws an exception,
                // else it means that the object exists.
                // Execution is successful.
                StatObjectArgs statObjectArgs = new StatObjectArgs()
                                        .WithBucket(file.BucketName)
                                        .WithObject(file.FileObject);
                await minioClient.StatObjectAsync(statObjectArgs);

                // Gets the object's data and stores it in photo.jpg
                GetObjectArgs getObjectArgs = new GetObjectArgs()
                                                  .WithBucket(file.BucketName)
                                                  .WithObject(file.FileObject)
                                                  .WithFile(file.FileName);
                                                 // .WithFileName(file.FileName);
                return (await minioClient.GetObjectAsync(getObjectArgs));

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
