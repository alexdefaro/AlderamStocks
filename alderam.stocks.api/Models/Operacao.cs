using System;
using System.ComponentModel.DataAnnotations;

namespace alderam.stocks.api.Models
{
    public class Operacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Ativo Ativo { get; set; }

        [Required]
        public DateTime DateDaOperacao { get; set; }

        [Required]
        public int Quantitidade { get; set; }

        [Required]
        public double PrecoDeCompra { get; set; }

        public double TaxaDeLiquidacao { get; set; }

        public double Emolumentos { get; set; }

        public double Corretagem { get; set; }

        public double ISS { get; set; }

        [Required]
        public DateTime DataDeCriacao { get; set; }

        /* Virtual Properties */
        public virtual double ValorDaOperacao { get { return (Quantitidade * PrecoDeCompra); } }

    }
}
