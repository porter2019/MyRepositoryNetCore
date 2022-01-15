﻿using MyNetCore.Model.CodeGenerate;
using System.IO;
using System.Reflection;

namespace MyNetCore.Services
{
    /// <summary>
    /// 代码生成服务,使用Razor引擎
    /// </summary>
    [ServiceLifetime(false)]
    public class CodeGenerateUseRazorService : ICodeGenerateService
    {
        private readonly ITemplateEngineService _templateEngine;
        private readonly IConfiguration _config;

        public CodeGenerateUseRazorService(ITemplateEngineService templateEngine, IConfiguration config)
        {
            _templateEngine = templateEngine;
            _config = config;
        }

        /// <summary>
        /// 生成实体IRepository、IServices、Repository、Services四个层的代码文件
        /// </summary>
        /// <param name="modelName">实体名称，类名而非文件名，MyNetCore.Model.Entity下</param>
        /// <returns></returns>
        public async Task<ApiResult> GenerateIIRSCodeFile(string modelName)
        {
            var model = loadEntityInfo(modelName);
            //IRepository
            var html = await _templateEngine.ParseAsync(GetViewTemplateRelativePath("IRepositoryTemplate"), model);
            SaveCodeToFile(html, $"I{modelName}Repository.cs", "IRepository");
            //IServices
            html = await _templateEngine.ParseAsync(GetViewTemplateRelativePath("IServicesTemplate"), model);
            SaveCodeToFile(html, $"I{modelName}Services.cs", "IServices");
            //Repository
            html = await _templateEngine.ParseAsync(GetViewTemplateRelativePath("RepositoryTemplate"), model);
            SaveCodeToFile(html, $"{modelName}Repository.cs", "Repository");
            //Services
            html = await _templateEngine.ParseAsync(GetViewTemplateRelativePath("ServicesTemplate"), model);
            SaveCodeToFile(html, $"{modelName}Services.cs", "Services");
            //Model ReqeustPageQueryModel
            html = await _templateEngine.ParseAsync(GetViewTemplateRelativePath("ModelRequestPageModel"), model);
            SaveCodeToFile(html, $"{modelName}PageModel.cs", "Model\\RequestModel");
            return ApiResult.OK();
        }

        /// <summary>
        /// 生成Api控制器文件
        /// </summary>
        /// <param name="name">类名/文件名</param>
        /// <param name="desc">说明，如果为空，则表示name是Entity实体，该值自动反射从实体中取得</param>
        /// <returns></returns>
        public async Task<ApiResult> GenerateApiControllerFile(string name, string desc)
        {
            var remark = desc;
            var templateName = "ApiControllerTemplate";
            if (remark.IsNull())
            {
                templateName = "ApiControllerWithEntityTemplate";
            }
            var model = loadEntityInfo(name, remark);
            var html = await _templateEngine.ParseAsync(GetViewTemplateRelativePath(templateName), model);
            SaveCodeToFile(html, $"{name}Controller.cs", "Web\\ApiControllers");

            return ApiResult.OK();
        }

        /// <summary>
        /// 生成前端Vue页面代码
        /// </summary>
        /// <param name="name">类名/文件名</param>
        /// <param name="desc">说明，如果为空，则表示name是Entity实体，该值自动反射从实体中取得，否则生成基础前端模板</param>
        /// <returns></returns>
        public async Task<ApiResult> GenerateVuePageFile(string name, string desc)
        {
            var remark = desc;
            var routeTemplateName = "Route";
            var apiTemplateName = "Api";
            var indexTemplateName = "Index";
            var editTemplateName = "Edit";
            var showTemplateName = "Show";
            if (remark.IsNull())
            {
                routeTemplateName = "RouteWithEntity";
                apiTemplateName = "ApiWithEntity";
                indexTemplateName = "IndexWithEntity";
                editTemplateName = "EditWithEntity";
                showTemplateName = "ShowWithEntity";
            }
            var entityInfo = loadEntityIncludeProperty(name, desc);
            var ss = entityInfo.GenerateIndexTableItems();
            //路由
            var html = await _templateEngine.ParseAsync(GetViewTemplateRelativePath($"Vue/{routeTemplateName}"), entityInfo);
            SaveVueCodeToFile(html, $"{entityInfo.ModelVariableName}.js", "\\src\\router\\modules\\");
            //api
            html = await _templateEngine.ParseAsync(GetViewTemplateRelativePath($"Vue/{apiTemplateName}"), entityInfo);
            SaveVueCodeToFile(html, $"{entityInfo.ModelVariableName}.js", $"\\src\\api\\{entityInfo.TableInfo?.VueModuleName ?? "empty"}\\");
            //Index页面
            html = await _templateEngine.ParseAsync(GetViewTemplateRelativePath($"Vue/{indexTemplateName}"), entityInfo);
            SaveVueCodeToFile(html, $"index.vue", $"\\src\\views\\{entityInfo.TableInfo?.VueModuleName ?? "empty"}\\{entityInfo.ModelVariableName}\\");
            //Edit页面
            html = await _templateEngine.ParseAsync(GetViewTemplateRelativePath($"Vue/{editTemplateName}"), entityInfo);
            SaveVueCodeToFile(html, $"edit.vue", $"\\src\\views\\{entityInfo.TableInfo?.VueModuleName ?? "empty"}\\{entityInfo.ModelVariableName}\\");
            //Show页面
            html = await _templateEngine.ParseAsync(GetViewTemplateRelativePath($"Vue/{showTemplateName}"), entityInfo);
            SaveVueCodeToFile(html, $"show.vue", $"\\src\\views\\{entityInfo.TableInfo?.VueModuleName ?? "empty"}\\{entityInfo.ModelVariableName}\\");

            return ApiResult.OK();
        }

        #region 私有方法

        /// <summary>
        /// 保存文件到指定的目录
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        /// <param name="targetProject">指定项目名</param>
        /// <param name="targetRelativePath">保存的目录，默认为\\，放在项目根目录，否则填写\\Common\\</param>
        /// <returns></returns>
        private void SaveCodeToFile(string content, string fileName, string targetProject, string targetRelativePath = "\\")
        {
            if (content.IsNull()) throw new ArgumentNullException("Content内容为空");
            if (fileName.IsNull()) throw new ArgumentNullException("文件名不明确");
            if (targetProject.IsNull()) throw new ArgumentNullException("目标项目名不明确");

            var projectName = content.GetProjectMainName();//MyNetCore
            var domainDir = AppDomain.CurrentDomain.BaseDirectory;//C:\WorkSpace\GitHub\MyRepositoryNetCore\MyNetCore.Web\bin\Debug\net5.0\
            var baseDir = domainDir.Substring(0, domainDir.LastIndexOf("\\bin"));//C:\WorkSpace\GitHub\MyRepositoryNetCore\MyNetCore.Web
            //当前解决方案目录
            var solutionPath = baseDir.Replace($"{AppDomain.CurrentDomain.FriendlyName}", "");//C:\WorkSpace\GitHub\MyRepositoryNetCore\
            var targetFolder = solutionPath + projectName + "." + targetProject + targetRelativePath; //C:\WorkSpace\GitHub\MyRepositoryNetCore\MyNetCore.IRepository\
            if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);
            var targetFilePath = targetFolder + fileName;
            if (File.Exists(targetFilePath)) return;//如果文件存在则不生成

            using (var targetFileInfo = File.Create(targetFilePath))
            {
                var writer = new StreamWriter(targetFileInfo, Encoding.UTF8);
                writer.Write(content);
                writer.Dispose();
            }
        }

        /// <summary>
        /// 保存文件到指定的目录
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        /// <param name="targetProject">指定项目名</param>
        /// <param name="targetRelativePath">保存的目录，默认为\\src\\，放在项目src目录，否则填写\\src\\</param>
        /// <returns></returns>
        private void SaveVueCodeToFile(string content, string fileName, string targetRelativePath = "\\src\\")
        {
            if (content.IsNull()) throw new ArgumentNullException("Content内容为空");
            if (fileName.IsNull()) throw new ArgumentNullException("文件名不明确");

            var baseDir = _config["VueProjectDirectory"];
            if (baseDir.IsNull()) throw new NullReferenceException("未配置Vue项目根目录");

            var targetFolder = baseDir + targetRelativePath;
            if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);
            var targetFilePath = targetFolder + fileName;
            if (File.Exists(targetFilePath)) return;//如果文件存在则不生成

            using (var targetFileInfo = File.Create(targetFilePath))
            {
                var writer = new StreamWriter(targetFileInfo, Encoding.UTF8);
                writer.Write(content);
                writer.Dispose();
            }
        }

        /// <summary>
        /// 获取实体的信息
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="entityDesc">实体说明，为空则反射获取</param>
        /// <returns></returns>
        private BaseCode loadEntityInfo(string entityName, string entityDesc = "")
        {
            if (entityDesc.IsNotNull())
            {
                return new BaseCode(entityName, entityDesc);
            }

            var resultModel = new BaseCode(entityName);

            var fristNameSpace = $"{entityName.GetProjectMainName()}.Model";
            var entityType = Assembly.Load(fristNameSpace).GetType($"{fristNameSpace}.Entity.{entityName}");
            if (entityType == null) throw new NullReferenceException($"找不到指定类:{fristNameSpace}.Entity.{entityName}");
            var fsTableInfo = entityType.GetCustomAttributes(typeof(Model.FsTableAttribute), true)[0] as Model.FsTableAttribute;
            if (fsTableInfo == null) throw new NullReferenceException($"指定类未指定【FsTable】标识");
            if (fsTableInfo.ViewClassName != null)
            {
                var viewClassInfo = fsTableInfo.ViewClassName.GetCustomAttributes(typeof(Model.FsTableAttribute), true)[0] as Model.FsTableAttribute;
                if (viewClassInfo.Name.IsNull()) throw new NullReferenceException($"视图查询类：【{fsTableInfo.ViewClassName.Name}】未指定视图名称");
                if (!viewClassInfo.DisableSyncStructure) throw new Exception($"视图查询类：【{fsTableInfo.ViewClassName.Name}】未禁用迁移(DisableSyncStructure=true)");
                //设置视图属性
                resultModel.HasView = true;
                resultModel.ViewClassName = fsTableInfo.ViewClassName.Name;
            }

            resultModel.ModelDesc = fsTableInfo.DisplayName;

            return resultModel;
        }

        /// <summary>
        /// 获取实体的信息
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="entityDesc">实体说明，为空则反射获取</param>
        /// <returns></returns>
        private EntityPropertys loadEntityIncludeProperty(string entityName, string entityDesc = "")
        {
            var baseModelEntity = loadEntityInfo(entityName, entityDesc);

            EntityPropertys entityInfo = new EntityPropertys
            {
                GeneratorTime = baseModelEntity.GeneratorTime,
                HasView = baseModelEntity.HasView,
                ModelDesc = baseModelEntity.ModelDesc,
                ModelName = baseModelEntity.ModelName,
                ViewClassName = baseModelEntity.ViewClassName
            };

            if (entityDesc.IsNotNull()) return entityInfo;

            var fristNameSpace = $"{entityName.GetProjectMainName()}.Model";
            var entityType = Assembly.Load(fristNameSpace).GetType($"{fristNameSpace}.Entity.{entityName}");
            if (entityType == null) throw new NullReferenceException($"找不到指定类:{fristNameSpace}.Entity.{entityName}");
            var fsTableInfo = entityType.GetCustomAttributes(typeof(Model.FsTableAttribute), true)[0] as Model.FsTableAttribute;
            if (fsTableInfo == null) throw new NullReferenceException($"指定类未指定【FsTable】标识");
            entityInfo.TableInfo = fsTableInfo;

            entityType.GetProperties().ToList().ForEach(clumn =>
            {
                if (clumn.PropertyType.Namespace == "System.Collections.Generic") return;
                if (!clumn.CanWrite) return;//只读的不要

                var fsColumnInfo = clumn.GetCustomAttributes<Model.FsColumnAttribute>(true); //clumn.GetCustomAttributes(typeof(Model.FsColumnAttribute), true)[0] as Model.FsColumnAttribute;
                if (fsColumnInfo.Any())
                {
                    entityInfo.PropertysItems.Add(new EntityPropertysItems() { ColumnInfo = fsColumnInfo.First(), ColumnTypeInfo = clumn });
                }
            });

            return entityInfo;
        }

        /// <summary>
        /// 获取视图相对路径
        /// </summary>
        /// <param name="lastPath"></param>
        /// <returns></returns>
        private string GetViewTemplateRelativePath(string lastPath)
        {
            return "/Views/CodeGenerateTemplate/" + lastPath + ".cshtml";
        }

        #endregion 私有方法
    }
}