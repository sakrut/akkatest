using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Test
{
    public class EditFileMessage
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public WatcherChangeTypes ChangeType { get; set; }
        public string UserName { get; set; }

        public EditFileMessage()
        {
        }

        public EditFileMessage(FileSystemEventArgs fileSystemEventArgs, string userName)
        {
            Name = fileSystemEventArgs.Name;
            FullPath= fileSystemEventArgs.FullPath;
            ChangeType = fileSystemEventArgs.ChangeType;
            UserName = userName;
        }
    }
}
