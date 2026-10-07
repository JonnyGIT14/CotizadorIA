# CotizadorIA

## Descripción

Sistema web para la gestión de cotizaciones desarrollado con ASP.NET Core MVC, Entity Framework Core y PostgreSQL.

El objetivo del sistema es facilitar la creación y administración de cotizaciones, así como integrar funcionalidades de inteligencia artificial para asistir en la selección de materiales y estimación de requerimientos.

---

## Tecnologías Utilizadas

- C#
- ASP.NET Core MVC
- Entity Framework Core
- PostgreSQL
- Visual Studio Code
- Git y GitHub

---

## Arquitectura

El proyecto sigue el patrón MVC (Model - View - Controller).

### Model

Contiene las entidades del sistema.

Ejemplos:

- Producto
- Cliente
- Cotizacion

### View

Contiene las interfaces de usuario.

### Controller

Gestiona la lógica de negocio y la comunicación entre modelos y vistas.

---

## Base de Datos

Motor:

- PostgreSQL

ORM:

- Entity Framework Core

Metodología:

- Code First

Las tablas son generadas mediante migraciones.

---

## Estructura del Proyecto

```text
CotizadorIA/
│
├── Controllers/
├── Models/
├── Views/
├── Data/
├── wwwroot/
├── Migrations/
├── appsettings.json
└── Program.cs

# CotizadorIA

## Descripción

CotizadorIA es una aplicación web desarrollada con ASP.NET Core MVC, Entity Framework Core y PostgreSQL.

Su propósito es facilitar la administración de productos, clientes y cotizaciones, además de integrar funcionalidades de inteligencia artificial para brindar recomendaciones automáticas durante la elaboración de cotizaciones.

---

## Objetivos

- Gestionar productos.
- Administrar clientes.
- Generar cotizaciones.
- Almacenar información en una base de datos PostgreSQL.
- Integrar asistencia basada en IA para recomendaciones de materiales.

---

## Tecnologías Utilizadas

### Backend

- C#
- ASP.NET Core MVC
- Entity Framework Core

### Base de Datos

- PostgreSQL

### Herramientas

- Visual Studio Code
- Git
- GitHub

---

## Arquitectura

El proyecto utiliza el patrón MVC.

### Model

Representa los datos del sistema.

Ejemplos:

- Producto
- Cliente
- Cotizacion

### View

Interfaz visual para el usuario.

### Controller

Gestiona la lógica de negocio y comunicación entre vistas y modelos.

---

## Estructura del Proyecto

```text
CotizadorIA
│
├── Controllers
├── Models
├── Views
├── Data
├── Migrations
├── docs
├── wwwroot
├── appsettings.json
└── Program.cs
```

---

## Estado Actual

### Configuración Inicial

- [x] Proyecto ASP.NET Core MVC creado
- [x] Repositorio Git inicializado
- [x] PostgreSQL instalado
- [x] Configuración de Entity Framework Core
- [x] Primera migración creada

### Desarrollo

- [ ] CRUD Productos
- [ ] CRUD Clientes
- [ ] Gestión de Cotizaciones
- [ ] Integración IA

---

## Autor

Jonathan Beltran

IT Support Analyst | Orbia Netafim