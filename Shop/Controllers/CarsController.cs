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
    public class CarsController : Controller
    {
        private readonly IAllCars _allCars;
        private readonly ICarsCategory _allCategory;

        public CarsController (IAllCars iAllCars, ICarsCategory iCateg) 
        {
            _allCars = iAllCars;
            _allCategory = iCateg;
        }

        [Route("Cars/List")]
        [Route("Cars/List/{category}")] //можно добавлять больше параметров, например:  [Route("Cars/List/{category}/{id}")], но в функции должен быть этот параметр public ViewResult List(string category, int id) 
        public ViewResult List(string category) 
        {
            string _category = category;
            IEnumerable<Car> cars = null;
            string currCategory ="";

            if (string.IsNullOrEmpty(category))
            {
                cars = _allCars.Cars.OrderBy(i => i.Id);
            }
            else
            {                                        //StringComparison.OrdinalIgnoreCase - параметр для игнорирования регистра в передаваемой строке
                if (string.Equals("electro", category, StringComparison.OrdinalIgnoreCase))
                {
                    cars = _allCars.Cars.Where(i => i.Category.CategoryName.Equals("Электромобили")).OrderBy(i => i.Id);
                    currCategory = "Электромобили";
                }
                else 
                {
                    if (string.Equals("fuel", category, StringComparison.OrdinalIgnoreCase))
                    {
                        cars = _allCars.Cars.Where(i => i.Category.CategoryName.Equals("Классический автомобиль")).OrderBy(i => i.Id);
                        currCategory = "Классический автомобиль";
                    }
                }

                //currCategory = _category;               
            }

            var carObj = new CarListViewModel()
            {
                AllCars = cars,
                CurrCategory = currCategory
            };

            ViewBag.Title = "Страница с автомобилями";
            //CarListViewModel obj = new CarListViewModel();
            //obj.AllCars = _allCars.Cars;
            //obj.CurrCategory = "Автомобили";
            //return View(obj);
            return View(carObj);
        }
    }
}
