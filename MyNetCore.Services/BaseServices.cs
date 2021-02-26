using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.IServices;
using MyNetCore.IRepository;
using MyNetCore.Repository;

namespace MyNetCore.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseServices<TEntity, TKey> : IBaseServices<TEntity> where TEntity : Model.BaseEntity, new()
    {
        private IBaseMyRepository<TEntity> _baseRepo;

        public BaseServices(BaseMyRepository<TEntity, TKey> baseRepo)
        {
            this._baseRepo = baseRepo;
        }

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<TEntity> Add(TEntity entity)
        {
            return _baseRepo.InsertAsync(entity);
        }

        /// <summary>
        /// 根据主键查找一条数据
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public Task<TEntity> QueryByID(object objId)
        {
            return _baseRepo.GetModelAsync(objId);
        }

        /// <summary>
        /// 修改一条数据，返回受影响的行数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> Update(TEntity entity)
        {
            return _baseRepo.UpdateAsync(entity);
        }

        /// <summary>
        /// 根据ids批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<int> DeleteByIds(object[] ids)
        {
            return _baseRepo.DeleteByIdsAsync(ids);
        }

    }
}
