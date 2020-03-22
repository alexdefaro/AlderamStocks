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

        public double TaxaDeLiquidacao { get; set; }

        public double Emolumentos { get; set; }

        public double Corretagem { get; set; }

        public double ISS { get; set; }

        [Required]
        public DateTime DataDeCriacao { get; set; }

        [MaxLength(500)]
        public string Observacoes { get; set; }

        public IList<Operacao> Operacoes { get; set; }
    }
}