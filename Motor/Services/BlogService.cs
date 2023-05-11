using Motor.ApiModel;
using Motor.Models;
using System.Net.NetworkInformation;

namespace Motor.Services
{
    public class BlogService
    {
        private readonly R4rContext _Db;
        public BlogService(R4rContext Db)
        {
            _Db = Db;
        }

        public listBlogs getblog(blogPage paging)
        {
            try
            {
                int pageNum = paging.PageNumber <= 0 ? 1 : paging.PageNumber;
                int pageSize = paging.PageSize <= 0 ? 10 : paging.PageSize;
                var search = paging.SearchData.ToUpper().Trim();


                var blog = _Db.Blogs.Where(e => e.Title.ToUpper().Trim().Contains(search))
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(s => s.createdDate)
                    .ToList();

                /*                var blog = _Db.Blogs.OrderByDescending(e=>e.createdDate).ToList();*/
                listBlogs data = new listBlogs();
                data.blogs = blog;
                data.total = _Db.Blogs.Where(e => e.Title.ToUpper().Trim().Contains(search)).Count();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Blog newBlog(newBlog x, string email)
        {
            try
            {
                Blog data = new Blog();
                data.Id = Guid.NewGuid().ToString();
                data.Content=x.Content;
                data.Title = x.Title;
                data.Img = x.Img;
                data.Createdby = email;
                data.createdDate = DateTime.Now;
                data.modifyDate = DateTime.Now;
                _Db.Blogs.Add(data);
                _Db.SaveChanges();

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Blog updateBlog(updateBlog x, string email)
        {
            try
            {
                var data = _Db.Blogs.Where(e => e.Id.Equals(x.id.Trim())
                    && e.Createdby.Equals(email)).SingleOrDefault();
                if (data != null)
                {
                    data.Content = x.Content;
                    data.Title = x.Title;
                    data.Img = x.Img;
                    data.modifyDate = DateTime.Now;
                    _Db.Blogs.Update(data);
                    _Db.SaveChanges();
                    return data;
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string delBlog(string Id, string email)
        {
            try
            {
                var data = _Db.Blogs.Where(e => e.Id.Equals(Id.Trim())
                    && e.Createdby.Equals(email)).SingleOrDefault();
                if (data != null)
                {
                    _Db.Blogs.Remove(data);
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
    }
}
