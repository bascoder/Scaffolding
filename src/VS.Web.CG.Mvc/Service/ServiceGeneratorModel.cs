using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Service
{
    public class ServiceGeneratorModel : CommonCommandLineModel
    {
        [Option(Name = "serviceName", ShortName = "service", Description = "Specify the name of the generated service")]
        public string ServiceName { get; set; }

        [Option(Name = "serviceNamespace", ShortName = "namespace", Description = "Specify the name of the namespace to use for the generated controller")]
        public string ServiceNamespace { get; set; }

        [Option(Name = "useAsyncActions", ShortName = "async", Description = "Switch to indicate whether to generate async controller actions")]
        public bool UseAsync { get; set; }

        public ServiceGeneratorModel(ServiceGeneratorModel copyFrom)
        {
            ServiceName = copyFrom.ServiceName;
            ServiceNamespace = copyFrom.ServiceNamespace;
            UseAsync = copyFrom.UseAsync;
        }

        public override CommonCommandLineModel Clone()
        {
            return new ServiceGeneratorModel(this);
        }
    }
}
