using Microsoft.EntityFrameworkCore;
using Motor.ApiModel;
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
            var priceSale = paging.PriceSale;
            var colPrice = $"u.price";
            var sql = $"";
            if (priceSale == 1)
            {
                colPrice = $"u.sale_price";
                sql = $" and u.sale_price IS NOT NULL";
            }
            int s = 0;

            Int32.TryParse(status, out s);

            int? va = null;
            var to = 0;
            var from = 0;

            if (price.Equals("first"))
            {
                to = 1000;
                from = 5000000;
            }
            else if (price.Equals("second"))
            {
                to = 5000000;
                from = 15000000;
            }
            else if (price.Equals("third"))
            {
                to = 15000000;
                from = 30000000;
            }
            else if (price.Equals("fourth"))
            {
                to = 30000000;
                from = 999999999;
            }

            var test = _Db.Motors
                    .FromSqlRaw($"select * from motor as u where ( '{price}' = '' or TO_NUMBER(" +
                    colPrice +
                    $",'9999999999') between '{to}' and '{from}') " +
                    sql)
                    .Where(p => (p.Name.ToUpper().Trim().Contains(search))
                        && (type == "" || p.Type.Equals(type))
                        && (status =="" || p.Status.Equals(s)) )
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(s => s.Createddate)
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
                    .OrderByDescending(s => s.Createddate)
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


        public Comment addCmt(newCmt cmt,string email)
        {
            try
            {
                Comment comment = new Comment();
                comment.Id = Guid.NewGuid().ToString();
                comment.comment = cmt.comment;
                comment.motorId = cmt.motorId;
                comment.Createdby = email;
                comment.createdDate = DateTime.Now;
                comment.modifyDate = DateTime.Now;
                _Db.Comments.Add(comment);
                _Db.SaveChanges();

                return comment;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Comment updateCmt(updateCmt cmt, string email)
        {
            try
            {
                var userCmt = _Db.Comments.Where(e => e.Id.Equals(cmt.cmtId.Trim())
                    && e.Createdby.Equals(email)).SingleOrDefault();
                if(userCmt != null)
                {
                    userCmt.comment = cmt.comment;
                    userCmt.modifyDate = DateTime.Now;
                    _Db.Comments.Update(userCmt);
                    _Db.SaveChanges();
                    return userCmt;
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string delCmt(string cmtId, string email)
        {
            try
            {
                var userCmt = _Db.Comments.Where(e => e.Id.Equals(cmtId.Trim())
                    && e.Createdby.Equals(email)).SingleOrDefault();
                if (userCmt != null)
                {
                    _Db.Comments.Remove(userCmt);
                    _Db.SaveChanges();
                    return "Xóa thành công";
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Comment> getCmtInMotor(string motorId)
        {
            try
            {
                var Cmt = _Db.Comments.Where(e => e.motorId.Equals(motorId.Trim()))
                    .OrderByDescending(e=>e.createdDate).ToList();
                return Cmt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



    }
}
