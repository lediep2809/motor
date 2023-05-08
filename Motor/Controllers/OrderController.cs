
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
    public class OrderController : ControllerBase
    {
        R4rContext _context = new R4rContext();

        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly CartService _cartService;
        private readonly UserService _userService;
        private readonly OrderService _orderService;
        public OrderController(IConfiguration configuration, UserService userService, CartService cartService, OrderService orderService)
        {
            _configuration = configuration;
            _userService = userService;
            _cartService = cartService;
            _orderService = orderService;
        }

        [HttpGet("newOrder")]
        [Authorize]
        public async Task<ActionResult> newOrder()
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var alert = _orderService.newOrder(email);
            return Ok(alert);
        }

        [HttpPost("addToCart")]
        [Authorize]
        public async Task<ActionResult> addToCart(List<CartOrder> carts)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var alert = _cartService.saveCartShop(carts, email);
            if (alert != null)
            {
                return Ok(alert);
            }
            return BadRequest("Thêm mới thất bại");
        }

        [HttpPost("delaValCart")]
        [Authorize]
        public async Task<ActionResult> delaValCart(string id)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var alert = _cartService.delValCart(id, email);
            if (alert != null)
            {
                return Ok(alert);
            }
            return BadRequest("del thất bại");
        }
    }
}
