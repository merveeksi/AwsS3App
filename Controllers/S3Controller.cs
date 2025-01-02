using AwsS3Api.Interfaces;
using AwsS3Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace AwsS3Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class S3Controller: ControllerBase
{
    private readonly IObjectStorage _objectStorage;

    public S3Controller(IObjectStorage objectStorage)
    {
        _objectStorage = objectStorage;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] S3UploadObjectRequestDto requestDto)
    {
        using var memoryStream = new MemoryStream();
        await requestDto.File.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();

        var objectPutRequestDto = new UploadObjectRequestDto(
            fileBytes: fileBytes,
            fileName: requestDto.File.FileName,
            fileExtension: Path.GetExtension(requestDto.File.FileName),
            fileType: requestDto.File.ContentType,
            metadata: new Dictionary<string, string> { { "UserId", "1" } }
        );

        var result = await _objectStorage.UploadObjectAsync(objectPutRequestDto);

        return Ok(result);
    }
}