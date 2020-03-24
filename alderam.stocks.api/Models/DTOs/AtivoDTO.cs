using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace alderam.stocks.api.Models.DTOs
{
    public class AtivoDTO
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Codigo do ativo é obrigatório.")]
        public string Codigo { get; set; }

        public string Nome { get; set; }
        
        public DateTime DataDeCriacao { get; set; }
    }
}
