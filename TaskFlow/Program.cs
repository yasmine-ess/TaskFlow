using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Configurer l'authentification JWT.
//On utilise la méthode AddAuthentication pour configurer le schéma d'authentification,
//en spécifiant JwtBearerDefaults.AuthenticationScheme pour indiquer que nous utilisons
//l'authentification par jeton JWT.
//Ensuite, on utilise AddJwtBearer pour configurer les options de validation du jeton.
//On récupère la clé secrète à partir du fichier de configuration (appsettings.json)
//et on configure les paramètres de validation du jeton, tels que la validation de l'émetteur,
//de l'audience, de la durée de vie et de la clé de signature.
//Assurez-vous que les valeurs dans appsettings.json correspondent à celles utilisées lors de
//la génération du jeton dans le contrôleur d'authentification.  

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });



builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///
builder.Services.AddAuthorization();

//Lier DbContext dans Program.cs
//On utilise la méthode d'extension UseSqlServer pour configurer le contexte de données pour utiliser SQL Server. La chaîne de connexion est récupérée à partir du fichier de configuration (appsettings.json) en utilisant la clé "DefaultConnection". Assurez-vous que cette clé est correctement définie dans votre fichier de configuration avec les informations de connexion appropriées pour votre base de données SQL Server.   
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
////////////////////
///


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // ?? AVANT
app.UseAuthorization();  // ?? APRÈS

app.MapControllers();
app.Run();