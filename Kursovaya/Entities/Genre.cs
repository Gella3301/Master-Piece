using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya.Entities
{
    public class Genre
    {
        [Key]
        public int genreId { get; set; }

        public string genreName { get; set; }
    }
}
