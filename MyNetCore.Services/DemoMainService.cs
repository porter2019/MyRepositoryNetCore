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
using MyNetCore.IServices;
using MyNetCore.IRepository;
using MyNetCore.Repository;
using MyNetCore.Model.Entity;
using Microsoft.Extensions.Logging;

namespace MyNetCore.Services
{
    /// <summary>
    /// 演示主体业务实现类
    /// </summary>
    [ServiceLifetime()]
    public class DemoMainService : BaseService<DemoMain, int>, IDemoMainService
    {
        private readonly IDemoMainRepository _demoMainRepository;
        private readonly ICommonAttachService _commonAttachServices;
        private readonly IFreeSql _fsq;

        public DemoMainService(ILogger<DemoMainService> logger, DemoMainRepository demoMainRepository, IFreeSql<DBFlagMain> fsq, ICommonAttachService commonAttachServices) : base(demoMainRepository, logger)
        {
            _demoMainRepository = demoMainRepository;
            _commonAttachServices = commonAttachServices;
            _fsq = fsq;
        }

        /// <summary>
        /// 获取完整的model信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DemoMain> GetModelFullAsync(int id)
        {
            if (id < 0) return new DemoMain();

            var entity = await _demoMainRepository.GetModelAsync(id);
            if (entity == null) return new DemoMain();

            entity.Items = await _demoMainRepository.GetDemoMainItems(id);
            entity.ImageList = await _commonAttachServices.GetAttachListAsync(id, typeof(DemoMain), "ImageList");
            entity.Attachs = await _commonAttachServices.GetAttachListAsync(id, typeof(DemoMain));

            return entity;

        }

        /// <summary>
        /// 添加或修改数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<DemoMain> ModifyAsync(DemoMain entity)
        {
            using (var uow = _fsq.CreateUnitOfWork())
            {
                var demoMainRepo = uow.GetRepository<DemoMain>();
                demoMainRepo.UnitOfWork = uow;

                var attachRepo = uow.GetRepository<CommonAttach>();
                attachRepo.UnitOfWork = uow;

                var itemRepo = uow.GetRepository<DemoMainItem>();
                itemRepo.UnitOfWork = uow;

                var newEntity = await demoMainRepo.InsertOrUpdateAsync(entity);
                var refModel = typeof(DemoMain).FullName;

                //明细
                await itemRepo.DeleteAsync(p => p.MainId == newEntity.MainId);
                if (entity.Items != null)
                {
                    entity.Items.ForEach(item =>
                    {
                        item.MainId = newEntity.MainId;
                        item.ItemId = 0;
                    });
                    await itemRepo.InsertAsync(entity.Items);
                }

                //附件
                string[] fields = { "Attach", "ImageList" };
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
                if (entity.ImageList != null)
                {
                    entity.ImageList.ForEach(item =>
                    {
                        item.AttachId = 0;
                        item.RefId = newEntity.Id;
                        item.RefModel = refModel;
                        item.Field = "ImageList";
                    });
                    await attachRepo.InsertAsync(entity.ImageList);
                }

                uow.Commit();
            }

            return entity;
        }
    }
}
