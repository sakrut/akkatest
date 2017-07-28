using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.Monitoring;
using Akka.Monitoring.PerformanceCounters;
using Akka.Persistence.SqlServer;
using Akka.Test;
using Topshelf;

namespace ServiceStarter
{
    class Program
    {
        static void Main(string[] args)
        {
             /*HostFactory.Run(x =>
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
             });*/
            using (var myActorService = new MyActorService())
            {
                myActorService.Start();
                Console.ReadKey();
            }
        }
    }

    public class MyActorService : IDisposable
    {
        private ActorSystem fileEditChekerSystem;
        private bool registeredMonitor;
        private SqlServerPersistence sqlServerPersistence;

        public void Start()
        {
            //var config = ConfigurationFactory.ParseString(ServiceStarter.Properties.Resources.akkaconfig);
            
            fileEditChekerSystem = ActorSystem.Create("fileEditCheker");
            sqlServerPersistence = SqlServerPersistence.Get(fileEditChekerSystem);
            
            registeredMonitor = ActorMonitoringExtension.RegisterMonitor(fileEditChekerSystem, new ActorPerformanceCountersMonitor());
            var actorRef = fileEditChekerSystem.ActorOf(Props.Create(() => new DispActor()), "disp");
            actorRef.Tell(new EditFileMessage() {Name = "ttttt",UserName = "Testoowy",FullPath = "ss",ChangeType = WatcherChangeTypes.Changed});
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
