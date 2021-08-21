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

        public IActionResult Checkout() //данная функция будет вызвана, когда мы перейдем на страничку заказа, но при этом мы еще не отправляли данны
        {
            return View();
        }

        [HttpPost] //функция, которая срабатывает только при HttpPost - т.е., когда мы заполнили поля формы и нажали подтвердить заказ, 
                   //значеие полей формы передаются в программу (asp-actions="Checkout" method="post" -- Checkout.cshtml блок <form asp-actions="Checkout" method="post" class="form-horizontal">)
        public IActionResult Checkout(Order order) //данная функция будет вызвана, когда мы заполнили поля формы, входной параметр Order соответствует @model Order Checkout.cshtml
        {
            _ShopCart.ListShopItems = _ShopCart.GetShopItems();
            if (_ShopCart.ListShopItems.Count == 0) // если товаров в корзине нет еще
            {
                ModelState.AddModelError("", "У вас дожны быть товары!"); // ключ и значение (в нашем случае ключ пустой)
            }
            else 
            {
                if (ModelState.IsValid) //вернет true только в том случае, если все поля формы, заполненные пользователем прошли проверку
                {
                    _IAllOrders.CreateOrder(order); //объект order - это тот объект, который мыполучаем от пользователя (из формы Checkout.cshtml)
                    return RedirectToAction("Complete"); //если все в порядке, то возвращаем нужную страницу редиректом (перенаправляем на нужную страницу Complete)
                }
            }
            return View(order); //если мы не попали в if (ModelState.IsValid) => return RedirectToAction("Complete"), то возвращаем форму с введенными ранее данными order
        }

        public IActionResult Complete() 
        {
            ViewBag.Message = "Заказ успешно обработан!";
            return View();
        }
    }
}
