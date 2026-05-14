# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Purpose

Alderam.Stocks is a personal Brazilian stock market portfolio tracker. It lets an investor record buy/sell/bonus transactions with full fee breakdowns, monitor real-time portfolio value and P&L, watch individual stocks for buy signals, and analyze sector allocation via pie charts. All prices are sourced from Brazilian exchange (B3) tickers.

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 10 Web API (C#) |
| ORM | Entity Framework Core 10 |
| Database | SQL Server (Azure SQL) |
| Auth | JWT Bearer (symmetric key, 2-hour expiry) |
| Mapping | AutoMapper 13 |
| Frontend | React 16.13, React Router v5 |
| HTTP client | Axios 0.19 |
| Forms | Formik 2 + Yup validation |
| Charts | Highcharts 8 (sector pie), Chart.js 2.9, React Google Charts 3 |
| Hosting | Azure App Service + Azure SQL |

## Commands

### Backend (from `alderam.stocks.api/`)
```sh
dotnet build
dotnet run
```
API runs on `https://localhost:44330` (IIS Express) or `https://localhost:5001`. Swagger UI at `/swagger`.

### Frontend (from `alderam.stocks.client/`)
```sh
npm install
npm start       # proxies API calls to https://localhost:44330/api/
npm run build
npm test
```
`BASE_URL` in `services/Api.js` switches automatically between localhost and the Azure URL based on `NODE_ENV`.

## Domain Vocabulary

All core names are in Portuguese — critical for reading the code:

| Term | Meaning |
|---|---|
| **Ativo** | A stock/security asset (ticker + name + sector + price) |
| **Operacao** | A single transaction: buy (`C`), sell (`V`), or bonus (`B`) |
| **Boleta** | A brokerage note / trade receipt grouping one or more Operacoes with all fees |
| **Carteira** | Portfolio view — holdings with current value and P&L |
| **Resumo** | Portfolio summary — 4 aggregate metrics shown on the dashboard |
| **Acompanhamento** | Watchlist entry with a target buy-price that triggers a bell alert |
| **Setor / Subsetor** | Two-level sector classification applied to each Ativo |
| **TipoDeInvestimento** | Asset class: `1` = Stock (Ação), `2` = Real Estate Fund (FII), `3` = Index |

## Architecture

### Backend (`alderam.stocks.api/`)

**`Services/StockService.cs`** is the core of the application. It:
- Fetches live prices from **HG Brasil API** (primary, 2-min cache) falling back to **Alpha Vantage** (5-min cache, rate-limited to 1 req/12 s). Both API keys are hard-coded in the file.
- Computes all fee components for a Boleta: settlement fee (`0.0275%`), emoluments (`0.003248%` normal / `0.007%` auction), ISS (`5.26%` of brokerage), and brokerage (user-defined rate).
- Calculates portfolio metrics: total invested, current value, unrealized P&L, and liquid profit (sum of only profitable positions).
- Builds the Carteira (portfolio snapshot) and sector chart data.

**Controllers/** — one per resource, all `[Authorize]`:
- `Authentication` — `POST /api/authentication` (login, returns JWT)
- `AtivosController` — asset CRUD; `POST /api/ativos` triggers a price refresh
- `BoletasController` — trade receipt CRUD
- `OperacoesController` — individual operation records
- `ResumosController` — `GET /api/resumos` returns the 4 dashboard KPIs
- `CarteiraController` — `GET /api/carteira?dataLimite={date}` returns holdings snapshot
- `GraficoDeSetoresController` — `GET /api/graficodesetores?tipoDeInvestimento={1|2|3}` returns chart data
- `AcompanhamentosController` — watchlist CRUD
- `SetoresController` — sector/subsector CRUD

**`Database/DatabaseContext.cs`** — EF Core DbContext with 6 DbSets. Unique index on `Ativo.Codigo`. `TipoDeOperacao` stored as a single char with enum conversion.

**`Services/TokenService.cs`** — generates JWT with symmetric key from `appsettings.json`. No issuer/audience validation.

**Key config**: `appsettings.json` holds the Azure SQL connection string and JWT secret. `appsettings.Development.json` overrides for local dev. CORS is fully open.

### Frontend (`alderam.stocks.client/src/`)

**Pages:**
- `pages/main/` — Login form; on success stores JWT in `sessionStorage` and navigates to `/dashboard`
- `pages/dashboard/` — Hosts Resumo, Carteira, Acompanhamentos, and sector charts; has a "Refresh" button that calls `POST /api/ativos`
- `pages/operacoes/` — Lists all Boletas with full fee breakdown; supports creating/editing/deleting Boletas and their Operacoes

**Components:**
- `resumo/` — 4 KPI cards: Total Investido, Valor Atual, Saldo Atual (%), Saldo Líquido (%)
- `carteira/` — Holdings table grouped by TipoDeInvestimento; each ticker links to TradingView; supports date filtering
- `graficos/GraficoDeSubsetoresHighcharts` — Dual pie charts (Ações + FIIs) using Highcharts
- `acompanhamentos/` — Watchlist with bell icon when current price ≤ target buy price
- `boletas/` — Trade receipt modal form with Formik

**Services:**
- `services/Api.js` — Axios instance; reads JWT from `sessionStorage` and injects `Authorization: Bearer` header on every request
- `services/Auth.js` — `isAuthenticated()` check used by private route wrapper in `routes.js`
