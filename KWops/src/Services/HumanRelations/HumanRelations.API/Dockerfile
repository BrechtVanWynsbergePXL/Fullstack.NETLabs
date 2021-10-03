#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/HumanRelations/HumanRelations.API/HumanRelations.API.csproj", "Services/HumanRelations/HumanRelations.API/"]
COPY ["Services/HumanRelations/HumanRelations.Infrastructure/HumanRelations.Infrastructure.csproj", "Services/HumanRelations/HumanRelations.Infrastructure/"]
COPY ["Services/HumanRelations/HumanRelations.Logic/HumanRelations.Logic.csproj", "Services/HumanRelations/HumanRelations.Logic/"]
COPY ["Services/HumanRelations/HumanRelations.Domain/HumanRelations.Domain.csproj", "Services/HumanRelations/HumanRelations.Domain/"]
RUN dotnet restore "Services/HumanRelations/HumanRelations.API/HumanRelations.API.csproj"
COPY . .
WORKDIR "/src/Services/HumanRelations/HumanRelations.API"
RUN dotnet build "HumanRelations.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HumanRelations.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HumanRelations.API.dll"]