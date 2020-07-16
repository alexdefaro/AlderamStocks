using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace alderam.stocks.api.Models.DTOs
{
    public class OperacaoDTO
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Codigo do ativo é obrigatório.")]
        public string CodigoDoAtivo { get; set; }

        [Required(ErrorMessage = "Nome do ativo é obrigatório.")]
        public string nomeDoAtivo { get; set; }

        [Required(ErrorMessage = "Tipo de investimento.")]
        public int tipoDeInvestimento { get; set; }

        [IgnoreDataMember]
        public Ativo Ativo { get; set; }
        
        [Required(ErrorMessage = "Data da operacao é obrigatória.")]
        [DataType(DataType.DateTime, ErrorMessage = "Formato da Data da operacao deve ser <ANO>-<MÊS>-<DIA>T<HORA>:<MINUTO>:<SEGUNDO>.")]
        [DataDaOperacaoValidation]
        public DateTime DataDaOperacao { get; set; }

        [Required(ErrorMessage = "Data da operacao é obrigatória.")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "Quantitidade é obrigatória")]
        [Range(1, 10000, ErrorMessage = "Quantitidade deve estar entre 1 e 1000.")]
        public int Quantitidade { get; set; }

        [Required(ErrorMessage = "Preco unitário é obrigatório")]
        [Range(1, 10000, ErrorMessage = "Preco de compra deve estar entre 1.00 e 100000.00.")]
        public decimal PrecoUnitario { get; set; }
        
        public decimal ValorDaOperacao { get; set; }

        public DateTime DataDeCriacao { get; set; }
    }

    public class DataDaOperacaoValidationAttribute : ValidationAttribute
    {
        public DataDaOperacaoValidationAttribute()
            : base("Data da operação inválida.") { }

        public override bool IsValid(object value)
        {
            return (((DateTime)value) == DateTime.MinValue) ? false : true;
        }
    }
}
