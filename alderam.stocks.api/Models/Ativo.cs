using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace alderam.stocks.api.Models
{
    public enum TiposDeInvestimento
    {
        [Description("Ação")]
        Acao = 1,

        [Description("Fundo Imobiliário")]
        FundoImobiliario = 2,

        [Description("Indice")]
        Indice = 3
    }

    public class Ativo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Codigo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        public DateTime DataDeCriacao { get; set; }

        public DateTime? DataDaUltimaCotacao { get; set; }

        public decimal? PrecoAnterior { get; set; }

        public decimal? PrecoAtual { get; set; }

        public Subsetor Subsetor { get; set; }

        public TiposDeInvestimento? TipoDeInvestimento { get; set; }

        public char Listar { get; set; } = 'S';
    }
}
