namespace WebApi.Models
{
    public class FileUpload_Model
    {
        public string BucketName { get; set; }
        public string FileObject { get; set; }
        public string? FileName { get; set; }
        public string ContentType { get; set; } = "application/octet-stream";
    }
}
