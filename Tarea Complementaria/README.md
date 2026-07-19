# Sistema de Gestión Clínica - Tarea Complementaria

Este proyecto consiste en un sistema de gestión clínica desarrollado bajo una arquitectura estructurada en N-Capas utilizando un backend REST en .NET 9.0 y un cliente frontend interactivo en Blazor WebAssembly.

---

## Estructura de la Solución (N-Capas)

La solución se divide en las siguientes capas para garantizar el desacoplamiento de responsabilidades:

1. **ClinicSystem.Domain (Capa de Dominio)**
   * Contiene el núcleo y reglas principales del negocio.
   * Define las 10 entidades clínicas fundamentales relacionales: `Paciente`, `Medico`, `Especialidad`, `Cita`, `HistorialMedico`, `Tratamiento`, `Medicamento`, `Receta`, `Factura`, y `Pago`.
   * Contiene las abstracciones/interfaces para el patrón de persistencia (ej. `IPacienteRepository`, `ICitaRepository`, etc.).

2. **ClinicSystem.Infrastructure (Capa de Infraestructura)**
   * Implementa la persistencia de datos y comunicación con la base de datos a través de Entity Framework Core.
   * Contiene `ClinicDbContext` con relaciones configuradas y datos de prueba precargados (seeds).
   * Implementa el patrón repositorio genérico y específico (`BaseRepository`, `PacienteRepository`, `CitaRepository`, etc.).

3. **ClinicSystem.Application (Capa de Aplicación)**
   * Contiene la lógica y casos de uso del negocio.
   * Define los contratos de servicios (`IPacienteService`, `ICitaService`).
   * Implementa la lógica de servicios y validaciones de negocio adicionales (ej. control de colisiones de horarios para médicos y unicidad de documentos de identidad).
   * Contiene los Objetos de Transferencia de Datos (DTOs) con anotaciones de datos (Data Annotations) para la validación automática en formularios.

4. **ClinicSystem.Api (Capa de Presentación - Backend API)**
   * Expone los endpoints REST para interactuar con los servicios del sistema.
   * Contiene los controladores (`PacientesController`, `CitasController`, `MedicosController`, `EspecialidadesController`).
   * Configura CORS para permitir el consumo por parte de clientes externos como Blazor WebAssembly.

5. **ClinicSystem.Client (Capa de Presentación - Frontend Blazor)**
   * Proyecto en Blazor WebAssembly que actúa como interfaz de usuario cliente de la API.
   * Implementa pantallas completas de visualización y operaciones CRUD relacionales para Pacientes y Citas Médicas.
   * Consume la API asíncronamente mediante HttpClient.

---

## Entidades y Relaciones (10 Tablas)

El sistema modela una clínica completa con las siguientes relaciones:
1. **Especialidad**: Clasifica a los médicos (ej. Cardiología, Pediatría).
2. **Medico**: Asociado a una especialidad. Tiene citas asignadas e historiales registrados.
3. **Paciente**: Registra datos generales, citas, historiales médicos y facturas.
4. **Cita**: Relaciona a un paciente y a un médico con fecha, motivo y estado de la cita.
5. **HistorialMedico**: Registro histórico clínico de un paciente atendido por un médico.
6. **Tratamiento**: Indicaciones clínicas vinculadas a un historial médico.
7. **Medicamento**: Catálogo general de fármacos de la clínica.
8. **Receta**: Prescripción emitida para un paciente que incluye medicamentos específicos, dosis y duración.
9. **Factura**: Comprobante de cobro asociado a un paciente.
10. **Pago**: Transacción de pago (efectivo, tarjeta, transferencia) asociada a una factura.

---

## Cómo Ejecutar y Probar el Sistema (Desde el IDE)

Para ejecutar el sistema completo utilizando la interfaz gráfica de Visual Studio, siga las instrucciones a continuación:

### 1. Aplicación de Migraciones
1. En la parte superior de Visual Studio, vaya al menú **Herramientas** -> **Administrador de paquetes NuGet** -> **Consola del Administrador de paquetes**.
2. En la barra superior de la consola, seleccione el proyecto predeterminado `ClinicSystem.Api`.
3. Para generar las tablas iniciales en la base de datos de SQL Server (LocalDB), ejecute la instrucción de migración inicial (generada automáticamente por Entity Framework al inicializar el modelo). O simplemente inicie el proyecto con depuración y este configurará la base de datos local predefinida en `appsettings.json`.

### 2. Ejecutar la API y el Cliente de forma Simultánea
1. Haga clic derecho sobre la solución `ClinicSystem` en el Explorador de soluciones y seleccione **Propiedades**.
2. Vaya a **Propiedades comunes** -> **Proyecto de inicio**.
3. Seleccione **Proyectos de inicio múltiples**.
4. Establezca la acción de `ClinicSystem.Api` y `ClinicSystem.Client` en **Iniciar**.
5. Presione **F5** o haga clic en **Iniciar** en la barra de herramientas para depurar ambos proyectos. 

La API se abrirá en `https://localhost:7000` con Swagger, y el cliente Blazor WebAssembly iniciará y consumirá los endpoints correspondientes de manera interactiva.
