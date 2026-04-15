🎯 Resolution Manager Pro

Resolution Manager es una utilidad avanzada para Windows diseñada para optimizar el rendimiento y la experiencia visual en juegos. Cambia automáticamente la resolución de pantalla y la frecuencia de actualización (Hz) al detectar procesos específicos, restaurando la configuración original al cerrar el juego o perder el foco.

🚀 Características Principales

Cambio Automático (Zero-Touch): Detecta cuando abres un juego y aplica la resolución/Hz configurados al instante.

🛡️ Modo Focus (Inteligente): Si activas esta opción, la resolución personalizada solo se mantendrá mientras el juego sea la ventana activa. Si haces Alt+Tab para ir al escritorio o navegador, la resolución se restaura automáticamente para tu comodidad.

⚡ Prioridad de Tiempo Real: El proceso se ejecuta con prioridad crítica en el sistema (RealTime Priority) para garantizar que la transición de resolución sea inmediata y sin lag.

🎨 Iconos de Estado Dinámicos: El icono en la bandeja del sistema (System Tray) cambia de color según el estado:

🔵 Azul: Sistema activo esperando un juego.

🟢 Verde: Juego detectado y resolución aplicada.

🟠 Naranja: Juego abierto pero minimizado (Resolución restaurada).

🔴 Rojo: Sistema pausado.

📉 Ultra Optimizado: Uso de RAM y CPU insignificante. Los iconos se generan en memoria y la lógica de monitoreo evita llamadas redundantes al driver de video.

🖥️ Soporte de Aspect Ratio: Selectores rápidos para ratios 4:3, 16:9, 16:10, 21:9 y 5:4 con las resoluciones más competitivas del mercado.

🛠️ Requisitos del Sistema

SO: Windows 10 o 11 (64 bits recomendado).

Framework: .NET Framework 4.7.2 o superior.

Permisos: Se recomienda ejecutar como Administrador para permitir el cambio de prioridad de proceso y la manipulación de la resolución sin restricciones.

📥 Instalación

Ve a la sección de Releases.

Descarga el archivo ResolutionManager.exe.

Colócalo en la carpeta de tu preferencia.

(Opcional) Crea un acceso directo en tu carpeta de Inicio para que arranque con Windows.

📖 Modo de Uso

Añadir un juego: Escribe el nombre del proceso (ej: cs2, Valorant, Overwatch) sin el .exe.

Configurar: Selecciona el Ratio de aspecto, la resolución deseada y los Hz de tu monitor.

Guardar: Haz clic en Agregar. El juego aparecerá en la lista y se guardará en config.json.

Editar: Haz clic sobre un juego en la lista para cargar sus datos, modifícalos y pulsa Actualizar.

Focus: Marca la casilla Modo Focus si quieres que la resolución solo cambie cuando estés dentro del juego.

🛠️ Tecnologías Utilizadas

Lenguaje: C#

Interfaz: WinForms con diseño Modern Dark.

API Nativa: Interoperabilidad con user32.dll para gestión de ventanas y configuraciones de pantalla.

Serialización: System.Text.Json para una configuración ligera.

📝 Contribuir

¿Tienes ideas para mejorar el escalado o la detección de procesos?

Haz un Fork del proyecto.

Crea una rama para tu mejora (git checkout -b feature/MejoraIncreible).

Envía un Pull Request.

👤 Autor

Desarrollado con ❤️ por Breniak.

¿Te sirvió el programa? ¡No olvides dejar una ⭐ en el repositorio!
