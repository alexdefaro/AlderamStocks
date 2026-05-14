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
| Frontend | Next.js 15 (App Router), React 19 |
| HTTP client | Axios 1.7 |
| Forms | Formik 2 + Yup / React Hook Form 7 |
| Charts | Highcharts 12 (sector pie charts) |
| Styles | Tailwind CSS 3 (npm) |
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
npm run dev     # starts on http://localhost:3000
npm run build
npm start       # production server
```
API base URL is set via `NEXT_PUBLIC_API_URL` in `.env.local` (dev) and `.env.production`.

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

### Frontend (`alderam.stocks.client/`)

App Router — all pages and components are `'use client'` (app is fully authenticated, no SSR required).

**`app/` (pages):**
- `page.js` — Login form; stores JWT in `sessionStorage`, navigates to `/dashboard`
- `dashboard/page.js` — Hosts Resumo, Carteira, Acompanhamentos, and sector charts; "Refresh" button calls `POST /api/ativos`
- `operacoes/page.js` — Lists all Boletas with full fee breakdown
- `layout.js` — Root layout: global CSS, Font Awesome CDN, ToastContainer

Auth guard pattern: each private page uses `useEffect` to check `sessionStorage.getItem('AUTH_TOKEN')` and calls `router.replace('/')` if missing.

**`components/`:**
- `resumo/` — 4 KPI cards: Total Investido, Valor Atual, Saldo Atual (%), Saldo Líquido (%)
- `carteira/` — Holdings table grouped by TipoDeInvestimento; each ticker links to TradingView; supports date filtering
- `graficos/subsetores/GraficoDeSubsetoresHighcharts` — Pie charts (Ações + FIIs) using Highcharts 12; loaded via `next/dynamic` with `ssr: false`
- `acompanhamentos/` — Watchlist with bell icon when current price ≤ target buy price; uses react-modal
- `boletas/` — Trade receipt list and detail with fee breakdown
- `header/` — Nav bar with Next.js `<Link>` and logout

**`services/`:**
- `Api.js` — Axios instance using `NEXT_PUBLIC_API_URL`; injects `Authorization: Bearer` from `sessionStorage`
- `Auth.js` — `isAuthenticated()` / `logout()` using `sessionStorage`
- `Toast.js` — Re-exports `toast` from react-toastify
