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
    public class CartService
    {

        private readonly R4rContext _Db;
        public CartService(R4rContext Db)
        {
            _Db = Db;
        }

        public List<listCart> getCartShop(string data)
        {
            try
            {
/*                var CartItems = _Db.CartItems.Where(e => e.createBy.Equals(data)).ToList();
*/
                var item = (
                from ai in _Db.CartItems

                join al in _Db.Motors on ai.motorId equals al.Id
                where (ai.createBy.Equals(data))
                select new listCart
                {
                    CartId = ai.CartId,
                    motorId = ai.motorId,
                    Quantity = ai.Quantity,
                    DateCreated = ai.DateCreated,
                    createBy = ai.createBy,
                    totalprice  = ai.totalprice,
                    price = al.Price,
                    priceSale = al.salePrice,
                    motorName=al.Name,
                    motorImg = _Db.ImgMotors.Where(e => e.idMotor.Equals(ai.motorId)).Select(e => e.Imgbase64).FirstOrDefault(),
                }).ToList();


                return item;
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

                foreach (var x in data)
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

                    var item = _Db.CartItems.Where(e => e.createBy.Equals(createBy) && e.motorId.Equals(x.motorId)).FirstOrDefault();
                    if (item != null)
                    {
                        var qauntity = x.Quantity + item.Quantity;
                        item.Quantity = qauntity;
                        item.totalprice = (item.Quantity * price).ToString();
                        _Db.CartItems.Update(item);
                    }
                    else
                    {
                        CartItem cartItem = new CartItem();
                        Guid myuuid = Guid.NewGuid();
                        cartItem.CartId = myuuid.ToString();
                        cartItem.createBy = createBy;
                        cartItem.motorId = x.motorId;
                        cartItem.Quantity = x.Quantity;
                        cartItem.totalprice = (x.Quantity * price).ToString();
                        cartItem.DateCreated = DateTime.Today;
                        _Db.CartItems.Add(cartItem);
                    }
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

        public string updateCartShop(cartUpdate data, string createBy)
        {
            try
            {
                int price = 0;
                string? test = _Db.Motors
                    .Where(e => e.Id.Equals(data.motorId))
                    .Select(e => e.Price).SingleOrDefault();
                string? priceSale = _Db.Motors
                       .Where(e => e.Id.Equals(data.motorId))
                       .Select(e => e.salePrice).SingleOrDefault();
                if (priceSale != null)
                {
                    price = int.Parse(priceSale);
                }
                else if (priceSale == null || test != null)
                {
                    price = int.Parse(test);
                }

                var item = _Db.CartItems.Where(e => e.createBy.Equals(createBy) && e.motorId.Equals(data.motorId)).FirstOrDefault();
                if (item != null)
                {
                    item.Quantity = data.Quantity;
                    item.totalprice = (item.Quantity * price).ToString();
                    _Db.CartItems.Update(item);
                    _Db.SaveChanges();
                    return "update thành công";
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
