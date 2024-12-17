using ApiPeliculas.Model;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface ICategoriasRepositorio
    {
        ICollection<Categoria> GetCategorias();
        Categoria GetCategoria(int categoriaId);
        bool ExistCategoria(int categoriaId);
        bool ExistCategoria(string nameCategoria);



        bool AddCategoria(Categoria categoria);
        bool DeleteCategoria(Categoria categoria);
        bool UpdateCategoria(Categoria categoria);

        bool Save();
    }
}
