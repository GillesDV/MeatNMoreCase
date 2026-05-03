# Meat & More Case

## TLDR 

## What to fill in before running
Change the following files from dummy values to super secret values.
- change `.env.example` to be just `.env` and fill in the values
- configure `Firebase:ProjectId` for `ArticleService.Api` with your Firebase project id
- optionally configure `Firebase:ServiceAccountKeyPath` with the path to a Firebase Admin SDK service account key file
- call `/articles` endpoints with a Firebase ID token in the `Authorization: Bearer <token>` header

## technologies used
- ASP.NET Core minimal API
- Firebase Authentication ID tokens validated with ASP.NET Core JWT bearer authentication


## What to improve, if given more time or if it became prod code


## Task

