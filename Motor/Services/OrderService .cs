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

        public List<CartItem> getCartShop(string data)
        {
            try
            {
                var CartItems = _Db.CartItems.Where(e => e.createBy.Equals(data)).ToList();

                return CartItems;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string saveCartShop(List<CartOrder> data,string createBy)
        {
            try
            {
                var CartItems = _Db.CartItems.Where(e => e.createBy.Equals(createBy)).ToList();
                _Db.CartItems.RemoveRange(CartItems);
                _Db.SaveChanges();

                foreach (var x in data)
                {
                    int price = 0;
                    string? test = _Db.Motors
                        .Where(e => e.Id.Equals(x.motorId))
                        .Select(e => e.Price).SingleOrDefault();
                    if(test != null)
                    {
                        price = int.Parse(test);
                    }

                    CartItem cartItem = new CartItem();
                    Guid myuuid = Guid.NewGuid();
                    cartItem.CartId = myuuid.ToString();
                    cartItem.createBy = createBy;
                    cartItem.motorId = x.motorId;
                    cartItem.Quantity = x.Quantity;
                    cartItem.totalprice = (x.Quantity * price).ToString();
                    cartItem.DateCreated = new DateTime();
                    _Db.CartItems .Add(cartItem);
                }
                
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
