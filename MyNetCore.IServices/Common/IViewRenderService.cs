using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.IServices
{
    public interface IViewRenderService : IBatchDIServicesTag
    {
        Task<string> RenderViewToStringAsync(string viewName, object model);
    }
}
