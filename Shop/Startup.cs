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
        public void ConfigureServices(IServiceCollection services) //������ ��� ����������� ������� � �������� � �������
        {
            services.AddDbContext<AppDBcontent>(options => options.UseSqlServer(_confString.GetConnectionString("DefaultConnection")));
            services.AddRazorPages();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //������, ����������� �������� � ��������
            services.AddScoped(sp => ShopCart.GetCart(sp)); //���� ��� ������������ ������ � �������, �� ��� ��� �������� ������ �������

            services.AddMvc(); //���������� ��������� MVC � �������
            services.AddMemoryCache(); // ���������, ��� ���������� ��� � ������
            services.AddSession(); // � ������ (��� ��, ��������� � Configure()  app.UseSession())

            //services.AddTransient<IAllCars, MockCars>(); // ��� ����������� ���������� � ������ ������������ ���������
            //services.AddTransient<ICarsCategory, MockCategory>();
            services.AddTransient<IAllCars, CarRepository>();
            services.AddTransient<ICarsCategory, CategoryRepository>();
            services.AddMvc(option => option.EnableEndpointRouting = false); //��������� ��� ������������� � Configure  app.UseMvcWithDefaultRoute();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //�� ������ �� �������� �������� /hello ������ ��������� Hello ASP.NET Core
            app.Map("/hello", ap => ap.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Hello ASP.NET Core");
            }));

            app.UseSession(); //���������, ��� ���������� ������            

            if (env.IsDevelopment()) //���� ���������� ����� ASPNETCORE_ENVIRONMENT ����� �������� Development
            {
                app.UseDeveloperExceptionPage(); //����������� ����������� ��������� � ��������
                //���� ���� ���� �������� ��������� �� 0, ��� ����� ��������� ������ � ��������� ���������������� ������ app.UseDeveloperExceptionPage()
                //� ������� �� �������� �������� ���������� ��������� ���������� �� ������
                /*app.Run(async (context) =>
                {
                    int x = 0;
                    int y = 8 / x;
                    await context.Response.WriteAsync($"Result = {y}");
                });*/

                //app.UseFileServer(enableDirectoryBrowsing: true); //��������� ������������� ���������� ��������� �� �����
                //app.UseDirectoryBrowser(); //��������� ������������� ���������� ��������� �� �����
                //��������� ������������� ���������� �������� Views � �������� �� ������ /Views // ������� ����������� using Microsoft.Extensions.FileProviders � using System.IO;  
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
            // ��� �������������� ������  app.UseExceptionHandler("/Error"); - �������� ��������� "DivideByZeroException occured!"
            app.Map("/error", ap => ap.Run(async context =>
            {
                await context.Response.WriteAsync("DivideByZeroException occured!");
            }));          

            //app.UseMvcWithDefaultRoute(); //url �����, ������� �������� ���������� �� ���������
            app.UseMvc(routes =>
            {                                                                               //id? - "?" �������������� ��������
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
                                                                                            // category - ��� ������ ����� ��������� � class CarsController, ���������� ������ public ViewResult List(string category)
                routes.MapRoute(name: "CategoryFilter", template: "{controller=Cars}/{action}/{category?}"); //, defaults: new { Controller = "Cars", action = "List" }
            });

            //����������� ����� ������� (404, 500, 200 - �������� ������)
            //������ ���������� ����������� MIME-��� ������, � ������ - �� ���������, ������� ������ ������������.
            app.UseStatusCodePages("text/plain", "Error. Status code : {0}"); 

            app.UseStaticFiles(); //������������� ����������� ������
                       
            
            using (var scope = app.ApplicationServices.CreateScope())
            {
                AppDBcontent content = scope.ServiceProvider.GetRequiredService<AppDBcontent>();
                DBObject.Initial(content); // �������, ��������� � �� �������, ������ ��� ��� ������� ���������
            }
            
        }
    }
}
