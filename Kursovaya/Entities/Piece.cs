using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya.Entities
{
    /// <summary>
    /// Класс для хранения произведений
    /// </summary>
    public class Piece
    {
        [Key]
        public int pieceId { get; set; }
        public string title { get; set; }

        public string Composer { get; set; }

        public int difficulty { get; set; }
        public string? coverUrl { get; set; } // строчка для хранения фото произведения

        public string status { get; set; }
        public int userId { get; set; }   // Внешний ключ для User
        public int genreId { get; set; }  // Внешний ключ для Genre

        public Genre? Genre { get; set; }
        public User ? User { get; set; }
    }
}
