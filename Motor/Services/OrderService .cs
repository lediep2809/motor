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

        public List<Order> getOrder(string data)
        {
            try
            {
                var order = _Db.Orders.Where(e => e.Createdby.Equals(data) && !e.Status.Equals(3)).ToList();

                return order;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<detailOrder> getOrderDetial(string idOrder, string data)
        {
            try
            {
                var orderDeatail = _Db.OrderDetials.Where(e => e.orderId.Equals(idOrder)).ToList();

                var item = (
               from ai in _Db.OrderDetials

               join al in _Db.Motors on ai.motorId equals al.Id
               where (ai.orderId.Equals(idOrder))
               select new detailOrder
               {
                   motorId = ai.motorId,
                   Quantity = ai.Quantity,
                   price = al.Price,
                   motorName = al.Name,
                   motorImg = _Db.ImgMotors.Where(e => e.idMotor.Equals(ai.motorId)).Select(e => e.Imgbase64).FirstOrDefault(),
               }).ToList();

                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Order newOrder(List<CartOrder> carts ,string createBy)
        {
            try
            {

                Order order = new Order();
                Guid myuuid = Guid.NewGuid();
                order.orderId = myuuid.ToString();
                order.Createdby = createBy;
                order.Status = 0;
                order.Createddate = new DateTime();

                int totalPrice = 0;
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
                    orderDetail.Id  = Guid.NewGuid().ToString();
                    orderDetail.motorId = x.motorId;
                    orderDetail.price = price.ToString();
                    orderDetail.Quantity = x.Quantity;
                    orderDetail.orderId = order.orderId;
                    totalPrice = totalPrice + x.Quantity * price;
                    _Db.OrderDetials.Add(orderDetail);
                }

                order.totalPrice = totalPrice.ToString();
                _Db.Orders.Add(order);

                _Db.SaveChanges();

                return order;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string payOrder(string id, string createBy)
        {
            try
            {
                var payOrder = _Db.Orders.Where(e => e.Createdby.Equals(createBy) &&
                e.orderId.Equals(id)).FirstOrDefault();
                
                if(payOrder != null)
                {
                    payOrder.Status = 1;
                    _Db.Orders.Update(payOrder);
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

        public string cacleOrder(string id, string createBy)
        {
            try
            {
                var payOrder = _Db.Orders.Where(e => e.Createdby.Equals(createBy) &&
                e.orderId.Equals(id)).FirstOrDefault();

                if (payOrder != null)
                {
                    payOrder.Status = 2;
                    _Db.Orders.Update(payOrder);
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
