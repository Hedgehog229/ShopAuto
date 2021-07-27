using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data
{
    public class DBObject
    {
        public static void Initial(AppDBcontent content)
        {

            if (!content.Category.Any())
            {
                content.Category.AddRange(Categories.Select(c => c.Value));
            }

            if (!content.Car.Any())
            {
                content.AddRange
                (
                    new Car
                {
                     Name = "Tesla Model S",
                     ShortDesc = "Быстрый автомобиль",
                     LongDesc = "Красивый, быстрый и очень тихий автомобиль компании Tesla",
                     /*Img = "https://www.zr.ru/d/story/6b/804715/1439271976_01_2012_tesla_model_s_fd_1347336745.jpg",*/
                     Img = "/img/Tesla_M_S.jpg",
                     Price = 45000,
                     IsFavorite = true,
                     Available = true,
                     Category = Categories["Электромобили"]
                },
                    new Car
                {
                    Name = "Ford Fiesta",
                    ShortDesc = "Тихий и спокойный",
                    LongDesc = "Удобный автомобиль для городской жизни",
                    Img = "https://carsdo.ru/job/CarsDo/photo-gallery/ford/fiesta-1-1.jpg",
                    Price = 11000,
                    IsFavorite = false,
                    Available = false,
                    Category = Categories["Классический автомобиль"]
                },
                    new Car
                {
                    Name = "BMW M3",
                    ShortDesc = "Дерзкий и стильный",
                    LongDesc = "Удобный автомобиль для городской жизни",
                    Img = "https://carsdo.ru/job/CarsDo/photo-gallery/bmw/m3-sedan-1.jpg",
                    Price = 65000,
                    IsFavorite = true,
                    Available = true,
                    Category = Categories["Классический автомобиль"]
                },
                    new Car
                {
                    Name = "Mercedes C Class",
                    ShortDesc = "Уютный и большой",
                    LongDesc = "Удобный автомобиль для городской жизни",
                    Img = "https://carsdo.ru/job/CarsDo/photo-gallery/mercedes-benz/c-class-1.jpg",
                    Price = 40000,
                    IsFavorite = true,
                    Available = true,
                    Category = Categories["Классический автомобиль"]
                },
                    new Car
                {
                    Name = "Nissan Leaf",
                    ShortDesc = "Бесшумный и экономичный",
                    LongDesc = "Удобный автомобиль для городской жизни",
                    Img = "https://carsdo.ru/job/CarsDo/photo-gallery/nissan/leaf-1.jpg",
                    Price = 14000,
                    IsFavorite = true,
                    Available = true,
                    Category = Categories["Электромобили"]
                }
                );
            }

            content.SaveChanges();
        }

        private static Dictionary<String, Category> Category;
        public static Dictionary<String, Category> Categories 
        {
            get 
            {
                if (Category == null) 
                {
                     var List = new Category[]
                     {
                            new Category { CategoryName = "Электромобили", Desc = "Современный вид транспорта" },
                            new Category{ CategoryName = "Классический автомобиль", Desc = "Машины с ДВС"}
                     };
                    Category = new Dictionary<string, Category>();
                    foreach (Category elemebt in List)
                        Category.Add(elemebt.CategoryName, elemebt);
                }
                return Category;
            }
        }
    }
}

