using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Topshelf;

namespace ServiceStarter
{
    class Program
    {
        static void Main(string[] args)
        {
            /* HostFactory.Run(x =>
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

        public void Start()
        {
            var config = ConfigurationFactory.ParseString(@"
            akka{
actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }
        remote{
 helios.tcp 
{
                          transport-class =
                       ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
                          transport-protocol = tcp
                          port = 8081
                          hostname = ""pswintst""
                      }
}
}
");
            fileEditChekerSystem = ActorSystem.Create("fileEditCheker", config);
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
