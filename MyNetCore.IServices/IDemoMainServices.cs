/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：演示主体业务逻辑接口                                                    
*│　作    者：杨习友                                            
*│　版    本：1.0 使用Razor引擎自动生成                                              
*│　创建时间：2021-03-28 15:32:02                           
*└──────────────────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.Model.Entity;

namespace MyNetCore.IServices
{
    /// <summary>
    /// 演示主体业务类接口
    /// </summary>
    public interface IDemoMainServices : IBaseServices<DemoMain>
    {
        /// <summary>
        /// 添加或修改数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<DemoMain> ModifyAsync(DemoMain entity);

        /// <summary>
        /// 获取完整的model信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DemoMain> GetModelFullAsync(int id);

    }
}