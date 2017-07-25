using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Test
{
    public class OrderPeopleChangeFile
    {
        public string FileName { get; set; }
        public string UserName { get; set; }

        public OrderPeopleChangeFile(string fileName, string userName)
        {
            FileName = fileName;
            UserName = userName;
        }
    }
}
