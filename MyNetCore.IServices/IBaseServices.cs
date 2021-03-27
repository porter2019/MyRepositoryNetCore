using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.IServices
{
    /// <summary>
    /// 业务基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseServices<TEntity> where TEntity : class, new() //Model.BaseEntity
    {

        #region 添加

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>添加后的实体</returns>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>添加后的实体</returns>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        List<TEntity> Insert(IEnumerable<TEntity> entitys);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        Task<List<TEntity>> InsertAsync(IEnumerable<TEntity> entitys);

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity InsertOrUpdate(TEntity entity);

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> InsertOrUpdateAsync(TEntity entity);

        #endregion

        #region 修改

        /// <summary>
        /// 修改数据(用于不更新属性值为null或默认值的字段)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>受影响的行数</returns>
        int Update(TEntity entity);

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

        #region 视图

        /// <summary>
        /// 查询单条视图数据，返回视图类型，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns>REntity</returns>
        REntity GetModelView<REntity>(object dywhere) where REntity : class;

        /// <summary>
        /// 查询单条视图数据，返回视图类型，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns>REntity</returns>
        Task<REntity> GetModelViewAsync<REntity>(object dywhere) where REntity : class;

        /// <summary>
        /// 查询单条视图数据，返回视图类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns>REntity</returns>
        REntity GetModelView<REntity>(Expression<Func<REntity, bool>> where) where REntity : class, new();

        /// <summary>
        /// 查询单条视图数据，返回视图类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns>REntity</returns>
        Task<REntity> GetModelViewAsync<REntity>(Expression<Func<REntity, bool>> where) where REntity : class, new();

        #endregion

        #endregion

        #region 查询集合

        /// <summary>
        /// 查询列表，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        List<TEntity> GetList(Expression<Func<TEntity, bool>> exp);

        /// <summary>
        /// 查询列表，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> exp);

        /// <summary>
        /// 查询列表，返回DTO对象，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        List<REntity> GetList<REntity>(Expression<Func<TEntity, bool>> exp);

        /// <summary>
        /// 查询列表，返回DTO对象，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        Task<List<REntity>> GetListAsync<REntity>(Expression<Func<TEntity, bool>> exp);

        /// <summary>
        /// 查询列表,原生sql语法条件，Where("id = @id", new { id = 1 })
        /// 提示：parms 参数还可以传 Dictionary<string, object>
        /// </summary>
        /// <param name="where">sql语法条件</param>
        /// <param name="parms">参数</param>
        /// <returns>List<TEntity></returns>
        List<TEntity> GetList(string where, object parms = null);

        /// <summary>
        /// 查询列表,原生sql语法条件，Where("id = @id", new { id = 1 })
        /// 提示：parms 参数还可以传 Dictionary<string, object>
        /// </summary>
        /// <param name="where">sql语法条件</param>
        /// <param name="parms">参数</param>
        /// <returns>List<TEntity></returns>
        Task<List<TEntity>> GetListAsync(string where, object parms = null);

        /// <summary>
        /// 查询列表，返回DTO对象,原生sql语法条件，Where("id = @id", new { id = 1 })
        /// 提示：parms 参数还可以传 Dictionary<string, object>
        /// </summary>
        /// <param name="where">sql语法条件</param>
        /// <param name="parms">参数</param>
        /// <returns>List<TEntity></returns>
        List<REntity> GetList<REntity>(string where, object parms = null);

        /// <summary>
        /// 查询列表，返回DTO对象,原生sql语法条件，Where("id = @id", new { id = 1 })
        /// 提示：parms 参数还可以传 Dictionary<string, object>
        /// </summary>
        /// <param name="where">sql语法条件</param>
        /// <param name="parms">参数</param>
        /// <returns>List<TEntity></returns>
        Task<List<REntity>> GetListAsync<REntity>(string where, object parms = null);

        #region 视图

        /// <summary>
        /// 查询视图列表，返回视图对象，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        List<REntity> GetListView<REntity>(Expression<Func<REntity, bool>> exp) where REntity : class, new();

        /// <summary>
        /// 查询视图列表，返回视图对象，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        Task<List<REntity>> GetListViewAsync<REntity>(Expression<Func<REntity, bool>> exp) where REntity : class, new();

        /// <summary>
        /// 查询视图列表，返回视图对象,原生sql语法条件，Where("id = @id", new { id = 1 })
        /// 提示：parms 参数还可以传 Dictionary<string, object>
        /// </summary>
        /// <param name="where">sql语法条件</param>
        /// <param name="parms">参数</param>
        /// <returns>List<TEntity></returns>
        List<REntity> GetListView<REntity>(string where, object parms = null) where REntity : class, new();

        /// <summary>
        /// 查询视图列表，返回视图对象,原生sql语法条件，Where("id = @id", new { id = 1 })
        /// 提示：parms 参数还可以传 Dictionary<string, object>
        /// </summary>
        /// <param name="where">sql语法条件</param>
        /// <param name="parms">参数</param>
        /// <returns>List<TEntity></returns>
        Task<List<REntity>> GetListViewAsync<REntity>(string where, object parms = null) where REntity : class, new();

        #endregion

        #endregion

        #region 查询分页

        #region PageOptions

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<TEntity> GetPageList(PageOptions<TEntity> options, out long total);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetPageListAsync(PageOptions<TEntity> options, out long total);

        /// <summary>
        /// 分页查询，自动构建where查询条件
        /// </summary>
        /// <param name="baseOption"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<TEntity> GetPageListBasic(Model.BaseRequestPageViewModel<TEntity> baseOption, out long total);

        /// <summary>
        /// 分页查询，自动构建where查询条件
        /// </summary>
        /// <param name="baseOption"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetPageListBasicAsync(Model.BaseRequestPageViewModel<TEntity> baseOption, out long total);


        /// <summary>
        /// 分页查询，返回DTO对象
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<REntity> GetPageList<REntity>(PageOptions<TEntity> options, out long total);

        /// <summary>
        /// 分页查询，返回DTO对象
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<REntity>> GetPageListAsync<REntity>(PageOptions<TEntity> options, out long total);

        /// <summary>
        /// 分页查询，返回DTO对象，自动构建where查询条件
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<REntity> GetPageBasicList<REntity>(Model.BaseRequestPageViewModel<TEntity> options, out long total);

        /// <summary>
        /// 分页查询，返回DTO对象，自动构建where查询条件
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<REntity>> GetPageListBasicAsync<REntity>(Model.BaseRequestPageViewModel<TEntity> options, out long total);

        #endregion

        #region Sql

        /// <summary>
        /// 分页查询，使用sql
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<TEntity> GetPageList(string sql, PageOptions<TEntity> options, out long total);

        /// <summary>
        /// 分页查询，使用sql
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetPageListAsync(string sql, PageOptions<TEntity> options, out long total);

        /// <summary>
        /// 分页查询，使用sql，自动构建where查询条件
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<TEntity> GetPageBasicList(string sql, Model.BaseRequestPageViewModel<TEntity> options, out long total);

        /// <summary>
        /// 分页查询，使用sql，自动构建where查询条件
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetPageListBasicAsync(string sql, Model.BaseRequestPageViewModel<TEntity> options, out long total);

        /// <summary>
        /// 分页查询，使用sql，返回DTO对象
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<REntity> GetPageList<REntity>(string sql, PageOptions<TEntity> options, out long total);

        /// <summary>
        /// 分页查询，使用sql，返回DTO对象
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<REntity>> GetPageListAsync<REntity>(string sql, PageOptions<TEntity> options, out long total);

        /// <summary>
        /// 分页查询，使用sql，返回DTO对象，自动构建where查询条件
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<REntity> GetPageBasicList<REntity>(string sql, Model.BaseRequestPageViewModel<TEntity> options, out long total);

        /// <summary>
        /// 分页查询，使用sql，返回DTO对象，自动构建where查询条件
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<REntity>> GetPageListBasicAsync<REntity>(string sql, Model.BaseRequestPageViewModel<TEntity> options, out long total);

        #endregion

        #region 视图

        /// <summary>
        /// 视图分页查询，返回视图对象
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<REntity> GetPageListView<REntity>(PageOptions<REntity> options, out long total) where REntity : class, new();

        /// <summary>
        /// 视图分页查询，返回视图对象
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<REntity>> GetPageListViewAsync<REntity>(PageOptions<REntity> options, out long total) where REntity : class, new();

        /// <summary>
        /// 视图分页查询，使用sql，返回视图对象
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<REntity> GetPageListView<REntity>(string sql, PageOptions<REntity> options, out long total) where REntity : class, new();

        /// <summary>
        /// 视图分页查询，使用sql，返回视图对象
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<REntity>> GetPageListViewAsync<REntity>(string sql, PageOptions<REntity> options, out long total) where REntity : class, new();

        /// <summary>
        /// 分页查询，自动构建where查询条件
        /// </summary>
        /// <param name="baseOption"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<REntity> GetPageListViewBasic<REntity>(Model.BaseRequestPageViewModel<REntity> baseOption, out long total) where REntity : class, new();

        /// <summary>
        /// 分页查询，自动构建where查询条件
        /// </summary>
        /// <param name="baseOption"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        Task<List<REntity>> GetPageListViewBasicAsync<REntity>(Model.BaseRequestPageViewModel<REntity> baseOption, out long total) where REntity : class, new();


        #endregion

        #endregion

    }
}
