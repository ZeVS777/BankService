#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SovComBankTest.ApiWebApp/SovComBankTest.ApiWebApp.csproj", "SovComBankTest.ApiWebApp/"]
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