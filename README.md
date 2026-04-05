📘 Project Overview
This repository documents the transformation of a public Udemy course project into a fully modernized, enterprise‑ready architecture.
The goal is to evolve a simple educational e‑commerce demo into a scalable, maintainable, production‑grade SaaS foundation.

🚀 1. From Udemy Course Project → Enterprise‑Ready Architecture
This project originally started from the public Udemy course:

“Build e-commerce web application using Blazor Web App and .NET 9 (EF Core)”

The course provides a solid introduction to Blazor and e‑commerce concepts, but the architecture is intentionally simplified for learning purposes.

This repository takes that foundation and upgrades it to enterprise standards, including:

✔ Backend isolation
A fully separated backend project following Clean Architecture and Domain‑Driven Design (DDD) principles.

✔ EF Core replaced with Dapper
Instead of relying on EF Core, the backend now uses Dapper with inline SQL, giving:

full control over SQL queries

predictable performance

lightweight data access

easier debugging and profiling

✔ Real-world layering
The backend is structured into:

Domain

Application

Infrastructure

API

This ensures long‑term maintainability and aligns with modern SaaS backend practices.

⚠️ 2. Lessons Learned: Blazor Web App Hybrid Was Not the Right Fit
The project initially attempted to rebuild the frontend using the new Blazor Web App template (the hybrid SSR + WASM model introduced in .NET 8/9).

While powerful, this architecture introduces significant complexity:

mixed rendering modes (SSR, WASM, Auto)

DI separation between server and client

layouts rendered in the wrong pipeline

components accidentally rendered server-side

RenderMode conflicts

hybrid lifecycle differences

During development, it became clear that this model did not serve the needs of the project and created unnecessary friction.

The conclusion:

Fighting the hybrid Blazor Web App pipeline was counterproductive for a SaaS architecture that needs clarity, separation, and predictable behavior.

🏗️ 3. Restarting the Frontend With the Correct Architecture
To align with real SaaS B2B patterns, the frontend is being rebuilt using a two‑portal architecture, each with the right Blazor model for its purpose:

✔ Blazor Server — Admin Portal
Used for internal administration, dashboards, and management tools.

Benefits:

secure by default

server‑side rendering

instant data access

ideal for admin back‑office scenarios

✔ Blazor WebAssembly — Client Portal
Used for the public‑facing e‑commerce experience.

Benefits:

fully client-side

responsive and interactive

scalable via CDN

decoupled from server rendering

✔ Backend API — Shared Between Both Portals
The isolated backend (Dapper + Clean Architecture) serves both portals through a clean API surface.

🎯 Final Result
This repository now represents a professional SaaS architecture:

Code
/Backend (API, Dapper, Clean Architecture)
/AdminPortal (Blazor Server)
/ClientPortal (Blazor WebAssembly)
This structure is:

scalable

maintainable

enterprise‑ready

aligned with real-world SaaS B2B systems
