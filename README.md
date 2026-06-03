# TaskFlow 

 projet **TaskFlow**, une application SaaS de gestion de tâches développée avec :

- ASP.NET Core Web API (.NET 8/9)
- Angular
- JWT Authentication
- ASP.NET Identity
- Role-based Authorization

---

# 🚀 Objectif du projet

Construire une application moderne de gestion de tâches permettant :

- authentification des utilisateurs
- sécurisation avec JWT
- gestion des rôles (Admin / User)
- communication Angular ↔ ASP.NET Core Web API
- architecture propre et scalable

Projet réalisé dans un objectif :

- d’apprentissage avancé .NET + Angular
- préparation aux entretiens techniques
- construction d’un projet fullstack crédible

---

# 🧠 Stack Technique

## Frontend

- Angular
- TypeScript
- HTML / CSS
- Angular Routing
- HttpClient

## Backend (API)

- ASP.NET Core Web API
- Entity Framework Core
- ASP.NET Identity
- JWT Authentication
- SQL Server

---

# 🔐 Fonctionnalités implémentées

## Backend

✅ Register utilisateur  
✅ Login utilisateur  
✅ JWT Authentication  
✅ Role-based Authorization  
✅ Swagger avec Bearer Token  
✅ Seeding automatique des rôles et admin  
✅ Routes protégées avec `[Authorize]`

---

## Frontend Angular

✅ Création du projet Angular  
✅ Connexion Angular ↔ API .NET  
✅ Service Auth Angular  
✅ Formulaire Login  
✅ Appel API Login  
✅ Récupération du JWT  
✅ Stockage du token dans `localStorage`  
✅ Routing Angular  
✅ Dashboard standalone component

---

# 📂 Structure du projet

```bash
src/app
│
├── pages
│   ├── login
│   └── dashboard
│
├── services
│   └── auth.service.ts
│
├── app.routes.ts
├── app.config.ts
└── app.ts
