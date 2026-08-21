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

        public int PlayerCount { get; set; }

        #region Sobrecarga de Constructores (Constructor Overloading)

        public TeamDto()
        {
        }

        public TeamDto(string name, string city)
        {
            Name = name;
            City = city;
        }

        public TeamDto(int id, string name, string city)
        {
            Id = id;
            Name = name;
            City = city;
        }

        public TeamDto(int id, string name, string city, int playerCount)
        {
            Id = id;
            Name = name;
            City = city;
            PlayerCount = playerCount;
        }

        #endregion
    }
}

