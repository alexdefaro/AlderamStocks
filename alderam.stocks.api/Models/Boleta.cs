using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace alderam.stocks.api.Models
{
    public class Boleta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Numero { get; set; }
        
        [Required]
        public DateTime DataDaOperacao { get; set; }

        public decimal TaxaDeLiquidacao { get; set; }

        public decimal Emolumentos { get; set; }

        public decimal Corretagem { get; set; }

        public decimal ISS { get; set; }

        [Required]
        public DateTime DataDeCriacao { get; set; }

        public decimal ValorDaOperacao { get; set; }

        public decimal ValorDaCompra { get; set; }

        [MaxLength(500)]
        public string Observacoes { get; set; }

        public IList<Operacao> Operacoes { get; set; }
    }
}