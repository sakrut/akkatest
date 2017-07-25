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

namespace Akka.Test.ClientService
{
    class Program
    {
        private static bool registeredMonitor;

        static void Main(string[] args)
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
                          port = 0
                          hostname = ""localhost""
                      }
}
}
");
            using (var fileEditChekerSystem = ActorSystem.Create("fileEditCheker", config))
            {
                registeredMonitor = ActorMonitoringExtension.RegisterMonitor(fileEditChekerSystem, new ActorPerformanceCountersMonitor());
                var akkaTcpFileeditchekerPswintst = "akka.tcp://fileEditCheker@pswintst:8081";
                var remoteAddress = Address.Parse(akkaTcpFileeditchekerPswintst);


                fileEditChekerSystem.ActorOf(props: Props.Create<CurrentUserActor>(akkaTcpFileeditchekerPswintst));

                Console.ReadKey();
            }
        }
    }
    
}
