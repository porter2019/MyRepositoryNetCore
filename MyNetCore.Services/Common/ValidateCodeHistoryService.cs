/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：验证码发送记录表业务逻辑接口                                                    
*│　作    者：杨习友                                          
*│　版    本：1.0 使用Razor引擎自动生成                                              
*│　创建时间：2021-04-07 20:44:39                            
*└──────────────────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.IServices;
using MyNetCore.IRepository;
using MyNetCore.Repository;
using MyNetCore.Model.Entity;
using Microsoft.Extensions.Logging;

namespace MyNetCore.Services
{
    /// <summary>
    /// 验证码发送记录表业务实现类
    /// </summary>
    [ServiceLifetime(true)]
    public class ValidateCodeHistoryService : BaseService<ValidateCodeHistory, int>, IValidateCodeHistoryService
    {
        private readonly IFreeSql _fsq;
        private readonly IValidateCodeHistoryRepository _validateCodeHistoryRepository;

        public ValidateCodeHistoryService(ILogger<ValidateCodeHistoryService> logger, IFreeSql<DBFlagMain> fsq, IValidateCodeHistoryRepository validateCodeHistoryRepository) : base(validateCodeHistoryRepository, logger)
        {
            _fsq = fsq;
            _validateCodeHistoryRepository = validateCodeHistoryRepository;
        }

    }
}
