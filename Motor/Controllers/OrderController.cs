
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

        [HttpGet("listOrder")]
        [Authorize]
        public async Task<ActionResult> listOrder()
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var alert = _orderService.getOrder(email);
            if (alert != null)
            {
                return Ok(alert);
            }
            return BadRequest("Bạn chưa có order nào");
        }

        [HttpGet("getOrderDetail")]
        [Authorize]
        public async Task<ActionResult> getOrderDetail(string idOrder)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var alert = _orderService.getOrderDetial(idOrder, email);
            if (alert != null)
            {
                return Ok(alert);
            }
            return BadRequest("Không tìm thấy giao dịch");
        }

        [HttpPost("newOrder")]
        [Authorize]
        public async Task<ActionResult> newOrder(newOrder newOrder)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var alert = _orderService.newOrder(newOrder, email);
            if (alert != null)
            {
                return Ok(alert);
            }
            return BadRequest("Thêm mới thất bại");
        }

        [HttpPost("payOrder")]
        [Authorize(Roles = DefaultString.ROLE_1)]
        public async Task<ActionResult> payOrder(string orderId)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var alert = _orderService.payOrder(orderId, email);
            if (alert != null)
            {
                return Ok(alert);
            }
            return BadRequest("Thanh toán thất bại");
        }

        [HttpPost("cacleOrder")]
        [Authorize]
        public async Task<ActionResult> cacleOrder(string orderId)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var alert = _orderService.cacleOrder(orderId, email);
            if (alert != null)
            {
                return Ok(alert);
            }
            return BadRequest("thất bại");
        }
    }
}
