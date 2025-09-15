# 🚀 velocist.LogService
<p align="center">
  <img src="https://img.shields.io/badge/License-LGPL%20v3-blue.svg" alt="License: LGPL v3">
  <img src="https://img.shields.io/badge/Author-velocist-green.svg" alt="Author: velocist">
  <img src="https://img.shields.io/badge/.NET-9.0-blueviolet" alt=".NET 9.0">
</p>

> **Biblioteca para la gestión centralizada de logs y recolección de errores en aplicaciones .NET**

---

## 📑 Tabla de Contenidos
- [Descripción](#descripcion)
- [Características](#caracteristicas)
- [Instalación](#instalacion)
- [Configuración](#configuracion)
  - [Archivo de configuración JSON](#archivo-de-configuracion-json)
  - [Configuración de Log4Net](#configuracion-de-log4net)
- [Uso del Sistema de Logging](#uso-del-sistema-de-logging)
  - [Inicialización básica](#inicializacion-basica)
  - [Uso con inyección de dependencias](#uso-con-inyeccion-de-dependencias)
  - [Uso estático](#uso-estatico)
- [Uso del ErrorCollector](#uso-del-errorcollector)
  - [ErrorCollector genérico](#errorcollector-generico)
  - [ErrorCollector no genérico](#errorcollector-no-generico)
  - [Manejo de eventos](#manejo-de-eventos)
- [Ejemplos Completos](#ejemplos-completos)
- [API Reference](#api-reference)
- [Licencia](#licencia)
- [Autor](#autor)

---

## �� Descripción<a name="descripcion"></a>

**velocist.LogService** es una biblioteca completa para la gestión centralizada de logs y recolección de errores en aplicaciones .NET. Proporciona una interfaz unificada para logging con soporte para múltiples proveedores (Console, Debug, Log4Net) y un sistema robusto de recolección y manejo de errores.

### Componentes principales:
- **StaticLoggerFactory**: Factory estático para la creación de loggers
- **ErrorCollector**: Sistema de recolección centralizada de errores
- **ErrorInfo**: Estructura de datos para información de errores
- **ServicesExtensions**: Métodos de extensión para inyección de dependencias

---

## ✨ Características<a name="caracteristicas"></a>

### Sistema de Logging:
- ✅ Integración con múltiples proveedores (Console, Debug, Log4Net)
- ✅ Configuración centralizada mediante archivos JSON
- ✅ Inyección de dependencias con Microsoft.Extensions.DependencyInjection
- ✅ Factory estático para acceso global a loggers
- ✅ Auto-inicialización cuando es necesario
- ✅ Soporte para .NET 9.0

### Sistema de ErrorCollector:
- ✅ Recolección centralizada de errores por tipo
- ✅ Thread-safe con locks internos
- ✅ Eventos para notificación de errores capturados
- ✅ Integración automática con el sistema de logging
- ✅ Versiones genérica y no genérica
- ✅ Información detallada de errores (timestamp, clase, método, excepción)

---

## �� Instalación<a name="instalacion"></a>

### Opción 1: Paquete NuGet (recomendado)
```bash
Install-Package velocist.LogService
```

### Opción 2: Compilación desde código fuente
1. Clona el repositorio
2. Compila el proyecto `velocist.LogService.csproj`
3. Agrega la referencia a tu proyecto

### Dependencias requeridas:
- Microsoft.Extensions.Configuration.FileExtensions (9.0.8)
- Microsoft.Extensions.Configuration.Json (9.0.8)
- Microsoft.Extensions.Logging.Console (9.0.8)
- Microsoft.Extensions.Logging.Debug (9.0.8)
- Microsoft.Extensions.Logging.Log4Net.AspNetCore (8.0.0)

---

## ⚙️ Configuración<a name="configuracion"></a>

### Archivo de configuración JSON<a name="archivo-de-configuracion-json"></a>

Crea el archivo `logSettings.json` en la carpeta `Settings/` de tu proyecto:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Debug",
      "Microsoft.EntityFrameworkCore.Database.Command": "Warning",
      "Microsoft.AspNetCore": "Warning",
      "velocist": "Debug"
    }
  }
}
```

### Configuración de Log4Net<a name="configuracion-de-log4net"></a>

Crea el archivo `log4net.config` en la raíz de tu proyecto:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <log4net>
    <appender name="ConsoleAppender" type="log4net.Appender.ConsoleAppender">
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
      </layout>
    </appender>
    
    <appender name="FileAppender" type="log4net.Appender.RollingFileAppender">
      <file value="logs/application.log" />
      <appendToFile value="true" />
      <rollingStyle value="Size" />
      <maxSizeRollBackups value="10" />
      <maximumFileSize value="10MB" />
      <staticLogFileName value="false" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
      </layout>
    </appender>
    
    <root>
      <level value="DEBUG" />
      <appender-ref ref="ConsoleAppender" />
      <appender-ref ref="FileAppender" />
    </root>
  </log4net>
</configuration>
```

---

## 📝 Uso del Sistema de Logging<a name="uso-del-sistema-de-logging"></a>

### Inicialización básica<a name="inicializacion-basica"></a>

```csharp
using velocist.LogService;
using static velocist.LogService.StaticLoggerFactory;

// El logger se inicializa automáticamente la primera vez que se usa
var logger = GetStaticLogger<MiClase>();
logger.LogInformation("Aplicación iniciada");
```

### Uso con inyección de dependencias<a name="uso-con-inyeccion-de-dependencias"></a>

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using velocist.LogService;

// Configuración en Program.cs o Startup.cs
var services = new ServiceCollection();

services.AddLogging(builder => {
    builder.AddConsole()
           .AddDebug()
           .AddLog4Net("log4net.config", true)
           .SetMinimumLevel(LogLevel.Debug);
});

var provider = services.BuildServiceProvider();

// Uso en clases
public class MiServicio
{
    private readonly ILogger<MiServicio> _logger;
    
    public MiServicio(ILogger<MiServicio> logger)
    {
        _logger = logger;
    }
    
    public void ProcesarDatos()
    {
        _logger.LogInformation("Iniciando procesamiento de datos");
        // ... lógica del método
        _logger.LogInformation("Procesamiento completado");
    }
}
```

### Uso estático<a name="uso-estatico"></a>

```csharp
using velocist.LogService;
using static velocist.LogService.StaticLoggerFactory;

public class MiClase
{
    private readonly ILogger _logger;
    
    public MiClase()
    {
        _logger = GetStaticLogger<MiClase>();
    }
    
    public void MetodoEjemplo()
    {
        _logger.LogInformation("Ejecutando método de ejemplo");
        
        try
        {
            // Lógica que puede fallar
            OperacionRiesgosa();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en OperacionRiesgosa");
        }
    }
}
```

---

## 🚨 Uso del ErrorCollector<a name="uso-del-errorcollector"></a>

### ErrorCollector genérico<a name="errorcollector-generico"></a>

```csharp
using velocist.LogService;

public class ServicioDatos
{
    public void ProcesarArchivo(string rutaArchivo)
    {
        try
        {
            // Lógica que puede fallar
            var contenido = File.ReadAllText(rutaArchivo);
            ProcesarContenido(contenido);
        }
        catch (FileNotFoundException ex)
        {
            ErrorCollector<ServicioDatos>.AddError(
                nameof(ProcesarArchivo), 
                ex, 
                $"Archivo no encontrado: {rutaArchivo}"
            );
        }
        catch (Exception ex)
        {
            ErrorCollector<ServicioDatos>.AddError(
                nameof(ProcesarArchivo), 
                ex
            );
        }
    }
    
    // Método para obtener errores específicos de esta clase
    public IReadOnlyList<ErrorInfo> ObtenerErrores()
    {
        return ErrorCollector<ServicioDatos>.GetErrors();
    }
    
    // Método para limpiar errores
    public void LimpiarErrores()
    {
        ErrorCollector<ServicioDatos>.Clear();
    }
}
```

### ErrorCollector no genérico<a name="errorcollector-no-generico"></a>

```csharp
using velocist.LogService;

public class Utilidades
{
    public static void ValidarDatos(object datos)
    {
        try
        {
            if (datos == null)
                throw new ArgumentNullException(nameof(datos));
                
            // Validaciones adicionales
        }
        catch (Exception ex)
        {
            ErrorCollector.AddError(
                nameof(Utilidades), 
                nameof(ValidarDatos), 
                ex, 
                "Error en validación de datos"
            );
        }
    }
    
    // Obtener todos los errores del sistema
    public static IReadOnlyList<ErrorInfo> ObtenerTodosLosErrores()
    {
        return ErrorCollector.GetErrors();
    }
}
```

### Manejo de eventos<a name="manejo-de-eventos"></a>

```csharp
using velocist.LogService;

public class SistemaNotificaciones
{
    public SistemaNotificaciones()
    {
        // Suscribirse a eventos de error
        ErrorCollector<MiClase>.ErrorCaptured += OnErrorCaptured;
        ErrorCollector.ErrorCaptured += OnGlobalErrorCaptured;
    }
    
    private void OnErrorCaptured(ErrorInfo error)
    {
        // Notificar error específico de MiClase
        Console.WriteLine($"Error en {error.ClassName}: {error.Message}");
        
        // Enviar notificación por email, Slack, etc.
        EnviarNotificacion(error);
    }
    
    private void OnGlobalErrorCaptured(ErrorInfo error)
    {
        // Notificar cualquier error del sistema
        Console.WriteLine($"Error global: {error}");
    }
    
    private void EnviarNotificacion(ErrorInfo error)
    {
        // Implementar lógica de notificación
    }
}
```

---

## 💡 Ejemplos Completos<a name="ejemplos-completos"></a>

### Ejemplo 1: Aplicación de consola con logging y manejo de errores

```csharp
using velocist.LogService;
using static velocist.LogService.StaticLoggerFactory;

class Program
{
    static void Main(string[] args)
    {
        var logger = GetStaticLogger<Program>();
        logger.LogInformation("Iniciando aplicación");
        
        try
        {
            var procesador = new ProcesadorDatos();
            procesador.ProcesarArchivos(args);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fatal en la aplicación");
            ErrorCollector<Program>.AddError(nameof(Main), ex);
        }
        
        // Mostrar errores capturados
        var errores = ErrorCollector<Program>.GetErrors();
        if (errores.Any())
        {
            Console.WriteLine($"Se capturaron {errores.Count} errores:");
            foreach (var error in errores)
            {
                Console.WriteLine(error.ToString());
            }
        }
    }
}

public class ProcesadorDatos
{
    private readonly ILogger _logger = GetStaticLogger<ProcesadorDatos>();
    
    public void ProcesarArchivos(string[] archivos)
    {
        foreach (var archivo in archivos)
        {
            try
            {
                _logger.LogInformation($"Procesando archivo: {archivo}");
                ProcesarArchivo(archivo);
            }
            catch (Exception ex)
            {
                ErrorCollector<ProcesadorDatos>.AddError(
                    nameof(ProcesarArchivo), 
                    ex, 
                    $"Error procesando archivo: {archivo}"
                );
            }
        }
    }
    
    private void ProcesarArchivo(string archivo)
    {
        // Simular procesamiento
        if (!File.Exists(archivo))
            throw new FileNotFoundException($"Archivo no encontrado: {archivo}");
            
        // Lógica de procesamiento...
        _logger.LogDebug($"Archivo {archivo} procesado exitosamente");
    }
}
```

### Ejemplo 2: Aplicación web con inyección de dependencias

```csharp
// Program.cs
using velocist.LogService;

var builder = WebApplication.CreateBuilder(args);

// Configurar logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole()
                 .AddDebug()
                 .AddLog4Net("log4net.config", true)
                 .SetMinimumLevel(LogLevel.Information);
});

// Registrar servicios
builder.Services.AddScoped<IServicioDatos, ServicioDatos>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

// ServicioDatos.cs
public interface IServicioDatos
{
    Task<string> ObtenerDatosAsync(int id);
}

public class ServicioDatos : IServicioDatos
{
    private readonly ILogger<ServicioDatos> _logger;
    
    public ServicioDatos(ILogger<ServicioDatos> logger)
    {
        _logger = logger;
    }
    
    public async Task<string> ObtenerDatosAsync(int id)
    {
        try
        {
            _logger.LogInformation($"Obteniendo datos para ID: {id}");
            
            // Simular operación asíncrona
            await Task.Delay(100);
            
            if (id < 0)
                throw new ArgumentException("ID no puede ser negativo");
                
            return $"Datos para ID {id}";
        }
        catch (Exception ex)
        {
            ErrorCollector<ServicioDatos>.AddError(
                nameof(ObtenerDatosAsync), 
                ex, 
                $"Error obteniendo datos para ID: {id}"
            );
            throw;
        }
    }
}
```

---

## 📚 API Reference<a name="api-reference"></a>

### StaticLoggerFactory

| Método | Descripción |
|--------|-------------|
| `InitializeLog(ILoggerFactory)` | Inicializa la factory de loggers |
| `GetStaticLogger<T>()` | Obtiene un logger para el tipo T |
| `InitializeLoggerFactory<T>(...)` | Inicializa la factory automáticamente |

### ErrorCollector<T>

| Método | Descripción |
|--------|-------------|
| `AddError(string, Exception, string)` | Agrega un error a la colección |
| `GetErrors()` | Obtiene todos los errores como lista de solo lectura |
| `Clear()` | Limpia todos los errores |
| `ErrorCaptured` | Evento que se dispara cuando se captura un error |

### ErrorCollector (no genérico)

| Método | Descripción |
|--------|-------------|
| `AddError(string, string, Exception, string)` | Agrega un error especificando clase y método |
| `GetErrors()` | Obtiene todos los errores del sistema |
| `Clear()` | Limpia todos los errores del sistema |
| `ErrorCaptured` | Evento global de errores |

### ErrorInfo

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| `Timestamp` | DateTime | Momento en que se capturó el error |
| `ClassName` | string | Nombre de la clase donde ocurrió el error |
| `MethodName` | string | Nombre del método donde ocurrió el error |
| `Exception` | Exception | Excepción asociada al error |
| `Message` | string | Mensaje del error |

---

## 📝 Licencia<a name="licencia"></a>

Este proyecto está licenciado bajo la **GNU Lesser General Public License v3.0 (LGPL-3.0)**. Consulta el archivo [LICENSE.txt](./LICENSE.txt) para más detalles.

---

## 👤 Autor<a name="autor"></a>

**velocist**

¿Dudas o sugerencias? Abre un issue o revisa la documentación XML en el código fuente para más detalles.