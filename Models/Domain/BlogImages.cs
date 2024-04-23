namespace CodePulse.Models.Domain
{
  public class BlogImages
  {
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public string FileExtention { get; set; }
    public DateTime DateCreated { get; set; }
  }
}
