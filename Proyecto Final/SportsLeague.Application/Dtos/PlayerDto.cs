using System.ComponentModel.DataAnnotations;

namespace SportsLeague.Application.Dtos
{
    public class PlayerDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre completo del jugador es obligatorio.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre completo debe tener entre 3 y 150 caracteres.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La posición del jugador es obligatoria.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "La posición debe tener entre 2 y 50 caracteres.")]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "El identificador del equipo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del equipo debe ser mayor a 0.")]
        public int TeamId { get; set; }

        public string? TeamName { get; set; }

        #region Sobrecarga de Constructores (Constructor Overloading)

        public PlayerDto()
        {
        }

        public PlayerDto(string fullName, string position, int teamId)
        {
            FullName = fullName;
            Position = position;
            TeamId = teamId;
        }

        public PlayerDto(int id, string fullName, string position, int teamId)
        {
            Id = id;
            FullName = fullName;
            Position = position;
            TeamId = teamId;
        }

        public PlayerDto(int id, string fullName, string position, int teamId, string? teamName)
        {
            Id = id;
            FullName = fullName;
            Position = position;
            TeamId = teamId;
            TeamName = teamName;
        }

        #endregion
    }
}

