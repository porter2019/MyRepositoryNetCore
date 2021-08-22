using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyNetCore.Model.CodeGenerate
{
    /// <summary>
    /// 实体属性集
    /// </summary>
    public class EntityPropertys : BaseCode
    {
        /// <summary>
        /// 表属性
        /// </summary>
        [JsonIgnore]
        public FsTableAttribute TableInfo { get; set; }

        /// <summary>
        /// 属性集合
        /// </summary>
        [JsonIgnore]
        public List<EntityPropertysItems> PropertysItems { get; set; } = new List<EntityPropertysItems>();

        /// <summary>
        /// Vue模块名称
        /// </summary>
        public string VueModuleName
        {
            get
            {
                return TableInfo != null ? TableInfo.VueModuleName : "";
            }
        }

        public bool HaveItems
        {
            get
            {
                return TableInfo != null ? TableInfo.HaveItems : false;
            }
        }

        /// <summary>
        /// 生成index的table列
        /// </summary>
        /// <returns></returns>
        public string GenerateIndexTableItems()
        {
            StringBuilder html = new StringBuilder();

            if (!PropertysItems.Any())
            {
                //给一些演示的列
                html.AppendLine("<el-table-column label=\"标题\" prop=\"Title\" sortable=\"custom\" fixed=\"left\" width=\"200\" align=\"center\" header-align=\"center\" show-overflow-tooltip></el-table-column>");
                html.AppendLine("<el-table-column label=\"数量\" prop=\"Num\" sortable=\"custom\" min-width=\"80\" align=\"center\" header-align=\"center\" show-overflow-tooltip></el-table-column>");
                html.AppendLine("<el-table-column label=\"数量2\" prop=\"Num\" sortable=\"custom\" min-width=\"90\" align=\"center\" header-align=\"center\" show-overflow-tooltip></el-table-column>");
                html.AppendLine("<el-table-column label=\"性别\" prop=\"SexText\" prop2=\"Sex\" sortable=\"custom\" min-width=\"80\" align=\"center\" header-align=\"center\" show-overflow-tooltip></el-table-column>");
                html.AppendLine("<el-table-column label=\"双精度\" prop=\"ValueD\" sortable=\"custom\" :formatter=\"(row,column,cellValue,index)=>$numberUtil.formatMoney(cellValue)\" min-width=\"90\" align=\"center\" header-align=\"center\" show-overflow-tooltip></el-table-column>");
                html.AppendLine("<el-table-column label=\"金额\" prop=\"ValueDe\" sortable=\"custom\" :formatter=\"(row,column,cellValue,index)=>$numberUtil.formatMoney(cellValue)\" min-width=\"80\" align=\"center\" header-align=\"center\" show-overflow-tooltip></el-table-column>");
                html.AppendLine("<el-table-column label=\"日期\" prop=\"Date1\" min-width=\"80\" :formatter=\"(row,column,cellValue,index)=>$dateUtil.formatDate(cellValue)\" align=\"center\" header-align=\"center\" show-overflow-tooltip></el-table-column>");
                html.AppendLine("<el-table-column label=\"日期时间\" prop=\"Date2\" width=\"160\" :formatter=\"(row,column,cellValue,index)=>$dateUtil.formatDate(cellValue,'yyyy-MM-dd hh:mm')\" align=\"center\" header-align=\"center\" show-overflow-tooltip></el-table-column>");
                html.AppendLine("<el-table-column label=\"更新时间\" prop=\"UpdatedDate1\" sortable=\"custom\" width=\"160\" align=\"center\" show-overflow-tooltip></el-table-column>");
                html.AppendLine("<el-table-column label=\"更新者\" prop=\"UpdatedUserName\" min-width=\"90\" align=\"center\" show-overflow-tooltip></el-table-column>");
                html.AppendLine("<el-table-column label=\"状态\" prop=\"Status\" sortable=\"custom\" width=\"100\" align=\"center\" fixed=\"right\">");
                html.AppendLine("    <template slot-scope=\"{row}\">");
                html.AppendLine("        <el-tag v-if=\"row.Status\" type=\"success\" size=\"small\" effect=\"light\">正常</el-tag>");
                html.AppendLine("        <el-tag v-else type=\"danger\" size=\"small\" effect=\"light\">禁用</el-tag>");
                html.AppendLine("    </template>");
                html.AppendLine("</el-table-column>");
                return html.ToString();
            }

            foreach (var item in PropertysItems)
            {
                if (item.ColumnInfo.IsPK) continue;
                var name = item.ColumnTypeInfo.Name;
                var desc = item.ColumnInfo.DisplayName;
                if (item.ColumnInfo.DbType?.ToLower() == "text" || name == "Remark") continue;
                if (name == "CreatedUserId" || name == "UpdatedUserId" || name == "IsDeleted" || name == "Version") continue;
                var dataType = item.ColumnTypeInfo.PropertyType.FullName;
                if (dataType.Contains("System.Int32"))
                    html.AppendLine($"<el-table-column label=\"{desc}\" prop=\"{name}\" sortable=\"custom\" min-width=\"80\" align=\"center\" header-align=\"center\" show-overflow-tooltip></el-table-column>");
                else if (dataType.Contains("System.String"))
                    html.AppendLine($"<el-table-column label=\"{desc}\" prop=\"{name}\" sortable=\"custom\" width=\"200\" align=\"center\" header-align=\"center\" show-overflow-tooltip></el-table-column>");
                else if (dataType.Contains("System.Double"))
                {
                    if (item.ColumnInfo.DbType?.ToLower() == "money")
                    {
                        html.AppendLine($"<el-table-column label=\"{desc}\" prop=\"{name}\" sortable=\"custom\" :formatter=\"(row,column,cellValue,index)=>$numberUtil.formatMoney(cellValue)\" min-width=\"90\" align=\"right\" header-align=\"center\" show-overflow-tooltip></el-table-column>");
                    }
                    else
                    {
                        html.AppendLine($"<el-table-column label=\"{desc}\" prop=\"{name}\" sortable=\"custom\" min-width=\"90\" align=\"right\" header-align=\"center\" show-overflow-tooltip></el-table-column>");
                    }
                }
                else if (dataType.Contains("System.Decimal"))
                    html.AppendLine($"<el-table-column label=\"{desc}\" prop=\"{name}\" sortable=\"custom\" min-width=\"90\" align=\"right\" header-align=\"center\" show-overflow-tooltip></el-table-column>");
                else if (dataType.Contains("System.DateTime"))
                {
                    if (name == "CreatedDate")
                    {
                        html.AppendLine($"<el-table-column label=\"{desc}\" prop=\"CreatedDate1\" sortable=\"custom\" width=\"160\" align=\"center\" show-overflow-tooltip></el-table-column>");
                    }
                    else if (name == "UpdatedDate")
                    {
                        html.AppendLine($"<el-table-column label=\"{desc}\" prop=\"UpdatedDate1\" sortable=\"custom\" width=\"160\" align=\"center\" show-overflow-tooltip></el-table-column>");
                    }
                    else
                    {
                        html.AppendLine($"<el-table-column label=\"{desc}\" prop=\"{name}\" min-width=\"160\" :formatter=\"(row,column,cellValue,index)=>$dateUtil.formatDate(cellValue,'yyyy-MM-dd hh:mm')\" align=\"center\" header-align=\"center\" show-overflow-tooltip></el-table-column>");
                    }
                }
                else if (dataType.Contains("System.Boolean"))
                {
                    html.AppendLine($"<el-table-column label=\"{desc}\" prop=\"{name}\" sortable=\"custom\" width=\"100\" align=\"center\">");
                    html.AppendLine("    <template slot-scope=\"{row}\">");
                    html.AppendLine($"        <el-tag v-if=\"row.{name}\" type=\"success\" size=\"small\" effect=\"light\">正常</el-tag>");
                    html.AppendLine("        <el-tag v-else type=\"danger\" size=\"small\" effect=\"light\">禁用</el-tag>");
                    html.AppendLine("    </template>");
                    html.AppendLine("</el-table-column>");
                }
                else if (dataType.StartsWith($"{item.GetProjectMainName()}.Model")) //枚举
                {
                    //枚举显示的名称默认Text结尾
                    html.AppendLine($"<el-table-column label=\"{desc}\" prop=\"{name}Text\" prop2=\"{name}\" sortable=\"custom\" min-width=\"80\" align=\"center\" header-align=\"center\" show-overflow-tooltip></el-table-column>");
                }
                else
                {
                    html.AppendLine($"<el-table-column label=\"{desc}\" prop=\"{name}\" sortable=\"custom\" width=\"200\" align=\"center\" header-align=\"center\" show-overflow-tooltip></el-table-column>");
                }
            }

            return html.ToString();
        }

        /// <summary>
        /// 生成Edit中的Form列
        /// </summary>
        /// <returns></returns>
        public string GenerateEditRow()
        {
            StringBuilder html = new StringBuilder();
            if (!PropertysItems.Any()) return "";
            bool needCloseTag = false;

            for (int i = 1; i <= PropertysItems.Count; i++)
            {
                var item = PropertysItems[i - 1];

                if (item.ColumnInfo.IsPK) continue;
                var name = item.ColumnTypeInfo.Name;
                var desc = item.ColumnInfo.DisplayName;
                var isIgonPro = new string[] { "CreatedUserId", "CreatedUserName", "CreatedDate", "UpdatedUserId", "UpdatedUserName", "UpdatedDate", "IsDeleted", "Version" };
                if (isIgonPro.Contains(name)) continue;

                StringBuilder inputHtml = new();

                var dataType = item.ColumnTypeInfo.PropertyType.FullName;
                if (dataType.Contains("System.Int32"))
                {
                    inputHtml.Append($"<el-input type=\"text\" v-model=\"formData.{name}\" clearable placeholder=\"请输入{desc}\"></el-input>");
                }
                else if (dataType.Contains("System.String"))
                {
                    if (name == "Remark")
                    {
                        inputHtml.Append("<el-input type=\"textarea\" v-model=\"formData." + name + "\" :autosize=\"{ minRows: 3, maxRows: 5}\" maxlength=\"200\" clearable placeholder=\"请输入" + desc + "\"></el-input>");
                    }
                    else if (item.ColumnInfo.DbType?.ToLower() == "text")
                    {
                        inputHtml.Append($"<editor v-model=\"formData.{name}\" />");
                    }
                    else
                        inputHtml.Append($"<el-input type=\"text\" v-model=\"formData.{name}\" clearable placeholder=\"请输入{desc}\"></el-input>");
                }
                else if (dataType.Contains("System.Double"))
                {
                    if (item.ColumnInfo.DbType?.ToLower() == "money")
                    {
                        inputHtml.Append($"<el-input type=\"text\" v-model=\"formData.{name}\" clearable placeholder=\"请输入{desc}\"><template slot=\"prepend\">￥</template></el-input>");
                    }
                    else
                    {
                        inputHtml.Append($"<el-input type=\"text\" v-model=\"formData.{name}\" clearable placeholder=\"请输入{desc}\"></el-input>");
                    }
                }
                else if (dataType.Contains("System.Decimal"))
                {
                    inputHtml.Append($"            <el-input type=\"text\" v-model=\"formData.{name}\" clearable placeholder=\"请输入{desc}\"></el-input>");
                }
                else if (dataType.Contains("System.DateTime"))
                {
                    inputHtml.Append($"<el-date-picker v-model=\"formData.{name}\" type=\"datetime\" value-format=\"yyyy-MM-dd HH:mm:ss\" :default-value=\"new Date()\" placeholder=\"选择\"></el-date-picker>");
                }
                else if (dataType.Contains("System.Boolean"))
                {
                    inputHtml.Append($"<el-switch v-model=\"formData.{name}\" active-text=\"启用\" inactive-text=\"禁用\"></el-switch>");
                }
                else if (dataType.StartsWith($"{item.GetProjectMainName()}.Model")) //枚举
                {
                    inputHtml.AppendLine($"<el-select v-model=\"formData.{name}\" placeholder=\"请选择\">");
                    inputHtml.AppendLine($"    <el-option v-for=\"item in {name.FirstCharToLower()}Options\" :key=\"item.value\" :label=\"item.label\" :value=\"item.value\">");
                    inputHtml.AppendLine($"    </el-option>");
                    inputHtml.Append($"</el-select>");
                }
                else
                {
                    inputHtml.Append($"<el-input type=\"text\" v-model=\"formData.{name}\" clearable placeholder=\"请输入{desc}\"></el-input>");
                }



                if (i % 2 == 0)
                {
                    html.AppendLine("<el-row>");
                    html.AppendLine("    <el-col :span=\"10\">");
                    html.AppendLine($"        <el-form-item label=\"{desc}\" prop=\"{name}\">");
                    html.AppendLine(inputHtml.ToString());
                    html.AppendLine("        </el-form-item>");
                    html.AppendLine("    </el-col>");
                    needCloseTag = true;
                }
                else
                {
                    html.AppendLine("    <el-col :span=\"10\">");
                    html.AppendLine($"        <el-form-item label=\"{desc}\" prop=\"{name}\">");
                    html.AppendLine(inputHtml.ToString());
                    html.AppendLine("        </el-form-item>");
                    html.AppendLine("    </el-col>");
                    html.AppendLine("</el-row>");
                    needCloseTag = false;
                }
            }
            if (needCloseTag)
            {
                html.AppendLine("</el-row>");
            }

            return html.ToString();
        }

        /// <summary>
        /// 生成Edit中表单验证的项
        /// </summary>
        /// <returns></returns>
        public string GenerateEditValidateItem()
        {
            StringBuilder html = new StringBuilder();
            if (!PropertysItems.Any()) return "";

            foreach (var item in PropertysItems)
            {

                if (item.ColumnInfo.IsPK) continue;
                var name = item.ColumnTypeInfo.Name;
                var desc = item.ColumnInfo.DisplayName;
                var isIgonPro = new string[] { "CreatedUserId", "CreatedUserName", "CreatedDate", "UpdatedUserId", "UpdatedUserName", "UpdatedDate", "IsDeleted", "Version", "Remark" };
                if (isIgonPro.Contains(name)) continue;

                var dataType = item.ColumnTypeInfo.PropertyType.FullName;
                if (dataType.Contains("System.Int32"))
                {
                    var required = dataType.Contains("System.Nullable") ? "false" : "true";
                    html.AppendLine($"{name}: [");
                    html.AppendLine("    { required: " + required + ", message: \"请输入" + desc + "\", trigger: \"blur\" },");
                    html.AppendLine("    { pattern: this.$global.RegEx_Number, message: \"只能输入正整数\" },");
                    html.AppendLine("],");
                }
                else if (dataType.Contains("System.String"))
                {
                    if (item.ColumnInfo.DbType?.ToLower() == "text")
                    {
                        continue;
                    }
                    else
                    {
                        html.AppendLine($"{name}: [");
                        html.AppendLine("    { required: false, message: \"请输入" + desc + "\", trigger: \"blur\" },");
                        html.AppendLine("    { min: 1, max: 30, message: \"" + desc + "长度范围在1-30之间\" },");
                        html.AppendLine("],");
                    }
                }
                else if (dataType.Contains("System.Double"))
                {
                    var required = dataType.Contains("System.Nullable") ? "false" : "true";
                    html.AppendLine($"{name}: [");
                    html.AppendLine("    { required: " + required + ", message: \"请输入" + desc + "\", trigger: \"blur\" },");
                    html.AppendLine("    { pattern: this.$global.RegEx_Money, message: \"只能输入小数\" },");
                    html.AppendLine("],");
                }
                else if (dataType.Contains("System.Decimal"))
                {
                    var required = dataType.Contains("Null") ? "false" : "true";
                    html.AppendLine($"{name}: [");
                    html.AppendLine("    { required: " + required + ", message: \"请输入" + desc + "\", trigger: \"blur\" },");
                    html.AppendLine("    { pattern: this.$global.RegEx_Money, message: \"只能输入小数\" },");
                    html.AppendLine("],");
                }
                else if (dataType.Contains("System.DateTime"))
                {
                    var required = dataType.Contains("System.Nullable") ? "false" : "true";
                    html.AppendLine($"{name}: [");
                    html.AppendLine("    { required: " + required + ", message: \"请选择" + desc + "\", trigger: [\"blur\",\"change\"] },");
                    html.AppendLine("],");
                }
                else if (dataType.Contains("System.Boolean"))
                {
                    continue;
                }
                else if (dataType.StartsWith($"{item.GetProjectMainName()}.Model")) //枚举
                {
                    html.AppendLine("Sex: [{ required: true, message: \"请选择\", trigger: \"change\" }],");
                }
                else
                {
                    html.AppendLine("" + name + ": [{ required: false, message: \"请输入\", trigger: [\"blur\",\"change\"] },");
                }

            }

            return html.ToString();
        }

        /// <summary>
        /// 生成Edit中表单枚举Option
        /// </summary>
        /// <returns></returns>
        public string GenerateEditEnumOption()
        {
            StringBuilder html = new StringBuilder();
            if (!PropertysItems.Any()) return "";

            foreach (var item in PropertysItems)
            {

                if (item.ColumnInfo.IsPK) continue;
                var name = item.ColumnTypeInfo.Name;
                var desc = item.ColumnInfo.DisplayName;
                var isIgonPro = new string[] { "CreatedUserId", "CreatedUserName", "CreatedDate", "UpdatedUserId", "UpdatedUserName", "UpdatedDate", "IsDeleted", "Version", "Remark" };
                if (isIgonPro.Contains(name)) continue;

                var dataType = item.ColumnTypeInfo.PropertyType.FullName;
                if (dataType.StartsWith($"{item.GetProjectMainName()}.Model")) //枚举
                {
                    html.AppendLine($"{name.FirstCharToLower()}Options: [");
                    var optinoItem = item.ColumnTypeInfo.PropertyType.GetEnumNameAndValue();
                    foreach (var option in optinoItem)
                    {
                        html.AppendLine("{ value: " + option.Value + ", label: \"" + option.Key + "\", },");
                    }
                    html.AppendLine("],");
                }
                else
                {
                    continue;
                }

            }

            return html.ToString();
        }

        /// <summary>
        /// 生成Show中的Form列
        /// </summary>
        /// <returns></returns>
        public string GenerateShowRow()
        {
            StringBuilder html = new StringBuilder();
            if (!PropertysItems.Any()) return "";
            bool needCloseTag = false;

            for (int i = 1; i <= PropertysItems.Count; i++)
            {
                var item = PropertysItems[i - 1];

                if (item.ColumnInfo.IsPK) continue;
                var name = item.ColumnTypeInfo.Name;
                var desc = item.ColumnInfo.DisplayName;
                var isIgonPro = new string[] { "CreatedUserId", "CreatedUserName", "CreatedDate", "UpdatedUserId", "UpdatedUserName", "UpdatedDate", "IsDeleted", "Version" };
                if (isIgonPro.Contains(name)) continue;

                StringBuilder inputHtml = new();

                var dataType = item.ColumnTypeInfo.PropertyType.FullName;
                if (dataType.Contains("System.Int32"))
                {
                    inputHtml.Append("{{ formData." + name + " }}");
                }
                else if (dataType.Contains("System.String"))
                {
                    if (item.ColumnInfo.DbType?.ToLower() == "text")
                    {

                        inputHtml.Append("<div v-html=\"formData." + name + "\"></div>");
                    }
                    else
                        inputHtml.Append("{{ formData." + name + " }}");
                }
                else if (dataType.Contains("System.Double"))
                {
                    if (item.ColumnInfo.DbType?.ToLower() == "money")
                    {
                        inputHtml.Append("{{ $numberUtil.formatMoney(formData." + name + ") }}");
                    }
                    else
                    {
                        inputHtml.Append("{{ formData." + name + " }}");
                    }
                }
                else if (dataType.Contains("System.Decimal"))
                {
                    inputHtml.Append("{{ formData." + name + " }}");
                }
                else if (dataType.Contains("System.DateTime"))
                {
                    inputHtml.Append("{{ $dateUtil.formatDate(formData." + name + ") }}");
                }
                else if (dataType.Contains("System.Boolean"))
                {
                    inputHtml.Append("{{ formData." + name + " ? \"正常\":\"禁用\" }}");
                }
                else if (dataType.StartsWith($"{item.GetProjectMainName()}.Model")) //枚举
                {
                    inputHtml.Append("{{ formData." + name + "Text }}");
                }
                else
                {
                    inputHtml.Append("{{ formData." + name + " }}");
                }



                if (i % 2 == 0)
                {
                    html.AppendLine("<el-row>");
                    html.AppendLine("    <el-col :span=\"10\">");
                    html.AppendLine($"        <el-form-item label=\"{desc}\">");
                    html.AppendLine(inputHtml.ToString());
                    html.AppendLine("        </el-form-item>");
                    html.AppendLine("    </el-col>");
                    needCloseTag = true;
                }
                else
                {
                    html.AppendLine("    <el-col :span=\"10\">");
                    html.AppendLine($"        <el-form-item label=\"{desc}\">");
                    html.AppendLine(inputHtml.ToString());
                    html.AppendLine("        </el-form-item>");
                    html.AppendLine("    </el-col>");
                    html.AppendLine("</el-row>");
                    needCloseTag = false;
                }
            }
            if (needCloseTag)
            {
                html.AppendLine("</el-row>");
            }

            return html.ToString();
        }

    }

    /// <summary>
    /// 实体具体属性列表
    /// </summary>
    public class EntityPropertysItems
    {
        /// <summary>
        /// 属性信息
        /// </summary>
        public System.Reflection.PropertyInfo ColumnTypeInfo { get; set; }

        /// <summary>
        /// 列属性
        /// </summary>
        public FsColumnAttribute ColumnInfo { get; set; }


    }

}
