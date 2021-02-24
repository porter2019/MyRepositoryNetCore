using MyNetCore.IRepository;
using MyNetCore.IServices;
using MyNetCore.Repository;
using System;

namespace MyNetCore.Services
{
    /// <summary>
    /// 实现演示服务
    /// </summary>
    public class DemoServices : IDemoServices
    {
        /// <summary>
        /// 具体与数据库交互的对象
        /// </summary>
        private readonly IDemoRepository _demo;

        public DemoServices() { }

        /// <summary>
        /// 注入时自动传进来
        /// </summary>
        /// <param name="demo"></param>
        public DemoServices(IDemoRepository demo)
        {
            _demo = demo;
        }

        public int Sum(int i, int j)
        {
            return _demo.Sum(i, j);
        }
    }
}
