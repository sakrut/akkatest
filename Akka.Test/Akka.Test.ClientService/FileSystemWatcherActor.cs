using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace Akka.Test.ClientService
{
    public class FileSystemWatcherActor : ActorBase
    {
        public string DrivePath;
        private IActorRef parent;

        public FileSystemWatcherActor(string drivePath, IActorRef parent)
        {
            try
            {
                this.parent = parent;
                this.DrivePath = drivePath;

                FileSystemWatcher watcher = new FileSystemWatcher();
                
                watcher.Path = DrivePath;
                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                       | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                // Only watch text files.
                watcher.Filter = "*.sql";

                // Add event handlers.
                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Created += new FileSystemEventHandler(OnChanged);
                watcher.Deleted += new FileSystemEventHandler(OnChanged);
                watcher.Renamed += new RenamedEventHandler(OnRenamed);
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            parent.Tell(e);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            parent.Tell(e);
        }

        protected override bool Receive(object message)
        {
            return false;
        }
    }
}
