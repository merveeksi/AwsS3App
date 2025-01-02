namespace AwsS3Api.Models;

public sealed record S3UploadObjectRequestDto
{
    public IFormFile File { get; init; }
}