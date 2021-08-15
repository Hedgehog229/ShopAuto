using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Data.Models;

namespace Shop.Data
{
    public class AppDBcontent : Microsoft.EntityFrameworkCore.DbContext
    {
        public AppDBcontent(DbContextOptions<AppDBcontent> options) : base(options)
        {

        }

        public DbSet<Car> Car { get; set; }
        public DbSet<Category> Category { get; set;}
        public DbSet<ShopCartItem> ShopCartItems { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<CarDetails> CarInformation { get; set; }

    }
}
