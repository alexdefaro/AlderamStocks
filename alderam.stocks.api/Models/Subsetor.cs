using System;
using System.ComponentModel.DataAnnotations;

namespace alderam.stocks.api.Models
{
    public class Subsetor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        public Setor Setor { get; set; }
    }
}
