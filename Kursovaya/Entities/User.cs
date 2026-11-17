using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya.Entities
{
    /// <summary>
    /// Класс для хранения пользователя
    /// </summary>
    public class User
    {
        [Key]
        public  int userId { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public string email { get; set; }
        public int achievementId { get; set; } // внешний ключ от достижений

        public Achievement ? Achievement { get; set; }
    }
}
