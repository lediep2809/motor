using web_motor.Models;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Xml.Linq;

namespace web_motor.Services
{
    public class RoomsService
    {

        private readonly R4rContext _Db;

        public RoomsService(R4rContext Db)
        {
            _Db = Db;
        }

        public List<getAllRoom> GetAll(Paging paging)
        {
            int pageNum = paging.PageNumber <=0 ? 1 : paging.PageNumber;
            int pageSize =  paging.PageSize <= 0 ? 10 : paging.PageSize;
            var search =paging.SearchQuery.ToUpper().Trim();
            var price = paging.Price.ToLower().Trim();
            var category = paging.Category.Trim();
            var util = string.Join(",", paging.utilities);
            var utilities = util;
            var noSex = paging.noSex.ToUpper().Trim();
            var status = paging.status;
            int s = 0;

            Int32.TryParse(status, out s);

            int? va = null;
            var to = 0;
            var from = 0;

            if (price.Equals("first"))
            {
                to = 1000000;
                from = 5000000;
            }
            else if (price.Equals("second"))
            {
                to = 6000000;
                from = 10000000;
            }
            else if (price.Equals("third"))
            {
                to = 11000000;
                from = 15000000;
            }

            var test = _Db.Rooms
                    .FromSqlRaw($"select * from motor as u where ( '{price}' = '' or TO_NUMBER(u.price,'9999999999') between '{to}' and '{from}')")
                    .Where(p => (p.Name.ToUpper().Trim().Contains(search))
                        && (category == "" || p.Type.Equals(category))
                        && (status =="" || p.Status.Equals(s)) )
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .OrderBy(s => s.Status)
                    .ToList();

            List<getAllRoom> allRooms = new List<getAllRoom>();

            foreach (var room in test)
            {
                getAllRoom allRoom = new getAllRoom();
                allRoom.room = room;

                var imgRooms = _Db.ImgMotors
                .Where(m => m.idMotor.Equals(room.Id))
                .Select(u => u.Imgbase64)
                .ToList();

                allRoom.ImgRoom = imgRooms ;
                allRooms.Add(allRoom);
            }


            return allRooms;
        }



        public List<getAllRoom> getRoomsByUser(Paging paging,string email)
        {
            int pageNum = paging.PageNumber <= 0 ? 1 : paging.PageNumber;
            int pageSize = paging.PageSize <= 0 ? 10 : paging.PageSize;
            var search = paging.SearchQuery.ToUpper().Trim();
            var price = paging.Price.ToLower().Trim();
            var category = paging.Category.Trim();
            var util = string.Join(",", paging.utilities);
            var utilities = util;
            var noSex = paging.noSex.ToUpper().Trim();
            var status = paging.status;
            int s = 0;

            Int32.TryParse(status, out s);

            int? va = null;
            var to = 0;
            var from = 0;

            if (price.Equals("first"))
            {
                to = 1000000;
                from = 5000000;
            }
            else if (price.Equals("second"))
            {
                to = 6000000;
                from = 10000000;
            }
            else if (price.Equals("third"))
            {
                to = 11000000;
                from = 15000000;
            }

            var test = _Db.Rooms
                    .FromSqlRaw($"select * from motor as u where ( '{price}' = '' or TO_NUMBER(u.price,'9999999999') between '{to}' and '{from}')")
                    .Where(p => (p.Name.ToUpper().Trim().Contains(search))
                        && (category == "" || p.Type.Equals(category))
                        && (status == "" || p.Status.Equals(s))
                        && (email == "" || p.Createdby.Equals(email)))
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .OrderBy(s => s.Createdby)
                    .ToList();

            List<getAllRoom> allRooms = new List<getAllRoom>();

            foreach (var room in test)
            {
                getAllRoom allRoom = new getAllRoom();
                allRoom.room = room;

                var imgRooms = _Db.ImgMotors
                 .Where(m => m.idMotor.Equals(room.Id))
                 .Select(u => u.Imgbase64)
                 .ToList();

                allRoom.ImgRoom = imgRooms;
                allRooms.Add(allRoom);
            }


            return allRooms;
        }

        public Motor saveRoom(Motor room, string[] img)
        {
            try
            {
                foreach (var i in img)
                {
                    imgMotor ro = new imgMotor();
                    Models.Type data = new Models.Type();
                    Guid myuuid = Guid.NewGuid();
                    ro.Id = myuuid.ToString();
                    ro.idMotor = room.Id;
                    ro.Imgbase64 = i;
                    _Db.ImgMotors.Add(ro);
                }

                _Db.Rooms.Add(room);
                _Db.SaveChanges();

                return room;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Motor updateRoom(Motor room, string[] img)
        {
            try
            {
                
                var imgRooms = _Db.ImgMotors
                    .Where(m => m.idMotor.Equals(room.Id))
                    .ToList();
                _Db.ImgMotors.RemoveRange(imgRooms);
                _Db.SaveChanges();

                foreach (var i in img)
                {
                    imgMotor ro = new imgMotor();
                    Models.Type data = new Models.Type();
                    Guid myuuid = Guid.NewGuid();
                    ro.Id = myuuid.ToString();
                    ro.idMotor = room.Id;
                    ro.Imgbase64 = i;
                    _Db.ImgMotors.Add(ro);
                }
                
                _Db.Rooms.Update(room);
                _Db.SaveChanges();

                return room;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Motor updateRoom(Motor room)
        {
            try
            {
                _Db.Rooms.Update(room);
                _Db.SaveChanges();

                return room;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
