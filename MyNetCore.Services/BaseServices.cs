using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.IServices;
using MyNetCore.IRepository;
using MyNetCore.Repository;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace MyNetCore.Services
{
    /// <summary>
    /// 数据库实体服务基础类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseServices<TEntity, TKey> : IBaseServices<TEntity> where TEntity : class, new() //Model.BaseEntity, new()
    {
        private IBaseMyRepository<TEntity> _baseRepo;
        protected ILogger _logger;

        public BaseServices(BaseMyRepository<TEntity, TKey> baseRepo, ILogger logger)
        {
            this._baseRepo = baseRepo;
            _logger = logger;
        }

        #region 添加

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>添加后的实体</returns>
        public TEntity Insert(TEntity entity)
        {
            return _baseRepo.Insert(entity);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>添加后的实体</returns>
        public Task<TEntity> InsertAsync(TEntity entity)
        {
            return _baseRepo.InsertAsync(entity);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public List<TEntity> Insert(IEnumerable<TEntity> entitys)
        {
            return _baseRepo.Insert(entitys);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public Task<List<TEntity>> InsertAsync(IEnumerable<TEntity> entitys)
        {
            return _baseRepo.InsertAsync(entitys);
        }

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity InsertOrUpdate(TEntity entity)
        {
            return _baseRepo.InsertOrUpdate(entity);
        }

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            return _baseRepo.InsertOrUpdateAsync(entity);
        }

        #endregion

        #region 修改

        /// <summary>
        /// 修改数据(用于不更新属性值为null或默认值的字段)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(TEntity entity)
        {
            return _baseRepo.Update(entity);
        }

        /// <summary>
        /// 修改数据(用于不更新属性值为null或默认值的字段)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> UpdateAsync(TEntity entity)
        {
            return _baseRepo.UpdateAsync(entity);
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
            return _baseRepo.UpdateOnlyChange(oldEntity, newEntity);
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
            return _baseRepo.UpdateOnlyChangeAsync(oldEntity, newEntity);
        }

        /// <summary>
        /// 修改数据(设置新的实体)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateEntity(TEntity entity)
        {
            return _baseRepo.UpdateEntity(entity);
        }

        /// <summary>
        /// 修改数据(设置新的实体)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> UpdateEntityAsync(TEntity entity)
        {
            return _baseRepo.UpdateEntityAsync(entity);
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
            return _baseRepo.DeleteByIds(ids);
        }

        /// <summary>
        /// 根据ids批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<int> DeleteByIdsAsync(object[] ids)
        {
            return _baseRepo.DeleteByIdsAsync(ids);
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
            return _baseRepo.GetCount(where);
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<long> GetCountAsync(Expression<Func<TEntity, bool>> where)
        {
            return _baseRepo.GetCountAsync(where);
        }

        /// <summary>
        /// 查询数量(原生sql语法条件，Where("id = @id", new { id = 1 }))
        /// </summary>
        /// <param name="sql">Sql语句 id = @id</param>
        /// <param name="parms">参数 new { id = 1 }</param>
        /// <returns></returns>
        public long GetCount(string sql, object parms = null)
        {
            return _baseRepo.GetCount(sql, parms);
        }

        /// <summary>
        /// 查询数量(原生sql语法条件，Where("id = @id", new { id = 1 }))
        /// </summary>
        /// <param name="sql">Sql语句 id = @id</param>
        /// <param name="parms">参数 new { id = 1 }</param>
        /// <returns></returns>
        public Task<long> GetCountAsync(string sql, object parms = null)
        {
            return _baseRepo.GetCountAsync(sql, parms);
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
            return _baseRepo.Exists(where);
        }

        /// <summary>
        /// 查询数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> where)
        {
            return _baseRepo.ExistsAsync(where);
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
            return _baseRepo.GetModel(dywhere);
        }

        /// <summary>
        /// 查询单条数据，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns></returns>
        public Task<TEntity> GetModelAsync(object dywhere)
        {
            return _baseRepo.GetModelAsync(dywhere);
        }

        /// <summary>
        /// 查询单条数据，返回ViewModel类型，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns></returns>
        public REntity GetModel<REntity>(object dywhere)
        {
            return _baseRepo.GetModel<REntity>(dywhere);
        }

        /// <summary>
        /// 查询单条数据，返回ViewModel类型，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns></returns>
        public Task<REntity> GetModelAsync<REntity>(object dywhere)
        {
            return _baseRepo.GetModelAsync<REntity>(dywhere);
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TEntity GetModel(Expression<Func<TEntity, bool>> where)
        {
            return _baseRepo.GetModel(where);
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<TEntity> GetModelAsync(Expression<Func<TEntity, bool>> where)
        {
            return _baseRepo.GetModelAsync(where);
        }

        /// <summary>
        /// 查询单条数据，返回ViewModel类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public REntity GetModel<REntity>(Expression<Func<TEntity, bool>> where)
        {
            return _baseRepo.GetModel<REntity>(where);
        }

        /// <summary>
        /// 查询单条数据，返回ViewModel类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<REntity> GetModelAsync<REntity>(Expression<Func<TEntity, bool>> where)
        {
            return _baseRepo.GetModelAsync<REntity>(where);
        }

        #region 视图

        /// <summary>
        /// 查询单条视图数据，返回视图类型，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns>REntity</returns>
        public REntity GetModelView<REntity>(object dywhere) where REntity : class
        {
            return _baseRepo.GetModelView<REntity>(dywhere);
        }

        /// <summary>
        /// 查询单条视图数据，返回视图类型，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns>REntity</returns>
        public Task<REntity> GetModelViewAsync<REntity>(object dywhere) where REntity : class
        {
            return _baseRepo.GetModelViewAsync<REntity>(dywhere);
        }

        /// <summary>
        /// 查询单条视图数据，返回视图类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns>REntity</returns>
        public REntity GetModelView<REntity>(Expression<Func<REntity, bool>> where) where REntity : class, new()
        {
            return _baseRepo.GetModelView(where);
        }

        /// <summary>
        /// 查询单条视图数据，返回视图类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns>REntity</returns>
        public Task<REntity> GetModelViewAsync<REntity>(Expression<Func<REntity, bool>> where) where REntity : class, new()
        {
            return _baseRepo.GetModelViewAsync(where);
        }

        #endregion

        #endregion

        #region 查询集合

        /// <summary>
        /// 查询列表，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> exp)
        {
            return _baseRepo.GetList(exp);
        }

        /// <summary>
        /// 查询列表，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        public Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> exp)
        {
            return _baseRepo.GetListAsync(exp);
        }

        /// <summary>
        /// 查询列表，返回DTO对象，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        public List<REntity> GetList<REntity>(Expression<Func<TEntity, bool>> exp)
        {
            return _baseRepo.GetList<REntity>(exp);
        }

        /// <summary>
        /// 查询列表，返回DTO对象，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        public Task<List<REntity>> GetListAsync<REntity>(Expression<Func<TEntity, bool>> exp)
        {
            return _baseRepo.GetListAsync<REntity>(exp);
        }

        /// <summary>
        /// 查询列表,原生sql语法条件，Where("id = @id", new { id = 1 })
        /// 提示：parms 参数还可以传 Dictionary<string, object>
        /// </summary>
        /// <param name="where">sql语法条件</param>
        /// <param name="parms">参数</param>
        /// <returns>List<TEntity></returns>
        public List<TEntity> GetList(string where, object parms = null)
        {
            return _baseRepo.GetList(where, parms);
        }

        /// <summary>
        /// 查询列表,原生sql语法条件，Where("id = @id", new { id = 1 })
        /// 提示：parms 参数还可以传 Dictionary<string, object>
        /// </summary>
        /// <param name="where">sql语法条件</param>
        /// <param name="parms">参数</param>
        /// <returns>List<TEntity></returns>
        public Task<List<TEntity>> GetListAsync(string where, object parms = null)
        {
            return _baseRepo.GetListAsync(where, parms);
        }

        /// <summary>
        /// 查询列表，返回DTO对象,原生sql语法条件，Where("id = @id", new { id = 1 })
        /// 提示：parms 参数还可以传 Dictionary<string, object>
        /// </summary>
        /// <param name="where">sql语法条件</param>
        /// <param name="parms">参数</param>
        /// <returns>List<TEntity></returns>
        public List<REntity> GetList<REntity>(string where, object parms = null)
        {
            return _baseRepo.GetList<REntity>(where, parms);
        }

        /// <summary>
        /// 查询列表，返回DTO对象,原生sql语法条件，Where("id = @id", new { id = 1 })
        /// 提示：parms 参数还可以传 Dictionary<string, object>
        /// </summary>
        /// <param name="where">sql语法条件</param>
        /// <param name="parms">参数</param>
        /// <returns>List<TEntity></returns>
        public Task<List<REntity>> GetListAsync<REntity>(string where, object parms = null)
        {
            return _baseRepo.GetListAsync<REntity>(where, parms);
        }

        #region 视图

        /// <summary>
        /// 查询视图列表，返回视图对象，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        public List<REntity> GetListView<REntity>(Expression<Func<REntity, bool>> exp) where REntity : class, new()
        {
            return _baseRepo.GetListView(exp);
        }

        /// <summary>
        /// 查询视图列表，返回视图对象，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        public Task<List<REntity>> GetListViewAsync<REntity>(Expression<Func<REntity, bool>> exp) where REntity : class, new()
        {
            return _baseRepo.GetListViewAsync(exp);
        }

        /// <summary>
        /// 查询视图列表，返回视图对象,原生sql语法条件，Where("id = @id", new { id = 1 })
        /// 提示：parms 参数还可以传 Dictionary<string, object>
        /// </summary>
        /// <param name="where">sql语法条件</param>
        /// <param name="parms">参数</param>
        /// <returns>List<TEntity></returns>
        public List<REntity> GetListView<REntity>(string where, object parms = null) where REntity : class, new()
        {
            return _baseRepo.GetListView<REntity>(where, parms);
        }

        /// <summary>
        /// 查询视图列表，返回视图对象,原生sql语法条件，Where("id = @id", new { id = 1 })
        /// 提示：parms 参数还可以传 Dictionary<string, object>
        /// </summary>
        /// <param name="where">sql语法条件</param>
        /// <param name="parms">参数</param>
        /// <returns>List<TEntity></returns>
        public Task<List<REntity>> GetListViewAsync<REntity>(string where, object parms = null) where REntity : class, new()
        {
            return _baseRepo.GetListViewAsync<REntity>(where, parms);
        }

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
        public List<TEntity> GetPageList(PageOptions<TEntity> options, out long total)
        {
            return _baseRepo.GetPageList(options, out total);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task<List<TEntity>> GetPageListAsync(PageOptions<TEntity> options, out long total)
        {
            return _baseRepo.GetPageListAsync(options, out total);
        }

        /// <summary>
        /// 分页查询，自动构建where查询条件
        /// </summary>
        /// <param name="baseOption"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<TEntity> GetPageListBasic(Model.BaseRequestPageViewModel<TEntity> baseOption, out long total)
        {
            baseOption.PageInfo.Where = baseOption.BuildPageSearchWhere();
            return GetPageList(baseOption.PageInfo, out total);
        }

        /// <summary>
        /// 分页查询，自动构建where查询条件
        /// </summary>
        /// <param name="baseOption"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task<List<TEntity>> GetPageListBasicAsync(Model.BaseRequestPageViewModel<TEntity> baseOption, out long total)
        {
            baseOption.PageInfo.Where = baseOption.BuildPageSearchWhere();
            return GetPageListAsync(baseOption.PageInfo, out total);
        }

        /// <summary>
        /// 分页查询，返回DTO对象
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<REntity> GetPageList<REntity>(PageOptions<TEntity> options, out long total)
        {
            return _baseRepo.GetPageList<REntity>(options, out total);
        }

        /// <summary>
        /// 分页查询，返回DTO对象
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task<List<REntity>> GetPageListAsync<REntity>(PageOptions<TEntity> options, out long total)
        {
            return _baseRepo.GetPageListAsync<REntity>(options, out total);
        }

        /// <summary>
        /// 分页查询，返回DTO对象，自动构建where查询条件
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<REntity> GetPageBasicList<REntity>(Model.BaseRequestPageViewModel<TEntity> options, out long total)
        {
            options.PageInfo.Where = options.BuildPageSearchWhere();
            return GetPageList<REntity>(options.PageInfo, out total);
        }

        /// <summary>
        /// 分页查询，返回DTO对象，自动构建where查询条件
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task<List<REntity>> GetPageListBasicAsync<REntity>(Model.BaseRequestPageViewModel<TEntity> options, out long total)
        {
            options.PageInfo.Where = options.BuildPageSearchWhere();
            return GetPageListAsync<REntity>(options.PageInfo, out total);
        }


        #endregion

        #region Sql

        /// <summary>
        /// 分页查询，使用sql
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<TEntity> GetPageList(string sql, PageOptions<TEntity> options, out long total)
        {
            return _baseRepo.GetPageList(sql, options, out total);
        }

        /// <summary>
        /// 分页查询，使用sql
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task<List<TEntity>> GetPageListAsync(string sql, PageOptions<TEntity> options, out long total)
        {
            return _baseRepo.GetPageListAsync(sql, options, out total);
        }

        /// <summary>
        /// 分页查询，使用sql，自动构建where查询条件
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<TEntity> GetPageBasicList(string sql, Model.BaseRequestPageViewModel<TEntity> options, out long total)
        {
            options.PageInfo.Where = options.BuildPageSearchWhere();
            return GetPageList(sql, options.PageInfo, out total);
        }

        /// <summary>
        /// 分页查询，使用sql，自动构建where查询条件
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task<List<TEntity>> GetPageListBasicAsync(string sql, Model.BaseRequestPageViewModel<TEntity> options, out long total)
        {
            options.PageInfo.Where = options.BuildPageSearchWhere();
            return GetPageListAsync(sql, options.PageInfo, out total);
        }

        /// <summary>
        /// 分页查询，使用sql，返回DTO对象
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<REntity> GetPageList<REntity>(string sql, PageOptions<TEntity> options, out long total)
        {
            return _baseRepo.GetPageList<REntity>(sql, options, out total);
        }

        /// <summary>
        /// 分页查询，使用sql，返回DTO对象
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task<List<REntity>> GetPageListAsync<REntity>(string sql, PageOptions<TEntity> options, out long total)
        {
            return _baseRepo.GetPageListAsync<REntity>(sql, options, out total);
        }

        /// <summary>
        /// 分页查询，使用sql，返回DTO对象，自动构建where查询条件
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<REntity> GetPageBasicList<REntity>(string sql, Model.BaseRequestPageViewModel<TEntity> options, out long total)
        {
            options.PageInfo.Where = options.BuildPageSearchWhere();
            return GetPageList<REntity>(options.PageInfo, out total);
        }

        /// <summary>
        /// 分页查询，使用sql，返回DTO对象，自动构建where查询条件
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task<List<REntity>> GetPageListBasicAsync<REntity>(string sql, Model.BaseRequestPageViewModel<TEntity> options, out long total)
        {
            options.PageInfo.Where = options.BuildPageSearchWhere();
            return GetPageListAsync<REntity>(options.PageInfo, out total);
        }

        #endregion

        #region 视图

        /// <summary>
        /// 视图分页查询，返回视图对象
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<REntity> GetPageListView<REntity>(PageOptions<REntity> options, out long total) where REntity : class, new()
        {
            return _baseRepo.GetPageListView(options, out total);
        }

        /// <summary>
        /// 视图分页查询，返回视图对象
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task<List<REntity>> GetPageListViewAsync<REntity>(PageOptions<REntity> options, out long total) where REntity : class, new()
        {
            return _baseRepo.GetPageListViewAsync(options, out total);
        }

        /// <summary>
        /// 视图分页查询，使用sql，返回视图对象
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<REntity> GetPageListView<REntity>(string sql, PageOptions<REntity> options, out long total) where REntity : class, new()
        {
            return _baseRepo.GetPageListView(sql, options, out total);
        }

        /// <summary>
        /// 视图分页查询，使用sql，返回视图对象
        /// </summary>
        /// <param name="sql">完整sql查询</param>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task<List<REntity>> GetPageListViewAsync<REntity>(string sql, PageOptions<REntity> options, out long total) where REntity : class, new()
        {
            return _baseRepo.GetPageListViewAsync(sql, options, out total);
        }

        /// <summary>
        /// 分页查询，自动构建where查询条件
        /// </summary>
        /// <param name="baseOption"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<REntity> GetPageListViewBasic<REntity>(Model.BaseRequestPageViewModel<REntity> baseOption, out long total) where REntity : class, new()
        {
            baseOption.PageInfo.Where = baseOption.BuildPageSearchWhere();
            return GetPageListView<REntity>(baseOption.PageInfo, out total);
        }

        /// <summary>
        /// 分页查询，自动构建where查询条件
        /// </summary>
        /// <param name="baseOption"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task<List<REntity>> GetPageListViewBasicAsync<REntity>(Model.BaseRequestPageViewModel<REntity> baseOption, out long total) where REntity : class, new()
        {
            baseOption.PageInfo.Where = baseOption.BuildPageSearchWhere();
            return GetPageListViewAsync<REntity>(baseOption.PageInfo, out total);
        }

        #endregion

        #endregion
    }
}
