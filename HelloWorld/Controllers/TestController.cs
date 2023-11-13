using Bogus;
using Bogus.DataSets;
using HelloWorld.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly BloggingContext _bloggingContext;

        public TestController(ILogger<TestController> logger, BloggingContext bloggingContext)
        {
            _logger = logger;
            _bloggingContext = bloggingContext;
        }

        [HttpPost("create-random-blog")]
        public IActionResult CreteBlog(int count)
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
            return Ok(_bloggingContext.SaveChanges());
        }


        [HttpGet("blog")]
        public IActionResult GetBlogById(int id)
        {
            return Ok(_bloggingContext.Blogs.FirstOrDefault(x => x.BlogId == id));
        }


        [HttpGet("blogs")]
        public IEnumerable<Blog> GetBlogsById()
        {
            var result = _bloggingContext.Blogs.ToList();
            return result;
        }
    }
}
