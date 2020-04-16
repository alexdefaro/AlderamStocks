using System;
using System.ComponentModel.DataAnnotations;

namespace alderam.stocks.api.Models
{
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

        public Setor Setor { get; set; }
    }
}
