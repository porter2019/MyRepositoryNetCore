using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyNetCore.Common.Helper
{
    /// <summary>
    /// 从xml中获取方法或属性的备注信息
    /// </summary>
    public static class DLLXmlHelper
    {

        /// <summary>
        /// 从xml读取所有的方法备注信息
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <param name="filter">M: 方法，T: 类</param>
        /// <returns></returns>
        public static List<DLLXmlMemberInfo> ReadInfo(string xmlPath, string filter = "")
        {
            List<DLLXmlMemberInfo> list = new List<DLLXmlMemberInfo>();

            XDocument doc = XDocument.Load(xmlPath);

            var memberList = doc.Element("doc").Element("members").Elements("member");

            foreach (var member in memberList)
            {

                var name = member.Attribute("name").Value;
                if (name.IsNull()) continue;
                DLLXmlMemberInfo entity = new DLLXmlMemberInfo();
                entity.Name = name;
                entity.Summary = member.Element("summary").Value.Replace("\r\n", "").Trim();
                entity.Remarks = member.Element("remarks")?.Value.Replace("\r\n", "").Trim() ?? "";

                if (filter.IsNull())
                {
                    list.Add(entity);
                }
                else
                {
                    if (name.StartsWith(filter))
                    {
                        entity.Name = name.Replace($"{filter}:", "").Trim();
                        if (entity.Name.IsNull()) continue;
                        list.Add(entity);
                    }
                }

            }

            return list;
        }



    }

    /// <summary>
    /// xml中的信息
    /// </summary>
    public class DLLXmlMemberInfo
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 接口名
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 接口备注
        /// </summary>
        public string Remarks { get; set; }

    }

}
