﻿using web_motor.Models;
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

namespace Motor.Services
{
    public class CategoryService
    {

        private readonly R4rContext _Db;

        public CategoryService(R4rContext Db)
        {
            _Db = Db;
        }

        public Models.Type getbycode(string data)
        {
            try
            {
                Models.Type category = _Db.Types.Where(e => e.Code == data.Trim()).FirstOrDefault();

                return category;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Models.Type saveCategory(Models.Type data)
        {
            try
            {
                _Db.Types.Add(data);
                _Db.SaveChanges();

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Models.Type updateCategory(Models.Type data)
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
