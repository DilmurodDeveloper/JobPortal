using System.Security.Claims;
using JobPortalAPI.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public ResumeController(IWebHostEnvironment env)
        {
            _env = env;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("upload")]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> UploadResume(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty.");

            var userId = GetUserId();
            var fileName = $"resume_{userId}{Path.GetExtension(file.FileName)}";
            var savePath = Path.Combine(_env.WebRootPath, "resumes", fileName);

            var dirPath = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath!);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { path = $"/resumes/{fileName}" });
        }

        [HttpGet]
        [Authorize(Roles = nameof(Role.User))]
        public IActionResult GetMyResume()
        {
            var userId = GetUserId();
            var resumeFolder = Path.Combine(_env.WebRootPath, "resumes");
            var filePath = Directory.GetFiles(resumeFolder, $"resume_{userId}.*").FirstOrDefault();

            if (filePath == null)
                return NotFound("Resume not found.");

            var mime = "application/octet-stream";
            var ext = Path.GetExtension(filePath).ToLower();
            if (ext == ".pdf") mime = "application/pdf";
            else if (ext == ".docx") mime = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

            return PhysicalFile(filePath, mime);
        }
    }
}
