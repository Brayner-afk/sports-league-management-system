using Microsoft.EntityFrameworkCore;
using SportsLeague.Domain.Interfaces;
using SportsLeague.Domain.Entities;
using SportsLeague.Infrastructure.Context;
using SportsLeague.Infrastructure.Repositories;
using SportsLeague.Application.Contract;
using SportsLeague.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar base de datos (DbContext en la capa de Infraestructura)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Políticas de CORS para permitir la comunicación con el frontend Blazor WebAssembly
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Inyectar Repositorios (Infraestructura)
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();

// Inyectar Servicios (Aplicación)
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IMatchService, MatchService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Inicialización y sembrado automático de base de datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();

        // Garantizar que la tabla Matches exista en la base de datos
        context.Database.ExecuteSqlRaw(@"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Matches')
            BEGIN
                CREATE TABLE [dbo].[Matches] (
                    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                    [HomeTeamId] INT NOT NULL,
                    [AwayTeamId] INT NOT NULL,
                    [MatchDate] DATETIME2 NOT NULL,
                    [Location] NVARCHAR(150) NOT NULL,
                    [HomeScore] INT NULL,
                    [AwayScore] INT NULL,
                    [Status] NVARCHAR(50) NOT NULL,
                    CONSTRAINT [FK_Matches_Teams_HomeTeamId] FOREIGN KEY ([HomeTeamId]) REFERENCES [dbo].[Teams] ([Id]),
                    CONSTRAINT [FK_Matches_Teams_AwayTeamId] FOREIGN KEY ([AwayTeamId]) REFERENCES [dbo].[Teams] ([Id])
                );
            END
        ");

        // Sembrado inicial de datos de Equipos y Jugadores si la base de datos está vacía
        if (!context.Teams.Any())
        {
            var team1 = new Team("Águilas Cibaeñas", "Santiago");
            var team2 = new Team("Tigres del Licey", "Santo Domingo");
            var team3 = new Team("Leones del Escogido", "Santo Domingo");
            var team4 = new Team("Gigantes del Cibao", "San Francisco de Macorís");

            context.Teams.AddRange(team1, team2, team3, team4);
            context.SaveChanges();

            var player1 = new Player("Juan Soto", "Jardinero Derecho", team1.Id);
            var player2 = new Player("Vladimir Guerrero Jr.", "Primera Base", team2.Id);
            var player3 = new Player("Rafael Devers", "Tercera Base", team3.Id);
            var player4 = new Player("Fernando Tatis Jr.", "Campocorto", team1.Id);
            var player5 = new Player("José Ramírez", "Tercera Base", team4.Id);
            var player6 = new Player("Ketel Marte", "Segunda Base", team2.Id);

            context.Players.AddRange(player1, player2, player3, player4, player5, player6);
            context.SaveChanges();
        }

        // Sembrado inicial de Partidos si la tabla está vacía
        if (!context.Matches.Any())
        {
            var teams = context.Teams.ToList();
            if (teams.Count >= 4)
            {
                var match1 = new Match(teams[0].Id, teams[1].Id, DateTime.Today.AddDays(1).AddHours(19), "Estadio Cibao, Santiago") { Status = "Programado" };
                var match2 = new Match(teams[2].Id, teams[3].Id, DateTime.Today.AddDays(2).AddHours(19).AddMinutes(30), "Estadio Quisqueya, Santo Domingo") { Status = "Programado" };
                var match3 = new Match(teams[1].Id, teams[2].Id, DateTime.Today.AddDays(-1).AddHours(18), "Estadio Quisqueya, Santo Domingo") { HomeScore = 5, AwayScore = 3, Status = "Finalizado" };
                var match4 = new Match(teams[0].Id, teams[3].Id, DateTime.Today.AddHours(20), "Estadio Julián Javier, SFM") { HomeScore = 2, AwayScore = 1, Status = "En Curso" };

                context.Matches.AddRange(match1, match2, match3, match4);
                context.SaveChanges();
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al inicializar y sembrar la base de datos.");
    }
}

// Habilitar CORS al inicio del pipeline antes de redirecciones o autorización
app.UseCors("AllowBlazorClient");

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();



