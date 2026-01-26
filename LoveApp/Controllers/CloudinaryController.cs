using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
public class CloudinarySettings
{
    public string CloudName { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string ApiSecret { get; set; } = string.Empty;
}

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CloudinaryController : ControllerBase
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryController(IOptions<CloudinarySettings> cloudinaryOptions)
    {
        var settings = cloudinaryOptions.Value;

        // Initialize Cloudinary with credentials from appsettings.json
        Account account = new Account(
            settings.CloudName,
            settings.ApiKey,
            settings.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

    /// <summary>
    /// Returns signature + timestamp for Angular to perform signed upload
    /// </summary>
    [HttpGet("signature")]
    public IActionResult GetSignature(string folder = "memories")
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var parameters = new SortedDictionary<string, object>
    {
        { "timestamp", timestamp },
        { "folder", folder } // must match exactly frontend
    };

        var signature = _cloudinary.Api.SignParameters(parameters);

        return Ok(new { signature, timestamp, folder });
    }
}
