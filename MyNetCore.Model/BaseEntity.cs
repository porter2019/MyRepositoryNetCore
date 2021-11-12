using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyNetCore.Model
{
    /// <summary>
    /// 实体基类，FreeSql文档：https://github.com/dotnetcore/FreeSql/wiki
    /// </summary>
    public class BaseEntity
    {
        protected readonly IConfiguration _config;

        public BaseEntity()
        {
            if (ServiceLocator.Instance == null) throw new Exception("单例ServiceLocator.Instance为NULL");
            _config = ServiceLocator.Instance.GetService<IConfiguration>();
            if (_config == null) throw new Exception("获取不到注入的文件配置对象");
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FsColumn("创建时间", ServerTime = DateTimeKind.Local, CanUpdate = false, Position = -3)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [FsColumn("更新时间", ServerTime = DateTimeKind.Local, Position = -4)]
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [FsColumn("是否删除", Position = -2)]
        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 乐观锁
        /// </summary>
        [FsColumn("乐观锁", IsVersion = true, Position = -1)]
        public int Version { get; set; }

        #region 只读属性

        [FsColumn("主键Id", IsIgnore = true)]
        public int Id
        {
            get
            {
                var newType = this.GetType();
                foreach (var item in newType.GetRuntimeProperties())
                {
                    var customAttr = item.CustomAttributes.Where(p => p.AttributeType == typeof(FsColumnAttribute)).ToList().FirstOrDefault();
                    if (customAttr == null) continue;
                    var customAttrIsPK = customAttr.NamedArguments.Where(p => p.MemberName == "IsPK").FirstOrDefault();
                    if (customAttrIsPK.TypedValue.Value.ObjToBool()) return item.GetValue(this).ObjToInt(0);
                }

                return 0;
            }
        }

        /// <summary>
        /// 创建时间，yyyy-MM-dd HH:mm
        /// </summary>
        [FsColumn("时间1", IsIgnore = true)]
        public string CreatedDate1
        {
            get
            {
                return CreatedDate.ToString("yyyy-MM-dd HH:mm");
            }
        }

        /// <summary>
        /// 更新时间，yyyy-MM-dd HH:mm
        /// </summary>
        [FsColumn("时间1", IsIgnore = true)]
        public string UpdatedDate1
        {
            get
            {
                return UpdatedDate.ToString("yyyy-MM-dd HH:mm");
            }
        }

        ///// <summary>
        ///// 创建时间，MM-dd HH:mm
        ///// </summary>
        //[FsColumn("时间2", IsIgnore = true)]
        //public string CreateDate2
        //{
        //    get
        //    {
        //        return CreatedDate.ToString("MM-dd HH:mm");
        //    }
        //}

        /// <summary>
        /// 附件列表
        /// </summary>
        public List<Entity.CommonAttach> Attachs { get; set; } = new List<Entity.CommonAttach>();

        #endregion

    }

    /// <summary>
    /// 实体基类，包含了创建者信息
    /// </summary>
    public class BaseEntityStandard : BaseEntity
    {
        /// <summary>
        /// 创建者ID
        /// </summary>
        [FsColumn("创建者ID", CanUpdate = false, Position = -6)]
        [JsonIgnore]
        public int? CreatedUserId { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        [FsColumn("创建者名称", CanUpdate = false, Position = -5, StringLength = 50)]
        public string CreatedUserName { get; set; } = "";

        /// <summary>
        /// 修改者ID
        /// </summary>
        [FsColumn("修改者ID", Position = -7)]
        [JsonIgnore]
        public int? UpdatedUserId { get; set; }

        /// <summary>
        /// 修改者名称
        /// </summary>
        [FsColumn("修改者名称", Position = -8, StringLength = 50)]
        public string UpdatedUserName { get; set; } = "";

        /// <summary>
        /// 浅复制
        /// </summary>
        /// <returns>新的实体对象</returns>
        public TEntity ShallowCopy<TEntity>() where TEntity : BaseEntity
        {
            return MemberwiseClone() as TEntity;
        }

    }

}
