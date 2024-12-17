using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Model.Dtos
{
    public class UserRegistrationRequestDto
    {
        [Required(ErrorMessage ="El campo es obligatorio")]
        [MaxLength(100,ErrorMessage ="el campo no puede tener mas de 100 caracteres!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        public string Password { get; set; }
    }
}
