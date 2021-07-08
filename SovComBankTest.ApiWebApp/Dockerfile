#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SovComBankTest.ApiWebApp/SovComBankTest.ApiWebApp.csproj", "SovComBankTest.ApiWebApp/"]
COPY ["SovComBankTest.Services.DependencyInjection/SovComBankTest.Services.DependencyInjection.csproj", "SovComBankTest.Services.DependencyInjection/"]
COPY ["SovComBankTest.Repositories.DependencyInjection/SovComBankTest.Repositories.DependencyInjection.csproj", "SovComBankTest.Repositories.DependencyInjection/"]
COPY ["SovComBankTest.Repositories/SovComBankTest.Repositories.csproj", "SovComBankTest.Repositories/"]
COPY ["SovComBankTest.Repositories.Abstractions/SovComBankTest.Repositories.Abstractions.csproj", "SovComBankTest.Repositories.Abstractions/"]
COPY ["SovComBankTest.Entities/SovComBankTest.Entities.csproj", "SovComBankTest.Entities/"]
COPY ["SovComBankTest.Services/SovComBankTest.Services.csproj", "SovComBankTest.Services/"]
COPY ["SovComBankTest.Services.Abstractions/SovComBankTest.Services.Abstractions.csproj", "SovComBankTest.Services.Abstractions/"]
COPY ["SovComBankTest.Services.Models/SovComBankTest.Services.Models.csproj", "SovComBankTest.Services.Models/"]
COPY ["SovComBankTest.Utils/SovComBankTest.Utils.csproj", "SovComBankTest.Utils/"]
RUN dotnet restore "SovComBankTest.ApiWebApp/SovComBankTest.ApiWebApp.csproj"
COPY . .
WORKDIR "/src/SovComBankTest.ApiWebApp"
RUN dotnet build "SovComBankTest.ApiWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SovComBankTest.ApiWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SovComBankTest.ApiWebApp.dll"]
