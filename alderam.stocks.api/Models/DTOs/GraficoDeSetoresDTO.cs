using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace alderam.stocks.api.Models.DTOs
{
    public class GraficoDeSetoresDTO
    {
        //public string Labels { get; set; }
        //public decimal Values { get; set; }

        public string[] Labels { get; set; }
        public decimal[] Values { get; set; }
    }
}
