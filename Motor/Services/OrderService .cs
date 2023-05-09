using web_motor.Models;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Motor.Constant;
using Motor.Models;
using Motor.ApiModel;

namespace Motor.Services
{   
    public class OrderService
    {

        private readonly R4rContext _Db;
        public OrderService(R4rContext Db)
        {
            _Db = Db;
        }

        public List<Order> getOrderDetial(string data)
        {
            try
            {
                var order = _Db.Orders.Where(e => e.orderId.Equals(data)).ToList();

                return order;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string newOrder(List<CartOrder> carts ,string createBy)
        {
            try
            {

                Order order = new Order();
                Guid myuuid = Guid.NewGuid();
                order.orderId = myuuid.ToString();
                order.Createdby = createBy;
                order.Status = 0;
                order.Createddate = new DateTime();

                foreach (var x in carts)
                {
                    int price = 0;
                    string? test = _Db.Motors
                        .Where(e => e.Id.Equals(x.motorId))
                        .Select(e => e.Price).SingleOrDefault();
                    if (test != null)
                    {
                        price = int.Parse(test);
                    }

                    OrderDetail orderDetail = new OrderDetail();

                    orderDetail.motorId = x.motorId;
                    orderDetail.price = price.ToString();
                    orderDetail.Quantity = x.Quantity;
                    orderDetail.orderId = order.orderId;

                    _Db.OrderDetials.Add(orderDetail);
                }


                _Db.Orders.Add(order);

                _Db.SaveChanges();

                return "Thêm mới thành công";
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string delValCart(string id, string createBy)
        {
            try
            {
                var CartItems = _Db.CartItems.Where(e => e.createBy.Equals(createBy) &&
                e.CartId.Equals(id)).FirstOrDefault();
                
                if(CartItems != null)
                {
                    _Db.CartItems.RemoveRange(CartItems);
                    _Db.SaveChanges();
                    return "ok";
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
