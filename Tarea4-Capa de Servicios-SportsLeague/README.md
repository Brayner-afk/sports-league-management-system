# SportsLeagueApi - Tarea 4: Capa de Servicios y Validaciones de Negocio

Este proyecto representa la evolucion de la API de gestion de ligas deportivas (SportsLeagueApi), incorporando la Capa de Servicios (Application Layer) dentro de la arquitectura limpia de la solucion. En esta fase, se ha extraido la logica de negocio y las validaciones fuera del controlador de la API Web, garantizando que el flujo de presentacion solo consuma contratos de servicio y DTOs validados.

---

## Estructura del Proyecto de la Tarea 4

La solucion se ha expandido para incluir el nuevo proyecto de aplicacion:

*   SportsLeague.sln: Solucion global.
*   SportsLeague.Domain: Capa de Dominio (Entidades de negocio y contratos de repositorios).
*   SportsLeague.Infrastructure: Capa de Infraestructura (Implementaciones de persistencia y base de datos).
*   SportsLeague.Application: Capa de Servicios (Reglas de negocio, validaciones y DTOs).
    *   Contract: Interfaces de cada servicio de negocio.
        *   ITeamService.cs
        *   IPlayerService.cs
    *   Dtos: Clases para la transferencia y validacion de datos.
        *   TeamDto.cs
        *   PlayerDto.cs
    *   Services: Implementaciones concretas de la logica de negocio.
        *   TeamService.cs
        *   PlayerService.cs
*   SportsLeagueApi: Capa de Presentacion (API Web refactorizada para inyectar servicios en lugar de repositorios).

---

## Objetivos de la Tarea 4

1. Introducir la Capa de Servicios: Desacoplar los controladores de la API Web de la persistencia de datos (repositorios), mediando todas las operaciones a traves de interfaces de servicio.
2. Implementar Validaciones de Datos: Asegurar la integridad de la entrada a traves de anotaciones de datos (Data Annotations) en los DTOs y validaciones manuales adicionales para las propiedades de las entidades.
3. Centralizar Reglas de Negocio: Aplicar restricciones de consistencia logica (por ejemplo, evitar duplicidad de nombres de equipos en la misma ciudad, o validar la existencia de un equipo al registrar un jugador) dentro de la capa de aplicacion.
4. Seguir Principios de Clean Architecture: Mantener la API como un simple despachador de solicitudes y respuestas HTTP, delegando todo el procesamiento a la capa de servicios inyectada.

---

## Validaciones Implementadas

Se han configurado validaciones estrictas tanto a nivel sintactico (DataAnnotations) como a nivel semantico (logica de negocio en servicios):

### 1. Validaciones en Equipos (TeamDto y TeamService)
*   Nombre del Equipo: Campo obligatorio, con una longitud requerida de entre 3 y 100 caracteres.
*   Ciudad del Equipo: Campo obligatorio, con una longitud requerida de entre 3 y 100 caracteres.
*   Regla de Negocio: No se permite la creacion o actualizacion de un equipo con un nombre identico al de otro equipo registrado en la misma ciudad (insensible a mayusculas/minusculas).

### 2. Validaciones en Jugadores (PlayerDto y PlayerService)
*   Nombre Completo: Campo obligatorio, con una longitud de entre 3 y 150 caracteres.
*   Posicion: Campo obligatorio, con una longitud de entre 2 y 50 caracteres.
*   Identificador de Equipo (TeamId): Campo obligatorio, que debe ser un entero estrictamente mayor a cero.
*   Regla de Negocio (Existencia): Se verifica la existencia del equipo (TeamId) asociado en la base de datos antes de registrar o actualizar al jugador.
*   Regla de Negocio (Duplicidad): No se permite la inscripcion de dos jugadores con el mismo nombre en el mismo equipo (insensible a mayusculas/minusculas).

---

## Inyeccion de Dependencias y Configuracion

Las dependencias se registran en el archivo Program.cs de la API Web mediante inyeccion de dependencias Scoped:

*   ITeamRepository se mapea a TeamRepository.
*   IPlayerRepository se mapea a PlayerRepository.
*   ITeamService se mapea a TeamService.
*   IPlayerService se mapea a PlayerService.

Esto garantiza el desacoplamiento de las implementaciones y facilita las pruebas unitarias de cada modulo de manera independiente.
