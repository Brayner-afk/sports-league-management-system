using Microsoft.EntityFrameworkCore;
using ClinicSystem.Domain.Entities;
using System;

namespace ClinicSystem.Infrastructure.Context
{
    public class ClinicDbContext : DbContext
    {
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
        {
        }

        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<HistorialMedico> HistorialesMedicos { get; set; }
        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones de relaciones y eliminación en cascada
            modelBuilder.Entity<Medico>()
                .HasOne(m => m.Especialidad)
                .WithMany(e => e.Medicos)
                .HasForeignKey(m => m.EspecialidadId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Paciente)
                .WithMany(p => p.Citas)
                .HasForeignKey(c => c.PacienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Medico)
                .WithMany(m => m.Citas)
                .HasForeignKey(c => c.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HistorialMedico>()
                .HasOne(h => h.Paciente)
                .WithMany(p => p.Historiales)
                .HasForeignKey(h => h.PacienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tratamiento>()
                .HasOne(t => t.HistorialMedico)
                .WithMany(h => h.Tratamientos)
                .HasForeignKey(t => t.HistorialMedicoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Receta>()
                .HasOne(r => r.Paciente)
                .WithMany()
                .HasForeignKey(r => r.PacienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Paciente)
                .WithMany(p => p.Facturas)
                .HasForeignKey(f => f.PacienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Factura)
                .WithMany(f => f.Pagos)
                .HasForeignKey(p => p.FacturaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Datos Iniciales (Seeds)
            modelBuilder.Entity<Especialidad>().HasData(
                new Especialidad { Id = 1, Nombre = "Cardiología", Descripcion = "Salud cardiovascular" },
                new Especialidad { Id = 2, Nombre = "Pediatría", Descripcion = "Atención médica infantil" },
                new Especialidad { Id = 3, Nombre = "Dermatología", Descripcion = "Tratamiento de la piel" }
            );

            modelBuilder.Entity<Medico>().HasData(
                new Medico { Id = 1, NombreCompleto = "Dr. Carlos Pérez", LicenciaMedica = "MED-10023", Telefono = "809-555-0199", EspecialidadId = 1 },
                new Medico { Id = 2, NombreCompleto = "Dra. María Gómez", LicenciaMedica = "MED-49930", Telefono = "809-555-0210", EspecialidadId = 2 }
            );

            modelBuilder.Entity<Paciente>().HasData(
                new Paciente { Id = 1, NombreCompleto = "Juan Bosch", DocumentoIdentidad = "001-2345678-9", FechaNacimiento = new DateTime(1985, 5, 20), Telefono = "809-444-1234", Direccion = "Calle Central 10, Santo Domingo" },
                new Paciente { Id = 2, NombreCompleto = "Salomé Ureña", DocumentoIdentidad = "002-9876543-2", FechaNacimiento = new DateTime(1992, 10, 15), Telefono = "829-333-5678", Direccion = "Av. Bolívar 45, Santo Domingo" }
            );

            modelBuilder.Entity<Medicamento>().HasData(
                new Medicamento { Id = 1, Nombre = "Paracetamol", Concentracion = "500mg", Presentacion = "Tabletas" },
                new Medicamento { Id = 2, Nombre = "Ibuprofeno", Concentracion = "400mg", Presentacion = "Tabletas" }
            );
        }
    }
}
