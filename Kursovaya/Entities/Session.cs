using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya.Entities
{
    public class Session
    {
        public int sessionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int selfrate { get; set; }
        public string type { get; set; }
         public Piece ? Piece { get; set; }


    }
}
