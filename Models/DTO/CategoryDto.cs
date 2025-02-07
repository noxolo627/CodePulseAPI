namespace CodePulse.Models.DTO
{
  public class CategoryDto
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UrlHandle { get; set; }
    public List<BlogPostDto> Posts { get; set; } = new List<BlogPostDto>();

  }
}
