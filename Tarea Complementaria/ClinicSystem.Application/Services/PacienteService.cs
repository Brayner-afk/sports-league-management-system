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
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _pacienteRepository;

        public PacienteService(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        public async Task<IEnumerable<PacienteDto>> GetAllPacientesAsync()
        {
            var pacientes = await _pacienteRepository.GetAllAsync();
            return pacientes.Select(p => new PacienteDto
            {
                Id = p.Id,
                NombreCompleto = p.NombreCompleto,
                DocumentoIdentidad = p.DocumentoIdentidad,
                FechaNacimiento = p.FechaNacimiento,
                Telefono = p.Telefono,
                Direccion = p.Direccion
            });
        }

        public async Task<PacienteDto?> GetPacienteByIdAsync(int id)
        {
            var p = await _pacienteRepository.GetByIdAsync(id);
            if (p == null) return null;

            return new PacienteDto
            {
                Id = p.Id,
                NombreCompleto = p.NombreCompleto,
                DocumentoIdentidad = p.DocumentoIdentidad,
                FechaNacimiento = p.FechaNacimiento,
                Telefono = p.Telefono,
                Direccion = p.Direccion
            };
        }

        public async Task<PacienteDto> CreatePacienteAsync(PacienteDto dto)
        {
            ValidateDto(dto);

            // Validar lógica de negocio: documento único (removiendo guiones para comparar)
            var all = await _pacienteRepository.GetAllAsync();
            var docClean = dto.DocumentoIdentidad.Replace("-", "");
            if (all.Any(p => p.DocumentoIdentidad.Replace("-", "").Equals(docClean, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ValidationException($"El documento de identidad '{dto.DocumentoIdentidad}' ya está registrado.");
            }

            var paciente = new Paciente
            {
                NombreCompleto = dto.NombreCompleto.Trim(),
                DocumentoIdentidad = dto.DocumentoIdentidad.Trim(),
                FechaNacimiento = dto.FechaNacimiento,
                Telefono = dto.Telefono.Trim(),
                Direccion = dto.Direccion.Trim()
            };

            await _pacienteRepository.AddAsync(paciente);
            dto.Id = paciente.Id;
            return dto;
        }

        public async Task<bool> UpdatePacienteAsync(int id, PacienteDto dto)
        {
            ValidateDto(dto);

            var paciente = await _pacienteRepository.GetByIdAsync(id);
            if (paciente == null) return false;

            // Validar lógica de negocio: documento único
            var all = await _pacienteRepository.GetAllAsync();
            var docClean = dto.DocumentoIdentidad.Replace("-", "");
            if (all.Any(p => p.Id != id && p.DocumentoIdentidad.Replace("-", "").Equals(docClean, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ValidationException($"El documento de identidad '{dto.DocumentoIdentidad}' ya está registrado en otro paciente.");
            }

            paciente.NombreCompleto = dto.NombreCompleto.Trim();
            paciente.DocumentoIdentidad = dto.DocumentoIdentidad.Trim();
            paciente.FechaNacimiento = dto.FechaNacimiento;
            paciente.Telefono = dto.Telefono.Trim();
            paciente.Direccion = dto.Direccion.Trim();

            await _pacienteRepository.UpdateAsync(paciente);
            return true;
        }

        public async Task<bool> DeletePacienteAsync(int id)
        {
            var p = await _pacienteRepository.GetByIdAsync(id);
            if (p == null) return false;

            await _pacienteRepository.DeleteAsync(id);
            return true;
        }

        private void ValidateDto(object dto)
        {
            var context = new ValidationContext(dto);
            Validator.ValidateObject(dto, context, validateAllProperties: true);
        }
    }
}
