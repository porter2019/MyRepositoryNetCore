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


namespace MyNetCore.Services
{
    /// <summary>
    /// 附件表业务实现类
    /// </summary>
    [ServiceLifetime()]
    public class CommonAttachServices : BaseServices<CommonAttach, int>, ICommonAttachServices
    {
        private readonly ICommonAttachRepository _commonAttachRepository;

        public CommonAttachServices(CommonAttachRepository commonAttachRepository) : base(commonAttachRepository)
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
        public Task<List<CommonAttach>> GetAttachList(int refId, Type refType, string field = "Attach")
        {
            return _commonAttachRepository.GetListAsync(p => p.RefId == refId && p.RefModel == refType.FullName && p.Field == field);
        }

    }
}
