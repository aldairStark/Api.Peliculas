using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Model
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
    }
}
