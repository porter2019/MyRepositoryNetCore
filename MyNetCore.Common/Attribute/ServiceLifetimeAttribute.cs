using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MyNetCore
{

    /// <summary>
    /// 指定服务生命周期，默认Scoped
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceLifetimeAttribute : Attribute
    {
        private ServiceLifetime _Lifetime;

        /// <summary>
        /// 默认Scoped
        /// </summary>
        public ServiceLifetimeAttribute()
        {
            this.Lifetime = ServiceLifetime.Scoped;
        }

        public ServiceLifetimeAttribute(ServiceLifetime serviceLifetime)
        {
            this.Lifetime = serviceLifetime;
        }

        /// <summary>
        /// 是否允许匿名访问
        /// </summary>
        public ServiceLifetime Lifetime
        {
            get
            {
                return _Lifetime;
            }
            set
            {
                _Lifetime = value;
            }
        }
    }
}
