using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace Akka.Test
{
    public class FileEditActor : ReceiveActor
    {
        private Dictionary<IActorRef,EditFileMessage> lastEdit { get; set; }
        public FileEditActor()
        {
            lastEdit = new Dictionary<IActorRef, EditFileMessage>();
            Receive<EditFileMessage>(onSomoneEdit);
        }

        private bool onSomoneEdit(EditFileMessage arg)
        {
            lastEdit[Sender] = arg;
            foreach (var editFileMessage in lastEdit)
            {
                if (editFileMessage.Key != Sender)
                {
                    editFileMessage.Key.Tell(new OrderPeopleChangeFile(arg.FileSystemEventArgs.Name,arg.UserName));
                }
            }
            return true;
        }
    }
}
