/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：验证码发送记录表仓储实现                                                    
*│　作    者：杨习友                                             
*│　版    本：1.0 使用Razor引擎自动生成                                              
*│　创建时间：2021-04-07 20:44:39                            
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

namespace MyNetCore.Repository
{
    /// <summary>
    /// 验证码发送记录表仓储实现
    /// </summary>
    public class ValidateCodeHistoryRepository : BaseMyRepository<ValidateCodeHistory, int>, IValidateCodeHistoryRepository
    {
        private readonly IFreeSql _freeSql;

        public ValidateCodeHistoryRepository(IFreeSql fsql) : base(fsql)
        {
            _freeSql = fsql;
        }
    }
}
