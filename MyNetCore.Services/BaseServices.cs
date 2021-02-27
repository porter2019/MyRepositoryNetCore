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
        /// <returns>TEntity，包含添加后的主键值</returns>
        public Task<TEntity> Add(TEntity entity)
        {
            return _baseRepo.InsertAsync(entity);
        }

        /// <summary>
        /// 根据主键查找一条数据
        /// </summary>
        /// <param name="objId"></param>
        /// <returns>TEntity</returns>
        public Task<TEntity> QueryByID(object objId)
        {
            return _baseRepo.GetModelAsync(objId);
        }

        /// <summary>
        /// 修改一条数据，返回受影响的行数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>受影响的行数</returns>
        public Task<int> Update(TEntity entity)
        {
            return _baseRepo.UpdateAsync(entity);
        }

        /// <summary>
        /// 修改数据(只更新变化的属性)
        /// </summary>
        /// <param name="oldEntity">修改前的实体</param>
        /// <param name="newEntity">修改后的实体</param>
        /// <remarks>【前提条件】需要该TEntity先调用TEntity newEntity = oldEntity.ShallowCopy&lt;TEntity&gt;()获得新对象，变更的值赋值给newEntity</remarks>
        /// <returns>受影响的行数</returns>
        public Task<int> UpdateOnlyChangeAsync(TEntity oldEntity, TEntity newEntity)
        {
            return _baseRepo.UpdateOnlyChangeAsync(oldEntity, newEntity);
        }

        /// <summary>
        /// 根据ids批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>受影响的行数</returns>
        public Task<int> DeleteByIds(object[] ids)
        {
            return _baseRepo.DeleteByIdsAsync(ids);
        }

    }
}
