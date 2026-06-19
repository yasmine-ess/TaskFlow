# TaskFlow API 🔐

 Backend ASP.NET Core Web API du projet **TaskFlow** — application SaaS de gestion de tâches avec authentification JWT et gestion des rôles.

## 🛠️ Stack technique


| ASP.NET Core Web API (.NET 8) : Framework backend           |
| Entity Framework Core         : ORM / accès base de données |
| ASP.NET Identity              :  Gestion des utilisateurs   |
| SQL Server                    : Base de données             |
| JWT Authentication :          : Sécurisation des routes     |
| Swagger                       : Documentation & tests API   |



## ✅ Fonctionnalités implémentées

-Inscription utilisateur
-Connexion utilisateur avec génération de JWT
-Validation des tokens
-Gestion des rôles (Admin / User)
-Routes protégées avec `[Authorize]`
-Contrôle d'accès par rôle (`[Authorize(Roles = "Admin")]`)
-Seed automatique des rôles et d'un administrateur par défaut
-Documentation Swagger avec support Bearer Token

## 🔜 À venir

-  CRUD complet des tâches
-  Gestion des catégories et priorités
-  Refresh Token
-  intercepteur d'erreurs global
-  Pagination des résultats
-  Validation avancée des données
-  Architecture Services + Interfaces complète
-  Déploiement cloud (Azure)

---

## 📡 Endpoints

### Auth — `/api/Auth`

| Méthode | Route        | Description                               | Auth requise |

| `POST`  | `/register`  | Création d'un utilisateur                 |      ❌      |
| `POST`  | `/login`     | Connexion — retourne un JWT               |      ❌      |
| `GET`   | `/test`      | Route protégée (test)                     |      ✅      |
| `GET`   | `/admin-only`| Accessible aux administrateurs uniquement |    ✅ Admin  |

---

## 🏗️ Architecture

```
TaskFlow-API/
├── Controllers/        # Endpoints HTTP
├── Models/             # Entités base de données
├── DTOs/               # Objets de transfert de données
├── Data/               # DbContext et configuration EF Core
└── Services/           # Logique métier
```

### Contenu du JWT

Le token contient les claims suivants :
- `UserId`
- `Email`
- `Roles`

Les rôles sont intégrés directement dans le token pour permettre l'autorisation basée sur les rôles sans appel supplémentaire à la base de données.

---

## 🚀 Lancement

**Prérequis :** .NET 8 SDK, SQL Server

```bash
# Restaurer les dépendances
dotnet restore

# Appliquer les migrations
dotnet ef database update

# Lancer le projet
dotnet run
```

**Swagger disponible sur :** `https://localhost:7243/swagger`

---

## 🎯 Objectif du projet

Projet réalisé pour approfondir et démontrer mes compétences en :
- Architecture API REST avec ASP.NET Core
- Sécurité applicative (JWT, RBAC, ASP.NET Identity)
- Entity Framework Core et SQL Server
- Développement Full Stack .NET + Angular

**Frontend associé :** [TaskFlow Frontend](https://github.com/yasmine-ess/TaskFlow-Frontend)
