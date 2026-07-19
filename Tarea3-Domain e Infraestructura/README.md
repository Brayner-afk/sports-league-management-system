# SportsLeagueApi - Tarea 3: Capas de Dominio e Infraestructura

Este proyecto representa la evolución de la API de gestión de ligas deportivas (`SportsLeagueApi`), aplicando los principios de **Clean Architecture** (Arquitectura Limpia). Se han separado las responsabilidades del sistema dividiéndolo en proyectos independientes dentro de la solución, aislando la lógica de negocio central (Dominio) y los detalles tecnológicos de acceso a datos (Infraestructura) de la capa de presentación (Web API).

---

## 📂 Estructura del Proyecto de la Tarea 3

La solución ahora se estructura de la siguiente manera:

```text
Tarea3-Domain e Infraestructura/
│
├── SportsLeague.sln                       # Solución global que agrupa los tres proyectos
│
├── SportsLeague.Domain/                   # Capa de Dominio (Lógica de Negocio Pura)
│   ├── Core/
│   │   └── BaseEntity.cs                  # Entidad base genérica (contiene Id)
│   ├── Entities/
│   │   ├── Team.cs                        # Entidad del Equipo (Hereda de BaseEntity)
│   │   └── Player.cs                      # Entidad del Jugador (Hereda de BaseEntity)
│   └── Interfaces/
│       ├── IBaseRepository.cs             # Interfaz genérica para operaciones CRUD asíncronas
│       ├── ITeamRepository.cs             # Interfaz específica para el repositorio de Equipos
│       └── IPlayerRepository.cs           # Interfaz específica para el repositorio de Jugadores
│
├── SportsLeague.Infrastructure/           # Capa de Infraestructura (Persistencia y Datos)
│   ├── Context/
│   │   └── ApplicationDbContext.cs        # Contexto de Entity Framework Core para SQL Server
│   ├── Core/
│   │   └── BaseRepository.cs              # Implementación genérica del repositorio genérico
│   ├── Models/
│   │   ├── TeamDto.cs                     # DTO para transferencia de datos de Equipo
│   │   └── PlayerDto.cs                   # DTO para transferencia de datos de Jugador
│   └── Repositories/
│       ├── TeamRepository.cs              # Implementación del repositorio de Equipos
│       └── PlayerRepository.cs            # Implementación del repositorio de Jugadores
│
└── SportsLeagueApi/                       # Capa de API Web (Presentación / Entrada)
    ├── Controllers/
    │   ├── TeamsController.cs             # Controlador de API para Equipos (Usa DI de repositorios)
    │   └── PlayersController.cs           # Controlador de API para Jugadores (Usa DI de repositorios)
    ├── Migrations/                        # Migraciones de base de datos actualizadas con los nuevos modelos
    ├── Program.cs                         # Configuración y registro de dependencias (IoC)
    └── appsettings.json                   # Cadena de conexión y configuraciones
```

---

## 🎯 Objetivos de la Tarea 3

1. **Desacoplamiento Estructural**: Mover el acceso directo a la base de datos y la definición de las tablas fuera del proyecto Web API.
2. **Implementación de Patrón Repositorio**: Centralizar la lógica de consultas a base de datos utilizando repositorios genéricos y específicos, evitando que los controladores conozcan los detalles del ORM (Entity Framework Core).
3. **Inyección de Dependencias (DI)**: Registrar los contratos (interfaces) del Dominio y sus implementaciones concretas en el contenedor de dependencias nativo de .NET Core.
4. **Cumplimiento de Clean Architecture**: Garantizar que el Dominio no dependa de ningún framework externo ni de la base de datos, mientras que la Infraestructura se encarga de los detalles tecnológicos e implementa los contratos requeridos por el Dominio.

---

## ⚙️ Funcionamiento de las Nuevas Capas

### 1. Capa de Dominio (`SportsLeague.Domain`)
Es el núcleo de la aplicación. No tiene dependencias de ningún framework de persistencia ni del proyecto de API. Contiene:
* **`Core/BaseEntity.cs`**: Una clase base abstracta de la cual heredan todas las entidades del dominio para unificar la clave primaria (`Id`).
* **`Entities/`**: Las entidades de negocio puro (`Team` y `Player`) que describen las tablas y relaciones lógicas del negocio deportivo.
* **`Interfaces/`**: Los contratos necesarios para interactuar con la persistencia. La Web API y la capa de servicios consumen estas interfaces sin conocer cómo o dónde se guardan los datos.

### 2. Capa de Infraestructura (`SportsLeague.Infrastructure`)
Provee la implementación concreta de los contratos definidos en el Dominio. Depende directamente de la capa de Dominio. Contiene:
* **`Context/ApplicationDbContext.cs`**: Representa la sesión con la base de datos SQL Server y mapea las entidades del dominio a tablas relacionales utilizando Entity Framework Core.
* **`Core/BaseRepository.cs`**: Implementa las operaciones CRUD asíncronas genéricas definidas en la interfaz `IBaseRepository<T>`.
* **`Repositories/`**: Implementaciones específicas (`TeamRepository` y `PlayerRepository`) para resolver cualquier lógica particular de acceso a datos de manera limpia.
* **`Models/`**: Los Data Transfer Objects (DTOs) que aíslan el modelo de base de datos relacional de la información expuesta al exterior.

### 3. Capa de Web API (`SportsLeagueApi`)
Es la capa de presentación que expone los endpoints HTTP.
* Depende de `SportsLeague.Domain` (para conocer las interfaces y entidades) y `SportsLeague.Infrastructure` (para registrar las implementaciones).
* En los controladores (`TeamsController` y `PlayersController`), se inyectan las interfaces (`ITeamRepository`, `IPlayerRepository`) mediante **Inyección de Dependencias constructor**, evitando acoplamiento directo con `DbContext` o Entity Framework.

---

## 🛠️ Cómo Ejecutar el Proyecto

1. Asegúrate de configurar la cadena de conexión de SQL Server en `appsettings.json` dentro del proyecto `SportsLeagueApi`.
2. Para aplicar las migraciones y crear la base de datos, ejecuta el siguiente comando en la consola de administración de paquetes o CLI de .NET:
   ```bash
   dotnet ef database update --project SportsLeagueApi
   ```
3. Ejecuta el servidor de desarrollo:
   ```bash
   dotnet run --project SportsLeagueApi
   ```
4. Accede a Swagger para realizar pruebas en los endpoints:
   `https://localhost:<port>/swagger` (o puerto correspondiente).
