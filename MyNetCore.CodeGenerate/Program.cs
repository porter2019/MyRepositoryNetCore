using System;
using System.IO;

namespace MyNetCore.CodeGenerate
{
    class Program
    {
        static void Main(string[] args)
        {
            var projectName = AppSettings.Get("GenerateBasic:ProjectName");
            var author = AppSettings.Get("GenerateBasic:Author");
            var modelSeparator = AppSettings.Get("GenerateBasic", "ModelSeparator");

            if (projectName.IsNull()) projectName = projectName.GetProjectMainName();
            if (author.IsNull()) author = "匿名";

            Console.WriteLine($"请输入实体名称(格式：【实体名称{modelSeparator}实体名称】)...");

            var input = Console.ReadLine().Trim().SplitString(modelSeparator);
            while (input.Length != 2)
            {
                Console.WriteLine($"格式不正确，请重新输入(格式：【实体名称{modelSeparator}实体名称】)...");
                input = Console.ReadLine().Trim().SplitString(modelSeparator);
            }

            CodeGenerateOption option = new CodeGenerateOption()
            {
                ProjectName = projectName,
                Author = author,
                ModelName = input[0],
                ModelDesc = input[1],
            };

            var domainDir = AppDomain.CurrentDomain.BaseDirectory;
            var baseDir = domainDir.Substring(0, domainDir.LastIndexOf("\\bin"));
            //当前解决方案目录
            var solutionPath = baseDir.Replace($"{AppDomain.CurrentDomain.FriendlyName}", "");
            var templateDir = baseDir + "\\CodeTemplate\\basic";

            var templateFilePathList = Directory.GetFiles(templateDir, "*Template.txt");
            foreach (var templatePath in templateFilePathList)
            {
                var templateFileName = templatePath.Substring(templatePath.LastIndexOf("\\") + 1);
                var targetLastNameSpace = templateFileName.Replace("Template.txt", "");//IRepository

                string targetFileName = "";
                if (targetLastNameSpace.StartsWith("I"))
                {
                    var fileLastName = targetLastNameSpace.Substring(1, targetLastNameSpace.Length - 1);
                    targetFileName = "I" + option.ModelName + fileLastName;
                }
                else
                {
                    targetFileName = option.ModelName + targetLastNameSpace;
                }

                var targetProjectPath = solutionPath + projectName + "." + targetLastNameSpace;//D:\WorkSpace\GitHub\MyNetCore\MyNetCore.IRepository

                var targetFilePath = targetProjectPath + "\\" + targetFileName + ".cs";

                if (File.Exists(targetFilePath)) continue; //如果文件存在则不生成

                var templateContent = File.ReadAllText(templatePath);
                templateContent = templateContent.Replace("{ProjectName}", option.ProjectName);
                templateContent = templateContent.Replace("{ModelName}", option.ModelName);
                templateContent = templateContent.Replace("{ModelVariableName}", option.ModelVariableName);
                templateContent = templateContent.Replace("{Author}", option.Author);
                templateContent = templateContent.Replace("{GeneratorTime}", option.GeneratorTime);
                templateContent = templateContent.Replace("{ModelDesc}", option.ModelDesc);

                using (var targetFileInfo = File.Create(targetFilePath))
                {
                    var writer = new StreamWriter(targetFileInfo, System.Text.Encoding.UTF8);
                    writer.Write(templateContent);
                    writer.Dispose();
                }
                Console.WriteLine($"【创建成功】{targetFileName}.cs");
            }

            Console.ReadKey();
        }
    }
}
