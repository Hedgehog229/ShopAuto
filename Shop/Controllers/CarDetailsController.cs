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
        private readonly IAllCarDetails _IAllCarDetails;
        private readonly CarDetails _CarDetails;

        public CarDetailsController(IAllCarDetails iAllCarDetails, CarDetails carDetails)
        {
            _IAllCarDetails = iAllCarDetails;
            _CarDetails = carDetails;
        }

        public ViewResult CarDetails()
        {            
            return View();
        }
    }
}
