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
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository? _playerRepository;

        #region Sobrecargas de Constructores (Constructor Overloading)

        /// <summary>
        /// Constructor principal: Inyecta el repositorio de equipos.
        /// </summary>
        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        }

        /// <summary>
        /// Sobrecarga de constructor: Permite inyectar también el repositorio de jugadores para operaciones cruzadas.
        /// </summary>
        public TeamService(ITeamRepository teamRepository, IPlayerRepository playerRepository)
        {
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
            _playerRepository = playerRepository;
        }

        #endregion

        #region Sobrecargas de Métodos (Method Overloading)

        public async Task<IEnumerable<TeamDto>> GetAllTeamsAsync()
        {
            var teams = await _teamRepository.GetAllTeamsWithPlayersAsync();
            return teams.Select(t => new TeamDto(
                t.Id,
                t.Name,
                t.City,
                t.Players?.Count ?? 0
            ));
        }

        /// <summary>
        /// Sobrecarga 1: Obtener todos los equipos con filtro opcional de ciudad.
        /// </summary>
        public async Task<IEnumerable<TeamDto>> GetAllTeamsAsync(string? cityFilter)
        {
            if (string.IsNullOrWhiteSpace(cityFilter))
            {
                return await GetAllTeamsAsync();
            }

            var teams = await _teamRepository.GetTeamsByCityAsync(cityFilter.Trim());
            return teams.Select(t => new TeamDto(
                t.Id,
                t.Name,
                t.City,
                t.Players?.Count ?? 0
            ));
        }

        public async Task<TeamDto?> GetTeamByIdAsync(int id)
        {
            var team = await _teamRepository.GetTeamWithPlayersAsync(id);
            if (team == null) return null;

            return new TeamDto(
                team.Id,
                team.Name,
                team.City,
                team.Players?.Count ?? 0
            );
        }

        public async Task<TeamDto> CreateTeamAsync(TeamDto teamDto)
        {
            ValidateDto(teamDto);

            // Validar lógica de negocio: nombre duplicado en la misma ciudad
            var existingTeams = await _teamRepository.GetAllAsync();
            if (existingTeams.Any(t => t.Name.Equals(teamDto.Name, StringComparison.OrdinalIgnoreCase) && 
                                       t.City.Equals(teamDto.City, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ValidationException($"Ya existe un equipo con el nombre '{teamDto.Name}' en la ciudad '{teamDto.City}'.");
            }

            var team = new Team(teamDto.Name.Trim(), teamDto.City.Trim());

            await _teamRepository.AddAsync(team);
            teamDto.Id = team.Id;
            return teamDto;
        }

        /// <summary>
        /// Sobrecarga 2: Crear equipo directamente a partir de sus parámetros individuales.
        /// </summary>
        public async Task<TeamDto> CreateTeamAsync(string name, string city)
        {
            var dto = new TeamDto(name, city);
            return await CreateTeamAsync(dto);
        }

        public async Task<bool> UpdateTeamAsync(int id, TeamDto teamDto)
        {
            ValidateDto(teamDto);

            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null) return false;

            // Validar lógica de negocio: nombre duplicado en la misma ciudad excluyendo el equipo actual
            var existingTeams = await _teamRepository.GetAllAsync();
            if (existingTeams.Any(t => t.Id != id && 
                                       t.Name.Equals(teamDto.Name, StringComparison.OrdinalIgnoreCase) && 
                                       t.City.Equals(teamDto.City, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ValidationException($"Ya existe otro equipo con el nombre '{teamDto.Name}' en la ciudad '{teamDto.City}'.");
            }

            team.Name = teamDto.Name.Trim();
            team.City = teamDto.City.Trim();

            await _teamRepository.UpdateAsync(team);
            return true;
        }

        public async Task<bool> DeleteTeamAsync(int id)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null) return false;

            await _teamRepository.DeleteAsync(id);
            return true;
        }

        #endregion

        private void ValidateDto(object dto)
        {
            var context = new ValidationContext(dto);
            Validator.ValidateObject(dto, context, validateAllProperties: true);
        }
    }
}

