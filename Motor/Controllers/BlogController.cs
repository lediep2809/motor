
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Motor.Models;
using Motor.Constant;
using web_motor.Models;
using Motor.Services;
using Motor.ApiModel;
using System.Reflection.Metadata;

namespace AuthenticationAndAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        R4rContext _context = new R4rContext();

        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly BlogService _blogService;
        private readonly UserService _userService;
        public BlogController(IConfiguration configuration, UserService userService,BlogService blogService)
        {
            _configuration = configuration;
            _userService = userService;
            _blogService = blogService;
        }
        [HttpPost("newBlog")]
        [Authorize]
        public async Task<ActionResult> newBlog(newBlog data)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var val = _blogService.newBlog(data, email);
            if (val == null)
            {
                return BadRequest("Tạo mới thất bại");
            }
            return Ok(val);
        }

        [HttpPost("updateBlog")]
        [Authorize]
        public async Task<ActionResult> updateBlog(updateBlog update)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var val = _blogService.updateBlog(update, email);
            if (val == null)
            {
                return BadRequest("update thất bại");
            }
            return Ok(val);
        }

        [HttpPost("delBlog")]
        [Authorize]
        public async Task<ActionResult> delBlog(delCart del)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var cmt = _blogService.delBlog(del.Id, email);
            if (cmt == null)
            {
                return BadRequest("xóa thất bại");
            }
            return Ok(cmt);
        }

        [HttpPost("getBlog")]
        public async Task<ActionResult> getBlog(blogPage paging)
        {
            var data = _blogService.getblog(paging);
            if (data == null)
            {
                return BadRequest("có lỗi xảy ra");
            }
            return Ok(data);
        }

    }
}
