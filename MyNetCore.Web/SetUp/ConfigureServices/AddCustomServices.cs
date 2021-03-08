using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MyNetCore.IServices;
using MyNetCore.Services;

namespace MyNetCore.Web.SetUp
{
    public static class AddCustomServices
    {
        /// <summary>
        /// 批量注入业务类Services
        /// </summary>
        /// <param name="services">DI服务</param>
        /// <param name="assemblys">需要批量注册的程序集集合</param>
        /// <param name="baseType">基础类/接口</param>
        public static void BatchRegisterServices(this IServiceCollection services, Assembly[] assemblys, Type baseType)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //基类是否是泛型
            var baseTypeIsGeneric = baseType.IsGenericType;

            //所有符合注册条件的类集合
            List<Type> typeList = new List<Type>();
            foreach (var assembly in assemblys)
            {
                //获取所有符合条件的实例类
                if (baseTypeIsGeneric)
                {
                    var types = assembly.GetTypes().Where(p => !p.IsInterface && !p.IsSealed && !p.IsAbstract &&
                                                                p.BaseType != null && p.BaseType.IsGenericType &&
                                                                p.BaseType.GetGenericTypeDefinition() == baseType);
                    if (types.Any()) typeList.AddRange(types);
                }
                else
                {
                    var types = assembly.GetTypes().Where(p => !p.IsInterface && !p.IsSealed && !p.IsAbstract &&
                                                                baseType.IsAssignableFrom(p));
                    if (types != null && types.Count() > 0) typeList.AddRange(types);
                }
            }
            if (typeList.Count() == 0) return;

            //待注册集合,key为实现类，value为抽象接口
            var typeDic = new Dictionary<Type, Type[]>();

            //获取实例类对应的接口
            foreach (var type in typeList)
            {
                //如果基类是泛型，则过滤掉泛型接口(IBaseServices<T>)
                if (baseTypeIsGeneric)
                {
                    var interfaces = type.GetInterfaces().Where(p => !p.IsGenericType).ToArray();
                    typeDic.Add(type, interfaces);
                }
                else//否则过滤掉用于批量注册标识的接口(IBatchDIServicesTag)
                {
                    var interfaces = type.GetInterfaces().Where(p => p != baseType).ToArray();
                    typeDic.Add(type, interfaces);
                }
            }

            if (typeDic.Keys.Count == 0) return;

            foreach (var instanceType in typeDic.Keys)
            {
                var serviceBatchTag = (instanceType.GetCustomAttributes(typeof(ServiceLifetimeAttribute), true)[0] as ServiceLifetimeAttribute);
                if (serviceBatchTag == null) continue;

                foreach (var interfaceType in typeDic[instanceType])
                {
                    switch (serviceBatchTag.Lifetime)
                    {
                        case ServiceLifetime.Singleton://单例
                            services.AddSingleton(interfaceType, instanceType);
                            break;
                        case ServiceLifetime.Scoped: //同一请求上下文都是一个对象
                            services.AddScoped(interfaceType, instanceType);
                            break;
                        case ServiceLifetime.Transient: //瞬时单例，每次访问都是新的对象
                            services.AddTransient(interfaceType, instanceType);
                            break;
                    }
                }
            }

        }

    }
}
