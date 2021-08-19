using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.IServices;

namespace MyNetCore.Services
{
    [ServiceLifetime()]
    public class TestService : ITestSesrvice
    {
        public int Sum(int x, int y)
        {
            return x + y;
        }
    }
}
