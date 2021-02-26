using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.IServices
{
    /// <summary>
    /// 业务积累
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseServices<TEntity> where TEntity : class
    {

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> Add(TEntity entity);

        /// <summary>
        /// 根据主键查找一条数据
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        Task<TEntity> QueryByID(object objId);

        /// <summary>
        /// 修改一条数据，返回受影响的行数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> Update(TEntity entity);

        /// <summary>
        /// 根据ids批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteByIds(object[] ids);

    }
}
