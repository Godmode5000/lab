using Microsoft.Xrm.Sdk;
using System;

namespace Plugins.Warehouse
{
    public interface ILocalPluginContext
    {
        IPluginExecutionContext PluginExecutionContext { get; }
        IOrganizationServiceFactory OrgSvcFactory { get; }
        ITracingService TracingService { get; }
    }

    public sealed class LocalPluginContext : ILocalPluginContext
    {
        public LocalPluginContext(IPluginExecutionContext pluginExecutionContext,
            IOrganizationServiceFactory orgSvcFactory,
            ITracingService tracingService)
        {
            PluginExecutionContext = pluginExecutionContext;
            OrgSvcFactory = orgSvcFactory;
            TracingService = tracingService;
        }

        public IPluginExecutionContext PluginExecutionContext { get; }
        public IOrganizationServiceFactory OrgSvcFactory { get; }
        public ITracingService TracingService { get; }
    }

    public abstract class PluginBase : IPlugin
    {
        protected PluginBase(Type pluginType)
        {
            PluginType = pluginType;
        }

        protected Type PluginType { get; }

        public void Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var localContext = new LocalPluginContext(context, serviceFactory, tracingService);

            ExecuteDataversePlugin(localContext);
        }

        protected abstract void ExecuteDataversePlugin(ILocalPluginContext localPluginContext);
    }
}
