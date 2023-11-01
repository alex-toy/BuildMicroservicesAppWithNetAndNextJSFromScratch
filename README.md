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


## Auction MicroService

### Nuget Packages
```
Automapper.Extensions.Microsoft.DependencyInjection
Microsoft.AspNetCore.Authentication.JwtBearer
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.Tools
Npgsql.EntityFrameworkCore.PostGreSQL
MassTransit.RabbitMQ
MassTransit.EntityFrameworkCore
```

### PostgreSQL

- run
```
Add-Migration InitialCreate
```

- Add PostgreSQL container
```
docker compose down
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


## Search MicroService

### Nuget Packages
```
MongoDB.Entities
Automapper.Extensions.Microsoft.DependencyInjection
Microsoft.Extensions.Http.Polly
```

- Add MongoDB container
```
docker compose up -d
```
<img src="/pictures/mongodb.png" title="search"  width="900">

### Test the API

- Get all items
<img src="/pictures/search.png" title="search"  width="900">

- Get all cars with make of Ford
<img src="/pictures/search1.png" title="search"  width="900">

- Get all items With PageSize and PageNumber
<img src="/pictures/search2.png" title="search"  width="900">


## RabbitMQ

### Nuget Packages
```
MassTransit.RabbitMQ
```

- Add RabbitMQ container
```
docker compose up -d
```
<img src="/pictures/rabbitmq.png" title="rabbitmq"  width="900">

- connect to rabbitmq (guest/guest)
<img src="/pictures/rabbitmq1.png" title="rabbitmq"  width="900">

- run the application and see the exchanges created
<img src="/pictures/rabbitmq2.png" title="rabbitmq"  width="900">

- create an auction
<img src="/pictures/rabbitmq3.png" title="rabbitmq"  width="900">

- see the result in rabbitmq
<img src="/pictures/rabbitmq4.png" title="rabbitmq"  width="900">

- check that the auction was created
<img src="/pictures/rabbitmq5.png" title="rabbitmq"  width="900">

- run
```
Add-Migration outbox
```

- stop RabbitMQ
<img src="/pictures/rabbitmq6.png" title="rabbitmq"  width="900">

- create a new auction. Since RabbitMQ is off, the message ends up in the OutboxMessage table, waiting to be delivered to the queue.
<img src="/pictures/rabbitmq7.png" title="rabbitmq"  width="900">
<img src="/pictures/rabbitmq8.png" title="rabbitmq"  width="900">

- if you try to get that auction from the search service, you will obviously get no result because RabbitMQ is off.
<img src="/pictures/rabbitmq9.png" title="rabbitmq"  width="900">

- run RabbitMQ again and search the item. This time you have a result
<img src="/pictures/rabbitmq91.png" title="rabbitmq"  width="900">


## Identity Server

### Create Service
```
dotnet new isaspid -o IdentityService
```

### Nuget Packages
```
Npgsql.EntityFrameworkCore.PostGreSQL
Polly
Serilog.AspNetCore
```

- run the Identity Service
<img src="/pictures/identity.png" title="identity service"  width="900">

- run
```
Add-Migration InitialCreate
Update-Database
```

- connect to the postgres database : localhost / postgres / postgrespw
<img src="/pictures/identity2.png" title="identity service"  width="900">

- get a token
<img src="/pictures/identity3.png" title="identity service"  width="900">
<img src="/pictures/identity4.png" title="identity service"  width="900">

- create an auction with no authorization
<img src="/pictures/identity5.png" title="identity service"  width="900">

- create an auction with authorization
<img src="/pictures/identity6.png" title="identity service"  width="900">

- update an auction with no authorization
<img src="/pictures/identity7.png" title="identity service"  width="900">

- update an auction with authorization
<img src="/pictures/identity8.png" title="identity service"  width="900">

- delete an auction with authorization
<img src="/pictures/identity9.png" title="identity service"  width="900">


## Gateway Service

### Nuget Packages
```
Yarp.ReverseProxy
Microsoft.AspNetCore.Authentication.JwtBearer
```

### Run the Gateway Service

- get a token for bob
<img src="/pictures/gateway.png" title="gateway service"  width="900">

- get all items
<img src="/pictures/gateway1.png" title="gateway service"  width="900">

- get all cars with make of Ford
<img src="/pictures/gateway2.png" title="gateway service"  width="900">

- create new Auction as Alice
<img src="/pictures/gateway3.png" title="gateway service"  width="900">

- Update the created auction as Bob
<img src="/pictures/gateway4.png" title="gateway service"  width="900">

- Update the created auction as Alice
<img src="/pictures/gateway5.png" title="gateway service"  width="900">

- Delete auction as Alice
<img src="/pictures/gateway6.png" title="gateway service"  width="900">