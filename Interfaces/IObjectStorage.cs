using AwsS3Api.Models;

namespace AwsS3Api.Interfaces;

public interface IObjectStorage
{
    Task<string> UploadObjectAsync(UploadObjectRequestDto requestDto, CancellationToken cancellationToken = default);
}