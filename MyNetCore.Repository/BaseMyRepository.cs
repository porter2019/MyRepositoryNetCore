using System.Linq.Expressions;

namespace MyNetCore.Repository
{
    /// <summary>
    /// 拓展FreeSql的IBaseRepository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class BaseMyRepository<TEntity, TKey> : BaseRepository<TEntity, TKey>, IBaseMyRepository<TEntity> where TEntity : class, new() //Model.BaseEntity
    {
        protected readonly IFreeSql _fsql;
        protected readonly ILogger _logger;

        public BaseMyRepository(IFreeSql fsql, ILogger logger) : base(fsql, null, null)
        {
            _fsql = fsql;
            _logger = logger;
        }

        #region 修改

        /// <summary>
        /// 修改数据(用于不更新属性值为null或默认值的字段)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int Update(TEntity entity)
        {
            var repo = _fsql.GetRepository<TEntity>();
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
            var repo = _fsql.GetRepository<TEntity>();
            repo.AttachOnlyPrimary(entity);

            return repo.UpdateAsync(entity);
        }

        /// <summary>
        /// 修改数据，指定的列为sql
        /// </summary>
        /// <param name="rowSql">Title='abc',TT=1</param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update(string rowSql, Expression<Func<TEntity, bool>> where)
        {
            var repo = _fsql.GetRepository<TEntity>();
            return repo.UpdateDiy.SetRaw(rowSql).Where(where).ExecuteAffrows();
        }

        /// <summary>
        /// 修改数据，指定的列为sql
        /// </summary>
        /// <param name="rowSql">Title='abc',TT=1</param>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<int> UpdateAsync(string rowSql, Expression<Func<TEntity, bool>> where)
        {
            var repo = _fsql.GetRepository<TEntity>();
            return repo.UpdateDiy.SetRaw(rowSql).Where(where).ExecuteAffrowsAsync();
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
            var repo = _fsql.GetRepository<TEntity>();
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
            var repo = _fsql.GetRepository<TEntity>();
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
            var runsql = _fsql.Update<TEntity>().SetSource(entity);

            return runsql.ExecuteAffrows();
        }

        /// <summary>
        /// 修改数据(设置新的实体)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> UpdateEntityAsync(TEntity entity)
        {
            var runsql = _fsql.Update<TEntity>().SetSource(entity);
            return runsql.ExecuteAffrowsAsync();
        }

        #endregion 修改

        #region 删除

        /// <summary>
        /// 根据ids批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteByIds(int[] ids)
        {
            if (typeof(TEntity).GetProperties().Any(p => p.Name == "IsDeleted"))
            {
                return _fsql.Update<TEntity>(ids).SetRaw("IsDeleted = 1").ExecuteAffrows();
            }
            else
            {
                return _fsql.Delete<TEntity>(ids).ExecuteAffrows();
            }
        }

        /// <summary>
        /// 根据ids批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<int> DeleteByIdsAsync(int[] ids)
        {
            if (typeof(TEntity).GetProperties().Any(p => p.Name == "IsDeleted"))
            {
                return _fsql.Update<TEntity>(ids).SetRaw("IsDeleted = 1").ExecuteAffrowsAsync();
            }
            else
            {
                return _fsql.Delete<TEntity>(ids).ExecuteAffrowsAsync();
            }
        }

        /// <summary>
        /// 根据ids批量删除数据，指定条件
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int DeleteByIds(int[] ids, Expression<Func<TEntity, bool>> where)
        {
            if (typeof(TEntity).GetProperties().Any(p => p.Name == "IsDeleted"))
            {
                return _fsql.Update<TEntity>(ids).SetRaw("IsDeleted = 1").Where(where).ExecuteAffrows();
            }
            else
            {
                return _fsql.Delete<TEntity>(ids).Where(where).ExecuteAffrows();
            }
        }

        /// <summary>
        /// 根据ids批量删除数据，指定条件
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<int> DeleteByIdsAsync(int[] ids, Expression<Func<TEntity, bool>> where)
        {
            if (typeof(TEntity).GetProperties().Any(p => p.Name == "IsDeleted"))
            {
                return _fsql.Update<TEntity>(ids).SetRaw("IsDeleted = 1").Where(where).ExecuteAffrowsAsync();
            }
            else
            {
                return _fsql.Delete<TEntity>(ids).Where(where).ExecuteAffrowsAsync();
            }
        }

        /// <summary>
        /// 根据条件批量删除数据，指定条件
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int DeleteByWhere(Expression<Func<TEntity, bool>> where)
        {
            if (typeof(TEntity).GetProperties().Any(p => p.Name == "IsDeleted"))
            {
                return _fsql.Update<TEntity>().SetRaw("IsDeleted = 1").Where(where).ExecuteAffrows();
            }
            else
            {
                return _fsql.Delete<TEntity>().Where(where).ExecuteAffrows();
            }
        }

        /// <summary>
        /// 根据条件批量删除数据，指定条件
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<int> DeleteByWhereAsync(Expression<Func<TEntity, bool>> where)
        {
            if (typeof(TEntity).GetProperties().Any(p => p.Name == "IsDeleted"))
            {
                return _fsql.Update<TEntity>().SetRaw("IsDeleted = 1").Where(where).ExecuteAffrowsAsync();
            }
            else
            {
                return _fsql.Delete<TEntity>().Where(where).ExecuteAffrowsAsync();
            }
        }

        /// <summary>
        /// 根据条件批量删除数据，指定条件
        /// </summary>
        /// <param name="where"></param>
        /// <param name="exp"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int DeleteByWhereIf(Expression<Func<TEntity, bool>> where, bool condition, Expression<Func<TEntity, bool>> exp)
        {
            if (typeof(TEntity).GetProperties().Any(p => p.Name == "IsDeleted"))
            {
                return _fsql.Update<TEntity>().SetRaw("IsDeleted = 1").Where(where).WhereIf(condition, exp).ExecuteAffrows();
            }
            else
            {
                return _fsql.Delete<TEntity>().Where(where).WhereIf(condition, exp).ExecuteAffrows();
            }
        }

        /// <summary>
        /// 根据条件批量删除数据，指定条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="exp"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<int> DeleteByWhereIfAsync(Expression<Func<TEntity, bool>> where, bool condition, Expression<Func<TEntity, bool>> exp)
        {
            if (typeof(TEntity).GetProperties().Any(p => p.Name == "IsDeleted"))
            {
                return _fsql.Update<TEntity>().SetRaw("IsDeleted = 1").Where(where).WhereIf(condition, exp).ExecuteAffrowsAsync();
            }
            else
            {
                return _fsql.Delete<TEntity>().Where(where).WhereIf(condition, exp).ExecuteAffrowsAsync();
            }
        }

        #endregion 删除

        #region 查询数量

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public long GetCount(Expression<Func<TEntity, bool>> where)
        {
            return _fsql.Select<TEntity>().Where(where).Count();
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<long> GetCountAsync(Expression<Func<TEntity, bool>> where)
        {
            return _fsql.Select<TEntity>().Where(where).CountAsync();
        }

        /// <summary>
        /// 查询数量(原生sql语法条件，Where("id = @id", new { id = 1 }))
        /// </summary>
        /// <param name="sql">Sql语句 id = @id</param>
        /// <param name="parms">参数 new { id = 1 }</param>
        /// <returns></returns>
        public long GetCount(string sql, object parms = null)
        {
            return _fsql.Select<TEntity>().Where(sql, parms).Count();
        }

        /// <summary>
        /// 查询数量(原生sql语法条件，Where("id = @id", new { id = 1 }))
        /// </summary>
        /// <param name="sql">Sql语句 id = @id</param>
        /// <param name="parms">参数 new { id = 1 }</param>
        /// <returns></returns>
        public Task<long> GetCountAsync(string sql, object parms = null)
        {
            return _fsql.Select<TEntity>().Where(sql, parms).CountAsync();
        }

        #endregion 查询数量

        #region 是否存在

        /// <summary>
        /// 查询数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Exists(Expression<Func<TEntity, bool>> where)
        {
            return _fsql.Select<TEntity>().Any(where);
        }

        /// <summary>
        /// 查询数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> where)
        {
            return _fsql.Select<TEntity>().AnyAsync(where);
        }

        /// <summary>
        /// 查询数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <param name="condition"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public bool ExistsWhereIf(Expression<Func<TEntity, bool>> where, bool condition, Expression<Func<TEntity, bool>> exp)
        {
            return _fsql.Select<TEntity>().Where(where).WhereIf(condition, exp).Count() > 0;
        }

        /// <summary>
        /// 查询数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <param name="condition"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<bool> ExistsWhereIfAsync(Expression<Func<TEntity, bool>> where, bool condition, Expression<Func<TEntity, bool>> exp)
        {
            return (await _fsql.Select<TEntity>().Where(where).WhereIf(condition, exp).CountAsync()) > 0;
        }

        #endregion 是否存在

        #region 查询单条数据

        /// <summary>
        /// 查询单条数据，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns></returns>
        public TEntity GetModel(object dywhere)
        {
            return _fsql.Select<TEntity>(dywhere).ToOne();
        }

        /// <summary>
        /// 查询单条数据，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns></returns>
        public Task<TEntity> GetModelAsync(object dywhere)
        {
            return _fsql.Select<TEntity>(dywhere).ToOneAsync();
        }

        /// <summary>
        /// 查询单条数据，返回ViewModel类型，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns></returns>
        public REntity GetModel<REntity>(object dywhere)
        {
            return _fsql.Select<TEntity>(dywhere).ToOne<REntity>();
        }

        /// <summary>
        /// 查询单条数据，返回ViewModel类型，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns></returns>
        public Task<REntity> GetModelAsync<REntity>(object dywhere)
        {
            return _fsql.Select<TEntity>(dywhere).ToOneAsync<REntity>();
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TEntity GetModel(Expression<Func<TEntity, bool>> where)
        {
            return _fsql.Select<TEntity>().Where(where).ToOne();
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<TEntity> GetModelAsync(Expression<Func<TEntity, bool>> where)
        {
            return _fsql.Select<TEntity>().Where(where).ToOneAsync();
        }

        /// <summary>
        /// 查询单条数据，指定排序
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public Task<TEntity> GetModelAsync(Expression<Func<TEntity, bool>> where, string orderBy)
        {
            return _fsql.Select<TEntity>().Where(where).OrderBy(orderBy).ToOneAsync();
        }

        /// <summary>
        /// 查询单条数据，返回ViewModel类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public REntity GetModel<REntity>(Expression<Func<TEntity, bool>> where)
        {
            return _fsql.Select<TEntity>().Where(where).ToOne<REntity>();
        }

        /// <summary>
        /// 查询单条数据，返回ViewModel类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<REntity> GetModelAsync<REntity>(Expression<Func<TEntity, bool>> where)
        {
            return _fsql.Select<TEntity>().Where(where).ToOneAsync<REntity>();
        }

        #region 视图

        /// <summary>
        /// 查询单条视图数据，返回视图类型，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns>REntity</returns>
        public REntity GetModelView<REntity>(object dywhere) where REntity : class
        {
            return _fsql.Select<REntity>(dywhere).ToOne();
        }

        /// <summary>
        /// 查询单条视图数据，返回视图类型，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} | new{id=1}
        /// </summary>
        /// <param name="dywhere">主键值、主键值集合、实体、实体集合、匿名对象、匿名对象集合</param>
        /// <returns>REntity</returns>
        public Task<REntity> GetModelViewAsync<REntity>(object dywhere) where REntity : class
        {
            return _fsql.Select<REntity>(dywhere).ToOneAsync();
        }

        /// <summary>
        /// 查询单条视图数据，返回视图类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns>REntity</returns>
        public REntity GetModelView<REntity>(Expression<Func<REntity, bool>> where) where REntity : class, new()
        {
            return _fsql.Select<REntity>().Where(where).ToOne();
        }

        /// <summary>
        /// 查询单条视图数据，返回视图类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns>REntity</returns>
        public Task<REntity> GetModelViewAsync<REntity>(Expression<Func<REntity, bool>> where) where REntity : class, new()
        {
            return _fsql.Select<REntity>().Where(where).ToOneAsync();
        }

        #endregion 视图

        #endregion 查询单条数据

        #region 查询集合

        /// <summary>
        /// 查询列表，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> exp)
        {
            return _fsql.Select<TEntity>().Where(exp).ToList();
        }

        /// <summary>
        /// 查询列表，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        public Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> exp)
        {
            return _fsql.Select<TEntity>().Where(exp).ToListAsync();
        }

        /// <summary>
        /// 查询列表，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <param name="orderBy">排序</param>
        /// <returns>List<TEntity></returns>
        public Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> exp, string orderBy)
        {
            return _fsql.Select<TEntity>().Where(exp).OrderBy(orderBy).ToListAsync();
        }

        /// <summary>
        /// 查询列表，返回DTO对象，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        public List<REntity> GetList<REntity>(Expression<Func<TEntity, bool>> exp)
        {
            return _fsql.Select<TEntity>().Where(exp).ToList<REntity>();
        }

        /// <summary>
        /// 查询列表，返回DTO对象，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        public Task<List<REntity>> GetListAsync<REntity>(Expression<Func<TEntity, bool>> exp)
        {
            return _fsql.Select<TEntity>().Where(exp).ToListAsync<REntity>();
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
            return _fsql.Select<TEntity>().Where(where, parms).ToList();
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
            return _fsql.Select<TEntity>().Where(where, parms).ToListAsync();
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
            return _fsql.Select<TEntity>().Where(where, parms).ToList<REntity>();
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
            return _fsql.Select<TEntity>().Where(where, parms).ToListAsync<REntity>();
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
            return _fsql.Select<REntity>().Where(exp).ToList();
        }

        /// <summary>
        /// 查询视图列表，返回视图对象，Where(a => a.Id > 10)，支持导航对象查询，Where(a => a.Author.Email == "2881099@qq.com")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp">lambda表达式</param>
        /// <returns>List<TEntity></returns>
        public Task<List<REntity>> GetListViewAsync<REntity>(Expression<Func<REntity, bool>> exp) where REntity : class, new()
        {
            return _fsql.Select<REntity>().Where(exp).ToListAsync();
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
            return _fsql.Select<REntity>().Where(where, parms).ToList();
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
            return _fsql.Select<REntity>().Where(where, parms).ToListAsync();
        }

        #endregion 视图

        #endregion 查询集合

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
            return _fsql.Select<TEntity>()
                        //.WhereIf(options.Where.IsNull(), "1=1 " + options.Where)
                        .Where("1=1 " + options.Where)
                        .Count(out total).Page(options.PageIndex, options.PageSize)
                        .OrderBy(options.OrderBy)
                        .ToList();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task<List<TEntity>> GetPageListAsync(PageOptions<TEntity> options, out long total)
        {
            return _fsql.Select<TEntity>()
                        //.WhereIf(options.Where.IsNotNull(), "1=1 " + options.Where)
                        .Where("1=1 " + options.Where)
                        .Count(out total).Page(options.PageIndex, options.PageSize)
                        .OrderBy(options.OrderBy)
                        .ToListAsync();
        }

        /// <summary>
        /// 分页查询，返回DTO对象
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<REntity> GetPageList<REntity>(PageOptions<TEntity> options, out long total)
        {
            return _fsql.Select<TEntity>()
                        //.WhereIf(options.Where.IsNull(), "1=1 " + options.Where)
                        .Where("1=1 " + options.Where)
                        .Count(out total).Page(options.PageIndex, options.PageSize)
                        .OrderBy(options.OrderBy)
                        .ToList<REntity>();
        }

        /// <summary>
        /// 分页查询，返回DTO对象
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task<List<REntity>> GetPageListAsync<REntity>(PageOptions<TEntity> options, out long total)
        {
            return _fsql.Select<TEntity>()
                        //.WhereIf(options.Where.IsNotNull(), "1=1 " + options.Where)
                        .Where("1=1 " + options.Where)
                        .Count(out total).Page(options.PageIndex, options.PageSize)
                        .OrderBy(options.OrderBy)
                        .ToListAsync<REntity>();
        }

        #endregion PageOptions

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
            return _fsql.Select<TEntity>()
                        .WithSql(sql)
                        .Count(out total).Page(options.PageIndex, options.PageSize)
                        .OrderBy(options.OrderBy)
                        .ToList();
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
            return _fsql.Select<TEntity>()
                        .WithSql(sql)
                        .Count(out total).Page(options.PageIndex, options.PageSize)
                        .OrderBy(options.OrderBy)
                        .ToListAsync();
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
            return _fsql.Select<TEntity>()
                        .WithSql(sql)
                        .WhereIf(options.Where.IsNotNull(), "1=1 " + options.Where)
                        .Count(out total).Page(options.PageIndex, options.PageSize)
                        .OrderBy(options.OrderBy)
                        .ToList<REntity>();
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
            return _fsql.Select<TEntity>()
                        .WithSql(sql)
                        .WhereIf(options.Where.IsNotNull(), "1=1 " + options.Where)
                        .Count(out total).Page(options.PageIndex, options.PageSize)
                        .OrderBy(options.OrderBy)
                        .ToListAsync<REntity>();
        }

        #endregion Sql

        #region 视图

        /// <summary>
        /// 视图分页查询，返回视图对象
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<REntity> GetPageListView<REntity>(PageOptions<REntity> options, out long total) where REntity : class, new()
        {
            return _fsql.Select<REntity>()
                        .Where("1=1 " + options.Where)
                        .Count(out total).Page(options.PageIndex, options.PageSize)
                        .OrderBy(options.OrderBy)
                        .ToList();
        }

        /// <summary>
        /// 视图分页查询，返回视图对象
        /// </summary>
        /// <param name="options"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Task<List<REntity>> GetPageListViewAsync<REntity>(PageOptions<REntity> options, out long total) where REntity : class, new()
        {
            return _fsql.Select<REntity>()
                        //.WhereIf(options.Where.IsNull(), "1=1 " + options.Where)
                        .Where("1=1 " + options.Where)
                        .Count(out total).Page(options.PageIndex, options.PageSize)
                        .OrderBy(options.OrderBy)
                        .ToListAsync();
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
            string where = string.Empty;
            if (options.Where.IsNotNull())
            {
                if (options.Where.StartsWith("and")) where = "1=1 " + options.Where;
                else where = options.Where;
            }
            return _fsql.Select<REntity>()
                        .WithSql(sql)
                        .WhereIf(where.IsNotNull(), where)
                        .Count(out total).Page(options.PageIndex, options.PageSize)
                        .OrderBy(options.OrderBy)
                        .ToList();
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
            return _fsql.Select<REntity>()
                        .WithSql(sql)
                        .Count(out total).Page(options.PageIndex, options.PageSize)
                        .OrderBy(options.OrderBy)
                        .ToListAsync();
        }

        #endregion 视图

        #endregion 查询分页
    }
}