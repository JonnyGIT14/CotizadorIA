# Configuración del Entorno de Desarrollo

## Información General

Proyecto: CotizadorIA

Tecnologías principales:

- ASP.NET Core MVC
- Entity Framework Core
- PostgreSQL
- GitHub
- Visual Studio Code

---

## Instalación de Herramientas

### Visual Studio Code

Se instaló Visual Studio Code como entorno principal de desarrollo.

### .NET SDK

Verificación:

```bash
dotnet --version
```

Resultado esperado:

```text
8.x.x o superior
```

### Git

Verificación:

```bash
git --version
```

### PostgreSQL

Verificación:

```bash
psql --version
```

Resultado:

```text
PostgreSQL 18.6
```

---

## Creación del Proyecto

Comando utilizado:

```bash
dotnet new mvc
```

Esto generó la estructura base ASP.NET Core MVC.

---

## Configuración de Entity Framework Core

Instalación de paquetes:

```bash
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

Instalación de herramientas:

```bash
dotnet tool install --global dotnet-ef
```

---

## Configuración de PostgreSQL

Cadena de conexión definida en:

```text
appsettings.json
```

Base de datos:

```text
CotizadorDB
```

Motor:

```text
PostgreSQL
```

---

## Control de Versiones

Repositorio administrado mediante Git.

Comandos básicos utilizados:

```bash
git init
git add .
git commit -m "Primer commit"
```

Posteriormente el repositorio fue vinculado con GitHub.

---

## Estado de Avance

Fecha de inicio:

07 de octubre de 2026

Componentes configurados:

- ASP.NET Core MVC
- Entity Framework Core
- PostgreSQL
- GitHub
- VS Code

Estado:

Configuración inicial completada.