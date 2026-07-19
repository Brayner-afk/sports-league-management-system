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

        public PlayerService(IPlayerRepository playerRepository, ITeamRepository teamRepository)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
        }

        public async Task<IEnumerable<PlayerDto>> GetAllPlayersAsync()
        {
            var players = await _playerRepository.GetAllAsync();
            return players.Select(p => new PlayerDto
            {
                Id = p.Id,
                FullName = p.FullName,
                Position = p.Position,
                TeamId = p.TeamId
            });
        }

        public async Task<PlayerDto?> GetPlayerByIdAsync(int id)
        {
            var player = await _playerRepository.GetByIdAsync(id);
            if (player == null) return null;

            return new PlayerDto
            {
                Id = player.Id,
                FullName = player.FullName,
                Position = player.Position,
                TeamId = player.TeamId
            };
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

            var player = new Player
            {
                FullName = playerDto.FullName.Trim(),
                Position = playerDto.Position.Trim(),
                TeamId = playerDto.TeamId
            };

            await _playerRepository.AddAsync(player);
            playerDto.Id = player.Id;
            return playerDto;
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

            player.FullName = playerDto.FullName.Trim();
            player.Position = playerDto.Position.Trim();
            player.TeamId = playerDto.TeamId;

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

        private void ValidateDto(object dto)
        {
            var context = new ValidationContext(dto);
            Validator.ValidateObject(dto, context, validateAllProperties: true);
        }
    }
}
