# Backend de catalogo y cotizaciones

El backend de catalogo del directorio `Backend-para-CotizadorIA` ya esta integrado en el proyecto MVC. Incluye las entidades del catalogo, configuracion de Entity Framework Core, acceso a productos y clientes, importacion JSON y calculo de cotizaciones. Se agregaron pantallas MVC para administrar clientes y productos, crear y consultar cotizaciones. Los servicios se registran desde `Program.cs`.

## Migracion de base de datos

Las migraciones agregan el catalogo, las cotizaciones y los campos requeridos por el formato comercial. La migracion `AgregarDatosFormatoCotizacion` agrega datos opcionales para conservar cotizaciones historicas y guardar tipo de cambio, cultivo, crop group, referencia de cliente, clasificacion y minimo de compra. Antes de usar estos cambios en otra base, revisa y aplica las migraciones pendientes:

```powershell
dotnet ef migrations script
dotnet ef database update
```

La migracion conserva las columnas actuales de productos y clientes. Los productos existentes se mantienen con su codigo original y precio, reciben un codigo de catalogo `LEGACY-{Id}` y quedan bajo la familia/subfamilia `Sin clasificar`. Los clientes existentes conservan sus datos y reciben una clave `LEGACY-{Id}`. Estos registros heredados quedan inactivos para no mezclarlos con el catalogo importado; sus factores comerciales quedan sin configurar (`null`) para evitar inventar valores.

La migracion no se aplica automaticamente al iniciar la aplicacion. Haz respaldo y revisa el script antes de aplicarlo a una base compartida.

## Importar el catalogo

El importador espera `prices.json` y `clients.json` como listas JSON con las propiedades compactas que define `CatalogoImportService`. Se pueden mantener fuera del repositorio porque el comando recibe las rutas completas.

Para una importacion puntual, ejecuta desde la raiz del proyecto:

```powershell
dotnet run -- --import-catalog "C:\ruta\prices.json" "C:\ruta\clients.json"
```

El modo de mantenimiento aplica primero las migraciones pendientes y despues importa los dos archivos dentro de una transaccion. Si la importacion falla, muestra el error y revierte los cambios de los archivos. No ejecutes la importacion automaticamente al iniciar el sitio; valida que los archivos sean las versiones vigentes antes de ejecutarla.

El proceso actualiza productos por `CodigoCatalogo`, clientes con SAP por codigo SAP y clientes sin SAP mediante una clave derivada de categoria, nombre y direccion. No elimina registros ausentes en los archivos ni reactiva registros desactivados.

## Servicios disponibles

- `ProductoData` y `ClienteData`: consultas, creacion y actualizacion de registros.
- `CatalogoImportService`: importacion transaccional del catalogo.
- `CotizacionCalculator`: calculo de factores, descuento, IVA y avisos comerciales.
- Las pantallas permiten mantener registros y archivar productos/clientes desactivandolos, sin borrar datos ligados a cotizaciones.
- La creacion de cotizaciones guarda un folio, asocia el usuario autenticado y conserva los datos comerciales de cada producto. El tipo de cambio, cultivo y crop group se capturan en cada cotizacion.
- La fecha de cotizacion inicia con la fecha actual y se puede cambiar; se distingue de la fecha/hora de creacion del registro.
- El detalle de la cotizacion permite imprimir o guardar como PDF un formato basado en la hoja `COTIZADOR EMERGENTE` del libro comercial, con datos del cliente, metadatos de impresion, productos, importes y espacio de aprobacion/firma.
- La busqueda de productos permite filtrar por SKU y seleccionar varios en una sola accion. Tambien se pueden agregar lineas de terceros solo para esa cotizacion; su costo se cotiza directamente, sin factores ni descuentos, y unicamente puede aplicarse IVA. El producto no se guarda en el catalogo.

Los formularios usan ViewModels y no enlazan entidades directamente. La autenticacion y autorizacion por roles aun no esta implementada en la aplicacion; no publiques estas pantallas en una red accesible hasta proteger la consulta y edicion de precios y factores.

## Reglas del calculador

- Usa `FactorGeneral` para productos normales y `FactorStreamline` si la familia contiene "Streamline".
- Solo permite descuento si `FactorGeneral` es exactamente `0.55` y hay productos no Streamline; el maximo es 20 % y solo afecta esos productos.
- Aplica IVA del 16 % si se solicita y el carrito no es exclusivamente Streamline.
- Emite avisos por factor menor a 0.35, entrega superior a cuatro semanas, cantidad bajo el minimo y posible interes en GrowSphere.
- No redondea durante el calculo; el redondeo corresponde a la presentacion.

Al guardar una cotizacion, conserva en cada detalle el codigo, descripcion, clasificacion, unidad, minimo de compra, precio, factor y totales usados en ese momento para que el historial no dependa de cambios posteriores al catalogo.
