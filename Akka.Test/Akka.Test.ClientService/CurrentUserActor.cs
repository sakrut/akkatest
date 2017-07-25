using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace Akka.Test.ClientService
{
    public class CurrentUserActor : ReceiveActor
    {
        Dictionary<string,IActorRef> fileActors = new Dictionary<string, IActorRef>();
        private string remoteAdress;
        public string userName { get; set; }

        public CurrentUserActor(string remoteAdress)
        {
            this.remoteAdress = remoteAdress;
            userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            foreach (var logicalDrive in Environment.GetLogicalDrives())
            {
                fileActors[logicalDrive] = Context.ActorOf(Props.Create<FileSystemWatcherActor>(logicalDrive, Self),
                     logicalDrive.Replace("\\", "").Replace(":",""));
            }

            //Receive<RenamedEventArgs>(renameFile);
            Receive<FileSystemEventArgs>(filechange);
            Receive<OrderPeopleChangeFile>(orderPeopleChangeFile);
        }

        
        private bool filechange(FileSystemEventArgs arg)
        {
            Console.WriteLine("To ty zmieniasz plik : "+ arg.Name);
            var fileRemoteAcctorOrCreate = getFileRemoteAcctor();
            fileRemoteAcctorOrCreate.Tell(new EditFileMessage(arg,userName));
            return true;
        }

        private bool orderPeopleChangeFile(OrderPeopleChangeFile arg)
        {
            Console.WriteLine(arg.UserName+ "  TEż edytuje plik : " +arg.FileName);
            return true;
        }


        private bool renameFile(RenamedEventArgs obj)
        {
            return true;
        }

        


        private ActorSelection getFileRemoteAcctor()
        {
            return Context.ActorSelection(remoteAdress +"/user/disp");
        }
    }
}
