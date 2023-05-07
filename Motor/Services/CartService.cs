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

        public string saveCategory(List<CartOrder> data,string createBy)
        {
            try
            {
                foreach(var x in data)
                {

                 /*   int test = _Db.Motors
                        .Where(e => e.Id.Equals(x.motorId))
                        .Select(e => e.Price).
                        ;*/


                    CartItem cartItem = new CartItem();
                    cartItem.createBy = createBy;
                    cartItem.motorId = x.motorId;
                    cartItem.Quantity = x.Quantity;
/*                    cartItem.totalprice = (x.Quantity * test).ToString();
*/                    cartItem.DateCreated = new DateTime();
                }
                
                _Db.SaveChanges();

                return "Thêm mới thành công";
            }
            catch (Exception ex)
            {
                return "Thêm mới thất bại";
            }
        }

        public TypeMotor updateCategory(TypeMotor data)
        {
            try
            {
                _Db.Types.Update(data);
                _Db.SaveChanges();

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
