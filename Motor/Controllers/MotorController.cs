using Motor.ApiModel;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Motor.Services;
using Motor.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Motor.Models;

namespace AuthenticationAndAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorController : ControllerBase
    {
        R4rContext _context = new R4rContext();
        /*CategoryService _categoryService = new CategoryService();*/

        private readonly IConfiguration _configuration;
        private readonly MotorService _motorService;
        private readonly CategoryService _categoryService;
        private readonly UserService _userService;
        private readonly ILogger _logger;


        public MotorController(IConfiguration configuration, MotorService motorService, CategoryService categoryService, UserService userService)
        {
            _configuration = configuration;
            _motorService = motorService;
            _categoryService = categoryService;
            _userService = userService;
        }


        [HttpPost("searchMotor")]
        public async Task<ActionResult> GetAll(Paging paging)
        {
            return Ok(_motorService.GetAll(paging));
        }

        [HttpPost("getRoomsByUser")]
        [Authorize]
        public async Task<ActionResult> getRoomsByUser(Paging paging)
        {
            var re = Request;
            var headers = re.Headers;
            string tokenString = headers.Authorization;
            var jwtEncodedString = tokenString.Substring(7); // trim 'Bearer ' from the start since its just a prefix for the token string
            var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);
            var email = token.Claims.First(c => c.Type == "Email").Value;

            return Ok(_motorService.getRoomsByUser(paging,email));
        }

        [HttpPost("getTypes")]
        public async Task<ActionResult> GetCategory()
        {
            var data = await _context.Types.ToListAsync();
            return Ok(data);
        }

        [HttpPost("newTypes")]
        [Authorize(Roles = DefaultString.ROLE_1)]
        public async Task<ActionResult> newCategory(NewCategory category)
        {
            var check = _categoryService.getbycode(category.Code);

            if (check != null)
            {
                return BadRequest("Loại đã tồn tại");
            }

            TypeMotor data = new TypeMotor();
            Guid myuuid = Guid.NewGuid();
            data.Id = myuuid.ToString();
            data.Code = category.Code.Trim();
            data.Name = category.Name;
            data.Status = "1";

            _context.Types.Add(data);
            _context.SaveChanges();

          /*  _categoryService.saveCategory(data);*/
            return Ok(category);
        }

        [HttpPost("editTypes")]
        [Authorize(Roles = DefaultString.ROLE_1)]
        public async Task<ActionResult> editCategory(editCategory category)
        {
            var check =  _categoryService.getbycode(category.Code.Trim().ToLower());

            if (check == null )
            {
                return BadRequest("Không tìm thấy Loại");
            }

            check.Name = category.Name;
            check.Status = "1".Equals(category.Status)? "1" : "0";
            try
            {
                _context.Types.Update(check);
                _context.SaveChanges();

                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest("Không tìm thấy Loại");
            }
            
        }

        [HttpPost("deleteTypes")]
        [Authorize(Roles = DefaultString.ROLE_1)]
        public async Task<ActionResult> deleteTypes(deleteTypes category)
        {
            var check = _categoryService.getbycode(category.Code.Trim().ToLower());

            if (check == null)
            {
                return BadRequest("Không tìm thấy ");
            }

            _context.Types.Remove(check);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("deleteMotor")]
        [Authorize(Roles = DefaultString.ROLE_1)]
        public async Task<ActionResult> deleteRoom(activeRoom room)
        {
            var roomCheck = _context.Motors.Where(e => e.Id == room.Id).FirstOrDefault();

            if (roomCheck == null)
            {
                return BadRequest("Không tìm thấy ");
            }

            _context.Motors.Remove(roomCheck);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("editMotor")]
        [Authorize(Roles = DefaultString.ROLE_1)]
        public async Task<ActionResult> editRooms(EditMotor room)
        {
            var roomCheck = _context.Motors.Where(e => e.Id == room.Id).FirstOrDefault();

            var re = Request;
            var headers = re.Headers;
            string tokenString = headers.Authorization;
            var jwtEncodedString = tokenString.Substring(7); // trim 'Bearer ' from the start since its just a prefix for the token string
            var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);
            var emailEdit = token.Claims.First(c => c.Type == "Email").Value;

            if (roomCheck == null && emailEdit.Equals(roomCheck.Createdby))
            {
                return BadRequest("Không tìm thấy ");
            }
            roomCheck.Name = room.Name;
            roomCheck.Type = room.Type;
            roomCheck.Description = room.Description;
            roomCheck.Price = room.Price;
            roomCheck.Status = room.Status;
            roomCheck.salePrice = room.salePrice == null || room.salePrice.Equals("undefined") ? null
                                : room.salePrice;
            roomCheck.farmous= room.farmous;
            var roomEdit = _motorService.updateRoom(roomCheck, room.imgMotor);
            if (roomEdit == null)
            {
                return BadRequest("Không tìm thấy ");
            }

            return Ok(roomEdit);
        }


        [HttpPost("saveNewMotor")]
        [Authorize]
        public async Task<ActionResult> saveMotor(SaveNewMotor newRoom)
        {
            var re = Request;
            var headers = re.Headers;
            string tokenString = headers.Authorization;
            var jwtEncodedString = tokenString.Substring(7); // trim 'Bearer ' from the start since its just a prefix for the token string
            var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);
            var email = token.Claims.First(c => c.Type == "Email").Value;

            MotorModel room = new MotorModel();
            Guid myuuid = Guid.NewGuid();
            room.Id = myuuid.ToString();
            room.Name = newRoom.Name;
            room.Type = newRoom.Type;
            room.Description = newRoom.Description;
            room.Price = newRoom.Price;
            room.Createdby = email;
            room.salePrice = newRoom.salePrice == null || newRoom.salePrice.Equals("undefined")? null
                : newRoom.salePrice;
            room.farmous = newRoom.farmous;
            room.Createddate = DateTime.Today;
            room.Status = 0;

            var roomNew = _motorService.saveRoom(room, newRoom.imgMotor);
            if (roomNew == null)
            {
                return BadRequest("Tạo mới thất bại");
            }
            return Ok(roomNew);
        }


        [HttpPost("newCmt")]
        [Authorize]
        public async Task<ActionResult> newCmt(newCmt newCmt)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var cmt = _motorService.addCmt(newCmt, email);
            if (cmt == null)
            {
                return BadRequest("Tạo mới thất bại");
            }
            return Ok(cmt);
        }

        [HttpPost("updateCmt")]
        [Authorize]
        public async Task<ActionResult> updateCmt(updateCmt updateCmt)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var cmt = _motorService.updateCmt(updateCmt, email);
            if (cmt == null)
            {
                return BadRequest("update thất bại");
            }
            return Ok(cmt);
        }

        [HttpPost("delCmt")]
        [Authorize]
        public async Task<ActionResult> delCmt(delCmt delCmt)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var cmt = _motorService.delCmt(delCmt.cmtId, email);
            if (cmt == null)
            {
                return BadRequest("xóa thất bại");
            }
            return Ok(cmt);
        }

        [HttpGet("getCmtMotor")]
        public async Task<ActionResult> getCmtMotor(string motortid)
        {
            var cmt = _motorService.getCmtInMotor(motortid);
            if (cmt == null)
            {
                return BadRequest("có lỗi xảy ra");
            }
            return Ok(cmt);
        }

    }
}
