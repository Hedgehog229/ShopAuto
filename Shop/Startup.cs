using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shop.Data;
using Shop.Data.Interfaces;
//using Shop.Data.Mocks;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Data.Repository;
using Shop.Data.Models;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Shop
{
    public class Startup
    {
        private IConfigurationRoot _confString;       

        public Startup(IHostEnvironment hostEnv) 
        {
            _confString = new ConfigurationBuilder().SetBasePath(hostEnv.ContentRootPath).AddJsonFile("DBsettings.json").Build();           
        }
        public Startup(IConfiguration configuration)
        {             
            Configuration = configuration;            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) //служит для регистрации модулей и плагинов в проекте
        {
            services.AddDbContext<AppDBcontent>(options => options.UseSqlServer(_confString.GetConnectionString("DefaultConnection")));
            services.AddRazorPages();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //сервис, позволяющий работать с сессиями
            services.AddScoped(sp => ShopCart.GetCart(sp)); //если два пользователя зайдут в корзину, то для них выдается разная корзина

            services.AddMvc(); //добавление поддержки MVC в проекте
            services.AddMemoryCache(); // указываем, что используем кеш и сессии
            services.AddSession(); // и сессии (так же, добавляем в Configure()  app.UseSession())

            //services.AddTransient<IAllCars, MockCars>(); // для объединения интерфейса и класса реализующего интерфейс
            //services.AddTransient<ICarsCategory, MockCategory>();
            services.AddTransient<IAllCars, CarRepository>();
            services.AddTransient<ICarsCategory, CategoryRepository>();
            services.AddMvc(option => option.EnableEndpointRouting = false); //требуется для использования в Configure  app.UseMvcWithDefaultRoute();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //по адресу на странице браузера /hello выдаст сообщение Hello ASP.NET Core
            app.Map("/hello", ap => ap.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Hello ASP.NET Core");
            }));

            app.UseSession(); //указываем, что используем сессии            

            if (env.IsDevelopment()) //если переменная среды ASPNETCORE_ENVIRONMENT имеет значение Development
            {
                app.UseDeveloperExceptionPage(); //подключение отображения странички с ошибками
                //блок кода ниже пытается разделить на 0, тем самым генерируя ошибку и благодаря задействованному методу app.UseDeveloperExceptionPage()
                //в вывлоде на странице браузера появляется детальная информация по ошибке
                /*app.Run(async (context) =>
                {
                    int x = 0;
                    int y = 8 / x;
                    await context.Response.WriteAsync($"Result = {y}");
                });*/

                //app.UseFileServer(enableDirectoryBrowsing: true); //позволяет просматривать содержимое каталогов на сайте
                //app.UseDirectoryBrowser(); //позволяет просматривать содержимое каталогов на сайте
                //позволяет просматривать содержимое каталога Views в браузере по ссылке /Views // Требует подключения using Microsoft.Extensions.FileProviders и using System.IO;  
                app.UseDirectoryBrowser(new DirectoryBrowserOptions()
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Views")),
                    RequestPath = new PathString("/Views")
                });
                app.UseDirectoryBrowser(new DirectoryBrowserOptions()
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Controllers")),
                    RequestPath = new PathString("/Controllers")             
                });
                app.UseDirectoryBrowser(new DirectoryBrowserOptions()
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"ViewModels")),
                    RequestPath = new PathString("/ViewModels")
                });
                app.UseDirectoryBrowser(new DirectoryBrowserOptions()
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Pages")),
                    RequestPath = new PathString("/Pages")
                });
                app.UseDirectoryBrowser(new DirectoryBrowserOptions()
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Data")),
                    RequestPath = new PathString("/Data")
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // при вознеикновении ошибки  app.UseExceptionHandler("/Error"); - выдается сообщение "DivideByZeroException occured!"
            app.Map("/error", ap => ap.Run(async context =>
            {
                await context.Response.WriteAsync("DivideByZeroException occured!");
            }));          

            //app.UseMvcWithDefaultRoute(); //url адрес, который вызывает контроллер по умолчанию
            app.UseMvc(routes =>
            {                                                                               //id? - "?" необязательный параметр
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
                                                                                            // category - имя должно точно совпадать с class CarsController, параметром метода public ViewResult List(string category)
                routes.MapRoute(name: "CategoryFilter", template: "{controller=Cars}/{action}/{category?}"); //, defaults: new { Controller = "Cars", action = "List" }
            });

            //отображение кодов страниц (404, 500, 200 - успешный запрос)
            //первым параметром указывается MIME-тип ответа, а второй - то сообщение, которое увидит пользователь.
            app.UseStatusCodePages("text/plain", "Error. Status code : {0}"); 

            app.UseStaticFiles(); //использование статических файлов
                       
            
            using (var scope = app.ApplicationServices.CreateScope())
            {
                AppDBcontent content = scope.ServiceProvider.GetRequiredService<AppDBcontent>();
                DBObject.Initial(content); // функция, добавляет в БД объекты, каждый раз при запуске программы
            }
            
        }
    }
}
