FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/NetInventory.Api/NetInventory.Api.csproj", "src/NetInventory.Api/"]
COPY ["src/NetInventory.Application/NetInventory.Application.csproj", "src/NetInventory.Application/"]
COPY ["src/NetInventory.Infrastructure/NetInventory.Infrastructure.csproj", "src/NetInventory.Infrastructure/"]
COPY ["src/NetInventory.Domain/NetInventory.Domain.csproj", "src/NetInventory.Domain/"]
RUN dotnet restore "src/NetInventory.Api/NetInventory.Api.csproj"
COPY . .
WORKDIR "/src/src/NetInventory.Api"
RUN dotnet build "NetInventory.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetInventory.Api.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetInventory.Api.dll"]
