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
        private bool _IsEnabled;

        /// <summary>
        /// 默认Scoped
        /// </summary>
        public ServiceLifetimeAttribute()
        {
            this._Lifetime = ServiceLifetime.Scoped;
            this._IsEnabled = true;
        }

        public ServiceLifetimeAttribute(ServiceLifetime serviceLifetime)
        {
            this._Lifetime = serviceLifetime;
            this._IsEnabled = true;
        }

        public ServiceLifetimeAttribute(bool isEnabled)
        {
            this._Lifetime = ServiceLifetime.Scoped;
            this._IsEnabled = isEnabled;
        }

        public ServiceLifetimeAttribute(ServiceLifetime serviceLifetime, bool isEnabled)
        {
            this._Lifetime = serviceLifetime;
            this._IsEnabled = isEnabled;
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

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            set
            {
                _IsEnabled = value;
            }
        }
    }
}