using image_upload.Services;
using Microsoft.AspNetCore.Mvc;

namespace image_upload.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ImageController : ControllerBase
  {
    private readonly AzureBlobStorageService _blobStorageService;

    public ImageController(AzureBlobStorageService blobStorageService)
    {
      _blobStorageService = blobStorageService;
    }

    [HttpPost]
    public async Task<ActionResult<string>> UploadFile(IFormFile file)
    {
      if (file == null || file.Length == 0)
      {
        return BadRequest("Please provide a file to upload.");
      }

      using (var stream = file.OpenReadStream())
      {
        string fileName = file.FileName;
        string url = await _blobStorageService.UploadFileAsync(fileName, stream);
        return Ok(url);
      }
    }

    [HttpGet("{fileName}")]
    public async Task<ActionResult<string>> DownloadFile(string fileName)
    {
      var fileStream = await _blobStorageService.DownloadFileAsync(fileName);

      if (fileStream == null)
      {
        return NotFound();
      }

      return File(fileStream, "application/octet-stream", fileName);
    }
  }
}