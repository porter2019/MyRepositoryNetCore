﻿/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：经典父子分类业务逻辑接口
*│　作    者：杨习友
*│　版    本：1.0 使用Razor引擎自动生成
*│　创建时间：2021-04-04 17:30:04
*└──────────────────────────────────────────────────────────────┘
*/

namespace MyNetCore.IServices
{
    /// <summary>
    /// 经典父子分类业务类接口
    /// </summary>
    public interface IBookCategoryService : IBaseService<BookCategory>
    {
        /// <summary>
        /// 添加或修改实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<BookCategory> ModifyAsync(BookCategory entity);

        /// <summary>
        /// 获取树形列表
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        Task<List<BookCategory>> GetTreeListAsync(string title);

        /// <summary>
        /// 获取某条数据的父级或子级ids
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isParent"></param>
        /// <returns></returns>
        Task<string> GetParentOrChildIdsAsync(int id, bool isParent);

        /// <summary>
        /// 执行更新层级关系的存储过程
        /// </summary>
        /// <returns></returns>
        Task<int> ExecUpdateLayerProcAsync();

        /// <summary>
        /// 根据id删除所有数据，包括子数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteIncludeChildsAsync(int id);
    }
}