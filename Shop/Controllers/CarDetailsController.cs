using Microsoft.AspNetCore.Mvc;
using Shop.Data.Interfaces;
using Shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class CarDetailsController : Microsoft.AspNetCore.Mvc.Controller
    {        
        private readonly CarDetails _CarDetails;
        private readonly IAllCars _iallCars;

        public CarDetailsController(CarDetails carDetails, IAllCars iAllCars)
        {            
            _CarDetails = carDetails;
            _iallCars = iAllCars;
        }

        public ViewResult CarDetails(int CarId)
        {
            var car = new CarDetails();

            foreach (Car c in _iallCars.Cars) 
            {
                if (c.Id == CarId) 
                {
                    car.CurentCar = c;                   
                    car.Id = CarId;
                }
            }           
            
            return View(car);
        }
    }
}
