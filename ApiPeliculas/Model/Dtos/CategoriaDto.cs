using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Model.Dtos
{
    public class CategoriaDto
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage ="El numero maximo de caracterer es 100!")]
        public string Name { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
    }
}
