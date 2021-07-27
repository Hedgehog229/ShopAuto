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
            app.UseSession(); //���������, ��� ���������� ������

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

            app.UseDeveloperExceptionPage(); //����������� ����������� ��������� � ��������
            app.UseStatusCodePages(); //����������� ����� ������� (404, 500, 200 - �������� ������)
            app.UseStaticFiles(); //������������� ����������� ������
            app.UseMvcWithDefaultRoute(); //url �����, ������� �������� ���������� �� ���������

            
            
            using (var scope = app.ApplicationServices.CreateScope())
            {
                AppDBcontent content = scope.ServiceProvider.GetRequiredService<AppDBcontent>();
                //DBObject.Initial(content); // �������, ��������� � �� �������, ������ ��� ��� ������� ���������
            }

            
        }
    }
}
