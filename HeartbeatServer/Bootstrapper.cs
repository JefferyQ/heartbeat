using System.Configuration;
using Castle.MicroKernel.Registration;
using ExF.Windsor.BaseContainer;

namespace HeartbeatServer
{
    internal class Bootstrapper : IBootstrapper
    {
        private ExfBootstrapperHelper<IHeartbeatServer> _xServicebootstrapperHelper;

        public void Init()
        {
            var maxConcurrentCalls = int.Parse(ConfigurationManager.AppSettings["MaxConcurrentCalls"]);
            var maxConcurrentInstances = int.Parse(ConfigurationManager.AppSettings["MaxConcurrentInstances"]);
            var serviceBaseUrl = ConfigurationManager.AppSettings["ServiceBaseUrl"];


            _xServicebootstrapperHelper =
                new ExfBootstrapperHelper<IHeartbeatServer>(maxConcurrentCalls, maxConcurrentInstances, serviceBaseUrl);

            _xServicebootstrapperHelper.RegisterBasicHttpBinding<HeartbeatServer>();

            _xServicebootstrapperHelper.GetWindsorContainer()
                .Register(Component.For<HbArchiveProcessor>().LifeStyle.Singleton)
                .Register(Component.For<AppStatsProcessor>().LifeStyle.Singleton)
                .Register(Component.For<DummyAppStatsSender>().LifeStyle.Singleton)
                .Register(Component.For<UdpServer>().LifeStyle.Singleton);

            //bunu silerseniz init olmaz, paketleri dinlemez.
            _xServicebootstrapperHelper.GetWindsorContainer().Resolve<UdpServer>();
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