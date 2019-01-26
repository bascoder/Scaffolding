using Microsoft.VisualStudio.Web.CodeGeneration;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.ProjectModel;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Service
{
    public abstract class ServiceGeneratorBase : CommonGeneratorBase
    {
        protected ICodeGeneratorActionsService CodeGeneratorActionsService
        {
            get;
            private set;
        }
        protected IProjectContext ProjectContext
        {
            get;
            private set;
        }
        protected IServiceProvider ServiceProvider
        {
            get;
            private set;
        }
        protected ILogger Logger
        {
            get;
            private set;
        }

        public ServiceGeneratorBase(IProjectContext projectContext,
            IApplicationInfo applicationInfo,
            ICodeGeneratorActionsService codeGeneratorActionsService,
            IServiceProvider serviceProvider,
            ILogger logger)
            : base(applicationInfo)
        {
            if (applicationInfo == null)
            {
                throw new ArgumentNullException(nameof(applicationInfo));
            }

            ProjectContext = projectContext ?? throw new ArgumentNullException(nameof(projectContext));
            CodeGeneratorActionsService = codeGeneratorActionsService ?? throw new ArgumentNullException(nameof(codeGeneratorActionsService));
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected virtual IEnumerable<string> TemplateFolders
        {
            get
            {
                return TemplateFoldersUtilities.GetTemplateFolders(
                    containingProject: Constants.ThisAssemblyName,
                    applicationBasePath: ApplicationInfo.ApplicationBasePath,
                    baseFolders: new[] { "ControllerGenerator", "ViewGenerator" },
                    projectContext: ProjectContext);
            }
        }

        protected string GetDefaultControllerNamespace(string relativeFolderPath)
        {
            return NameSpaceUtilities.GetSafeNameSpaceFromPath(relativeFolderPath, ApplicationInfo.ApplicationName);
        }

        protected void ValidateNameSpaceName(ServiceGeneratorModel generatorModel)
        {
            if (!string.IsNullOrEmpty(generatorModel.ServiceNamespace) &&
                !RoslynUtilities.IsValidNamespace(generatorModel.ServiceNamespace))
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.CurrentCulture,
                    MessageStrings.InvalidNamespaceName,
                    generatorModel.ServiceNamespace));
            }
        }

        protected string ValidateAndGetOutputPath(ServiceGeneratorModel generatorModel)
        {
            return ValidateAndGetOutputPath(generatorModel, generatorModel.ServiceName + Constants.CodeFileExtension);
        }

        private bool IsInArea(string appBasePath, string outputPath)
        {
            return outputPath.StartsWith(Path.Combine(appBasePath, "Areas") + Path.DirectorySeparatorChar);
        }

        protected string GetAreaName(string appBasePath, string outputPath)
        {
            if (IsInArea(appBasePath, outputPath))
            {
                var relativePath = outputPath.Substring(Path.Combine(appBasePath, "Areas").Length);
                return relativePath.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            }
            return string.Empty;
        }

        public abstract Task Generate(ServiceGeneratorModel controllerGeneratorModel);
        protected abstract string GetTemplateName(ServiceGeneratorModel controllerGeneratorModel);
    }
}
