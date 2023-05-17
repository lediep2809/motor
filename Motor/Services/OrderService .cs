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


        public listChartOrder getOrderChart(ChartOrder paging, string created)
        {
            try
            {
                int pageNum = paging.PageNumber <= 0 ? 1 : paging.PageNumber;
                int pageSize = paging.PageSize <= 0 ? 10 : paging.PageSize;
                var status = paging.status;
                int s = 0;

                Int32.TryParse(status, out s);

                var order = _Db.Orders.Where(e => e.Status.Equals(s))
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(s => s.Createddate)
                    .ToList();
                var total = _Db.Orders.Where(e => e.Status.Equals(s)).Count();
                listChartOrder data = new listChartOrder();
                data.orders = order;
                data.total = total;

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public deatailO getOrderDetial(string idOrder, string data)
        {
            try
            {
                var order = _Db.Orders.Where(e => e.orderId.Equals(idOrder)).FirstOrDefault();

                var item = (
               from ai in _Db.OrderDetials

               join al in _Db.Motors on ai.motorId equals al.Id
               where (ai.orderId.Equals(idOrder))
               select new detailOrder
               {
                   motorId = ai.motorId,
                   Quantity = ai.Quantity,
                   priceSale = al.salePrice,
                   price = al.Price,
                   motorName = al.Name,
                   motorImg = _Db.ImgMotors.Where(e => e.idMotor.Equals(ai.motorId)).Select(e => e.Imgbase64).FirstOrDefault(),
               }).ToList();

                deatailO deatail = new deatailO();
                deatail.adress = order.address;
                deatail.detailOrder = item;
                return deatail;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Order newOrder(newOrder newOrder, string createBy)
        {
            try
            {
                
                Order order = new Order();
                Guid myuuid = Guid.NewGuid();
                order.orderId = myuuid.ToString();
                order.Createdby = createBy;
                order.Status = 0;
                order.Createddate = DateTime.Today;
                order.address = newOrder.address;
                int totalPrice = 0;
                foreach (var x in newOrder.carts)
                {
                    int price = 0;
                    string? test = _Db.Motors
                        .Where(e => e.Id.Equals(x.motorId))
                        .Select(e => e.Price).SingleOrDefault();
                    string? priceSale = _Db.Motors
                        .Where(e => e.Id.Equals(x.motorId))
                        .Select(e => e.salePrice).SingleOrDefault();
                    if (priceSale != null)
                    {
                        price = int.Parse(priceSale);
                    }
                    else if (priceSale == null || test != null)
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
                var payOrder = _Db.Orders.Where(e => e.orderId.Equals(id)).FirstOrDefault();
                
                if(payOrder != null)
                {
                    var orderDeatail = _Db.OrderDetials.Where(e => e.orderId.Equals(id)).ToList();
                    foreach(var x in orderDeatail)
                    {
                        var item = _Db.CartItems.Where(e => e.createBy.Equals(payOrder.Createdby) && e.motorId.Equals(x.motorId)).FirstOrDefault();
                        if(item != null)
                        {
                            _Db.CartItems.Remove(item);
                        }
                    }

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
