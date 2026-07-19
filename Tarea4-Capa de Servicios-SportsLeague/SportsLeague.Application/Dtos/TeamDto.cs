using System.ComponentModel.DataAnnotations;

namespace SportsLeague.Application.Dtos
{
    public class TeamDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del equipo es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del equipo debe tener entre 3 y 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ciudad del equipo es obligatoria.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La ciudad debe tener entre 3 y 100 caracteres.")]
        public string City { get; set; } = string.Empty;
    }
}
