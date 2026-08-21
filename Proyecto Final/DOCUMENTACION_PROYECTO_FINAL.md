# Documentación del Proyecto Final: Sistema de Gestión de Ligas Deportivas (SportsLeague)

---

## 1. Idea del proyecto
El **Sistema de Gestión de Ligas Deportivas (Sports League Management System)** es una solución de software empresarial distribuida y modular diseñada para la administración integral de organizaciones, franquicias y torneos deportivos. Su propósito principal es centralizar la gestión de equipos (franquicias), plantillas de jugadores y el calendario oficial de partidos (encuentros, sedes y marcadores en tiempo real), permitiendo a directores técnicos, coordinadores y fanáticos consultar estadísticas y resultados bajo una experiencia visual moderna, fluida y de alto rendimiento.

---

## 2. Objetivo general
Desarrollar una solución tecnológica distribuida bajo la plataforma **.NET 9**, compuesta por un backend con arquitectura limpia (**Clean Architecture**), una API RESTful, persistencia en **SQL Server** mediante **Entity Framework Core**, y una aplicación cliente interactiva en **Blazor WebAssembly** de una sola página (SPA). El proyecto implementa de forma estricta los principios de la **Programación Orientada a Objetos (POO)** —con énfasis en clases base abstractas, polimorfismo y sobrecarga de constructores y métodos— combinados con una interfaz de usuario moderna (UI/UX deportiva con paleta en Amarillo, Negro, Blanco y Gris, animaciones fluidas e integración de logotipo corporativo).

---

## 3. Alcance
El sistema cubre la administración completa del ciclo de vida de una liga deportiva:
- **Gestión de Equipos:** Registro, edición, listado, consulta detallada con conteo de plantilla y eliminación de franquicias deportivas.
- **Gestión de Jugadores:** Registro, edición, transferencia de equipo, asignación de posiciones en el campo y eliminación de atletas.
- **Calendario y Encuentros (Partidos):** Programación de partidos entre equipos locales y visitantes, asignación de fecha/hora y sede/estadio, actualización dinámica de marcadores y seguimiento de estados (*Programado*, *En Curso / En Vivo*, *Finalizado*, *Cancelado*).
- **Ajustes de POO y Arquitectura:** Implementación visible de clases abstractas, métodos polimórficos obligatorios y sobrecargas en dominio, aplicación e infraestructura.
- **Dashboard y Analítica:** Métricas clave en tiempo real (KPIs), próximos encuentros y estado del servicio.

*Límites del alcance:* No incluye módulos de pasarela de pago para boletería, autenticación federada OAuth2/JWT avanzada ni gestión de contratos financieros de jugadores.

---

## 4. Arquitectura propuesta
Para garantizar la separación estricta de responsabilidades, alta cohesión, bajo acoplamiento y facilidad de mantenimiento y pruebas, la solución está estructurada en **cinco capas independientes**, organizadas en proyectos de .NET:

| Capa | Proyecto | Responsabilidad |
| :--- | :--- | :--- |
| **Frontend** | `SportsLeague.WebAssembly` | Cliente Web SPA en **Blazor WebAssembly** (.NET 9). Consume los endpoints HTTP de la API mediante `HttpClient`, implementa diseño UI/UX con paleta temática (#FFB800, #090A0F, #FFFFFF, #262B38), animaciones de hover/transición, modales interactivos y alertas Toast. |
| **API** | `SportsLeagueApi` | Capa de presentación del backend. Expone controladores REST (`TeamsController`, `PlayersController`, `MatchesController`), configura políticas de CORS, documentación interactiva con **Swagger / OpenAPI** y orquesta la inyección de dependencias y el sembrado automático de datos en base de datos. |
| **Application** | `SportsLeague.Application` | Contiene los Objetos de Transferencia de Datos (**DTOs** con validaciones DataAnnotations), contratos de servicios (`ITeamService`, `IPlayerService`, `IMatchService`) y lógica de negocio/orquestación (`TeamService`, `PlayerService`, `MatchService`) con sobrecargas de métodos. |
| **Infrastructure** | `SportsLeague.Infrastructure` | Implementa la persistencia de datos mediante `ApplicationDbContext` de **Entity Framework Core**, repositorios concretos (`TeamRepository`, `PlayerRepository`, `MatchRepository`) y la clase abstracta genérica base `BaseRepository<T>`. |
| **Domain** | `SportsLeague.Domain` | Núcleo del sistema sin dependencias externas. Define la clase abstracta base `BaseEntity` (con métodos abstractos `GetEntitySummary()` y virtuales `GetDisplayInfo()`), las entidades del negocio (`Team`, `Player`, `Match`) con sobrecargas de constructores/métodos y las interfaces de repositorio (`IBaseRepository<T>`, `ITeamRepository`, `IPlayerRepository`, `IMatchRepository`). |

---

## 5. Requerimientos funcionales

| Código | Requerimiento Funcional | Prioridad |
| :--- | :--- | :---: |
| **RF-01** | El sistema debe permitir registrar nuevos equipos deportivos especificando su nombre oficial y la ciudad sede. | **Alta** |
| **RF-02** | El sistema debe permitir listar, consultar en detalle, filtrar por ciudad, editar y eliminar equipos. | **Alta** |
| **RF-03** | El sistema debe permitir registrar jugadores especificando su nombre completo, posición en el terreno y equipo al que pertenece. | **Alta** |
| **RF-04** | El sistema debe permitir listar, consultar, filtrar por equipo asignado, editar datos/transferir y eliminar jugadores. | **Alta** |
| **RF-05** | El sistema debe permitir programar partidos en el calendario oficial asociando un equipo local, un equipo visitante, fecha/hora y sede/estadio. | **Alta** |
| **RF-06** | El sistema debe validar que el equipo local y el equipo visitante sean distintos al momento de programar o editar un partido. | **Alta** |
| **RF-07** | El sistema debe permitir actualizar marcadores (puntuación local y visitante) y cambiar el estado del encuentro (*Programado*, *En Curso*, *Finalizado*, *Cancelado*). | **Alta** |
| **RF-08** | El sistema debe permitir filtrar el calendario de partidos por estado (*Todos*, *Programados*, *En Vivo*, *Finalizados*, *Cancelados*) y por franquicia participante. | **Media** |
| **RF-09** | El backend debe exponer endpoints RESTful documentados con Swagger para todas las operaciones CRUD de Equipos, Jugadores y Partidos. | **Alta** |
| **RF-10** | El sistema debe implementar clases base abstractas obligatorias (`BaseEntity` y `BaseRepository<T>`) y sobrecargas de constructores y métodos en entidades y servicios. | **Alta** |
| **RF-11** | El sistema debe validar campos obligatorios y reglas de negocio (nombres no vacíos, longitud mínima/máxima, IDs existentes) en las capas de DTO y Servicio. | **Media** |
| **RF-12** | La eliminación de un equipo debe manejar la integridad referencial en cascada para su plantilla y restringida para el histórico de partidos. | **Media** |
| **RF-13** | El frontend Blazor WebAssembly debe contar con un Dashboard analítico con tarjetas métricas (KPIs), listas de accesos rápidos y próximos encuentros. | **Alta** |
| **RF-14** | La interfaz de usuario debe integrar el logotipo oficial, paleta de colores corporativa (Amarillo `#FFB800`, Negro `#090A0F`, Blanco `#FFFFFF`, Gris `#262B38`), animaciones y modales de confirmación interactivos. | **Alta** |
| **RF-15** | El sistema debe mostrar notificaciones visuales emergentes (*Toast Alerts*) tanto para operaciones exitosas como para el manejo descriptivo de errores. | **Media** |

---

## 6. Requerimientos no funcionales

1. **Separación Estricta de Capas:** El backend debe respetar el flujo unidireccional de dependencias de *Clean Architecture* (Domain $\leftarrow$ Application $\leftarrow$ Infrastructure $\leftarrow$ API). Los controladores nunca deben interactuar directamente con `DbContext` ni entidades de dominio no mapeadas.
2. **Persistencia y Motor de Base de Datos:** Se utiliza **Microsoft SQL Server** como motor relacional gestionado mediante **Entity Framework Core 9.0**, con relaciones foráneas configuradas, llaves primarias autoincrementales y sembrado inicial automático.
3. **Estándares REST y Formato JSON:** Todos los endpoints de la API deben comunicarse exclusivamente mediante formato JSON estándar y retornar los códigos de estado HTTP correspondientes (`200 OK`, `201 Created`, `204 NoContent`, `400 BadRequest`, `404 NotFound`, `500 InternalServerError`).
4. **Arquitectura Distribuida y Desacoplada:** El frontend ejecuta de forma autónoma en el navegador del cliente mediante **Blazor WebAssembly**, comunicándose de manera asíncrona con el backend a través de `HttpClient` y servicios tipados con mecanismos de tolerancia a fallos.
5. **Configuración de CORS:** La API debe incluir políticas de *Cross-Origin Resource Sharing* (CORS) configuradas al inicio del pipeline para permitir la interoperabilidad transparente con el cliente WebAssembly en entornos HTTP y HTTPS.
6. **Buenas Prácticas de Programación (POO):** El código debe aplicar polimorfismo, encapsulamiento, abstracción, nomenclatura estándar en C#, inyección de dependencias nativa y tipado fuerte sin redundancias.
7. **Diseño Visual y Experiencia de Usuario (UI/UX):** La interfaz web debe ser responsiva, con microinteracciones en botones y tarjetas, contraste de accesibilidad optimizado y componentes reutilizables (*StatCard*, *ConfirmModal*, *ToastAlert*).
8. **Control de Versiones y Repositorio:** El código debe residir en un repositorio de **GitHub** estructurado con ramas principales (`main` y `develop`) y confirmaciones (*commits*) descriptivas por módulo.