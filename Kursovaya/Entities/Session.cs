using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya.Entities
{
    /// <summary>
    /// Класс для хранения сессий урока
    /// </summary>
    public class Session
    {
        [Key]
        public int sessionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int selfrate { get; set; }
        public string type { get; set; }
         public Piece ? Piece { get; set; }


    }
}
