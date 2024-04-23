using CodePulse.Models.Domain;
using CodePulse.Models.DTO;
using CodePulse.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BlogImagesController : ControllerBase
  {

    private readonly IBlogPostImagesRepository blogPostImagesRepository;
    public BlogImagesController(IBlogPostImagesRepository blogPostImagesRepository)
    {
      this.blogPostImagesRepository = blogPostImagesRepository;
    }

    [HttpPost]
    public async Task<IActionResult> AddNewBlogImage(
      [FromForm] IFormFile file,
      [FromForm] string fileName,
      [FromForm] string title)
    {
      validateFile(file);

      if (ModelState.IsValid)
      {
        var blogImage = new BlogImages
        {
          FileExtention = Path.GetExtension(file.FileName).ToLower(),
          Title = title,
          FileName = fileName,
          DateCreated = DateTime.Now
        };

        blogImage =  await blogPostImagesRepository.CreateAsync(file, blogImage);

        var response = new BlogImagesDto
        {
          Id= blogImage.Id,
          Title= blogImage.Title,
          DateCreated = blogImage.DateCreated,
          FileExtention = blogImage.FileExtention,
          FileName = blogImage.FileName,
          Url = blogImage.Url,
        };

        return Ok(response);
      }
      return BadRequest(ModelState);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllImages()
    {
      var images = await blogPostImagesRepository.GetAll();

      var response = new List<BlogImagesDto>();
      foreach (var image in images)
      {
        response.Add(new BlogImagesDto
        {
          Id = image.Id,
          Title = image.Title,
          FileName = image.FileName,
          Url = image.Url,
          DateCreated = DateTime.Now,
          FileExtention = image.FileExtention,
        });
      }

      return Ok(response);
    }
    private void validateFile(IFormFile file)
    {
      var allowedFileExtentions = new string[]
      {
        ".jpeg", ".jpg", ".png", ".avif"
      };

      if(!allowedFileExtentions.Contains(Path.GetExtension(file.FileName).ToLower()))
      {
        ModelState.AddModelError("file", "Unsupported File Format");
      }

      if(file.Length > 10485760)
      {
        ModelState.AddModelError("file", "file cannot be greater than 10MB");
      }
    }
  }
}
