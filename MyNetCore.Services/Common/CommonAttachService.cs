/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：附件表业务逻辑接口                                                    
*│　作    者：杨习友                                          
*│　版    本：1.0 使用Razor引擎自动生成                                              
*│　创建时间：2021-03-28 22:32:57                            
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
    /// 附件表业务实现类
    /// </summary>
    [ServiceLifetime()]
    public class CommonAttachService : BaseService<CommonAttach, int>, ICommonAttachService
    {
        private readonly ICommonAttachRepository _commonAttachRepository;

        public CommonAttachService(ILogger<CommonAttachService> logger, CommonAttachRepository commonAttachRepository) : base(commonAttachRepository, logger)
        {
            _commonAttachRepository = commonAttachRepository;
        }

        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="refId"></param>
        /// <param name="refType"></param>
        /// <param name="field">关联字段</param>
        /// <returns></returns>
        public Task<List<CommonAttach>> GetAttachListAsync(int refId, Type refType, string field = "Attach")
        {
            //var classInfo = refType.GetCustomAttributes(typeof(Model.FsTableAttribute), true)[0] as Model.FsTableAttribute;
            //if (classInfo.DisableSyncStructure) throw new Exception("视图类型的Class不能用作引用标识");

            return _commonAttachRepository.GetListAsync(p => p.RefId == refId && p.RefModel == refType.FullName && p.Field == field);
        }

    }
}
