using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace alderam.stocks.api.Models.DTOs
{
    public class AcompanhamentoDTO
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Codigo do ativo é obrigatório.")]
        public string CodigoDoAtivo { get; set; }
        
        [IgnoreDataMember]
        public Ativo Ativo { get; set; }
        
        [Required(ErrorMessage = "Preco atual é obrigatório")]
        [Range(1, 100, ErrorMessage = "Preco atual deve estar entre 1.00 e 100.00.")]
        public double PrecoAtual { get; set; }

        [Required(ErrorMessage = "Preco de compra é obrigatório")]
        [Range(1, 100, ErrorMessage = "Preco de compra deve estar entre 1.00 e 100.00.")]
        public double PrecoDeCompra { get; set; }
    }   
}
