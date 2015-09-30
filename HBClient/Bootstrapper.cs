using System.Configuration;
using Castle.Core;
using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using ExF.Windsor.BaseContainer;
using Heartbeat;

namespace XService
{
    internal class Bootstrapper : IBootstrapper
    {
        private ExfBootstrapperHelper<IXService> _xServicebootstrapperHelper;

        public void Init()
        {
            int maxConcurrentCalls = int.Parse(ConfigurationManager.AppSettings["MaxConcurrentCalls"]);
            int maxConcurrentInstances = int.Parse(ConfigurationManager.AppSettings["MaxConcurrentInstances"]);
            string serviceBaseUrl = ConfigurationManager.AppSettings["ServiceBaseUrl"];


            _xServicebootstrapperHelper =
                new ExfBootstrapperHelper<IXService>(maxConcurrentCalls, maxConcurrentInstances, serviceBaseUrl);

            _xServicebootstrapperHelper.GetWindsorContainer()
                .Register(Component.For<ClientProcessor>().LifeStyle.Singleton)
                .Register(Component.For<HbInterceptor>().LifeStyle.PerWcfOperation());

            _xServicebootstrapperHelper.RegisterBasicHttpBinding<XService>(new[]
            {
                InterceptorReference.ForType<HbInterceptor>()
            });

            ContainerValidator.ValidateContainer(_xServicebootstrapperHelper.GetWindsorContainer());
        }

        public void WriteBaseUrlToConsole()
        {
            _xServicebootstrapperHelper.WriteBaseUrlToConsole();
        }

        public void CopyBaseUrlToClipboard()
        {
            _xServicebootstrapperHelper.CopyBaseUrlToClipboard();
        }
    }
}