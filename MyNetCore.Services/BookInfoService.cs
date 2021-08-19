/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：书籍信息业务逻辑接口                                                    
*│　作    者：杨习友                                          
*│　版    本：1.0 使用Razor引擎自动生成                                              
*│　创建时间：2021-04-06 20:19:48                            
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
    /// 书籍信息业务实现类
    /// </summary>
    [ServiceLifetime()]
    public class BookInfoService : BaseService<BookInfo, int>, IBookInfoService
    {
        private readonly IBookInfoRepository _bookInfoRepository;
        private readonly ICommonAttachService _commonAttachServices;
        private readonly IFreeSql _fsq;

        public BookInfoService(ILogger<BookInfoService> logger, BookInfoRepository bookInfoRepository, IFreeSql<DBFlagMain> fsq, ICommonAttachService commonAttachServices) : base(bookInfoRepository, logger)
        {
            _bookInfoRepository = bookInfoRepository;
            _commonAttachServices = commonAttachServices;
            _fsq = fsq;
        }

        /// <summary>
        /// 获取完整的model信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BookInfoView> GetModelFullAsync(int id)
        {
            if (id < 0) return new BookInfoView();

            var entity = await _bookInfoRepository.GetModelViewAsync<BookInfoView>(id);
            if (entity == null) return new BookInfoView();

            entity.Attachs = await _commonAttachServices.GetAttachListAsync(id, typeof(BookInfo));

            return entity;

        }

        /// <summary>
        /// 添加或修改数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<BookInfoView> ModifyAsync(BookInfoView entity)
        {
            using (var uow = _fsq.CreateUnitOfWork())
            {
                var bookInfoRepo = uow.GetRepository<BookInfo>();
                bookInfoRepo.UnitOfWork = uow;

                var attachRepo = uow.GetRepository<CommonAttach>();
                attachRepo.UnitOfWork = uow;


                var newEntity = await bookInfoRepo.InsertOrUpdateAsync(entity);
                var refModel = typeof(BookInfo).FullName;

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
