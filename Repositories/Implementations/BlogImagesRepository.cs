using CodePulse.Data;
using CodePulse.Models.Domain;
using CodePulse.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace CodePulse.Repositories.Implementations
{
  public class BlogImagesRepository : IBlogPostImagesRepository
  {
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ApplicationDBContext dbContext;

    public BlogImagesRepository(
      IWebHostEnvironment webHostEnvironment,
      IHttpContextAccessor httpContextAccessor,
      ApplicationDBContext dbContext)
    {
      this.webHostEnvironment = webHostEnvironment;
      this.httpContextAccessor = httpContextAccessor;
      this.dbContext = dbContext;
    }
    public async Task<BlogImages> CreateAsync(IFormFile file, BlogImages blogImage)
    {
      var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtention}");

      using var stream = new FileStream(localPath, FileMode.Create);
      await file.CopyToAsync(stream);

      var httpRequest = httpContextAccessor.HttpContext.Request;
      var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtention}";
      blogImage.Url = urlPath;
      await dbContext.AddAsync(blogImage);
      await dbContext.SaveChangesAsync();
      return blogImage;
    }

    public async Task<IEnumerable<BlogImages>> GetAll()
    {
      return await dbContext.BlogImages.ToListAsync();
    }
  }
}
