using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyNetCore.IServices;
using MyNetCore.Services;
using MyNetCore.IRepository;
using MyNetCore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNetCore.Web.SetUp;

namespace MyNetCore.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //初始化公共DI需要
            services.AddHttpContextAccessor();

            //添加Swgger
            services.AddSwaggerServices();

            //添加FreeSql
            services.AddFreeSqlServices();

            //强制跳转https
            services.AddHttpsRedirectionServices();

            //注入Services层中的业务
            services.AddMyCustomServices();

            //添加MVC相关
            services.AddWebMVCServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //获取所有注入的服务
            ServiceLocator.Instance = app.ApplicationServices;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //静态文件
            app.UseMyStaticFiles();

            //程序启动/停止进行的操作
            app.UseMyAppStartup();

            //Swagger
            app.UseMySwagger();

            //MVC
            app.UseMyWebMVC();
        }
    }
}
