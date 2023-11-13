using Bogus;
using Bogus.DataSets;
using HelloWorld.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly ILogger<BlogController> _logger;
        private readonly BloggingContext _bloggingContext;

        public BlogController(ILogger<BlogController> logger, BloggingContext bloggingContext)
        {
            _logger = logger;
            _bloggingContext = bloggingContext;
        }

        [HttpPost("create-random-blog")]
        public async Task<IActionResult> CreateBlog()
        {
            try
            {
                var postFaker = new Faker<Post>()
                    .RuleFor(p => p.PostId, f => f.IndexFaker + 1) // Sequential PostId
                    .RuleFor(p => p.Title, f => f.Lorem.Sentence()) // Random sentence for title
                    .RuleFor(p => p.Content, f => f.Lorem.Paragraph()); // Random paragraph for content
                var faker = new Faker<Blog>()
                    .RuleFor(b => b.BlogId, f => f.Random.Number(1, 1000)) // Random number between 1 and 1000
                    .RuleFor(b => b.Url, f => f.Internet.Url()) // Random URL
                    .RuleFor(b => b.Rating, f => f.Random.Number(1, 5)) // Random rating between 1 and 5
                    .RuleFor(b => b.Posts, f => postFaker.GenerateBetween(1, 5)); // Generate 1 to 5 posts

                _bloggingContext.Blogs.Add(faker);
                await _bloggingContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception and return an appropriate error response
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("blog")]
        public async Task<IActionResult> GetBlogById(int id)
        {
            try
            {
                var blog = await _bloggingContext.Blogs.Include(x=> x.Posts).FirstOrDefaultAsync(x => x.BlogId == id);
                return Ok(blog);
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("blogs")]
        public async Task<IEnumerable<Blog>> GetAllBlogs()
        {
            var blogs = await _bloggingContext.Blogs.ToListAsync();
            return blogs;
        }
    }
}
