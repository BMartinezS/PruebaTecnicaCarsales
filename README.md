# Carsales - Rick & Morty API

## 📝 Descripción del Proyecto
Aplicación web fullstack que consume la API de Rick and Morty implementando una arquitectura Backend for Frontend (BFF). El proyecto está construido con .NET 8 en el backend y Angular 19 en el frontend.

## 🛠 Tecnologías Utilizadas

### Backend
- .NET 8
- Clean Architecture
- Swagger para documentación de API
- Patrón BFF (Backend For Frontend)
- Injección de dependencias
- Principios SOLID

### Frontend
- Angular 19
- Signals para manejo de estado
- Standalone Components
- CSS puro (sin frameworks externos)

## 📋 Requisitos Previos

1. **Backend**
   - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - Visual Studio 2022 o Visual Studio Code

2. **Frontend**
   - [Node.js](https://nodejs.org/) (versión 18 o superior)
   - Angular CLI versión 19
   ```bash
   npm install -g @angular/cli@19
   ```

## 🚀 Instalación y Ejecución

### Backend

1. Navegar al directorio del backend:
```bash
cd Backend/PruebaTecnicaCarsales.API
```

2. Restaurar las dependencias:
```bash
dotnet restore
```

3. Ejecutar el proyecto:
```bash
dotnet run
```

El backend estará disponible en:
- API: https://localhost:5059
- Swagger UI: https://localhost:5059/swagger

### Frontend

1. Navegar al directorio del frontend:
```bash
cd Frontend/PruebaTecnicaCarsales
```

2. Instalar dependencias:
```bash
npm install
```

3. Ejecutar el proyecto:
```bash
ng serve
```

La aplicación estará disponible en:
- http://localhost:4200

## 🏗 Estructura del Proyecto

```
PruebaTecnicaCarsales/
├── Backend/
│   ├── PruebaTecnicaCarsales.API/          # Proyecto principal API
│   │   ├── Controllers/                     # Controladores de la API
│   │   ├── Program.cs                       # Configuración de la aplicación
│   │   └── appsettings.json                # Configuraciones
│   ├── PruebaTecnicaCarsales.Core/         # Capa de dominio
│   │   ├── Interfaces/                      # Contratos de servicios
│   │   └── Models/                          # Modelos de dominio
│   ├── PruebaTecnicaCarsales.Infrastructure/# Capa de infraestructura
│   │   └── Services/                        # Implementaciones de servicios
│   └── PruebaTecnicaCarsales.sln           # Solución .NET
│
└── Frontend/
    └── PruebaTecnicaCarsales/
        ├── src/
        │   ├── app/
        │   │   ├── components/              # Componentes de la aplicación
        │   │   ├── models/                  # Interfaces y tipos
        │   │   └── services/                # Servicios
        │   └── environments/                # Configuraciones por ambiente
        └── angular.json                     # Configuración de Angular
```

## 🌟 Características

- Lista paginada de episodios de Rick & Morty
- Vista detallada de cada episodio
- Información de personajes por episodio
- Diseño responsive
- Manejo de errores y estados de carga
- Implementación de Clean Architecture
- Patrón BFF para optimizar las peticiones
- Utilización de caché

## 🔗 API Endpoints

### Episodios
```
GET /api/episodes?page={page}
GET /api/episodes/{id}
```

### Personajes
```
GET /api/characters/batch?ids={ids}
GET /api/characters/{id}
```

## 🎯 Patrones de Diseño Implementados

### Backend
- Patrón repositorio
- Inyección de dependencias
- Backend for Frontend (BFF)
- Patrón fachada
- Patrón estrategia

### Frontend
- Patrón observador (RxJS y Signals)
- Patrón Singleton
- 

## 🔐 Variables de Entorno

### Backend (appsettings.json)
```json
{
  "RickAndMortyApi": {
    "BaseUrl": "https://rickandmortyapi.com/api"
  }
}
```

### Frontend (environment.ts)
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5059/api'
};
```

## 👨‍💻 Autor
Bastián Martínez
