/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：经典父子分类仓储接口
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-04-04 17:30:04
*└──────────────────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using MyNetCore.Model.Entity;

namespace MyNetCore.IRepository
{
    /// <summary>
    /// 经典父子分类仓储接口
    /// </summary>
    public interface IBookCategoryRepository : IBaseMyRepository<BookCategory>
    {

    }
}
