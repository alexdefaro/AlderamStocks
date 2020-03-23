using System;
using System.ComponentModel.DataAnnotations;

namespace alderam.stocks.api.Models
{
    public class Operacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Boleta Boleta { get; set; }

        [Required]
        public Ativo Ativo { get; set; }

        [Required]
        public DateTime DataDaOperacao { get; set; }

        [Required]
        public int Quantitidade { get; set; }

        [Required]
        public double PrecoDeCompra { get; set; }

        [Required]
        public DateTime DataDeCriacao { get; set; }

        [Required]
        public double ValorDaOperacao { get; set; }
    }
}
