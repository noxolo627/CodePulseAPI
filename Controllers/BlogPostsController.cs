using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CodePulse.Models.Domain;
using CodePulse.Models.DTO;
using CodePulse.Repositories.Interfaces;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authorization;

namespace CodePulse.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BlogPostsController : ControllerBase
  {
    private readonly IBlogPostsRepository blogPostsRepository;
    private readonly ICategoryRepository categoryRepository;

    public BlogPostsController(IBlogPostsRepository blogPostsRepository, ICategoryRepository categoryRepository)
    {
      this.blogPostsRepository = blogPostsRepository;
      this.categoryRepository = categoryRepository;
    }

    [HttpPost]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostResquestDto request)
    {
      //convert Dto to Domain
      var blog = new BlogPost
      {
        Title = request.Title,
        ShortDescription = request.ShortDescription,
        Content = request.Content,
        FeaturedImageUrl = request.FeaturedImageUrl,
        PublishedDate= request.PublishedDate,
        UrlHandle= request.UrlHandle,
        Author = request.Author,
        IsVisible = request.IsVisible,
        Categories = new List<Category>(),
      };

      foreach (var categoryGuid in request.Categories)
      {
        var existingCategory = await categoryRepository.GetByIdAsync(categoryGuid);

        if(existingCategory != null)
        {
          //creating a list of Categories for the new blogPost 
          blog.Categories.Add(existingCategory);
        }
      }

      await blogPostsRepository.CreateAsync(blog);

      //convert Domain back to Dto

      var response = new BlogPostDto
      {

        Title = blog.Title,
        ShortDescription = blog.ShortDescription,
        Content = blog.Content,
        FeaturedImageUrl = blog.FeaturedImageUrl,
        PublishedDate= blog.PublishedDate,
        UrlHandle= request.UrlHandle,
        Author = blog.Author,
        IsVisible = blog.IsVisible,
        Categories = (List<CategoryDto>)blog.Categories.Select(x => new CategoryDto
        {
          Id= x.Id,
          Name= x.Name,
          UrlHandle= x.UrlHandle,
        }).ToList(),
      };

      return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> getAllBlogPosts()
    {
      var posts = await blogPostsRepository.GetAllAsync();

      var response = posts.Select(post => new BlogPostDto
      {
        Id = post.Id,
        Title = post.Title,
        ShortDescription = post.ShortDescription,
        Content = post.Content,
        FeaturedImageUrl = post.FeaturedImageUrl,
        PublishedDate = post.PublishedDate,
        UrlHandle = post.UrlHandle,
        Author = post.Author,
        IsVisible = post.IsVisible,
        Categories = post.Categories.Select(x => new CategoryDto
        {
          Id= x.Id,
          Name= x.Name,
          UrlHandle= x.UrlHandle,
        }).ToList(),
      }).ToList();
      return Ok(response);
    }

    [HttpGet]
    [Route("{urlHandle}")]
    public async Task<IActionResult> getBlogPostByUrl([FromRoute] string urlHandle)
    {
      var blogDetails = await blogPostsRepository.GetByUrlAsync(urlHandle);

      if (blogDetails == null)
      {
        return NotFound();
      }

      var response = new BlogPostDto
      {
        Title = blogDetails.Title,
        ShortDescription = blogDetails.ShortDescription,
        Content = blogDetails.Content,
        FeaturedImageUrl = blogDetails.FeaturedImageUrl,
        PublishedDate = blogDetails.PublishedDate,
        UrlHandle = blogDetails.UrlHandle,
        Author = blogDetails.Author,
        IsVisible = blogDetails.IsVisible,
        Categories = blogDetails.Categories.Select(x => new CategoryDto
        {
          Id = x.Id,
          Name = x.Name,
          UrlHandle = x.UrlHandle,
        }).ToList()
      };

      return Ok(response);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> deleteBlogPost([FromRoute] Guid id)
    {
      var category = await blogPostsRepository.DeleteAsync(id);

      if(category == null)
      {
        return NotFound();
      }

      var response = new BlogPostDto
      {
        Title = category.Title,
        ShortDescription = category.ShortDescription,
        Content = category.Content,
        FeaturedImageUrl = category.FeaturedImageUrl,
        PublishedDate = category.PublishedDate,
        UrlHandle = category.UrlHandle,
        Author = category.Author,
        IsVisible = category.IsVisible
      };

      return Ok(response);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetBlogById([FromRoute] Guid id)
    {
      var eBlog = await blogPostsRepository.GetByIdAsync(id);

      if(eBlog == null)
      {
        return NotFound();
      }

      var response = new BlogPostDto
      {
        Id = eBlog.Id,
        Title = eBlog.Title,
        ShortDescription = eBlog.ShortDescription,
        Content = eBlog.Content,
        FeaturedImageUrl = eBlog.FeaturedImageUrl,
        PublishedDate = eBlog.PublishedDate,
        UrlHandle = eBlog.UrlHandle,
        Author = eBlog.Author,
        IsVisible = eBlog.IsVisible,
        Categories = eBlog.Categories.Select(x => new CategoryDto
        {
          Id = x.Id,
          Name = x.Name,
          UrlHandle = x.UrlHandle,
        }).ToList()
      };

      return Ok(response);
    }

    [HttpPut]
    [Route("{id:Guid}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> UpdateCategories([FromRoute] Guid id, UpdateBlogPostRequestDto request)
    {
      var blogPost = new BlogPost
      {
        Id = id,
        Title = request.Title,
        ShortDescription = request.ShortDescription,
        Content = request.Content,
        FeaturedImageUrl = request.FeaturedImageUrl,
        PublishedDate = request.PublishedDate,
        UrlHandle = request.UrlHandle,
        Author = request.Author,
        IsVisible = request.IsVisible,
        Categories = new List<Category>()
      };

      foreach(var categoryGuid in request.Categories)
      {
        var eCategory = await categoryRepository.GetByIdAsync(categoryGuid);

        if (eCategory != null)
        {
          blogPost.Categories.Add(eCategory);
        }
      }

      var blog = await blogPostsRepository.UpdateAsync(blogPost);

      if (blog == null)
      {
        return NotFound();
      }

      var response = new BlogPostDto
      {
        Id = blogPost.Id,
        Title = blogPost.Title,
        ShortDescription = blogPost.ShortDescription,
        Content = blogPost.Content,
        FeaturedImageUrl= blogPost.FeaturedImageUrl,
        PublishedDate = blogPost.PublishedDate,
        UrlHandle = blogPost.UrlHandle,
        Author = blogPost.Author,
        IsVisible = blogPost.IsVisible,
        Categories = blogPost.Categories.Select(x => new CategoryDto
        {
          Id = x.Id,
          Name = x.Name,
          UrlHandle = x.UrlHandle
        }).ToList()
      };

      return Ok(response);
    }
  }
}
