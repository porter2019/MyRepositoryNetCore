/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：经典父子分类业务逻辑接口                                                    
*│　作    者：杨习友                                          
*│　版    本：1.0 使用Razor引擎自动生成                                              
*│　创建时间：2021-04-04 17:30:04                            
*└──────────────────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNetCore.IServices;
using MyNetCore.IRepository;
using MyNetCore.Repository;
using MyNetCore.Model.Entity;


namespace MyNetCore.Services
{
    /// <summary>
    /// 经典父子分类业务实现类
    /// </summary>
    [ServiceLifetime()]
    public class BookCategoryServices : BaseServices<BookCategory, int>, IBookCategoryServices
    {
        private readonly IBookCategoryRepository _bookCategoryRepository;

        public BookCategoryServices(BookCategoryRepository bookCategoryRepository) : base(bookCategoryRepository)
        {
            _bookCategoryRepository = bookCategoryRepository;
        }

        /// <summary>
        /// 获取树形列表
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Task<List<BookCategory>> GetTreeList(string title)
        {
            return _bookCategoryRepository.Select.WhereIf(title.IsNotNull(), p => p.Title.Contains(title)).ToTreeListAsync();
        }

        /// <summary>
        /// 获取某条数据的父级或子级ids
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isParent"></param>
        /// <returns></returns>
        public Task<string> GetParentOrChildIds(int id, bool isParent)
        {
            var fnName = isParent ? "fn_GetCategoryParentIds" : "fn_GetCategoryChildIds";
            string sql = $"select dbo.{fnName}({id})";
            return _bookCategoryRepository.Orm.Ado.QuerySingleAsync<string>(sql);
        }

        /// <summary>
        /// 根据id删除所有数据，包括子数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<int> DeleteIncludeChilds(int id)
        {
            //软删除
            return _bookCategoryRepository.Select.Where(p => p.BookId == id)
                                                 .AsTreeCte()
                                                 .ToUpdate()
                                                 .Set(a => a.IsDeleted, true)
                                                 .ExecuteAffrowsAsync();
            ////物理删除
            //return _bookCategoryRepository.Select.Where(p => p.BookId == id)
            //                                     .AsTreeCte()
            //                                     .ToDelete()
            //                                     .ExecuteAffrowsAsync();
        }

    }
}
