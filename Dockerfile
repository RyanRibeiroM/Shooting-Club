FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY src/Shared/ShootingClub.Communication/ShootingClub.Communication.csproj ./src/Shared/ShootingClub.Communication/
COPY src/Shared/ShootingClub.Exceptions/ShootingClub.Exceptions.csproj ./src/Shared/ShootingClub.Exceptions/
COPY src/Backend/ShootingClub.Domain/ShootingClub.Domain.csproj ./src/Backend/ShootingClub.Domain/
COPY src/Backend/ShootingClub.Application/ShootingClub.Application.csproj ./src/Backend/ShootingClub.Application/
COPY src/Backend/ShootingClub.Infrastructure/ShootingClub.Infrastructure.csproj ./src/Backend/ShootingClub.Infrastructure/
COPY src/Backend/ShootingClub.API/ShootingClub.API.csproj ./src/Backend/ShootingClub.API/

RUN dotnet restore src/Backend/ShootingClub.API/ShootingClub.API.csproj

COPY ./src ./src

RUN dotnet publish src/Backend/ShootingClub.API/ShootingClub.API.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build-env /app/out .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ShootingClub.API.dll"]