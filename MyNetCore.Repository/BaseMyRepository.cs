using FreeSql;
using MyNetCore.IRepository;
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

        /// <summary>
        /// 根据ids批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteByIds(string ids)
        {
            return _freeSql.Delete<TEntity>(ids.SplitWithComma()).ExecuteAffrows();
        }

        /// <summary>
        /// 根据ids批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteByIdsSync(string ids)
        {
            return await _freeSql.Delete<TEntity>(ids.SplitWithComma()).ExecuteAffrowsAsync();
        }
    }
}
