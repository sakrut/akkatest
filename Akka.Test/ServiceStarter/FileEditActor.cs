using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Persistence;
using Akka.Test;

namespace ServiceStarter
{
    public class FileEditActor : ReceivePersistentActor
    {
        private List<string> editorsList { get; set; }
        private Dictionary<IActorRef,EditFileMessage> lastEdit { get; set; }
        public FileEditActor(string key)
        {
            editorsList = new List<string>();
            Key = key;
            lastEdit = new Dictionary<IActorRef, EditFileMessage>();
            Command< EditFileMessage>(s =>
            {
                Persist(s, onSomoneEdit);
            });
            Recover<EditFileMessage>(recoverFunction);
        }

        private bool recoverFunction(EditFileMessage arg)
        {
            editorsList.Add(arg.UserName);
            return true;
        }

        private void onSomoneEdit(EditFileMessage arg)
        {
            if (!editorsList.Any(x=> x.Equals(arg.UserName)))
            {
                editorsList.Add(arg.UserName);
            }
            lastEdit[Sender] = arg;
            foreach (var editFileMessage in lastEdit)
            {
                if (editFileMessage.Key.Path != Sender.Path)
                {
                    editFileMessage.Key.Tell(new OrderPeopleChangeFile(arg.Name,string.Join(", ",editorsList)));
                }
            }
            
        }

        public override string PersistenceId { get { return Key; } }
        public string Key { get; set; }
    }
}
