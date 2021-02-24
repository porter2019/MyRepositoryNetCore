using MyNetCore.IRepository;
using System;

namespace MyNetCore.Repository
{
    /// <summary>
    /// 实现演示仓储
    /// </summary>
    public class DemoRepository : IDemoRepository
    {
        public int Sum(int i, int j)
        {
            return i + j;
        }
    }
}
