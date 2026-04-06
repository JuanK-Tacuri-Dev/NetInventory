FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/NetInventory.Client/NetInventory.Client.csproj", "src/NetInventory.Client/"]
RUN dotnet restore "src/NetInventory.Client/NetInventory.Client.csproj"
COPY . .
WORKDIR "/src/src/NetInventory.Client"
RUN dotnet publish "NetInventory.Client.csproj" -c Release -o /app/publish --no-restore

FROM nginx:alpine AS final
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY src/NetInventory.Client/nginx.conf /etc/nginx/nginx.conf
