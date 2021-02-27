using FreeSql;
using MyNetCore.IRepository;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyNetCore.Repository
{
    /// <summary>
    /// 拓展FreeSql的IBaseRepository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class BaseMyRepository<TEntity, TKey> : BaseRepository<TEntity, TKey>, IBaseMyRepository<TEntity> where TEntity : Model.BaseEntity
    {
        private readonly IFreeSql _freeSql;

        public BaseMyRepository(IFreeSql fsql) : base(fsql, null, null)
        {
            _freeSql = fsql;
        }

        #region 修改

        /// <summary>
        /// 修改数据(用于不更新属性值为null或默认值的字段)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int Update(TEntity entity)
        {
            var repo = _freeSql.GetRepository<TEntity>();
            repo.AttachOnlyPrimary(entity);

            return repo.Update(entity);
        }

        /// <summary>
        /// 修改数据(用于不更新属性值为null或默认值的字段)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> UpdateAsync(TEntity entity)
        {
            var repo = _freeSql.GetRepository<TEntity>();
            repo.AttachOnlyPrimary(entity);

            return repo.UpdateAsync(entity);
        }

        /// <summary>
        /// 修改数据(只更新变化的属性)
        /// </summary>
        /// <param name="oldEntity">修改前的实体</param>
        /// <param name="newEntity">修改后的实体</param>
        /// <remarks>【前提条件】需要该TEntity先调用TEntity newEntity = oldEntity.ShallowCopy&lt;TEntity&gt;()获得新对象，变更的值赋值给newEntity</remarks>
        /// <returns></returns>
        public int UpdateOnlyChange(TEntity oldEntity, TEntity newEntity)
        {
            var repo = _freeSql.GetRepository<TEntity>();
            repo.Attach(oldEntity);
            return repo.Update(newEntity);
        }

        /// <summary>
        /// 修改数据(只更新变化的属性)
        /// </summary>
        /// <param name="oldEntity">修改前的实体</param>
        /// <param name="newEntity">修改后的实体</param>
        /// <remarks>【前提条件】需要该TEntity先调用TEntity newEntity = oldEntity.ShallowCopy&lt;TEntity&gt;()获得新对象，变更的值赋值给newEntity</remarks>
        /// <returns></returns>
        public Task<int> UpdateOnlyChangeAsync(TEntity oldEntity, TEntity newEntity)
        {
            var repo = _freeSql.GetRepository<TEntity>();
            repo.Attach(oldEntity);
            return repo.UpdateAsync(newEntity);
        }

        /// <summary>
        /// 修改数据(设置新的实体)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateEntity(TEntity entity)
        {
            var runsql = _freeSql.Update<TEntity>().SetSource(entity);

            return runsql.ExecuteAffrows();
        }

        /// <summary>
        /// 修改数据(设置新的实体)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> UpdateEntityAsync(TEntity entity)
        {
            var runsql = _freeSql.Update<TEntity>().SetSource(entity);
            return runsql.ExecuteAffrowsAsync();
        }

        #endregion

        #region 删除

        /// <summary>
        /// 根据ids批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteByIds(object[] ids)
        {
            if (typeof(TEntity).GetProperties().Any(p => p.Name == "IsDeleted"))
            {
                return _freeSql.Update<TEntity>(ids).SetRaw("IsDeleted = 1").ExecuteAffrows();
            }
            else
            {
                return _freeSql.Delete<TEntity>(ids).ExecuteAffrows();
            }
        }

        /// <summary>
        /// 根据ids批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteByIdsAsync(object[] ids)
        {
            if (typeof(TEntity).GetProperties().Any(p => p.Name == "IsDeleted"))
            {
                return await _freeSql.Update<TEntity>(ids).SetRaw("IsDeleted = 1").ExecuteAffrowsAsync();
            }
            else
            {
                return await _freeSql.Delete<TEntity>(ids).ExecuteAffrowsAsync();
            }
        }

        #endregion

        #region 查询数量

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public long GetCount(Expression<Func<TEntity, bool>> where)
        {
            return _freeSql.Select<TEntity>().Where(where).Count();
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<long> GetCountAsync(Expression<Func<TEntity, bool>> where)
        {
            return _freeSql.Select<TEntity>().Where(where).CountAsync();
        }

        /// <summary>
        /// 查询数量(原生sql语法条件，Where("id = @id", new { id = 1 }))
        /// </summary>
        /// <param name="sql">Sql语句 id = @id</param>
        /// <param name="parms">参数 new { id = 1 }</param>
        /// <returns></returns>
        public long GetCount(string sql, object parms = null)
        {
            return _freeSql.Select<TEntity>().Where(sql, parms).Count();
        }

        /// <summary>
        /// 查询数量(原生sql语法条件，Where("id = @id", new { id = 1 }))
        /// </summary>
        /// <param name="sql">Sql语句 id = @id</param>
        /// <param name="parms">参数 new { id = 1 }</param>
        /// <returns></returns>
        public Task<long> GetCountAsync(string sql, object parms = null)
        {
            return _freeSql.Select<TEntity>().Where(sql, parms).CountAsync();
        }

        #endregion

        #region 是否存在

        /// <summary>
        /// 查询数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Exists(Expression<Func<TEntity, bool>> where)
        {
            return _freeSql.Select<TEntity>().Any(where);
        }

        /// <summary>
        /// 查询数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> where)
        {
            return _freeSql.Select<TEntity>().AnyAsync(where);
        }

        #endregion

        #region 查询单条数据

        /// <summary>
        /// 查询单条数据，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns></returns>
        public TEntity GetModel(object dywhere)
        {
            return _freeSql.Select<TEntity>(dywhere).ToOne();
        }

        /// <summary>
        /// 查询单条数据，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns></returns>
        public Task<TEntity> GetModelAsync(object dywhere)
        {
            return _freeSql.Select<TEntity>(dywhere).ToOneAsync();
        }

        /// <summary>
        /// 查询单条数据，返回ViewModel类型，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns></returns>
        public REntity GetModel<REntity>(object dywhere)
        {
            return _freeSql.Select<TEntity>(dywhere).ToOne<REntity>();
        }

        /// <summary>
        /// 查询单条数据，返回ViewModel类型，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns></returns>
        public Task<REntity> GetModelAsync<REntity>(object dywhere)
        {
            return _freeSql.Select<TEntity>(dywhere).ToOneAsync<REntity>();
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TEntity GetModel(Expression<Func<TEntity, bool>> where)
        {
            return _freeSql.Select<TEntity>().Where(where).ToOne();
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<TEntity> GetModelAsync(Expression<Func<TEntity, bool>> where)
        {
            return _freeSql.Select<TEntity>().Where(where).ToOneAsync();
        }

        /// <summary>
        /// 查询单条数据，返回ViewModel类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public REntity GetModel<REntity>(Expression<Func<TEntity, bool>> where)
        {
            return _freeSql.Select<TEntity>().Where(where).ToOne<REntity>();
        }

        /// <summary>
        /// 查询单条数据，返回ViewModel类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<REntity> GetModelAsync<REntity>(Expression<Func<TEntity, bool>> where)
        {
            return _freeSql.Select<TEntity>().Where(where).ToOneAsync<REntity>();
        }


        #endregion

    }
}
