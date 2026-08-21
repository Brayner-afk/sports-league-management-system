using SportsLeague.Domain.Core;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsLeague.Domain.Entities
{
    /// <summary>
    /// Entidad que representa un Partido / Encuentro deportivo en el calendario de la liga.
    /// Hereda de la clase abstracta BaseEntity.
    /// </summary>
    public class Match : BaseEntity
    {
        public int HomeTeamId { get; set; }

        [ForeignKey("HomeTeamId")]
        public Team? HomeTeam { get; set; }

        public int AwayTeamId { get; set; }

        [ForeignKey("AwayTeamId")]
        public Team? AwayTeam { get; set; }

        public DateTime MatchDate { get; set; } = DateTime.UtcNow.AddDays(1);
        public string Location { get; set; } = string.Empty;

        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }

        // Estados posibles: Programado, En Curso, Finalizado, Cancelado
        public string Status { get; set; } = "Programado";

        #region Sobrecarga de Constructores (Constructor Overloading)

        public Match()
        {
        }

        public Match(int homeTeamId, int awayTeamId, DateTime matchDate, string location)
        {
            HomeTeamId = homeTeamId;
            AwayTeamId = awayTeamId;
            MatchDate = matchDate;
            Location = location;
            Status = "Programado";
        }

        public Match(int id, int homeTeamId, int awayTeamId, DateTime matchDate, string location, int? homeScore, int? awayScore, string status)
        {
            Id = id;
            HomeTeamId = homeTeamId;
            AwayTeamId = awayTeamId;
            MatchDate = matchDate;
            Location = location;
            HomeScore = homeScore;
            AwayScore = awayScore;
            Status = status;
        }

        #endregion

        #region Sobrecarga de Métodos (Method Overloading)

        /// <summary>
        /// Sobrecarga 1: Actualizar resultado y marcar automáticamente como Finalizado.
        /// </summary>
        public void UpdateResult(int homeScore, int awayScore)
        {
            HomeScore = homeScore;
            AwayScore = awayScore;
            Status = "Finalizado";
        }

        /// <summary>
        /// Sobrecarga 2: Actualizar resultado con estado explícito (ej. En Curso o Finalizado).
        /// </summary>
        public void UpdateResult(int homeScore, int awayScore, string status)
        {
            HomeScore = homeScore;
            AwayScore = awayScore;
            Status = status;
        }

        #endregion

        #region Polimorfismo y Métodos Abstractos Implementados

        public override string GetEntitySummary()
        {
            return $"Partido: {HomeTeam?.Name ?? $"Equipo #{HomeTeamId}"} vs {AwayTeam?.Name ?? $"Equipo #{AwayTeamId}"} | Fecha: {MatchDate:g} | Estado: {Status}";
        }

        public override string ToString()
        {
            return $"{HomeTeam?.Name ?? "Local"} vs {AwayTeam?.Name ?? "Visitante"} ({MatchDate:dd/MM/yyyy})";
        }

        #endregion
    }
}