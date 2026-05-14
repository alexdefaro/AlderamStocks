# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Alderam.Stocks is a stock portfolio management web application. It is a full-stack project with an ASP.NET Core 3.1 Web API backend and a React 16 frontend.

- **Backend API** hosted on Azure App Service: `alderamstocksapi.azurewebsites.net`
- **Database**: Azure SQL Server (`alderamdatabaseserver.database.windows.net`)
- **Frontend** uses session-storage JWT tokens for auth

## Commands

### Backend (run from `alderam.stocks.api/`)
```sh
dotnet build
dotnet run
```
API runs on `https://localhost:44330` (IIS Express) or `https://localhost:5001`. Swagger UI is at `/swagger`.

### Frontend (run from `alderam.stocks.client/`)
```sh
npm install
npm start      # dev server, proxies API to https://localhost:44330/api/
npm run build
npm test
```

## Architecture

### Domain model
Core Brazilian investment terms used throughout:
- **Ativo** — a stock/security asset
- **Operacao** — a buy/sell/bonus transaction on an asset
- **Boleta** — a trading receipt/document
- **Carteira** — portfolio/wallet view
- **Setor/Subsetor** — sector/subsector hierarchy for asset categorization
- **Acompanhamento** — watchlist tracking entry

### Backend (`alderam.stocks.api/`)
- **Controllers/**: One controller per resource — `AtivosController`, `OperacoesController`, `BoletasController`, `BoletasController`, `SetoresController`, `GraficoDeSetoresController`, `ResumosController`, `AcompanhamentosController`, `CarteiraController`, plus `Authentication`
- **Services/StockService.cs**: Core business logic — loads assets, computes portfolio positions, and refreshes prices
- **Services/TokenService.cs**: Generates JWT tokens (2-hour expiry, no issuer/audience validation)
- **Database/DatabaseContext.cs**: EF Core `DbContext` for the six entity sets; SQL Server migrations in `Migrations/`
- **Models/**: Plain domain model classes; AutoMapper profiles map between models and DTOs

**Key config**: `appsettings.json` holds the Azure SQL connection string and JWT secret key. `appsettings.Development.json` overrides for local dev.

CORS is fully open (all origins, headers, methods). Auth uses JWT Bearer with no issuer/audience validation.

### Frontend (`alderam.stocks.client/src/`)
- **routes.js**: React Router v5 — `/` (login), `/dashboard`, `/operacoes`. Private routes check `authenticationService.isAuthenticated()` (reads session-storage token).
- **services/Api.js**: Axios wrapper that injects the Bearer token. `BASE_URL` switches between `localhost:44330/api/` and the Azure URL based on `NODE_ENV`.
- **services/Auth.js**: Checks session-storage for a valid token.
- **pages/**: `main/` (login), `dashboard/` (portfolio overview), `operacoes/` (trading operations)
- **components/**: `carteira/`, `resumo/`, `graficos/`, `boletas/`, `acompanhamentos/`, `header/`, `spinner/`

Charts use Chart.js, Highcharts, and Google Charts depending on the component.
