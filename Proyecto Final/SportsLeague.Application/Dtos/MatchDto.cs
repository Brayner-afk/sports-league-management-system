using System;
using System.ComponentModel.DataAnnotations;

namespace SportsLeague.Application.Dtos
{
    public class MatchDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El equipo local es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un equipo local válido.")]
        public int HomeTeamId { get; set; }
        public string? HomeTeamName { get; set; }

        [Required(ErrorMessage = "El equipo visitante es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un equipo visitante válido.")]
        public int AwayTeamId { get; set; }
        public string? AwayTeamName { get; set; }

        [Required(ErrorMessage = "La fecha y hora del partido es obligatoria.")]
        public DateTime MatchDate { get; set; } = DateTime.Today.AddDays(1).AddHours(19);

        [Required(ErrorMessage = "La sede o estadio del partido es obligatoria.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "La sede debe tener entre 3 y 150 caracteres.")]
        public string Location { get; set; } = string.Empty;

        [Range(0, 999, ErrorMessage = "El puntaje local no puede ser negativo.")]
        public int? HomeScore { get; set; }

        [Range(0, 999, ErrorMessage = "El puntaje visitante no puede ser negativo.")]
        public int? AwayScore { get; set; }

        [Required(ErrorMessage = "El estado del partido es obligatorio.")]
        public string Status { get; set; } = "Programado";

        #region Sobrecarga de Constructores (Constructor Overloading)

        public MatchDto()
        {
        }

        public MatchDto(int homeTeamId, int awayTeamId, DateTime matchDate, string location)
        {
            HomeTeamId = homeTeamId;
            AwayTeamId = awayTeamId;
            MatchDate = matchDate;
            Location = location;
            Status = "Programado";
        }

        public MatchDto(int id, int homeTeamId, string? homeTeamName, int awayTeamId, string? awayTeamName, DateTime matchDate, string location, int? homeScore, int? awayScore, string status)
        {
            Id = id;
            HomeTeamId = homeTeamId;
            HomeTeamName = homeTeamName;
            AwayTeamId = awayTeamId;
            AwayTeamName = awayTeamName;
            MatchDate = matchDate;
            Location = location;
            HomeScore = homeScore;
            AwayScore = awayScore;
            Status = status;
        }

        #endregion
    }
}