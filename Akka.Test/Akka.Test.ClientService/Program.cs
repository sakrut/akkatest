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
            akka.remote.helios.tcp {
                          transport-class =
                       ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
                          transport-protocol = tcp
                          port = 0
                          hostname = ""pswintst""
                      }");
            using (var fileEditChekerSystem = ActorSystem.Create("fileEditCheker", config))
            {
                var akkaTcpFileeditchekerPswintst = "akka.tcp://fileEditCheker@pswintst:8091";
                var remoteAddress = Address.Parse(akkaTcpFileeditchekerPswintst);


                fileEditChekerSystem.ActorOf(props: Props.Create<CurrentUserActor>(akkaTcpFileeditchekerPswintst));

                Console.ReadKey();
            }
        }
    }
    
}
