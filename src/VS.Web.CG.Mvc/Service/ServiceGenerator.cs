using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Service
{
    [Alias("service")]
    public class ServiceGenerator : ICodeGenerator
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceGenerator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task GenerateCode(ServiceGeneratorModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            ServiceGeneratorBase generator = null;

            if (string.IsNullOrEmpty(model.ModelClass))
            {
                generator = GetGenerator<Service>();
            }
            else
            {
                // TODO
            }

            if (generator != null)
            {
                await generator.Generate(model);
            }
        }

        private ServiceGeneratorBase GetGenerator<TChild>() where TChild : ServiceGeneratorBase
        {
            return ActivatorUtilities.CreateInstance<TChild>(_serviceProvider);
        }
    }
}
