using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace alderam.stocks.api.Models
{
    public enum TiposDeOperacao
    {
        [Description("Compra")]
        Compra = 'C',

        [Description("Venda")]
        Venda = 'V',

        [Description("Bonificação")]
        Bonificacao = 'B'
    }

    public class Operacao
    {
        [Key]
        public int Id { get; set; }

        public TiposDeOperacao? TipoDeOperacao { get; set; } = TiposDeOperacao.Compra;

        [Required]
        public Boleta Boleta { get; set; }

        [Required]
        public Ativo Ativo { get; set; }

        [Required]
        public DateTime DataDaOperacao { get; set; }

        [Required]
        public int Quantitidade { get; set; }

        [Required]
        public decimal PrecoUnitario { get; set; }

        [Required]
        public DateTime DataDeCriacao { get; set; }

        [Required]
        public decimal ValorDaOperacao { get; set; }
    }
}
