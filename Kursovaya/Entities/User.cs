using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya.Entities
{
    public class User
    {
        public static int userId { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public string email { get; set; }

        public Achievement ? Achievement { get; set; }
    }
}
