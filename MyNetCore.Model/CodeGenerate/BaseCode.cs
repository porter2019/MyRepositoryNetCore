using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetCore.Model.CodeGenerate
{
    /// <summary>
    /// 代码生成器所需实体基类
    /// </summary>
    public class BaseCode
    {
        public BaseCode() { }

        public BaseCode(string modelName, string modelDesc)
        {
            this.ModelName = modelName;
            this.ModelDesc = modelDesc;
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName => new String('a', 1).GetProjectMainName();

        /// <summary>
        /// 作者
        /// </summary>
        public string Author => "杨习友";

        /// <summary>
        /// 代码生成时间
        /// </summary>
        public string GeneratorTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// 实体名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 实体名称，首字母小写
        /// </summary>
        public string ModelVariableName
        {
            get
            {
                if (ModelName.IsNull()) return "";

                if (ModelName.Length < 2) return ModelName.ToLower();

                var fristChar = ModelName.Substring(0, 1);
                return fristChar.ToLower() + ModelName.Substring(1, ModelName.Length - 1);

            }
        }

        /// <summary>
        /// 实体中文名称
        /// </summary>
        public string ModelDesc { get; set; }
    }
}
