using Microsoft.Extensions.WebEncoders;
using MyNetCore.Web.SetUp;
using NLog;
using NLog.Web;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    //Nlog
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    builder.Services.AddMyConsulKV(builder.Configuration, builder.Environment);//ConsulKV
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSwaggerServices(builder.Configuration);//Swgger
    builder.Services.AddFreeSqlServices(builder.Configuration);//FreeSql
    builder.Services.AddMyCache(builder.Configuration);//缓存
    builder.Services.AddHttpsRedirectionServices(builder.Configuration);//强制跳转https
                                                                        //批量注入Services层中数据库实体业务，注意给的baseType是公共基础业务泛型(BaseServices<,>)
    builder.Services.BatchRegisterServices(new Assembly[] { Assembly.GetExecutingAssembly(), Assembly.Load($"{builder.Services.GetProjectMainName()}.Services") }, typeof(BaseService<,>));
    //批量注入Services层中普通业务，注意给的baseType是接口类型(IBatchDIServicesTag)
    builder.Services.BatchRegisterServices(new Assembly[] { Assembly.Load($"{builder.GetProjectMainName()}.Services") }, typeof(IBatchDIServicesTag));
    //解决Razor生成html时中文被转成Unicode码的问题
    builder.Services.Configure<WebEncoderOptions>(options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));
    builder.Services.AddWebApiServices(builder.Configuration);//WebApi相关

    var app = builder.Build();
    ServiceLocator.Instance = app.Services;

    if (!app.Environment.IsProduction())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseWebResponseStatus();
        app.UseExceptionHandler("/Home/Error");
    }

    //非生产环境下显示所有注入的服务路由
    app.UseAllServicesRoute(app.Environment, builder.Services);

    //静态文件
    app.UseMyStaticFiles(app.Configuration);

    //程序启动/停止进行的操作
    app.UseMyAppLaunch();

    //Swagger
    app.UseMySwagger(app.Configuration);

    //Consul站点监控
    app.UseMyConsul(app.Lifetime, app.Configuration);

    //WebApi
    app.UseMyWebApi(app.Configuration);

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "系统无法启动");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}