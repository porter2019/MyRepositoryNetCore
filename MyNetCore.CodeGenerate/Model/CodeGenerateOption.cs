using System;
using System.Collections.Generic;
using System.Text;

namespace MyNetCore.CodeGenerate
{
    public class CodeGenerateOption
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

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
