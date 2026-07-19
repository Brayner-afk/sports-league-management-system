using ClinicSystem.Application.Contract;
using ClinicSystem.Application.Dtos;
using ClinicSystem.Domain.Entities;
using ClinicSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicSystem.Application.Services
{
    public class CitaService : ICitaService
    {
        private readonly ICitaRepository _citaRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IMedicoRepository _medicoRepository;

        public CitaService(ICitaRepository citaRepository, IPacienteRepository pacienteRepository, IMedicoRepository medicoRepository)
        {
            _citaRepository = citaRepository;
            _pacienteRepository = pacienteRepository;
            _medicoRepository = medicoRepository;
        }

        public async Task<IEnumerable<CitaDto>> GetAllCitasAsync()
        {
            var citas = await _citaRepository.GetAllAsync();
            var pacientes = await _pacienteRepository.GetAllAsync();
            var medicos = await _medicoRepository.GetAllAsync();

            return citas.Select(c => new CitaDto
            {
                Id = c.Id,
                FechaHora = c.FechaHora,
                Motivo = c.Motivo,
                Estado = c.Estado,
                PacienteId = c.PacienteId,
                MedicoId = c.MedicoId,
                PacienteNombre = pacientes.FirstOrDefault(p => p.Id == c.PacienteId)?.NombreCompleto ?? "Desconocido",
                MedicoNombre = medicos.FirstOrDefault(m => m.Id == c.MedicoId)?.NombreCompleto ?? "Desconocido"
            });
        }

        public async Task<CitaDto?> GetCitaByIdAsync(int id)
        {
            var c = await _citaRepository.GetByIdAsync(id);
            if (c == null) return null;

            var paciente = await _pacienteRepository.GetByIdAsync(c.PacienteId);
            var medico = await _medicoRepository.GetByIdAsync(c.MedicoId);

            return new CitaDto
            {
                Id = c.Id,
                FechaHora = c.FechaHora,
                Motivo = c.Motivo,
                Estado = c.Estado,
                PacienteId = c.PacienteId,
                MedicoId = c.MedicoId,
                PacienteNombre = paciente?.NombreCompleto ?? "Desconocido",
                MedicoNombre = medico?.NombreCompleto ?? "Desconocido"
            };
        }

        public async Task<CitaDto> CreateCitaAsync(CitaDto dto)
        {
            ValidateDto(dto);

            if (!await _pacienteRepository.ExistsAsync(dto.PacienteId))
            {
                throw new ValidationException($"El paciente con Id {dto.PacienteId} no existe.");
            }
            if (!await _medicoRepository.ExistsAsync(dto.MedicoId))
            {
                throw new ValidationException($"El médico con Id {dto.MedicoId} no existe.");
            }

            if (dto.FechaHora <= DateTime.Now)
            {
                throw new ValidationException("La fecha y hora de la cita deben ser futuras.");
            }

            // Evitar empalmes de citas para el mismo médico
            var allCitas = await _citaRepository.GetAllAsync();
            if (allCitas.Any(c => c.MedicoId == dto.MedicoId && 
                                 c.Estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase) &&
                                 Math.Abs((c.FechaHora - dto.FechaHora).TotalMinutes) < 30))
            {
                throw new ValidationException("El médico ya tiene una cita agendada en un horario cercano (rango de 30 min).");
            }

            var cita = new Cita
            {
                FechaHora = dto.FechaHora,
                Motivo = dto.Motivo.Trim(),
                Estado = dto.Estado,
                PacienteId = dto.PacienteId,
                MedicoId = dto.MedicoId
            };

            await _citaRepository.AddAsync(cita);
            dto.Id = cita.Id;
            return dto;
        }

        public async Task<bool> UpdateCitaAsync(int id, CitaDto dto)
        {
            ValidateDto(dto);

            var cita = await _citaRepository.GetByIdAsync(id);
            if (cita == null) return false;

            if (!await _pacienteRepository.ExistsAsync(dto.PacienteId))
            {
                throw new ValidationException($"El paciente con Id {dto.PacienteId} no existe.");
            }
            if (!await _medicoRepository.ExistsAsync(dto.MedicoId))
            {
                throw new ValidationException($"El médico con Id {dto.MedicoId} no existe.");
            }

            // Evitar empalmes de citas excluyendo esta cita
            var allCitas = await _citaRepository.GetAllAsync();
            if (allCitas.Any(c => c.Id != id && 
                                 c.MedicoId == dto.MedicoId && 
                                 c.Estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase) &&
                                 Math.Abs((c.FechaHora - dto.FechaHora).TotalMinutes) < 30))
            {
                throw new ValidationException("El médico ya tiene otra cita agendada en un horario cercano.");
            }

            cita.FechaHora = dto.FechaHora;
            cita.Motivo = dto.Motivo.Trim();
            cita.Estado = dto.Estado;
            cita.PacienteId = dto.PacienteId;
            cita.MedicoId = dto.MedicoId;

            await _citaRepository.UpdateAsync(cita);
            return true;
        }

        public async Task<bool> DeleteCitaAsync(int id)
        {
            var c = await _citaRepository.GetByIdAsync(id);
            if (c == null) return false;

            await _citaRepository.DeleteAsync(id);
            return true;
        }

        private void ValidateDto(object dto)
        {
            var context = new ValidationContext(dto);
            Validator.ValidateObject(dto, context, validateAllProperties: true);
        }
    }
}
