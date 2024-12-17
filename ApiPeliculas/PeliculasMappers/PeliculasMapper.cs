using ApiPeliculas.Model;
using ApiPeliculas.Model.Dtos;
using AutoMapper;

namespace ApiPeliculas.PeliculasMappers
{
    public class PeliculasMapper:Profile
    {
        public PeliculasMapper()
        {
            CreateMap<Categoria,CategoriaDto>().ReverseMap();
            CreateMap<Categoria, AddCategoriaDto>().ReverseMap();
            CreateMap<UserRegistrationRequest, UserRegistrationRequestDto>().ReverseMap();
        }
    }
}
