/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：经典父子分类仓储实现                                                    
*│　作    者：杨习友                                             
*│　版    本：1.0 使用Razor引擎自动生成                                              
*│　创建时间：2021-04-04 17:30:04                            
*└──────────────────────────────────────────────────────────────┘
*/

using FreeSql;
using MyNetCore.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.Model.Entity;
using Microsoft.Extensions.Logging;

namespace MyNetCore.Repository
{
    /// <summary>
    /// 经典父子分类仓储实现
    /// </summary>
    public class BookCategoryRepository : BaseMyRepository<BookCategory, int>, IBookCategoryRepository
    {

        public BookCategoryRepository(ILogger<BookCategoryRepository> logger, IFreeSql<DBFlagMain> fsql) : base(fsql, logger)
        {

        }
    }
}
