Resolution Manager

Aplicación ligera para Windows diseñada para optimizar la experiencia de juego mediante el cambio automático de resolución y frecuencia de actualización (Hz) al detectar procesos específicos.

Características Principales

Detección Automática: Cambia la configuración de pantalla al iniciar un juego y la restaura instantáneamente al cerrarlo.

Control de Frecuencia (Hz): Permite definir los Hz específicos para cada juego, evitando que el sistema regrese a los 60Hz por defecto.

Forzado de Señal Física: Implementa lógica para intentar forzar el modo nativo del monitor, buscando reducir el escalado por software (GPU Scaling).

Gestión Inteligente: Soporta nombres de procesos con espacios y permite tener múltiples configuraciones guardadas.

Modo Segundo Plano: Se minimiza a la bandeja del sistema (System Tray) para no interferir con el escritorio.

Interruptor de Control: Botón de Activar/Desactivar para pausar el monitoreo sin cerrar la aplicación.

Requisitos

Windows 10 / 11.

.NET Framework 4.7.2 o superior.

Permisos de Administrador: Necesarios para interactuar con las APIs de cambio de resolución de Windows (user32.dll).

Instalación

Descarga el ejecutable desde la sección Releases.

Ejecuta la aplicación como administrador.

Uso

Agregar Juego: Escribe el nombre del proceso (ej: VALORANT-Win64-Shipping o Risk of Rain 2) sin la extensión .exe.

Configurar Pantalla: Define el ancho, alto y los Hz deseados.

Guardar: Haz clic en "Agregar". La configuración se guardará automáticamente en un archivo config.json.

Minimizar: Al minimizar la ventana, el programa seguirá trabajando desde la barra de tareas.

Restauración: Al cerrar el juego, el monitor volverá a su configuración original de escritorio.

Notas Técnicas

El programa utiliza EnumDisplaySettings para encontrar modos compatibles reportados por el driver de video.

Para evitar el escalado, se intenta aplicar el flag DMDFO_DEFAULT en la estructura DEVMODE.

Si una resolución/frecuencia no es soportada por el monitor, el cambio no se aplicará para proteger el hardware.

Contribuir

Si encuentras algún error o tienes ideas para mejorar el forzado de señal física, ¡siéntete libre de abrir un issue o enviar un pull request!

Autor

Desarrollado por Breniak.
