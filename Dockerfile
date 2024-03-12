FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV TZ=America/Sao_Paulo
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/BurgerRoyale.Auth.API/BurgerRoyale.Auth.API.csproj", "src/BurgerRoyale.Auth.API/"]
COPY ["src/BurgerRoyale.Auth.Application/BurgerRoyale.Auth.Application.csproj", "src/BurgerRoyale.Auth.Application/"]
COPY ["src/BurgerRoyale.Auth.Domain/BurgerRoyale.Auth.Domain.csproj", "src/BurgerRoyale.Auth.Domain/"]
COPY ["src/BurgerRoyale.Auth.Infrastructure/BurgerRoyale.Auth.Infrastructure.csproj", "src/BurgerRoyale.Auth.Infrastructure/"]
COPY ["src/BurgerRoyale.Auth.IOC/BurgerRoyale.Auth.IOC.csproj", "src/BurgerRoyale.Auth.IOC/"]
RUN dotnet restore "src/BurgerRoyale.Auth.API/BurgerRoyale.Auth.API.csproj"
COPY . .
WORKDIR "/src/src/BurgerRoyale.Auth.API"
RUN dotnet build "BurgerRoyale.Auth.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BurgerRoyale.Auth.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BurgerRoyale.Auth.API.dll"]