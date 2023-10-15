# Build a Microservices app with .Net and NextJS from scratch

This project aims to see how to build a microservices based app using .Net for the backend services and Next.js for the client app. Here are some of the things that are covered in this project:
- Creating several backend services using .Net that provide functionality for the app
- Service to service communication using RabbitMQ and gRPC
- Using IdentityServer as the identity provider.
- Creating a gateway using Microsoft YARP
- Building a client side app with Next.js using the new App Router functionality (since Next.js 13.4)
- Using SignalR for push notifications to the client app
- Dockerizing our different services
- CI/CD workflows using GitHub actions
- Adding ingress controllers
- Publishing the app locally using docker compose
- Unit and integration testing
- Publishing locally to Kubernetes
- Publishing the app to a Kubernetes cluster on the internet


## General architecture
<img src="/pictures/architecture.png" title="architecture"  width="900">


## AuctionService

### Nuget Packages
```
Automapper.Extensions.Microsoft.DependencyInjection
Microsoft.AspNetCore.Authentication.JwtBearer
Microsoft.EntityFrameworkCore.Design
Npgsql.EntityFrameworkCore.PostGreSQL
MassTransit.RabbitMQ
```

### PostgreSQL

- run
```
Add-Migration InitialCreate
```

- Add PostgreSQL container
```
docker compose up -d
```
<img src="/pictures/postgres.png" title="postgres"  width="900">

- run
```
Update-Database
```

- connect to the postgres database. It is empty by now.
<img src="/pictures/postgres2.png" title="postgres"  width="900">

- seed the database
<img src="/pictures/postgres3.png" title="postgres"  width="900">

### Test the API

- GetAllAuctions
<img src="/pictures/auction_api.png" title="auction api"  width="900">

- GetAuctionById
<img src="/pictures/auction_api2.png" title="auction api"  width="900">

- CreateAuction
<img src="/pictures/auction_api3.png" title="auction api"  width="900">

- CreateAuction with error
<img src="/pictures/auction_api4.png" title="auction api"  width="900">

- UpdateAuction
<img src="/pictures/auction_api5.png" title="auction api"  width="900">
