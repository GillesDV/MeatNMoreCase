# Meat & More Case

## TLDR 

## What to fill in before running
Change the following files from dummy values to super secret values.
- change `.env.example` to be just `.env` and fill in the values
- configure `Firebase:ProjectId` for `ArticleService.Api` with your Firebase project id
- optionally configure `Firebase:ServiceAccountKeyPath` with the path to a Firebase Admin SDK service account key file
- start SQL Server with `docker compose up -d sqlserver`; the API uses `ConnectionStrings:ArticleDb`
- call `/articles` endpoints with a Firebase ID token in the `Authorization: Bearer <token>` header

## technologies used
- ASP.NET Core minimal API
- Firebase Authentication ID tokens validated with ASP.NET Core JWT bearer authentication
- SQL Server in Docker, consumed with EF Core


## What to improve, if given more time or if it became prod code
- Maybe extract the Minimal Api endpoints from Program.cs into another file / structure as well? Especially if more controllers might be added in the future

## Task
