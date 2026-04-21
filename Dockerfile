# 1. Etapa de construcción (Usamos el SDK para compilar)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# CAMBIA "TuProyecto.csproj" por el nombre real de tu proyecto
COPY ["DragRacingAPI.csproj", "./"]
RUN dotnet restore "DragRacingAPI.csproj"
COPY . .
# Compilamos en modo Release
RUN dotnet publish "DragRacingAPI.csproj" -c Release -o /app/publish

# 2. Etapa de ejecución (Usamos un entorno ligero solo para correr el código)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# Exponemos el puerto 8080 (El puerto por defecto de .NET 8)
EXPOSE 8080
COPY --from=build /app/publish .

# CAMBIA "TuProyecto.dll" por el nombre de tu dll principal
ENTRYPOINT ["dotnet", "DragRacingAPI.dll"]