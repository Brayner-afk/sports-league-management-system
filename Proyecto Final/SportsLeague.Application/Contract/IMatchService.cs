using SportsLeague.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsLeague.Application.Contract
{
    public interface IMatchService
    {
        Task<IEnumerable<MatchDto>> GetAllMatchesAsync();
        
        // Sobrecarga 1: Filtrar partidos por estado (Programado, En Curso, Finalizado)
        Task<IEnumerable<MatchDto>> GetAllMatchesAsync(string? statusFilter);

        // Sobrecarga 2: Filtrar partidos por equipo participante
        Task<IEnumerable<MatchDto>> GetMatchesByTeamAsync(int teamId);

        Task<MatchDto?> GetMatchByIdAsync(int id);

        Task<MatchDto> CreateMatchAsync(MatchDto matchDto);

        // Sobrecarga 3: Crear partido mediante parámetros individuales
        Task<MatchDto> CreateMatchAsync(int homeTeamId, int awayTeamId, DateTime matchDate, string location);

        Task<bool> UpdateMatchAsync(int id, MatchDto matchDto);
        Task<bool> DeleteMatchAsync(int id);
    }
}