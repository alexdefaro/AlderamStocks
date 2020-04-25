using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alderam.stocks.api.Models.DTOs
{
    public class BoletaDTO
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Número da boleta é obrigatório.")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Data da operação é obrigatória.")]
        [DataType(DataType.DateTime, ErrorMessage = "Formato da Data da operação deve ser <ANO>-<MÊS>-<DIA>T<HORA>:<MINUTO>:<SEGUNDO>.")]
        [DataDaBoletaValidation]
        public DateTime DataDaOperacao { get; set; }

        public decimal ValorDaOperacao { get; set; }

        public decimal ValorDaCompra { get; set; }

        public decimal TaxaDaCoretagem { get; set; } = 10;

        [MaxLength(500)]
        public string Observacoes { get; set; }

        public IList<OperacaoDTO> Operacoes { get; set; }
    }

    public class DataDaBoletaValidationAttribute : ValidationAttribute
    {
        public DataDaBoletaValidationAttribute()
            : base("Data da boleta inválida.") { }

        public override bool IsValid(object value)
        {
            return (((DateTime)value) == DateTime.MinValue) ? false : true;
        }
    }
}
