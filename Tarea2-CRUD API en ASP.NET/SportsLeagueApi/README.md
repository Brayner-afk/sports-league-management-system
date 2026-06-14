# Sistema de Gestión de Liga Deportiva

Hey, Soy Brayner Melo; y este es mi proyecto para la Tarea 2 de Desarrollo de Software. He construido un Web API robusto utilizando ASP.NET Core y Entity Framework Core para gestionar equipos y jugadores de una liga deportiva.

## Objetivo del Sistema
El objetivo principal de esta aplicación es servir como el "Backend" o cerebro de una liga. Permite administrar de forma persistente (en una base de datos SQL Server) toda la información relevante de los equipos y sus respectivos integrantes, manteniendo una relación lógica donde cada jugador pertenece obligatoriamente a un equipo.

## Guía de Uso (Interfaz Swagger)
Al ejecutar el proyecto, verás una interfaz llamada Swagger que nos permite probar el sistema. Aquí te explico qué hace cada botón según su color:

### Azul (GET - Buscar/Ver)
**Objetivo:** Consultar información.
**Uso:** Úsalo para ver la lista completa de equipos o jugadores, o para buscar uno solo usando su número de ID.

### Verde (POST - Agregar/Crear)
**Objetivo:** Registrar nuevos datos.
**Uso:** Úsalo para inscribir un nuevo equipo o un nuevo jugador. Recuerda que al crear un jugador, debes poner el ID de un equipo que ya exista. El ID del nuevo registro se genera automáticamente (puedes dejarlo en 0).

### Naranja (PUT - Actualizar/Editar)
**Objetivo:** Modificar información existente.
**Uso:** Si te equivocaste en un nombre o una posición, usa este botón. Debes indicar el ID de lo que quieres cambiar y enviar los datos corregidos.

### Rojo (DELETE - Eliminar/Borrar)
**Objetivo:** Quitar registros del sistema.
**Uso:** Úsalo con precaución para borrar un equipo o jugador permanentemente de la base de datos usando su ID.

## Detalles Técnicos
*   **Framework:** .NET 9.0
*   **Base de Datos:** SQL Server (LocalDB)
*   **Patrón:** DTO (Data Transfer Objects) para limpieza de datos.
*   **Documentación:** Swagger UI personalizada en español.

Eso sería todo, gracias por leer esto.
