using Shop.Data.Interfaces;
using Shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data.Mocks
{
    public class CarsRepository : IAllCars
    {
        private readonly ICarsCategory _categoryCars = new MockCategory();
        IEnumerable<Car> IAllCars.Cars { get { return new List<Car> {
                new Car {
                    Name = "Tesla Model S",
                    ShortDesc = "Быстрый автомобиль",
                    LongDesc = "Красивый, быстрый и очень тихий автомобиль компании Tesla",
                    /*Img = "https://www.zr.ru/d/story/6b/804715/1439271976_01_2012_tesla_model_s_fd_1347336745.jpg",*/
                    Img = "/img/Tesla_M_S.jpg",
                    Price = 45000, 
                    IsFavorite = true,
                    Available = true, 
                    //Category = _categoryCars.AllCategories.First()
                },
                new Car{
                    Name = "Ford Fiesta",
                    ShortDesc = "Тихий и спокойный",
                    LongDesc = "Удобный автомобиль для городской жизни",
                    Img = "https://carsdo.ru/job/CarsDo/photo-gallery/ford/fiesta-1-1.jpg",
                    Price = 11000,
                    IsFavorite = false,
                    Available = false,
                    Category = _categoryCars.AllCategories.Last()
                },
                new Car{
                    Name = "BMW M3",
                    ShortDesc = "Дерзкий и стильный",
                    LongDesc = "Удобный автомобиль для городской жизни",
                    Img = "https://carsdo.ru/job/CarsDo/photo-gallery/bmw/m3-sedan-1.jpg",
                    Price = 65000,
                    IsFavorite = true,
                    Available = true,
                    Category = _categoryCars.AllCategories.Last()
                },
                new Car{
                    Name = "Mercedes C Class",
                    ShortDesc = "Уютный и большой",
                    LongDesc = "Удобный автомобиль для городской жизни",
                    Img = "https://carsdo.ru/job/CarsDo/photo-gallery/mercedes-benz/c-class-1.jpg",
                    Price = 40000,
                    IsFavorite = true,
                    Available = true,
                    Category = _categoryCars.AllCategories.Last()
                },
                new Car{
                    Name = "Nissan Leaf",
                    ShortDesc = "Бесшумный и экономичный",
                    LongDesc = "Удобный автомобиль для городской жизни",
                    Img = "https://carsdo.ru/job/CarsDo/photo-gallery/nissan/leaf-1.jpg",
                    Price = 14000,
                    IsFavorite = true,
                    Available = true,
                    Category = _categoryCars.AllCategories.First()
                }
        }; } }
        IEnumerable<Car> IAllCars.GetFavCars { get; }
        Car IAllCars.GetObjectCar(int carId)
        {
            throw new NotImplementedException();
        }
    }
}
