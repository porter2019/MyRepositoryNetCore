﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.IServices
{
    public interface ITestSesrvice : IBatchDIServicesTag
    {
        int Sum(int x, int y);
    }
}
