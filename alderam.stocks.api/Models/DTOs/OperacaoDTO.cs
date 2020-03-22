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
        public int id { get; set; }

        [Required(ErrorMessage = "Codigo do ativo é obrigatório.")]
        public string codigoDoAtivo { get; set; }


        [IgnoreDataMember]
        public Ativo ativo { get; set; }
        
        [Required(ErrorMessage = "Data da operacao é obrigatória.")]
        [DataType(DataType.DateTime, ErrorMessage = "Formato da Data da operacao deve ser <ANO>-<MÊS>-<DIA>T<HORA>:<MINUTO>:<SEGUNDO>.")]
        [DataDaOperacaoValidation]
        public DateTime dataDaOperacao { get; set; }

        [Required(ErrorMessage = "Quantitidade é obrigatória")]
        [Range(1, 10000, ErrorMessage = "Quantitidade deve estar entre 1 e 1000.")]
        public int quantitidade { get; set; }

        [Required(ErrorMessage = "Preco de compra é obrigatório")]
        [Range(1, 10000, ErrorMessage = "Preco de compra deve estar entre 1.00 e 100000.00.")]
        public double precoDeCompra { get; set; }
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
