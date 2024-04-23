using CodePulse.Data;
using CodePulse.Models.Domain;

using CodePulse.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.Repositories.Implementations
{
  public class CategoryRepository : ICategoryRepository
  {
    private readonly ApplicationDBContext dbContext;
    public CategoryRepository(ApplicationDBContext dbContext) 
    { 
      this .dbContext = dbContext;
    }
    public async Task<Category> CreateAsync(Category category)
    {
      await dbContext.Categories.AddAsync(category);
      await dbContext.SaveChangesAsync();
      return category;
    }

    public async Task<Category?> DeleteAsync(Guid id)
    {
      var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
      if (existingCategory == null)
      {
        return null;
      }

      dbContext.Categories.Remove(existingCategory);
      await dbContext.SaveChangesAsync();
      return existingCategory;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
      return await dbContext.Categories.Include(x => x.BlogPosts).ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
      return await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category> UpdateAsync(Category category)
    {
      var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

      if (existingCategory != null)
      {
        dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
        await dbContext.SaveChangesAsync();
        return category;
      }

      return null;
    }
  }
}
