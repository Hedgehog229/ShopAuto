using Microsoft.AspNetCore.Mvc;
using Shop.Data.Interfaces;
using Shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class OrderController : Microsoft.AspNetCore.Mvc.Controller
    {
        

        private readonly IAllOrders _IAllOrders;
        private readonly ShopCart _ShopCart;

        public OrderController(IAllOrders allOrders, ShopCart shopCart)
        {
            _IAllOrders = allOrders;
            _ShopCart = shopCart;
        }

        public IActionResult Checkout() 
        {
            return View();
        }
    }
}
