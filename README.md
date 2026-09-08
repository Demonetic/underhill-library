# Underhill Library

Underhill Library är en responsiv CRUD applikation där användare kan hantera sina böcker och favoritcitat. Varje användare har ett eget bibliotek som skyddas med JWT-autentisering.

## Live-demo

[Öppna Underhill Library](https://dreamy-treacle-019eb2.netlify.app)

Du kan registrera en egen användare direkt i applikationen.

## Funktioner

- Registrering och inloggning med JWT
- Personligt bibliotek för varje användare
- Lägga till, visa, redigera och ta bort böcker
- Lägga till, visa, redigera och ta bort citat
- Fem standardcitat för nya användare
- Responsiv navigering och layout
- Ljust och mörkt tema
- Automatisk utloggning när token har gått ut

## Tekniker

### Frontend

- Angular 20
- TypeScript
- Bootstrap
- Font Awesome

### Backend

- .NET 9
- C#
- ASP.NET Core REST API
- Entity Framework Core
- JWT-autentisering

### Databas och deployment

- MySQL
- Docker Compose för lokal utveckling
- Railway för API och databas
- Netlify för frontend

## Databasdesign

[Visa ER-diagrammet](docs/er-diagram.png)

## Projektstruktur

```text
underhill-library/
├── backend/
│   └── UnderhillLibrary.Api/
├── frontend/
├── docs/
└── docker-compose.yml
```

## Köra projektet lokalt

### Förutsättningar

- .NET 9 SDK
- Node.js och npm
- Docker Desktop

### 1. Starta databasen

Skapa en lokal `.env`-fil utifrån `.env.example` och ange egna lösenord.

Starta sedan MySQL från projektets rotmapp:


```bash
docker compose up -d
```

### 2. Starta backend

Backend kräver en anslutningsträng och en Base64-kodad JWT-nyckel. Dessa ska anges som miljövariabler och får inte sparas i Git.

Exempel för PowerShell:


```powershell
$env:ConnectionStrings__DefaultConnection="Server=localhost;Port=3306;Database=underhill_library;User ID=underhill_user;Password=replace_with_password;"
$env:Jwt__Key="replace_with_base64_encoded_key"

dotnet run --project backend/UnderhillLibrary.Api
```

Databasmigrationerna körs automatiskt när backend startar.

### 3. Starta frontend

Öppna en ny terminal:


```bash
cd .\frontend\
npm install
npm start
```

Frontend körs därefter på:

```text
http://localhost:4200
```

## Säkerhet

- Lösenord lagras hashade
- CRUD operationerna skyddas med JWT
- Användare kan endast komma åt sina egna böcker och citat
- Hemliga nycklar och produktionsinställningar hanters som miljövariabler