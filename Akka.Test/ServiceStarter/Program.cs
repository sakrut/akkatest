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
        }
    }

    public class MyActorService
    {
        private ActorSystem fileEditChekerSystem;

        public void Start()
        {
            var config = ConfigurationFactory.ParseString(@"
            akka.remote.helios.tcp {
                          transport-class =
                       ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
                          transport-protocol = tcp
                          port = 8091
                          hostname = ""localhost""
                      }");
            fileEditChekerSystem = ActorSystem.Create("fileEditCheker", config);
        }

        public async void Stop()
        {
            //this is where you stop your actor system
            await fileEditChekerSystem.Terminate();
        }
    }
}
