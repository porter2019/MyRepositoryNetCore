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


namespace MyNetCore.Services
{
    /// <summary>
    /// 验证码发送记录表业务实现类
    /// </summary>
    [ServiceLifetime()]
    public class ValidateCodeHistoryServices : BaseServices<ValidateCodeHistory, int>, IValidateCodeHistoryServices
    {
        private readonly IFreeSql _fsq;
        private readonly ICommonAttachServices _commonAttachServices;
        private readonly IValidateCodeHistoryRepository _validateCodeHistoryRepository;

        public ValidateCodeHistoryServices(IFreeSql fsq, ICommonAttachServices commonAttachServices, ValidateCodeHistoryRepository validateCodeHistoryRepository) : base(validateCodeHistoryRepository)
        {
            _fsq = fsq;
            _commonAttachServices = commonAttachServices;
            _validateCodeHistoryRepository = validateCodeHistoryRepository;
        }

        /// <summary>
        /// 获取完整的model信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ValidateCodeHistory> GetModelFullAsync(int id)
        {
            if (id < 0) return new ValidateCodeHistory();
             var entity = await _validateCodeHistoryRepository.GetModelAsync(id);
            
            if (entity == null) return new ValidateCodeHistory();

                entity.Attachs = await _commonAttachServices.GetAttachListAsync(id, typeof(ValidateCodeHistory));

            return entity;

        }

        /// <summary>
        /// 添加或修改数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ValidateCodeHistory> ModifyAsync(ValidateCodeHistory entity)
        {
            using (var uow = _fsq.CreateUnitOfWork())
            {
                var validateCodeHistoryRepo = uow.GetRepository<ValidateCodeHistory>();
                validateCodeHistoryRepo.UnitOfWork = uow;

                var attachRepo = uow.GetRepository<CommonAttach>();
                attachRepo.UnitOfWork = uow;


                var newEntity = await validateCodeHistoryRepo.InsertOrUpdateAsync(entity);
                var refModel = typeof(ValidateCodeHistory).FullName;

                //附件
                string[] fields = { "Attach" };
                await attachRepo.DeleteAsync(p => p.RefId == newEntity.Id && p.RefModel == refModel && fields.Contains(p.Field));

                if (entity.Attachs != null)
                {
                    entity.Attachs.ForEach(item =>
                    {
                        item.AttachId = 0;
                        item.RefId = newEntity.Id;
                        item.RefModel = refModel;
                        item.Field = "Attach";
                    });
                    await attachRepo.InsertAsync(entity.Attachs);
                }

                uow.Commit();
            }

            return entity;
        }

    }
}
