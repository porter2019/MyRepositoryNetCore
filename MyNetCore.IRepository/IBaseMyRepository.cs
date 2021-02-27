using FreeSql;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyNetCore.IRepository
{
    /// <summary>
    /// 拓展FreeSql的IBaseRepository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseMyRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {


        #region 修改

        /// <summary>
        /// 修改数据(用于不更新属性值为null或默认值的字段)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>受影响的行数</returns>
        new int Update(TEntity entity);

        /// <summary>
        /// 修改数据(用于不更新属性值为null或默认值的字段)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>受影响的行数</returns>
        Task<int> UpdateAsync(TEntity entity);

        /// <summary>
        /// 修改数据(只更新变化的属性)
        /// </summary>
        /// <param name="oldEntity">修改前的实体</param>
        /// <param name="newEntity">修改后的实体</param>
        /// <remarks>【前提条件】需要该TEntity先调用TEntity newEntity = oldEntity.ShallowCopy&lt;TEntity&gt;()获得新对象，变更的值赋值给newEntity</remarks>
        /// <returns>受影响的行数</returns>
        int UpdateOnlyChange(TEntity oldEntity, TEntity newEntity);

        /// <summary>
        /// 修改数据(只更新变化的属性)
        /// </summary>
        /// <param name="oldEntity">修改前的实体</param>
        /// <param name="newEntity">修改后的实体</param>
        /// <remarks>【前提条件】需要该TEntity先调用TEntity newEntity = oldEntity.ShallowCopy&lt;TEntity&gt;()获得新对象，变更的值赋值给newEntity</remarks>
        /// <returns>受影响的行数</returns>
        Task<int> UpdateOnlyChangeAsync(TEntity oldEntity, TEntity newEntity);

        /// <summary>
        /// 修改数据(设置新的实体)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>受影响的行数</returns>
        int UpdateEntity(TEntity entity);

        /// <summary>
        /// 修改数据(设置新的实体) Async
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>受影响的行数</returns>
        Task<int> UpdateEntityAsync(TEntity entity);


        #endregion

        #region 删除

        /// <summary>
        /// 根据ids批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>受影响的行数</returns>
        int DeleteByIds(object[] ids);

        /// <summary>
        /// 根据ids批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>受影响的行数</returns>
        Task<int> DeleteByIdsAsync(object[] ids);

        #endregion

        #region 查询数量

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        long GetCount(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<long> GetCountAsync(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// 查询数量(原生sql语法条件，Where("id = @id", new { id = 1 }))
        /// </summary>
        /// <param name="sql">Sql语句 id = @id</param>
        /// <param name="parms">参数 new { id = 1 }</param>
        /// <returns></returns>
        long GetCount(string sql, object parms = null);

        /// <summary>
        /// 查询数量(原生sql语法条件，Where("id = @id", new { id = 1 }))
        /// </summary>
        /// <param name="sql">Sql语句 id = @id</param>
        /// <param name="parms">参数 new { id = 1 }</param>
        /// <returns></returns>
        Task<long> GetCountAsync(string sql, object parms = null);

        #endregion

        #region 是否存在

        /// <summary>
        /// 查询数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns>bool</returns>
        bool Exists(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// 查询数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns>bool</returns>
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> where);


        #endregion

        #region 查询单条数据

        /// <summary>
        /// 查询单条数据，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns></returns>
        TEntity GetModel(object dywhere);

        /// <summary>
        /// 查询单条数据，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns>TEntity</returns>
        Task<TEntity> GetModelAsync(object dywhere);

        /// <summary>
        /// 查询单条数据，返回ViewModel类型，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns>REntity</returns>
        REntity GetModel<REntity>(object dywhere);

        /// <summary>
        /// 查询单条数据，返回ViewModel类型，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns>REntity</returns>
        Task<REntity> GetModelAsync<REntity>(object dywhere);

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns>TEntity</returns>
        TEntity GetModel(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns>TEntity</returns>
        Task<TEntity> GetModelAsync(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// 查询单条数据，返回ViewModel类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns>REntity</returns>
        REntity GetModel<REntity>(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// 查询单条数据，返回ViewModel类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns>REntity</returns>
        Task<REntity> GetModelAsync<REntity>(Expression<Func<TEntity, bool>> where);


        #endregion


    }
}
