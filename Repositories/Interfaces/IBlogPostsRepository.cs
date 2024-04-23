using CodePulse.Models.Domain;

namespace CodePulse.Repositories.Interfaces
{
  public interface IBlogPostsRepository
  {
    Task<BlogPost> CreateAsync(BlogPost blog);
    Task<IEnumerable<BlogPost>> GetAllAsync();
    Task<BlogPost?> DeleteAsync(Guid id);
    Task<BlogPost?> GetByIdAsync(Guid id);
    Task<BlogPost?> UpdateAsync(BlogPost blogPost);
    Task<BlogPost?> GetByUrlAsync(String urlHandle);
  }
}
