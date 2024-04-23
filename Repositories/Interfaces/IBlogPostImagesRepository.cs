using CodePulse.Models.Domain;

namespace CodePulse.Repositories.Interfaces
{
  public interface IBlogPostImagesRepository
  {
    Task<BlogImages> CreateAsync (IFormFile file, BlogImages blogImage);
    Task<IEnumerable<BlogImages>> GetAll();
  }
}
