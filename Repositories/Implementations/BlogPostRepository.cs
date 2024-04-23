using CodePulse.Data;
using CodePulse.Models.Domain;
using CodePulse.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.Repositories.Implementations
{
  public class BlogPostRepository : IBlogPostsRepository
  {
    private readonly ApplicationDBContext dbContext;

    public BlogPostRepository(ApplicationDBContext dbContext)
    {
      this.dbContext = dbContext;
    }
    public async Task<BlogPost> CreateAsync(BlogPost blog)
    {
      await dbContext.BlogPosts.AddAsync(blog);
      await dbContext.SaveChangesAsync();
      return blog;
    }

    public async Task<BlogPost> DeleteAsync(Guid id)
    {
      var existingBlog =  await dbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id );

      if (existingBlog == null)
      {
        return null;
      }
      dbContext.BlogPosts.Remove(existingBlog);
      await dbContext.SaveChangesAsync();
      return existingBlog;
    }

    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
      return await dbContext.BlogPosts.Include(x => x.Categories).ToListAsync();
    }

    public async Task<BlogPost?> GetByIdAsync(Guid id)
    {
      return await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<BlogPost?> GetByUrlAsync(string urlHandle)
    {
      return await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
    }

    public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
    {
      var eBlog = await dbContext.BlogPosts.Include(x =>x.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

      if (eBlog == null)
      {
        return null;
      }

      dbContext.BlogPosts.Entry(eBlog).CurrentValues.SetValues(blogPost);
      eBlog.Categories = blogPost.Categories;
      await dbContext.SaveChangesAsync();
      return blogPost;
    }
  }
}
