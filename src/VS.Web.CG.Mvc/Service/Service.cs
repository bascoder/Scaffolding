using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.ProjectModel;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;

namespace Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Service
{
    public class Service : ServiceGeneratorBase
    {
        public Service(IProjectContext projectContext, IApplicationInfo applicationInfo, ICodeGeneratorActionsService codeGeneratorActionsService, IServiceProvider serviceProvider, ILogger logger) : base(projectContext, applicationInfo, codeGeneratorActionsService, serviceProvider, logger)
        {
        }

        public override async Task Generate(ServiceGeneratorModel controllerGeneratorModel)
        {
            if (!string.IsNullOrEmpty(controllerGeneratorModel.ServiceName))
            {
                if (!controllerGeneratorModel.ServiceName.EndsWith(Constants.ServiceSuffix, StringComparison.Ordinal))
                {
                    controllerGeneratorModel.ServiceName = controllerGeneratorModel.ServiceName + Constants.ServiceSuffix;
                }
            }
            else
            {
                throw new ArgumentException("Service name is required");
            }
            ValidateNameSpaceName(controllerGeneratorModel);
            var namespaceName = string.IsNullOrEmpty(controllerGeneratorModel.ServiceNamespace)
                ? GetDefaultControllerNamespace(controllerGeneratorModel.RelativeFolderPath)
                : controllerGeneratorModel.ServiceNamespace;
            var templateModel = new ClassNameModel(className: controllerGeneratorModel.ServiceName, namespaceName: namespaceName);

            var outputPath = ValidateAndGetOutputPath(controllerGeneratorModel);
            await CodeGeneratorActionsService.AddFileFromTemplateAsync(outputPath, GetTemplateName(controllerGeneratorModel), TemplateFolders, templateModel);
            Logger.LogMessage(string.Format(MessageStrings.AddedController, outputPath.Substring(ApplicationInfo.ApplicationBasePath.Length)));
        }

        protected override string GetTemplateName(ServiceGeneratorModel generatorModel)
        {
            return Constants.ServiceEmptyTemplate;
        }
    }
}
