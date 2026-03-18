using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using TutorMatch.DAO;

var builder = WebApplication.CreateBuilder(args);

//Extraemos del archivo appsettings.json la cadena de conexión a la base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Le pasas las opciones de configuración al constructor de ApplicationDbContext que hereda las propiedades de DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

// Añadir los servicios aquí

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // /scalar/v1 para acceder a la documentación de la API
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/db-check", async (ApplicationDbContext context) =>
{
    try
    {
        
        await context.Database.OpenConnectionAsync();
        await context.Database.CloseConnectionAsync();

        return Results.Ok(new { canConnect = true });
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.ToString());
    }
});




app.Run();
