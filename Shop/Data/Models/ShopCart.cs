using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data.Models
{
    public class ShopCart
    {
        private readonly AppDBcontent appDBContent;
        //public Car Car { get; set; }
        public string ShopCartId { get; set; }
        public List<ShopCartItem> ListShopItems { get; set; }
        public ShopCart(AppDBcontent appDBContent)
        {
            this.appDBContent = appDBContent;

        }
        public static ShopCart GetCart(IServiceProvider services) 
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session; // создание новой сессии (объект для работы с сессиями)
            var context = services.GetService<AppDBcontent>();
            string shopCartId = session.GetString("CartId")??Guid.NewGuid().ToString();
            session.SetString("CartId", shopCartId);

            return new ShopCart(context) { ShopCartId = shopCartId };
        }
        public void AddToCar(Car car, int amout=0) 
        {
            //Car = car;
            appDBContent.ShopCartItems.Add(new ShopCartItem() { ShopCartId = ShopCartId, Car = car, Price = car.Price });
            appDBContent.SaveChanges();                        
        }
        public List<ShopCartItem> GetShopItems() 
        {
            List<ShopCartItem> ret = new List<ShopCartItem>();
            foreach (ShopCartItem c in appDBContent.ShopCartItems) 
            {
                if (c.ShopCartId == ShopCartId) 
                {                    
                    ret.Add(c);
                }
            }

            return ret;//appDBContent.ShopCartItems.Where(c => c.ShopCartId == ShopCartId).Include(s => s.Car).ToList();
            
        }
    }
}
