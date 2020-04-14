using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace alderam.stocks.api.Models.DTOs
{
    public class ResumoDTO
    {
        public decimal ValorTotalInvestido { get; set; }
        public decimal ValorAtualDaCarteiral { get; set; }
        public decimal SaldoAtualDaCarteiral { get; set; }
        public decimal LiquidezAtualDaCarteiral { get; set; }
    }
}
