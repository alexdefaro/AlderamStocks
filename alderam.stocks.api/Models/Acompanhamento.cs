using System;
using System.ComponentModel.DataAnnotations;

namespace alderam.stocks.api.Models
{
    public class Acompanhamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Ativo Ativo { get; set; }

        [Required]
        public double PrecoAtual { get; set; }

        [Required]
        public double PrecoDeCompra { get; set; }
    }
}
