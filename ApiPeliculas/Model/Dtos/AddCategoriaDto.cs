using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Model.Dtos
{
    public class AddCategoriaDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El numero maximo de caracterer es 100!")]
        public string Name { get; set; }

    }
}
