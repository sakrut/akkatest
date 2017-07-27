using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.Monitoring;
using Akka.Monitoring.PerformanceCounters;
using Topshelf;

namespace ServiceStarter
{
    class Program
    {
        static void Main(string[] args)
        {
             HostFactory.Run(x =>
             {
                 x.Service<MyActorService>(s =>
                 {
                     s.ConstructUsing(n => new MyActorService());
                     s.WhenStarted(service => service.Start());
                     s.WhenStopped(service => service.Stop());
                     //continue and restart directives are also available
                 });

                 x.RunAsLocalSystem();
                 x.UseAssemblyInfoForServiceInfo();
             });
            /*using (var myActorService = new MyActorService())
            {
                myActorService.Start();
                Console.ReadKey();
            }*/
        }
    }

    public class MyActorService : IDisposable
    {
        private ActorSystem fileEditChekerSystem;
        private bool registeredMonitor;

        public void Start()
        {
            var config = ConfigurationFactory.ParseString(ServiceStarter.Properties.Resources.akkaconfig);
            fileEditChekerSystem = ActorSystem.Create("fileEditCheker", config);
            registeredMonitor = ActorMonitoringExtension.RegisterMonitor(fileEditChekerSystem, new ActorPerformanceCountersMonitor());
            fileEditChekerSystem.ActorOf(Props.Create(() => new DispActor()), "disp");
        }

        public async void Stop()
        {
            //this is where you stop your actor system
            await fileEditChekerSystem.Terminate();
        }

        public void Dispose()
        {
            fileEditChekerSystem?.Dispose();
        }
    }
}
