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
        public FileSystemEventArgs FileSystemEventArgs { get; set; }
        public string UserName { get; set; }

        public EditFileMessage(FileSystemEventArgs fileSystemEventArgs, string userName)
        {
            FileSystemEventArgs = fileSystemEventArgs;
            UserName = userName;
        }
    }
}
