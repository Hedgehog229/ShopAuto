using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shop.Data;
using Shop.Data.Interfaces;
using Shop.Data.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Data.Repository;
using Shop.Data.Models;

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
            app.UseSession(); //указываем, что используем сессии

            /*if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
                        
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });*/

            app.UseDeveloperExceptionPage(); //подключение отображения странички с ошибками
            app.UseStatusCodePages(); //отображение кодов страниц (404, 500, 200 - успешный запрос)
            app.UseStaticFiles(); //использование статических файлов
            app.UseMvcWithDefaultRoute(); //url адрес, который вызывает контроллер по умолчанию

            
            
            using (var scope = app.ApplicationServices.CreateScope())
            {
                AppDBcontent content = scope.ServiceProvider.GetRequiredService<AppDBcontent>();
                //DBObject.Initial(content); // функция, добавляет в БД объекты, каждый раз при запуске программы
            }

            
        }
    }
}
