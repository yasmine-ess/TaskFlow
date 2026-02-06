using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Models;

var builder = WebApplication.CreateBuilder(args);
//Lier DbContext dans Program.cs
//On utilise la méthode d'extension UseSqlServer pour configurer le contexte de données pour utiliser SQL Server. La chaîne de connexion est récupérée à partir du fichier de configuration (appsettings.json) en utilisant la clé "DefaultConnection". Assurez-vous que cette clé est correctement définie dans votre fichier de configuration avec les informations de connexion appropriées pour votre base de données SQL Server.   
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
////////////////////
///

//Configurer l'authentification et l'autorisation avec Identity. On utilise la méthode AddIdentity pour configurer les services d'identité, en spécifiant les types d'utilisateur et de rôle (ApplicationUser et IdentityRole). Ensuite, on utilise AddEntityFrameworkStores pour indiquer que les données d'identité seront stockées dans la base de données à l'aide d'Entity Framework Core, en spécifiant le contexte de données (ApplicationDbContext). Enfin, on ajoute les fournisseurs de jetons par défaut avec AddDefaultTokenProviders, ce qui permet de gérer les fonctionnalités telles que la réinitialisation de mot de passe et la confirmation d'e-mail.   
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
