using Shop.Data.Interfaces;
using Shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data.Mocks
{
    public class MockCategory : ICarsCategory
    {
        IEnumerable<Category> ICarsCategory.AllCategories
        { get 
            { 
                return new List<Category> { 
                                               new Category { CategoryName = "Электромобили", Desc = "Современный вид транспорта" },
                                               new Category{ CategoryName = "Классический автомобиль", Desc = "Машины с ДВС"} 
                                          }; 
            } 
        }
    }
}
