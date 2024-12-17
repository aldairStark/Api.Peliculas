using ApiPeliculas.Data;
using ApiPeliculas.Model;
using ApiPeliculas.Repositorio.IRepositorio;

namespace ApiPeliculas.Repositorio
{
    public class CategoriasRepositorio : ICategoriasRepositorio
    {
        private readonly ApiDBContext _db;
        public CategoriasRepositorio(ApiDBContext db)
        {
            _db = db;
        }

        public bool AddCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _db.Categorias.Add(categoria);
            return Save();
        }

        public bool DeleteCategoria(Categoria categoria)
        {
            _db.Categorias.Remove(categoria);
            return Save();
        }

        public bool ExistCategoria(int categoriaId)
        {
            return _db.Categorias.Any(c => c.Id == categoriaId);
        }

        public bool ExistCategoria(string nameCategoria)
        {
            return _db.Categorias.Any(c=>c.Name == nameCategoria);
        }

        public Categoria GetCategoria(int categoriaId)
        {
          return _db.Categorias.FirstOrDefault(c=>c.Id == categoriaId);
        }

        public ICollection<Categoria> GetCategorias()
        {
             return   _db.Categorias.OrderBy(c => c.Name).ToList();
            
        }

        public bool Save()
        {
           return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            //Arregla problema del PUT
            var categoriaExistente = _db.Categorias.Find(categoria.Id);
            if (categoriaExistente != null)
            {
                _db.Entry(categoriaExistente).CurrentValues.SetValues(categoria);
            }
            else
            {
                _db.Update(categoria);
            }
            _db.Categorias.Update(categoria);
            return Save();
        }
    }
}
