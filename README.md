# 🚀 velocist.LogService
<p align="center">
  <img src="https://img.shields.io/badge/License-LGPL%20v3-blue.svg" alt="License: LGPL v3">
  <img src="https://img.shields.io/badge/Author-velocist-green.svg" alt="Author: velocist">
  <img src="https://img.shields.io/badge/.NET-9.0-blueviolet" alt=".NET 9.0">
</p>

> **Biblioteca para la gestión centralizada de logs en aplicaciones .NET**
---

## 📑 Tabla de Contenidos
- [Descripción](#descripcion)
- [Características](#caracteristicas)
- [Instalación y Uso](#instalacion-y-uso) 
  - [1. Ejemplo de configuración](#1)
  - [2. Configuración manual en un host genérico](#2)
- [Licencia](#licencia)
- [Autor](#autor)

---

## 📝 Descripción<a name="descripcion"></a>

Biblioteca para la gestión centralizada de logs en aplicaciones .NET, integrando proveedores como Log4Net y consola, y permitiendo configuración flexible mediante archivos JSON.

## ✨ Características<a name="caracteristicas"></a>
- Integración con Log4Net y consola.
- Configuración centralizada mediante archivos JSON.
- Inyección de dependencias con Microsoft.Extensions.DependencyInjection.
- Métodos de extensión para facilitar la integración.

---

## �� Instalación y Uso<a name="instalacion-y-uso"></a>

1. Agrega la referencia al proyecto o compila la biblioteca.
2. Asegúrate de tener los archivos de configuración en la ruta `Settings/logSettings.json` y `Settings/log4net.config`.

## 1. Ejemplo de configuración (`Settings/logSettings.json`)<a name="1"></a>
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Debug",
      "Microsoft.EntityFrameworkCore.Database.Command": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 2. Configuración manual en un host genérico<a name="2"></a>
```csharp
using Microsoft.Extensions.DependencyInjection;
using velocist.LogService;

var services = new ServiceCollection();
services.AddLogging(builder => {
    builder.AddConsole();
    // builder.AddLog4Net("Settings/log4net.config", true);
});
var provider = services.BuildServiceProvider();
var logger = provider.GetRequiredService<ILogger<MiClase>>();
logger.LogInformation("Mensaje desde DI");
```

---

## 📝 Licencia<a name="licencia"></a>

Este proyecto está licenciado bajo la **GNU Lesser General Public License v3.0 (LGPL-3.0)**. Consulta el archivo [LICENSE.txt](./LICENSE.txt) para más detalles.

---

## 👤 Autor<a name="autor"></a>

**velocist**

¿Dudas o sugerencias? Abre un issue o revisa la documentación XML en el código fuente para más detalles.