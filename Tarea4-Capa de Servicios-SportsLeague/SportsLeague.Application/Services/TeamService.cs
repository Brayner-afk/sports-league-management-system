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

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<IEnumerable<TeamDto>> GetAllTeamsAsync()
        {
            var teams = await _teamRepository.GetAllAsync();
            return teams.Select(t => new TeamDto
            {
                Id = t.Id,
                Name = t.Name,
                City = t.City
            });
        }

        public async Task<TeamDto?> GetTeamByIdAsync(int id)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null) return null;

            return new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                City = team.City
            };
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

            var team = new Team
            {
                Name = teamDto.Name.Trim(),
                City = teamDto.City.Trim()
            };

            await _teamRepository.AddAsync(team);
            teamDto.Id = team.Id;
            return teamDto;
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

        private void ValidateDto(object dto)
        {
            var context = new ValidationContext(dto);
            Validator.ValidateObject(dto, context, validateAllProperties: true);
        }
    }
}
