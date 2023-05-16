# Calculadora MVC
![Captura de la vista Home](/imagesGit/calc.png)

Este es un proyecto de una calculadora implementada utilizando el patrón de diseño Modelo-Vista-Controlador (MVC) en .NET Core 6. Proporciona una funcionalidad completa de cálculo y seguimiento de historial de operaciones para cada usuario. Además, se utiliza el sistema de autenticación y autorización de ASP.NET Core Identity para gestionar la creación de usuarios y las validaciones de formulario. 

## Características destacadas :computer:

- Calculadora científica.
- Historial de operaciones: se guarda un registro de las operaciones realizadas por cada usuario.
- Gestión de usuarios: permite la creación de cuentas de usuario con Identity, mediante registro y login.
- Validaciones de formulario: se aplican validaciones para garantizar la integridad de los datos ingresados.
- Eliminación de operaciones: los usuarios pueden eliminar operaciones individuales o varias a la vez.
- Página de perfil: se proporciona una página para que los usuarios pueden ver sus datos.
- Eliminación de cuenta: los usuarios tienen la opción de eliminar su cuenta por completo.

## Estructura del proyecto :thought_balloon:

El proyecto está organizado en las siguientes secciones:

- **Vistas**: he dividido las vistas en dos grupos para agrupar distintas funcionalidades. Las vistas relacionadas con la gestión de usuarios y las operaciones disponibles una vez que el usuario ha iniciado sesión se encuentran separadas de la página de inicio principal.
- **Controladores**: he separado los controladores para mejorar la legibilidad y mantener un código más limpio. Ayuda a gestionar de manera más eficient diferentes funcionalidades y pensar a futuro en posible escalabilidad de nuevas funciones.
- **Base de datos**: el proyecto se distribuye con una base de datos vacía. Esto significa que cuando se ejecuta por primera vez, no se encontrarán registros existentes y el sistema estará listo para su uso.

## Requisitos de instalación :open_file_folder:

Asegúrese de tener instalado lo siguiente antes de ejecutar el proyecto:

- .NET Core 6 SDK
- Un entorno de desarrollo integrado (IDE) compatible, como Visual Studio o Visual Studio Code.
- No olvides ejecutar `npm install` para instalar los paquetes de node.

## Licencia :scroll:

Este proyecto se distribuye bajo la licencia MIT. Para obtener más información, consulta el archivo [LICENSE](./LICENSE).




