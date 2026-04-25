# TaskTracker

A full-stack task management application built with .NET 10 (C#) and React.

## What it does
Create, update, and manage tasks through a clean web interface. 
Tasks are stored in a PostgreSQL database and served through a REST API.

## Tech stack
- **Backend:** .NET 10 Minimal API, Entity Framework Core, PostgreSQL
- **Frontend:** React (JavaScript)
- **Infrastructure:** Docker, Azure Container Apps

## How to run locally
1. Clone both repos: `TaskTracker` (backend) and `TaskTracker-Frontend` (frontend)
2. Copy `appsettings.example.json` to `appsettings.json` and fill in your database details
3. Run `docker-compose up` to start the database
4. Run `dotnet run` to start the API
5. In the frontend folder, run `npm install` then `npm start`

## Project status
Actively being developed. Currently adding authentication, 
categories, and AI-powered features.
