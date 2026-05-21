using TutorMatch.DAO;
using TutorMatch.Interfaces;
using TutorMatch.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using TutorMatch.DAO;

var builder = WebApplication.CreateBuilder(args);

//Extraemos del archivo appsettings.json la cadena de conexión a la base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Le pasas las opciones de configuración al constructor de ApplicationDbContext que hereda las propiedades de DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});
// Le pasas las opciones de configuración al constructor de ApplicationDbContext que hereda las propiedades de DbContext
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddScoped<ITutorProfileService, TutorProfileService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IMessageService, MessageService>();

// Añadir los servicios aquí

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy.AllowAnyOrigin() // En producción se pone la URL de tu front
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // /scalar/v1 para acceder a la documentación de la API
}

app.UseHttpsRedirection();

app.UseCors("ReactApp");

app.UseAuthentication();

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

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Tutor", "Student", "Admin" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TutorMatch.DAO.ApplicationDbContext>();
        context.Database.Migrate();

        await TutorMatch.Data.DbSeeder.SeedAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ocurrió un error al sembrar la base de datos: {ex.Message}");
    }
}

app.Run();
