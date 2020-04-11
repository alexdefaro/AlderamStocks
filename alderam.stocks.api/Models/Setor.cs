using System;
using System.ComponentModel.DataAnnotations;

namespace alderam.stocks.api.Models
{
    public class Setor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Codigo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } 
    }
}
