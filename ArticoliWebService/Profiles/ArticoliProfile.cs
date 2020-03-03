using ArticoliWebService.Dtos;
using ArticoliWebService.Models;
using AutoMapper;

namespace ArticoliWebService.Profiles
{
    public class ArticoliProfile : Profile
    {
        public ArticoliProfile()
        {
            CreateMap<Articoli, ArticoliDTO>()
                .ForMember
                (
                    dest => dest.Categoria,
                    opt => opt.MapFrom(src => $"{src.IdFamAss} {src.FamAssort.Descrizione}")
                )
                .ForMember
                (
                    dest => dest.CodStat,  
                    opt => opt.MapFrom(src => src.CodStat.Trim()) //trim -> per togliere spazi in lettura
                )
                .ForMember
                (
                    dest => dest.Um,
                    opt => opt.MapFrom(src => src.Um.Trim())
                )
                .ForMember
                (
                    dest => dest.IdStatoArt,
                    opt => opt.MapFrom(src => src.IdStatoArt.Trim())
                );
        }
    }
}