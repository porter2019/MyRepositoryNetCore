using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyNetCore.IServices;
using MyNetCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNetCore.Web.SetUp;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace MyNetCore.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        private IServiceCollection _services;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;
            //初始化公共DI需要
            services.AddHttpContextAccessor();

            //添加Swgger
            services.AddSwaggerServices(Configuration);

            //添加FreeSql
            services.AddFreeSqlServices(Configuration);

            //Excel导入导出
            //services.AddMagicodesIEServices();

            //添加缓存
            services.AddMyCache(Configuration);

            //强制跳转https
            services.AddHttpsRedirectionServices(Configuration);

            //批量注入Services层中数据库实体业务，注意给的baseType是公共基础业务泛型(BaseServices<,>)
            services.BatchRegisterServices(new Assembly[] { Assembly.GetExecutingAssembly(), Assembly.Load($"{services.GetProjectMainName()}.Services") }, typeof(BaseService<,>));

            //批量注入Services层中普通业务，注意给的baseType是接口类型(IBatchDIServicesTag)
            services.BatchRegisterServices(new Assembly[] { Assembly.Load($"{services.GetProjectMainName()}.Services") }, typeof(IBatchDIServicesTag));

            //解决Razor生成html时中文被转成Unicode码的问题
            services.Configure<WebEncoderOptions>(options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

            //添加WebApi相关
            services.AddWebApiServices(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            //获取所有注入的服务
            ServiceLocator.Instance = app.ApplicationServices;

            if (!env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseWebResponseStatus();
                app.UseExceptionHandler("/Home/Error");
            }

            //非生产环境下显示所有注入的服务路由
            app.UseAllServicesRoute(env, _services);

            //静态文件
            app.UseMyStaticFiles(Configuration);

            //程序启动/停止进行的操作
            app.UseMyAppLaunch();

            //Swagger
            app.UseMySwagger(Configuration);

            //Consul站点监控
            app.UseMyConsul(lifetime, Configuration);

            //WebApi
            app.UseMyWebApi(Configuration);
        }
    }
}
