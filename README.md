# Redline Rush: Vertical Shift - Backend & Game Server 

Este repositorio contiene la API REST y el servidor de WebSockets (Multijugador en tiempo real) que da vida a **Redline Rush**. Actúa como el motor central del juego, gestionando el progreso de los jugadores, la economía (Garaje) y la telemetría de las carreras 1 vs 1.

## Autor
* **Jesús Elías Arriaga Salinas** - *Arquitectura Backend, Base de Datos y Lógica SignalR*

## Institución
**Universidad Politécnica de la Región Ribereña (UPRR)**

## Stack Tecnológico
* **Framework:** .NET 8 (C#)
* **Real-Time Engine:** Microsoft SignalR (WebSockets)
* **Base de Datos:** SQLite
* **ORM:** Entity Framework Core

## Características Principales

1. **API RESTful (Gestión de Jugadores):**
   * Creación y autenticación de usuarios.
   * Gestión del inventario (Autos en el garaje).
   * Sistema de economía (Recompensas al finalizar carreras).

2. **SignalR Hub (`/racehub`):**
   * **Matchmaking Concurrente:** Emparejamiento seguro 1v1 utilizando semáforos (`SemaphoreSlim`) para evitar colisiones en la memoria.
   * **Telemetría Bidireccional:** Transmisión de datos de posición (distancia recorrida) entre clientes a 60 FPS para renderizado en tiempo real.
   * **Manejo de Desconexiones:** Cancelación segura y limpieza de salas en caso de pérdida de red (Resiliencia).

## Cómo ejecutar el servidor en local

Si deseas probar el servidor en tu propia máquina para desarrollo, sigue estos pasos:

1. Clona el repositorio:
   ```bash
   git clone [URL_DE_ESTE_REPOSITORIO]
