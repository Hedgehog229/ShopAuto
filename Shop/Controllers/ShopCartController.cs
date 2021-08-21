using Microsoft.AspNetCore.Mvc;
using Shop.Data.Interfaces;
using Shop.Data.Models;
using Shop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly IAllCars _carRep;
        private readonly ShopCart _shopCart;
        //private Car _car;
        public ShopCartController(IAllCars carRep, ShopCart shopCart ) 
        {
            _carRep = carRep;
            _shopCart = shopCart;
        }

        //функция возвращает определенный шаблон
        public ViewResult Index() 
        {
            var Items = _shopCart.GetShopItems();
            //Items[0].Car = _car;
            _shopCart.ListShopItems = Items;

            var obj = new ShopCartViewModels { shopCart = _shopCart};
            return View(obj);
        }

        //добавляет товары в корзину и переадресовывает на другую страничку
        public RedirectToActionResult AddToCart(int id) 
        {
            var item = _carRep.Cars.FirstOrDefault(i => i.Id == id);
            if (item != null) 
            {
                _shopCart.AddToCar(item); // вызов функции из модели
                //_car = (Car)item;
            }
            return RedirectToAction("Index"); //вызов метода public ViewResult Index() 
        }
    }
}
