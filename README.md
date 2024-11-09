# Build Your Own Workout
A web application to create custom workout routines and run them for a guided session of timed exercises.

## Contents
* [Overview](https://github.com/tim-honchel/buildyourownworkout/edit/main/README.md#overview)
* [Architecture](https://github.com/tim-honchel/buildyourownworkout/edit/main/README.md#architecture)

## Overview
The application is built using the following principles and technologies:

### Principles
* Authentication
* Clean Architecture
* Dependency Injection
* Single Responsibility
* Test Driven Development

### Technologies
* AJAX
* ASP.NET Core 6.0
* Auth0
* Blazor
* Bootstrap
* BUnit
* Entity Framework
* JQuery
* Moq
* SQL Server
* Swagger
* XUnit

## Architecture
After developing a [proof of concept](https://github.com/tim-honchel/ExerciseVideo), I rebuilt the application with an architecture that would be more testable, maintainable, and scalable. There are 2 startup projects, Presentation and API, with a number of class libraries and test projects. In general, the presentation layer makes calls to the API layer, which uses the Logic layer, which uses the Data layer to read/write to the database.

### Presentation
Blazor web application UI with Razor pages, JavaScript, and C# partial class code behinds.

### API
Web API with controller and endpoints.

### Logic
Class library performs necessary logic and data transformations.

### Data
Class library with repositories for querying the database via Entity Framework.

### Entities
Class library with the entity models used in all other projects.

### Services
Class library with shared code for reuse by other projects.

### Unit Tests
Happy path and edge case tests on public C# methods.

### Integration Tests
Tests for interaction of various layers and components.
