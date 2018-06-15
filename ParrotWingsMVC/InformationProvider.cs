using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ParrotWingsMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParrotWingsMVC
{
    public static class InformationProvider // this static class provides useful information from database
    {
        public static string GetCurrentUserName()
        {
            string result = "";
            using (var db = new ApplicationDbContext())
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = manager.FindById(HttpContext.Current.User.Identity.GetUserId()); //getting current user by id
                if (currentUser != null)
                    result = currentUser.Name;
            }
            return result;
        }

        public static int GetCurrentUserBalance()
        {
            int result = 0;
            using (var db = new ApplicationDbContext())
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = manager.FindById(HttpContext.Current.User.Identity.GetUserId()); //getting current user by id
                if (currentUser != null)
                    result = currentUser.Balance;
            }
            return result;
        }

        public static string GetCurrentUserId()
        {
            string result = "";
            using (var db = new ApplicationDbContext())
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = manager.FindById(HttpContext.Current.User.Identity.GetUserId()); //getting current user by id
                if (currentUser != null)
                    result = currentUser.Id;
            }
            return result;
        }
    }
}