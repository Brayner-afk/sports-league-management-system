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
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly bool _strictValidation;

        #region Sobrecargas de Constructores (Constructor Overloading)

        /// <summary>
        /// Constructor principal: Inyecta los repositorios necesarios.
        /// </summary>
        public PlayerService(IPlayerRepository playerRepository, ITeamRepository teamRepository)
        {
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
            _strictValidation = true;
        }

        /// <summary>
        /// Sobrecarga de constructor: Permite configurar el modo de validación estricto de forma explícita.
        /// </summary>
        public PlayerService(IPlayerRepository playerRepository, ITeamRepository teamRepository, bool strictValidation)
        {
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
            _strictValidation = strictValidation;
        }

        #endregion

        #region Sobrecargas de Métodos (Method Overloading)

        public async Task<IEnumerable<PlayerDto>> GetAllPlayersAsync()
        {
            var players = await _playerRepository.GetAllPlayersWithTeamAsync();
            return players.Select(p => new PlayerDto(
                p.Id,
                p.FullName,
                p.Position,
                p.TeamId,
                p.Team?.Name ?? "Sin Equipo"
            ));
        }

        /// <summary>
        /// Sobrecarga 1: Obtener jugadores filtrando por ID de equipo.
        /// </summary>
        public async Task<IEnumerable<PlayerDto>> GetAllPlayersAsync(int? teamIdFilter)
        {
            if (!teamIdFilter.HasValue || teamIdFilter.Value <= 0)
            {
                return await GetAllPlayersAsync();
            }

            var players = await _playerRepository.GetPlayersByTeamIdAsync(teamIdFilter.Value);
            return players.Select(p => new PlayerDto(
                p.Id,
                p.FullName,
                p.Position,
                p.TeamId,
                p.Team?.Name ?? "Sin Equipo"
            ));
        }

        public async Task<PlayerDto?> GetPlayerByIdAsync(int id)
        {
            var player = await _playerRepository.GetPlayerWithTeamAsync(id);
            if (player == null) return null;

            return new PlayerDto(
                player.Id,
                player.FullName,
                player.Position,
                player.TeamId,
                player.Team?.Name ?? "Sin Equipo"
            );
        }

        public async Task<PlayerDto> CreatePlayerAsync(PlayerDto playerDto)
        {
            ValidateDto(playerDto);

            // Validar que el equipo exista
            if (!await _teamRepository.ExistsAsync(playerDto.TeamId))
            {
                throw new ValidationException($"El equipo con Id {playerDto.TeamId} no existe.");
            }

            // Validar lógica de negocio: nombre duplicado en el mismo equipo
            var existingPlayers = await _playerRepository.GetAllAsync();
            if (existingPlayers.Any(p => p.TeamId == playerDto.TeamId && 
                                         p.FullName.Equals(playerDto.FullName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ValidationException($"Ya existe un jugador con el nombre '{playerDto.FullName}' en este equipo.");
            }

            var player = new Player(playerDto.FullName.Trim(), playerDto.Position.Trim(), playerDto.TeamId);

            await _playerRepository.AddAsync(player);
            playerDto.Id = player.Id;

            var team = await _teamRepository.GetByIdAsync(playerDto.TeamId);
            playerDto.TeamName = team?.Name ?? "Sin Equipo";

            return playerDto;
        }

        /// <summary>
        /// Sobrecarga 2: Crear jugador pasando directamente los argumentos primitivos.
        /// </summary>
        public async Task<PlayerDto> CreatePlayerAsync(string fullName, string position, int teamId)
        {
            var dto = new PlayerDto(fullName, position, teamId);
            return await CreatePlayerAsync(dto);
        }

        public async Task<bool> UpdatePlayerAsync(int id, PlayerDto playerDto)
        {
            ValidateDto(playerDto);

            var player = await _playerRepository.GetByIdAsync(id);
            if (player == null) return false;

            // Validar que el equipo exista
            if (!await _teamRepository.ExistsAsync(playerDto.TeamId))
            {
                throw new ValidationException($"El equipo con Id {playerDto.TeamId} no existe.");
            }

            // Validar lógica de negocio: nombre duplicado en el mismo equipo excluyendo al jugador actual
            var existingPlayers = await _playerRepository.GetAllAsync();
            if (existingPlayers.Any(p => p.Id != id && 
                                         p.TeamId == playerDto.TeamId && 
                                         p.FullName.Equals(playerDto.FullName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ValidationException($"Ya existe otro jugador con el nombre '{playerDto.FullName}' en este equipo.");
            }

            player.UpdateInfo(playerDto.FullName.Trim(), playerDto.Position.Trim(), playerDto.TeamId);

            await _playerRepository.UpdateAsync(player);
            return true;
        }

        public async Task<bool> DeletePlayerAsync(int id)
        {
            var player = await _playerRepository.GetByIdAsync(id);
            if (player == null) return false;

            await _playerRepository.DeleteAsync(id);
            return true;
        }

        #endregion

        private void ValidateDto(object dto)
        {
            if (!_strictValidation) return;
            var context = new ValidationContext(dto);
            Validator.ValidateObject(dto, context, validateAllProperties: true);
        }
    }
}

