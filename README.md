🎯 Resolution Manager Pro

Resolution Manager Pro es una utilidad avanzada y ultraligera para Windows diseñada para optimizar el rendimiento y la experiencia visual de tus juegos. Cambia automáticamente la resolución de pantalla y la frecuencia de actualización (Hz) al detectar procesos específicos, restaurando la configuración original de tu escritorio al cerrar el juego o perder el foco.

🚀 Características Principales

Cambio Automático (Zero-Touch): Detecta cuando abres un juego y aplica la resolución/Hz configurados al instante.

🛡️ Modo Focus (Inteligente): Si activas esta opción, la resolución personalizada solo se mantendrá mientras el juego sea la ventana activa. Si haces Alt+Tab para ir al escritorio o navegador, la resolución se restaura automáticamente para tu comodidad.

🎨 Nueva Interfaz Minimalista: Diseño compacto (sin bordes de Windows) en elegantes tonos oscuros con acentos rojos. Diseñado para no deslumbrar y ser fácilmente manejable en cualquier resolución de pantalla, con navegación rápida por pestañas.

⚡ Prioridad de Tiempo Real: El proceso se ejecuta con prioridad crítica en el sistema (RealTime Priority) para garantizar que la transición de resolución sea inmediata y sin generar lag.

🚥 Iconos de Estado Dinámicos: El icono en la bandeja del sistema (System Tray) cambia de color según el estado:

🔵 Azul: Sistema activo esperando un juego.

🟢 Verde: Juego detectado y resolución aplicada.

🟠 Naranja: Juego abierto pero minimizado (Resolución de escritorio restaurada).

🔴 Rojo: Sistema pausado manualmente.

🥷 Modo Sigilo y Arranque Fiable: Opción para iniciar con Windows (vía Registro para máxima fiabilidad) y arrancar directamente minimizado en la bandeja del sistema sin parpadeos visuales en pantalla.

📌 Siempre Encima (TopMost): Nueva opción para mantener el panel de control sobre el resto de ventanas mientras configuras tus juegos.

📉 Ultra Optimizado y Portable: Un único archivo .exe que no requiere instalación. Uso de RAM y CPU insignificante, generando sus iconos directamente en memoria y evitando llamadas redundantes al driver de video.

🖥️ Soporte de Aspect Ratio: Selectores rápidos para ratios 4:3, 16:9, 16:10, 21:9 y 5:4 con las resoluciones más competitivas del mercado (o modo completamente manual).

🛠️ Requisitos del Sistema

SO: Windows 10 o Windows 11.

Framework: .NET Framework 4.7.2 o superior.

Permisos: Se recomienda ejecutar como Administrador para permitir el cambio de prioridad del proceso y la modificación de claves del registro (para el inicio automático).

📥 Instalación

Ve a la sección de Releases.

Descarga el archivo ResolutionManager.exe. ¡Es un único archivo portable, sin dependencias extrañas!

Colócalo en la carpeta de tu preferencia y ejecútalo.

📖 Modo de Uso

Pestaña de Juegos (🎮)

Añadir un juego: Escribe el nombre del proceso (ej: cs2, Valorant, Overwatch) sin el .exe.

Configurar: Selecciona el Ratio de aspecto, la resolución deseada y los Hz de tu monitor.

Guardar: Haz clic en Agregar. El juego aparecerá en la lista y se guardará automáticamente.

Editar: Haz clic sobre un juego en la lista para cargar sus datos, modifícalos y pulsa Actualizar. Para cancelar la edición, simplemente haz clic en un espacio vacío.

Pestaña de Ajustes (⚙️)

Aquí puedes activar/desactivar el Modo Focus, obligar a que la ventana se mantenga Siempre Encima, y configurar el comportamiento de arranque (Iniciar con Windows e Iniciar Minimizado).

🛠️ Tecnologías Utilizadas

Lenguaje: C#

Interfaz: WinForms con diseño UI personalizado (GDI+ Custom Painting).

API Nativa: Interoperabilidad con user32.dll (gestión de ventanas y pantallas) y advapi32.dll (Registro de Windows).

Empaquetado: Costura.Fody para incrustación de librerías en un único .exe portable.

Serialización: System.Text.Json para una configuración estructurada ultraligera.

📝 Contribuir

¿Tienes ideas para mejorar el escalado o la detección de procesos?

Haz un Fork del proyecto.

Crea una rama para tu mejora (git checkout -b feature/MejoraIncreible).

Envía un Pull Request.

👤 Autor

Desarrollado con ❤️ por Breniak.

¿Te sirvió el programa para mejorar tus FPS o comodidad? ¡No olvides dejar una ⭐ en el repositorio!
