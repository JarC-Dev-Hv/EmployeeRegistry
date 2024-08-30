# Descripción General

EmployeeRegistry es una pequeña API. Los requisitos de la prueba incluían:

1. **API Rest:**
    - Construir una API Rest basada en el modelo de datos. Se aprecia la separación de la capa de acceso a datos de la capa de servicios.
    - Para la búsqueda, uno por el id y otro para búsqueda por cualquiera de los campos primer nombre y primer apellido. En este último se debe listar de forma paginada de forma que por parámetros se defina el número de página y la cantidad de registros a traer por página.

La API backend está construida con ASP.NET Core y sigue buenas prácticas de desarrollo para asegurar que el código sea mantenible, escalable y de alta calidad.

## Requisitos de la Prueba

Para la siguiente prueba tenga en cuenta lo siguiente:
1. Enviar la URL del repositorio público (GIT) donde queda alojada la prueba para poder descargarla.
2. El resultado de la prueba debe ser una solución en Visual Studio.
3. Para base de datos puede utilizar una base de datos SQL (SqlServer, PostgreSql, MariaDB).
4. Se debe utilizar .Net 8 para realizar la prueba.
5. Pruebas unitarias y de integración (opcionales).

Para la prueba considere el siguiente caso:
Se necesita tener un API REST que permita realizar la creación, edición, eliminación y búsqueda de usuarios, para los cuales se debe almacenar la siguiente información:
1. Primer nombre: max 50 caracteres, no se permiten números, obligatorio.
2. Segundo nombre: max 50 caracteres, no se permiten números, opcional.
3. Primer apellido: max 50 caracteres, no se permiten números, obligatorio.
4. Segundo apellido: max 50 caracteres, no se permiten números, opcional.
5. Fecha de nacimiento: obligatoria.
6. Sueldo: obligatorio, no debe ser 0.
7. Se debe guardar la fecha de creación y la fecha de modificación correspondiente.

Se deben crear los endpoints correspondientes para todas las operaciones (CRUD), y para la búsqueda, uno por el id y otro para búsqueda por cualquiera de los campos primer nombre y primer apellido. En este último se debe listar de forma paginada de forma que por parámetros se defina el número de página y la cantidad de registros a traer por página.

La idea es que al ejecutar la aplicación con solo configurar la cadena de conexión a la base de datos en el archivo de configuración correspondiente se cree la base de datos con la tabla donde se va a almacenar la información y se pueda probar el funcionamiento de los servicios REST correspondientes por medio de Swagger.

## Prerrequisitos

Antes de comenzar, asegúrate de tener lo siguiente instalado en tu máquina:

- [.NET 8 SDK](https://dotnet.microsoft.com/es-es/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (recomendado para desarrollo)

## Instalación

1. **Clonar el repositorio:**

    ```sh
    git clone https://github.com/
    cd EmployeeRegistry
    ```

2. **Restaurar dependencias:**

    - **Desde la línea de comandos:**

        ```sh
        cd EmployeeRegistry.API
        dotnet restore
        ```

    - **Desde Visual Studio:**
        - Abre la solución [`EmployeeRegistry.sln`] en Visual Studio.
        - Haz clic derecho en la solución en el Explorador de Soluciones y selecciona **"Restaurar paquetes NuGet"**.

## Configuración de la Base de Datos

Se utilizó un enfoque Code First con Entity Framework Core y SQL Server como base de datos.

1. **Configurar la cadena de conexión:**

    La cadena de conexión se encuentra configurada en el archivo [`appsettings.json`] de la API:

    ```json
    "ConnectionStrings": {
        "EmployeeRegistryConnection": "Server=tcp:localhost,1455;Initial Catalog=EmployeeRegistry;Persist Security Info=False;User ID=sa;Password=******;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"
    }
    ```

2. **Aplicar migraciones y crear la base de datos:**

    - **Desde la línea de comandos:**

        ```sh
        cd EmployeeRegistry.API
        dotnet ef migrations add InitialCreate
        dotnet ef database update
        ```

    - **Desde Visual Studio:**
        - Abre la Consola del Administrador de Paquetes y ejecuta los siguientes comandos:

            ```powershell
            Add-Migration InitialCreate
            Update-Database
            ```

## Ejecución del Proyecto

1. **Iniciar la API:**

    - **Desde Visual Studio:** Selecciona [`EmployeeRegistry.API`] como proyecto de inicio y ejecuta la aplicación.
    - **Desde la línea de comandos:**

        ```sh
        cd EmployeeRegistry.API
        dotnet run
        ```

2. **Acceso a la documentación de la API:**

    - Una vez la API esté en ejecución, la documentación de Swagger estará disponible en [`https://localhost:7121/swagger/index.html`].

## Buenas Prácticas de Desarrollo

En la implementación de esta solución, se han seguido las siguientes buenas prácticas:

1. **Patrones de Diseño:**
    - Uso del patrón MVC en la API.
    - Inyección de dependencias para modularidad y testabilidad.

2. **Automapper:**
    - Configuración de AutoMapper para mapear entre entidades y DTOs.
    - Configuración de perfiles de mapeo en [`MappingProfile`].

3. **FluentValidation:**
    - Validación de DTOs con FluentValidation.
    - Ejemplo: Validación de [`EmployeeInsertDto`], [`EmployeeUpdateDto`] y [`EmployeeSearchDto`].

4. **Controladores:**
    - Manejo adecuado de solicitudes HTTP y excepciones en los controladores.

5. **Modelos y DTOs:**
    - Separación clara entre modelos de dominio y DTOs para mantener la arquitectura limpia.

6. **Repositorio:**
    - Patrón de repositorio implementado para abstracción del acceso a la base de datos.

7. **Servicios:**
    - Servicios para encapsular la lógica de negocio, como [`EmployeeService`].

8. **Seguridad:**
    - Prácticas de seguridad implementadas, como validación de entradas y protección contra inyección SQL.

9. **Optimización y Rendimiento:**
    - Técnicas de optimización, incluyendo caché y optimización de consultas.

Estas prácticas aseguran que el código sea mantenible, escalable y de alta calidad.