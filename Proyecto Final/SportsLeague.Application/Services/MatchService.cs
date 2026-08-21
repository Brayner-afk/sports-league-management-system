using SportsLeague.Application.Contract;
using SportsLeague.Application.Dtos;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsLeague.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ITeamRepository _teamRepository;

        #region Sobrecarga de Constructores (Constructor Overloading)

        public MatchService(IMatchRepository matchRepository, ITeamRepository teamRepository)
        {
            _matchRepository = matchRepository ?? throw new ArgumentNullException(nameof(matchRepository));
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        }

        #endregion

        #region Sobrecargas de Métodos (Method Overloading)

        public async Task<IEnumerable<MatchDto>> GetAllMatchesAsync()
        {
            var matches = await _matchRepository.GetAllMatchesWithTeamsAsync();
            return matches.Select(MapToDto);
        }

        public async Task<IEnumerable<MatchDto>> GetAllMatchesAsync(string? statusFilter)
        {
            if (string.IsNullOrWhiteSpace(statusFilter) || statusFilter.Equals("Todos", StringComparison.OrdinalIgnoreCase))
            {
                return await GetAllMatchesAsync();
            }

            var matches = await _matchRepository.GetMatchesByStatusAsync(statusFilter.Trim());
            return matches.Select(MapToDto);
        }

        public async Task<IEnumerable<MatchDto>> GetMatchesByTeamAsync(int teamId)
        {
            if (teamId <= 0)
            {
                return await GetAllMatchesAsync();
            }

            var matches = await _matchRepository.GetMatchesByTeamIdAsync(teamId);
            return matches.Select(MapToDto);
        }

        public async Task<MatchDto?> GetMatchByIdAsync(int id)
        {
            var match = await _matchRepository.GetMatchWithTeamsByIdAsync(id);
            if (match == null) return null;

            return MapToDto(match);
        }

        public async Task<MatchDto> CreateMatchAsync(MatchDto matchDto)
        {
            ValidateDto(matchDto);

            // Regla de negocio: El equipo local y visitante no pueden ser el mismo
            if (matchDto.HomeTeamId == matchDto.AwayTeamId)
            {
                throw new ValidationException("El equipo local y el equipo visitante no pueden ser el mismo.");
            }

            // Validar que ambos equipos existan
            if (!await _teamRepository.ExistsAsync(matchDto.HomeTeamId))
            {
                throw new ValidationException($"El equipo local con ID {matchDto.HomeTeamId} no existe.");
            }

            if (!await _teamRepository.ExistsAsync(matchDto.AwayTeamId))
            {
                throw new ValidationException($"El equipo visitante con ID {matchDto.AwayTeamId} no existe.");
            }

            var match = new Match(
                matchDto.HomeTeamId,
                matchDto.AwayTeamId,
                matchDto.MatchDate,
                matchDto.Location.Trim()
            );

            match.HomeScore = matchDto.HomeScore;
            match.AwayScore = matchDto.AwayScore;
            match.Status = string.IsNullOrWhiteSpace(matchDto.Status) ? "Programado" : matchDto.Status.Trim();

            await _matchRepository.AddAsync(match);
            matchDto.Id = match.Id;

            // Enriquecer nombres de equipos
            var homeTeam = await _teamRepository.GetByIdAsync(matchDto.HomeTeamId);
            var awayTeam = await _teamRepository.GetByIdAsync(matchDto.AwayTeamId);
            matchDto.HomeTeamName = homeTeam?.Name ?? "Equipo Local";
            matchDto.AwayTeamName = awayTeam?.Name ?? "Equipo Visitante";

            return matchDto;
        }

        public async Task<MatchDto> CreateMatchAsync(int homeTeamId, int awayTeamId, DateTime matchDate, string location)
        {
            var dto = new MatchDto(homeTeamId, awayTeamId, matchDate, location);
            return await CreateMatchAsync(dto);
        }

        public async Task<bool> UpdateMatchAsync(int id, MatchDto matchDto)
        {
            ValidateDto(matchDto);

            if (matchDto.HomeTeamId == matchDto.AwayTeamId)
            {
                throw new ValidationException("El equipo local y el equipo visitante no pueden ser el mismo.");
            }

            var match = await _matchRepository.GetByIdAsync(id);
            if (match == null) return false;

            if (!await _teamRepository.ExistsAsync(matchDto.HomeTeamId))
            {
                throw new ValidationException($"El equipo local con ID {matchDto.HomeTeamId} no existe.");
            }

            if (!await _teamRepository.ExistsAsync(matchDto.AwayTeamId))
            {
                throw new ValidationException($"El equipo visitante con ID {matchDto.AwayTeamId} no existe.");
            }

            match.HomeTeamId = matchDto.HomeTeamId;
            match.AwayTeamId = matchDto.AwayTeamId;
            match.MatchDate = matchDto.MatchDate;
            match.Location = matchDto.Location.Trim();
            match.HomeScore = matchDto.HomeScore;
            match.AwayScore = matchDto.AwayScore;
            match.Status = matchDto.Status.Trim();

            await _matchRepository.UpdateAsync(match);
            return true;
        }

        public async Task<bool> DeleteMatchAsync(int id)
        {
            var match = await _matchRepository.GetByIdAsync(id);
            if (match == null) return false;

            await _matchRepository.DeleteAsync(id);
            return true;
        }

        #endregion

        private static MatchDto MapToDto(Match m)
        {
            return new MatchDto(
                m.Id,
                m.HomeTeamId,
                m.HomeTeam?.Name ?? $"Equipo #{m.HomeTeamId}",
                m.AwayTeamId,
                m.AwayTeam?.Name ?? $"Equipo #{m.AwayTeamId}",
                m.MatchDate,
                m.Location,
                m.HomeScore,
                m.AwayScore,
                m.Status
            );
        }

        private void ValidateDto(object dto)
        {
            var context = new ValidationContext(dto);
            Validator.ValidateObject(dto, context, validateAllProperties: true);
        }
    }
}