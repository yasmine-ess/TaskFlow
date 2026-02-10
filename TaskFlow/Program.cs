using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskFlow.Models;
using TaskFlow.Services;

var builder = WebApplication.CreateBuilder(args);

////////////////////////////////////////////////////////////////
/// 🔵 1. CONFIGURATION DE LA BASE DE DONNÉES (Entity Framework)
////////////////////////////////////////////////////////////////
/// Permet à ton API de parler avec SQL Server.
/// La chaîne de connexion est dans appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

////////////////////////////////////////////////////////////////
/// 🔵 2. CONFIGURATION D'IDENTITY (gestion des utilisateurs)
////////////////////////////////////////////////////////////////
/// Identity gère :
/// - création de compte
/// - login
/// - hash des mots de passe
/// - rôles

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = 403;
        return Task.CompletedTask;
    };
});
////////////////////////////////////////////////////////////////
/// 🔵 3. AUTHENTIFICATION JWT (sécuriser l'API)
////////////////////////////////////////////////////////////////
/// Ici on dit :
/// 👉 "Toutes les routes avec [Authorize] nécessitent un token"
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,        // Vérifie qui a créé le token
            ValidateAudience = true,      // Vérifie à qui il est destiné
            ValidateLifetime = true,      // Vérifie qu'il n'est pas expiré
            ValidateIssuerSigningKey = true, // Vérifie la signature

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

////////////////////////////////////////////////////////////////
/// 🔵 4. AUTORISATION
////////////////////////////////////////////////////////////////
/// Permet d'utiliser [Authorize]
builder.Services.AddAuthorization();

////////////////////////////////////////////////////////////////
/// 🔵 5. CONTROLLERS
////////////////////////////////////////////////////////////////
builder.Services.AddControllers();

////////////////////////////////////////////////////////////////
/// 🔵 6. SWAGGER + JWT (IMPORTANT 🔥)
////////////////////////////////////////////////////////////////
/// Ajoute le bouton 🔐 Authorize dans Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Tape : Bearer {ton token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddScoped<ITokenService, TokenService>();
///////////////////////////////////////////////////////////////
/// 🔵 BUILD DE L'APPLICATION
///////////////////////////////////////////////////////////////
var app = builder.Build();

////////////////////////////////////////////////////////////////
/// 🔵 MIDDLEWARE (ordre ULTRA IMPORTANT)
////////////////////////////////////////////////////////////////

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/// ⚠️ Toujours dans cet ordre :
app.UseAuthentication(); // vérifie le token
app.UseAuthorization();  // vérifie les droits

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "User" };

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
    await DbSeeder.SeedRolesAndAdminAsync(services);
}
app.Run();