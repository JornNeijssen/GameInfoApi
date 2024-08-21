# GameInfoAPI

This project provides a simple API for managing information about games, 
including details about authors and players. 
The API is built using .NET and follows best practices in software design, 
such as the Repository and DTO patterns.

## Table of Contents

- [Project Overview](#project-overview)
- [Repository Structure](#repository-structure)
- [Important Files](#important-files)
- [Getting Started](#getting-started)
  - [Running the Application](#running-the-application)
  
## Project Overview

**GameInfoAPI** is a RESTful API that allows users to perform CRUD (Create, Read, Update, Delete) operations on a set of resources including games, authors, and players. 
The API supports operations like getting a list of games, retrieving details about a specific game by its ID, adding new games, updating existing ones, and deleting games.

The API is structured to follow the Repository pattern. It also uses the DTO (Data Transfer Object) pattern enhancing the maintainability and scalability of the code but also the safety in communication.

## Repository Structure

```
GameInfoApi/
├── .github/
│   └── workflows/
│       └── main.yaml
├── GameInfoAPI/
│   ├── Controllers/
│   ├── DTOs/
│   ├── Data/
│   ├── Repositories/
│   ├── Entities/
│   ├── GameInfoAPI.csproj
│   ├── Dockerfile
│   └── ...
├── GameInfoAPI.Tests/
│   ├── UnitTests/
│   ├── IntegrationTests/
│   ├── GameInfoAPI.Tests.csproj
├── README.md
└── .gitignore
```

## Important Files
GameInfoAPI/GameInfoAPI.csproj: The main project file for the API.
GameInfoAPI/Controllers/: Contains the API controllers that handle HTTP requests.
GameInfoAPI/DTOs/: Data Transfer Objects used for data encapsulation.
GameInfoAPI/Entities/: Defines the data models representing the domain entities.
GameInfoAPI/Repositories/: Implements the Repository pattern for data access.
GameInfoAPI/Dockerfile: A Dockerfile for containerizing the application.
GameInfoAPI.Tests/: Contains unit and integration tests for the API.
.github/workflows/main.yaml: GitHub Actions workflow for continuous dockerhub updates.

## Getting Started
Before you begin, ensure you have the following tools installed:

.NET SDK (version 8.0 or later)
Docker (for containerization)
Git

### Running the Application
Clone the Repository
```
git clone https://github.com/JornNeijssen/GameInfoApi.git
cd GameInfoApi
Build the Project
```

Navigate to the GameInfoAPI directory and build the project:
```
cd GameInfoAPI
dotnet build
```

Run the Application
You can run the API locally using the following command:
```
dotnet run
```

The API will be accessible at https://localhost:7153 (HTTPS) or http://localhost:5036 (HTTP) if you would like to change this this vcan be done in the properties map launchsettings file.
