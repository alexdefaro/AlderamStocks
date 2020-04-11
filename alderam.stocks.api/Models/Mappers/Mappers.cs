using alderam.stocks.api.Controllers;
using alderam.stocks.api.Models.DTOs;
using AutoMapper;

namespace alderam.stocks.api.Models.Mappers
{
    public class Mappers: Profile
    {
        public Mappers()
        {
            CreateMap<Boleta, BoletaDTO>(); 
            CreateMap<BoletaDTO, Boleta>(); 

            CreateMap<Operacao, OperacaoDTO>(); 
            CreateMap<OperacaoDTO, Operacao>(); 

            CreateMap<Ativo, AtivoDTO>(); 
            CreateMap<AtivoDTO, Ativo>(); 

            CreateMap<Acompanhamento, AcompanhamentoDTO>(); 
            CreateMap<AcompanhamentoDTO, Acompanhamento>(); 

        }
    }
}
