using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya.Entities
{
    public class Piece
    {
        public int pieceId { get; set; }
        public string title { get; set; }

        public string Composer { get; set; }

        public int difficulty { get; set; }

        public string status { get; set; }

        public Genre? Genre { get; set; }
        public User ? User { get; set; }
    }
}
