using Shop.Data.Interfaces;
using Shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data.Repository
{
    public class OrdersRepository : IAllOrders
    {
        private readonly AppDBcontent _AppDBcontent;
        private readonly ShopCart _ShopCart;

        public OrdersRepository(AppDBcontent appDBcontent, ShopCart shopCart) 
        {
            _AppDBcontent = appDBcontent;
            _ShopCart = shopCart;
        }
        public void CreateOrder(Order order)
        {
            order.OrderTime = DateTime.Now;
            _AppDBcontent.Order.Add(order);
            _AppDBcontent.SaveChanges(); // сохраняем внесенные изменения, 

            var items = _ShopCart.ListShopItems;

            foreach (var el in items) 
            {
                var orderDetails = new OrderDetails() { 
                    CarId = el.Car.Id, 
                    OrderId = order.Id, 
                    Price = el.Car.Price
                };
                _AppDBcontent.OrderDetails.Add(orderDetails); // Добавляем в БД [16:46]
            }
            _AppDBcontent.SaveChanges();
        }
    }
}
