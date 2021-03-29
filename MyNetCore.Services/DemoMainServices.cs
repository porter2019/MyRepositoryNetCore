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


namespace MyNetCore.Services
{
    /// <summary>
    /// 演示主体业务实现类
    /// </summary>
    [ServiceLifetime()]
    public class DemoMainServices : BaseServices<DemoMain, int>, IDemoMainServices
    {
        private readonly IDemoMainRepository _demoMainRepository;
        private readonly IFreeSql _fsq;

        public DemoMainServices(DemoMainRepository demoMainRepository, IFreeSql fsq) : base(demoMainRepository)
        {
            _demoMainRepository = demoMainRepository;
            _fsq = fsq;
        }

        /// <summary>
        /// 添加或修改数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<DemoMain> Modify(DemoMain entity)
        {
            using (var uow = _fsq.CreateUnitOfWork())
            {
                var demoMainRepo = uow.GetRepository<DemoMain>();
                demoMainRepo.UnitOfWork = uow;

                var attachRepo = uow.GetRepository<CommonAttach>();
                attachRepo.UnitOfWork = uow;

                var newEntity = await demoMainRepo.InsertOrUpdateAsync(entity);
                var refModel = typeof(DemoMain).FullName;

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
