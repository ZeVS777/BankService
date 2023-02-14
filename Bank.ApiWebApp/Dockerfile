#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Bank.ApiWebApp/Bank.ApiWebApp.csproj", "Bank.ApiWebApp/"]
COPY ["Bank.Services.DependencyInjection/Bank.Services.DependencyInjection.csproj", "Bank.Services.DependencyInjection/"]
COPY ["Bank.Repositories.DependencyInjection/Bank.Repositories.DependencyInjection.csproj", "Bank.Repositories.DependencyInjection/"]
COPY ["Bank.Repositories/Bank.Repositories.csproj", "Bank.Repositories/"]
COPY ["Bank.Repositories.Abstractions/Bank.Repositories.Abstractions.csproj", "Bank.Repositories.Abstractions/"]
COPY ["Bank.Entities/Bank.Entities.csproj", "Bank.Entities/"]
COPY ["Bank.Services/Bank.Services.csproj", "Bank.Services/"]
COPY ["Bank.Services.Abstractions/Bank.Services.Abstractions.csproj", "Bank.Services.Abstractions/"]
COPY ["Bank.Services.Models/Bank.Services.Models.csproj", "Bank.Services.Models/"]
COPY ["Bank.Utils/Bank.Utils.csproj", "Bank.Utils/"]
RUN dotnet restore "Bank.ApiWebApp/Bank.ApiWebApp.csproj"
COPY . .
WORKDIR "/src/Bank.ApiWebApp"
RUN dotnet build "Bank.ApiWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bank.ApiWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bank.ApiWebApp.dll"]
