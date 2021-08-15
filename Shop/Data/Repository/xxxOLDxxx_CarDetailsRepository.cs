using Shop.Data.Interfaces;
using Shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data.Repository
{
    public class xxxOLDxxx_CarDetailsRepository : IAllCarDetails //класс Repository служит для сохранения информации в БД (в данном случае он нам не нужен)
    {
        private readonly AppDBcontent _AppDBcontent;
        private readonly CarDetails _CarDetails;

        public xxxOLDxxx_CarDetailsRepository(AppDBcontent appDBcontent, CarDetails carDetails ) 
        {
            _AppDBcontent = appDBcontent;
            _CarDetails = carDetails;
        }
        public void CreateCarDetails(CarDetails carDetails)
        {
            
        }
    }
}
