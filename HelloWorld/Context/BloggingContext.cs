using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Context
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("blog");
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }
    }

    public class Blog
    {
        [Key]
        public long BlogId { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
        public virtual IEnumerable<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public long BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }
}
