using Microsoft.EntityFrameworkCore;
using Motor.Models;

namespace Motor.Services
{
    public class MotorService
    {

        private readonly R4rContext _Db;

        public MotorService(R4rContext Db)
        {
            _Db = Db;
        }

        public List<getAllMotor> GetAll(Paging paging)
        {
            int pageNum = paging.PageNumber <=0 ? 1 : paging.PageNumber;
            int pageSize =  paging.PageSize <= 0 ? 10 : paging.PageSize;
            var search =paging.SearchQuery.ToUpper().Trim();
            var price = paging.Price.ToLower().Trim();
            var type = paging.type.Trim();
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

            var test = _Db.Motors
                    .FromSqlRaw($"select * from motor as u where ( '{price}' = '' or TO_NUMBER(u.price,'9999999999') between '{to}' and '{from}')")
                    .Where(p => (p.Name.ToUpper().Trim().Contains(search))
                        && (type == "" || p.Type.Equals(type))
                        && (status =="" || p.Status.Equals(s)) )
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(s => s.Status)
                    .ToList();

            List<getAllMotor> allRooms = new List<getAllMotor>();

            foreach (var room in test)
            {
                getAllMotor allRoom = new getAllMotor();
                allRoom.motor = room;

                var imgRooms = _Db.ImgMotors
                .Where(m => m.idMotor.Equals(room.Id))
                .Select(u => u.Imgbase64)
                .ToList();

                allRoom.ImgMotor = imgRooms ;
                var total = _Db.ImgMotors.Count();
                allRoom.total = _Db.Motors.Count();
                allRooms.Add(allRoom);
            }
            

            return allRooms;
        }



        public List<getAllMotor> getRoomsByUser(Paging paging,string email)
        {
            int pageNum = paging.PageNumber <= 0 ? 1 : paging.PageNumber;
            int pageSize = paging.PageSize <= 0 ? 10 : paging.PageSize;
            var search = paging.SearchQuery.ToUpper().Trim();
            var price = paging.Price.ToLower().Trim();
            var type = paging.type.Trim();
            var status = paging.status;
            int s = 0;

            Int32.TryParse(status, out s);

            int? va = null;
            var to = 0;
            var from = 0;

            if (price.Equals("first"))
            {
                to = 10000000;
                from = 50000000;
            }
            else if (price.Equals("second"))
            {
                to = 50000000;
                from = 100000000;
            }
            else if (price.Equals("third"))
            {
                to = 100000000;
                from = 200000000;
            }

            var test = _Db.Motors
                    .FromSqlRaw($"select * from motor as u where ( '{price}' = '' or TO_NUMBER(u.price,'9999999999') between '{to}' and '{from}')")
                    .Where(p => (p.Name.ToUpper().Trim().Contains(search))
                        && (type == "" || p.Type.Equals(type))
                        && (status == "" || p.Status.Equals(s))
                        && (email == "" || p.Createdby.Equals(email)))
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(s => s.Createdby)
                    .ToList();

            List<getAllMotor> allRooms = new List<getAllMotor>();

            foreach (var room in test)
            {
                getAllMotor allRoom = new getAllMotor();
                allRoom.motor = room;

                var imgRooms = _Db.ImgMotors
                 .Where(m => m.idMotor.Equals(room.Id))
                 .Select(u => u.Imgbase64)
                 .ToList();

                allRoom.ImgMotor = imgRooms;

                var total = _Db.ImgMotors.Count();
                allRoom.total = _Db.Motors.Count();
                allRooms.Add(allRoom);
            }


            return allRooms;
        }

        public MotorModel saveRoom(MotorModel room, string[] img)
        {
            try
            {
                foreach (var i in img)
                {
                    imgMotor ro = new imgMotor();
                    TypeMotor data = new TypeMotor();
                    Guid myuuid = Guid.NewGuid();
                    ro.Id = myuuid.ToString();
                    ro.idMotor = room.Id;
                    ro.Imgbase64 = i;
                    _Db.ImgMotors.Add(ro);
                }

                _Db.Motors.Add(room);
                _Db.SaveChanges();

                return room;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public MotorModel updateRoom(MotorModel room, string[] img)
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
                    TypeMotor data = new TypeMotor();
                    Guid myuuid = Guid.NewGuid();
                    ro.Id = myuuid.ToString();
                    ro.idMotor = room.Id;
                    ro.Imgbase64 = i;
                    _Db.ImgMotors.Add(ro);
                }
                
                _Db.Motors.Update(room);
                _Db.SaveChanges();

                return room;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public MotorModel updateRoom(MotorModel room)
        {
            try
            {
                _Db.Motors.Update(room);
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
