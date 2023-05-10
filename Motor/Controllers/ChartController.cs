
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
    public class ChartController : ControllerBase
    {
        R4rContext _context = new R4rContext();

        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly CartService _cartService;
        private readonly UserService _userService;
        private readonly OrderService _orderService;
        public ChartController(IConfiguration configuration, UserService userService, CartService cartService, OrderService orderService)
        {
            _configuration = configuration;
            _userService = userService;
            _cartService = cartService;
            _orderService = orderService;
        }

        [HttpPost("getChart")]
        [Authorize(Roles = DefaultString.ROLE_1)]
        public async Task<ActionResult> getCart(ChartOrder chart)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var alert = _orderService.getOrderChart(chart,email);
            return Ok(alert);
        }
    }
}
