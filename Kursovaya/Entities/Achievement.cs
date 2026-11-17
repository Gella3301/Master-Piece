using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya.Entities
{
    /// <summary>
    /// Класс для хранения достижений
    /// </summary>
    public class Achievement
    {
        [Key]
        public int achievementId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }
}
