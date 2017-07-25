using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Test;

namespace ServiceStarter
{
    public class DispActor : ReceiveActor
    {
        Dictionary<string,IActorRef> childs = new Dictionary<string, IActorRef>();
        public DispActor()
        {
            Receive<EditFileMessage>(onSomoneEdit);
        }

        private bool onSomoneEdit(EditFileMessage arg)
        {
            var actorRef = childs[getFileName(arg)];
            if (actorRef == null)
            {
                childs[getFileName(arg)] = actorRef = Context.ActorOf(Props.Create(() => new FileEditActor()));
            }
            actorRef.Tell(arg,Sender);
            return true;
        }

        public string getFileName(EditFileMessage e)
        {
            return e.FileSystemEventArgs.Name.Replace("\\", "_");
        }
    }
}
