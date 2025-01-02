using Amazon.S3;
using Amazon.S3.Model;
using AwsS3Api.Interfaces;
using AwsS3Api.Models;
using AwsS3Api.Settings;
using Microsoft.Extensions.Options;

namespace AwsS3Api.Services;

public sealed class S3ObjectStorageManager : IObjectStorage
{
    private readonly S3Settings _s3Settings;
    private readonly IAmazonS3 _s3Client;

    public S3ObjectStorageManager(IOptions<S3Settings> s3Settings)
    {
        _s3Settings = s3Settings.Value;
        // Credentials are required to access the S3 bucket
        _s3Client = new AmazonS3Client(
            _s3Settings.AccessKey,
            _s3Settings.SecretKey,
            Amazon.RegionEndpoint.GetBySystemName(_s3Settings.BucketRegion));
    }

    public async Task<string> UploadObjectAsync(UploadObjectRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        var objectKey = $"{Guid.NewGuid()}{requestDto.FileExtension}";

        var putRequest1 = new PutObjectRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = objectKey,
            InputStream = new MemoryStream(requestDto.FileBytes),
            ContentType = requestDto.FileType
        };

        // Add metadata to the object
        foreach (var metadata in requestDto.Metadata)
            putRequest1.Metadata.Add(metadata.Key, metadata.Value);

        PutObjectResponse response1 = await _s3Client.PutObjectAsync(putRequest1);

        return objectKey;
    }
}
