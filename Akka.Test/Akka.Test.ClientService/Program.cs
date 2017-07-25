using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Topshelf;

namespace Akka.Test.ClientService
{
    class Program
    {
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
                var akkaTcpFileeditchekerPswintst = "akka.tcp://fileEditCheker@192.168.15.19:8081";
                var remoteAddress = Address.Parse(akkaTcpFileeditchekerPswintst);


                fileEditChekerSystem.ActorOf(props: Props.Create<CurrentUserActor>(akkaTcpFileeditchekerPswintst));

                Console.ReadKey();
            }
        }
    }
    
}
