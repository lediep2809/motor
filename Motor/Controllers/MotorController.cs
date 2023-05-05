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

        private readonly IConfiguration _configuration;
        private readonly RoomsService _roomsService;
        private readonly CategoryService _categoryService;
        private readonly ILogger _logger;


        public MotorController(IConfiguration configuration, RoomsService roomsService)
        {
            _configuration = configuration;
            _roomsService = roomsService;
        }


        [HttpPost("searchRooms")]
        public async Task<ActionResult> GetAll(Paging paging)
        {
            return Ok(_roomsService.GetAll(paging));
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

            return Ok(_roomsService.getRoomsByUser(paging,email));
        }

        [HttpPost("getType")]
        public async Task<ActionResult> GetCategory()
        {
            var data = await _context.Types.ToListAsync();
            return Ok(data);
        }

        [HttpPost("newCategory")]
        [Authorize(Roles = DefaultString.ROLE_1)]
        public async Task<ActionResult> newCategory(NewCategory category)
        {
            TypeMotor data = new TypeMotor();
            Guid myuuid = Guid.NewGuid();
            data.Id = myuuid.ToString();
            data.Code = category.Code;
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
            var check =  _categoryService.getbycode(category.Code);

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

        [HttpPost("deleteRoom")]
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

        [HttpPost("editRooms")]
        [Authorize]
        public async Task<ActionResult> editRooms(EditRoom room)
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
            roomCheck.Type = room.Category;
            roomCheck.Description = room.Description;
            roomCheck.Price = room.Price;
            roomCheck.Status = room.Status;
            var util = string.Join(",", room.utilities);
            var roomEdit = _roomsService.updateRoom(roomCheck, room.imgRoom);
            if (roomEdit == null)
            {
                return BadRequest("Không tìm thấy ");
            }

            return Ok(roomEdit);
        }


        [HttpPost("saveNewRoom")]
        [Authorize]
        public async Task<ActionResult> saveRoom(SaveNewRoom newRoom)
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
            room.Type = newRoom.Category;
            room.Description = newRoom.Description;
            room.Price = newRoom.Price;
            room.Createdby = email;

            var util = string.Join(",", newRoom.utilities);
            room.Createddate = new DateTime();
            room.Status = 0;

            var roomNew = _roomsService.saveRoom(room, newRoom.imgRoom);
            if (roomNew == null)
            {
                return BadRequest("Tạo mới thất bại");
            }
            return Ok(roomNew);
        }

    }
}
