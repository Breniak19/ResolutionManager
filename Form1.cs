using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.IO;

namespace ResolutionManager
{
    public partial class Form1 : Form
    {
        private bool programaActivo = true;
        private ToolTip toolTip;

        // --- APIs DE WINDOWS PARA RESOLUCIÓN Y FOCUS ---
        [DllImport("user32.dll")]
        private static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
        [DllImport("user32.dll")]
        private static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private const int ENUM_CURRENT_SETTINGS = -1;
        private const int CDS_UPDATEREGISTRY = 0x01;
        private const int DISP_CHANGE_SUCCESSFUL = 0;

        private const int DM_PELSWIDTH = 0x00080000;
        private const int DM_PELSHEIGHT = 0x00100000;
        private const int DM_DISPLAYFREQUENCY = 0x00400000;
        private const int DM_DISPLAYFIXEDOUTPUT = 0x20000000;
        private const int DMDFO_DEFAULT = 0;

        [StructLayout(LayoutKind.Sequential)]
        private struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public int dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        private DEVMODE originalMode;
        private Timer monitorTimer;
        private Dictionary<string, GameConfig> procesosMonitoreados;
        private const string ConfigFile = "config.json";
        private const string SettingsFile = "settings.json";

        private string procesoActualActivo = "";
        private bool resolucionActualModificada = false; // Optimización de RAM y CPU

        // --- ICONOS DINÁMICOS EN MEMORIA ---
        private Icon iconEstado1_Idle;
        private Icon iconEstado2_Activo;
        private Icon iconEstado3_FocoPerdido;
        private Icon iconEstado0_Desactivado;

        private Dictionary<string, List<string>> resolucionesPorAspecto = new Dictionary<string, List<string>>()
        {
            { "4:3", new List<string> { "2048x1536", "1920x1440", "1600x1200", "1440x1080", "1400x1050", "1280x960", "1152x864", "1024x768", "960x720", "800x600", "640x480", "480x360", "320x240" } },
            { "16:9", new List<string> { "3840x2160", "2560x1440", "1920x1080", "1600x900", "1536x864", "1366x768", "1280x720", "1152x648", "1024x576", "960x540", "854x480", "640x360", "426x240" } },
            { "16:10", new List<string> { "1920x1200", "1680x1050", "1600x1000", "1440x900", "1280x800", "1152x720", "1024x640", "960x600", "896x560", "800x500", "768x480", "640x400", "512x320", "320x200" } },
            { "21:9", new List<string> { "5120x2160", "3840x1600", "3440x1440", "2560x1080", "1280x540" } },
            { "5:4", new List<string> { "1280x1024", "1152x921", "1024x819", "800x640", "720x576", "640x512" } }
        };

        public Form1()
        {
            EstablecerPrioridadTiempoReal(); // Asignar máxima prioridad de CPU al iniciar
            GenerarIconosOptimizados(); // Generar en memoria para no usar archivos locales

            InitializeComponent();
            ApplyModernTheme();
            SetupControls();
            ConfigureTrayIcon();

            procesosMonitoreados = new Dictionary<string, GameConfig>();
            monitorTimer = new Timer { Interval = 1500 }; // Más rápido ya que está muy optimizado
            monitorTimer.Tick += MonitorTimer_Tick;

            LoadConfig();

            originalMode = new DEVMODE();
            originalMode.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref originalMode);
        }

        private void EstablecerPrioridadTiempoReal()
        {
            try
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    p.PriorityClass = ProcessPriorityClass.RealTime; // Prioridad Máxima
                }
            }
            catch
            {
                try
                {
                    using (Process p = Process.GetCurrentProcess())
                    {
                        p.PriorityClass = ProcessPriorityClass.High; // Fallback
                    }
                }
                catch { } // Silencioso si falta permisos de admin
            }
        }

        private void GenerarIconosOptimizados()
        {
            // Creamos los iconos una vez y los guardamos en memoria para no llenar la RAM
            iconEstado1_Idle = CrearIconoCirculo(Color.DodgerBlue); // Esperando
            iconEstado2_Activo = CrearIconoCirculo(Color.LimeGreen); // Focus/Cambiado
            iconEstado3_FocoPerdido = CrearIconoCirculo(Color.Orange); // Minimizaste
            iconEstado0_Desactivado = CrearIconoCirculo(Color.Red); // Pausado
        }

        private Icon CrearIconoCirculo(Color color)
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                using (Brush b = new SolidBrush(color))
                {
                    g.FillEllipse(b, 2, 2, 12, 12);
                }
                using (Pen p = new Pen(Color.White, 1))
                {
                    g.DrawEllipse(p, 2, 2, 12, 12);
                }
            }
            return Icon.FromHandle(bmp.GetHicon());
        }

        private void ActualizarIconoBandeja(int estado)
        {
            if (!programaActivo)
            {
                if (trayIcon.Icon != iconEstado0_Desactivado)
                {
                    trayIcon.Icon = iconEstado0_Desactivado;
                    trayIcon.Text = "Resolution Manager - Pausado";
                }
                return;
            }

            switch (estado)
            {
                case 1:
                    if (trayIcon.Icon != iconEstado1_Idle)
                    {
                        trayIcon.Icon = iconEstado1_Idle;
                        trayIcon.Text = "Esperando juego...";
                    }
                    break;
                case 2:
                    if (trayIcon.Icon != iconEstado2_Activo)
                    {
                        trayIcon.Icon = iconEstado2_Activo;
                        trayIcon.Text = "¡Juego en Focus! Resolución aplicada.";
                    }
                    break;
                case 3:
                    if (trayIcon.Icon != iconEstado3_FocoPerdido)
                    {
                        trayIcon.Icon = iconEstado3_FocoPerdido;
                        trayIcon.Text = "Juego minimizado (Resolución de escritorio).";
                    }
                    break;
            }
        }

        private void ApplyModernTheme()
        {
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.BackColor = Color.FromArgb(28, 28, 28);
            this.ForeColor = Color.White;

            toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            toolTip.SetToolTip(btnAgregar, "Guarda o actualiza la configuración.");
            toolTip.SetToolTip(btnActivarDesactivar, "Pausa o reanuda el sistema.");
            toolTip.SetToolTip(chkFocusMode, "Si sales de la ventana del juego (Alt+Tab), la resolución volverá a la normalidad automáticamente.");
        }

        private void SetupControls()
        {
            numAncho.Minimum = 320; numAncho.Maximum = 7680; numAncho.Value = 1920;
            numAlto.Minimum = 200; numAlto.Maximum = 4320; numAlto.Value = 1080;
            numHz.Minimum = 30; numHz.Maximum = 500; numHz.Value = 60;

            cmbAspectRatio.Items.Clear();
            cmbAspectRatio.Items.Add("Ninguna (Manual)");
            foreach (var key in resolucionesPorAspecto.Keys) cmbAspectRatio.Items.Add(key);
            cmbAspectRatio.SelectedIndex = 0;

            lstProcesos.SelectedIndexChanged += lstProcesos_SelectedIndexChanged;
        }

        private void LimpiarCampos()
        {
            lstProcesos.SelectedIndexChanged -= lstProcesos_SelectedIndexChanged;
            lstProcesos.SelectedIndex = -1;
            lstProcesos.SelectedIndexChanged += lstProcesos_SelectedIndexChanged;

            txtNombreProceso.Text = "proceso";
            txtNombreProceso.ForeColor = Color.Gray;

            cmbAspectRatio.SelectedIndex = 0;
            numAncho.Value = 1920;
            numAlto.Value = 1080;
            numHz.Value = 60;

            btnAgregar.Text = "Agregar";
            btnAgregar.ForeColor = Color.Lime;
            this.ActiveControl = null;
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void lstProcesos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstProcesos.SelectedIndex == -1)
            {
                LimpiarCampos();
                return;
            }

            string selectedText = lstProcesos.SelectedItem.ToString();
            int lastParen = selectedText.LastIndexOf(" (");
            string nombreProceso = lastParen != -1 ? selectedText.Substring(0, lastParen) : selectedText;

            if (procesosMonitoreados.ContainsKey(nombreProceso))
            {
                var config = procesosMonitoreados[nombreProceso];
                txtNombreProceso.Text = nombreProceso;
                txtNombreProceso.ForeColor = Color.White;

                cmbAspectRatio.SelectedIndex = 0;
                numAncho.Value = config.Width;
                numAlto.Value = config.Height;
                numHz.Value = config.RefreshRate;

                btnAgregar.Text = "Actualizar";
                btnAgregar.ForeColor = Color.Cyan;
            }
        }

        private void cmbAspectRatio_SelectedIndexChanged(object sender, EventArgs e)
        {
            string seleccionado = cmbAspectRatio.SelectedItem.ToString();
            cmbResoluciones.Items.Clear();

            if (seleccionado.StartsWith("Ninguna"))
            {
                cmbResoluciones.Enabled = false;
                numAncho.Enabled = true;
                numAlto.Enabled = true;
            }
            else
            {
                cmbResoluciones.Enabled = true;
                numAncho.Enabled = false;
                numAlto.Enabled = false;
                foreach (var res in resolucionesPorAspecto[seleccionado])
                {
                    cmbResoluciones.Items.Add(res);
                }
                if (cmbResoluciones.Items.Count > 0) cmbResoluciones.SelectedIndex = 0;
            }
        }

        private void cmbResoluciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbResoluciones.SelectedIndex == -1) return;
            string[] partes = cmbResoluciones.SelectedItem.ToString().Split('x');
            if (partes.Length == 2)
            {
                numAncho.Value = int.Parse(partes[0]);
                numAlto.Value = int.Parse(partes[1]);
            }
        }

        private void ConfigureTrayIcon()
        {
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Mostrar", null, (s, e) => ShowMainWindow());
            trayMenu.Items.Add(new ToolStripSeparator());
            trayMenu.Items.Add("Salir", null, (s, e) => ExitApplication());

            trayIcon.Icon = iconEstado1_Idle;
            trayIcon.Text = "Resolution Manager Pro";
            trayIcon.ContextMenuStrip = trayMenu;
            trayIcon.Visible = true;
            trayIcon.DoubleClick += (s, e) => ShowMainWindow();
        }

        private void ShowMainWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void ExitApplication()
        {
            RestaurarResolucionOriginal();
            SaveConfig();
            trayIcon.Visible = false;
            Application.Exit();
        }

        private void LoadConfig()
        {
            try
            {
                // Cargar Configuración de Juegos
                if (File.Exists(ConfigFile))
                {
                    string json = File.ReadAllText(ConfigFile);
                    procesosMonitoreados = JsonSerializer.Deserialize<Dictionary<string, GameConfig>>(json) ?? new Dictionary<string, GameConfig>();
                    ActualizarListaVisual();
                    if (procesosMonitoreados.Count > 0) monitorTimer.Start();
                }

                // Cargar Configuración del Botón Focus
                if (File.Exists(SettingsFile))
                {
                    string txt = File.ReadAllText(SettingsFile);
                    chkFocusMode.Checked = txt.Contains("true");
                }
            }
            catch { procesosMonitoreados = new Dictionary<string, GameConfig>(); }
        }

        private void SaveConfig()
        {
            try
            {
                string json = JsonSerializer.Serialize(procesosMonitoreados);
                File.WriteAllText(ConfigFile, json);

                // Guardar preferencia del Botón Focus
                File.WriteAllText(SettingsFile, chkFocusMode.Checked ? "true" : "false");
            }
            catch (Exception ex) { Debug.WriteLine("Error al guardar: " + ex.Message); }
        }

        private void ActualizarListaVisual()
        {
            lstProcesos.Items.Clear();
            foreach (var item in procesosMonitoreados)
            {
                lstProcesos.Items.Add($"{item.Key} ({item.Value.Width}x{item.Value.Height} @ {item.Value.RefreshRate}Hz)");
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.ShowInTaskbar = false;
            }
        }

        // --- NÚCLEO OPTIMIZADO (Sin fugas de RAM) ---
        private void MonitorTimer_Tick(object sender, EventArgs e)
        {
            if (!programaActivo)
            {
                ActualizarIconoBandeja(0);
                return;
            }

            string procesoDetectado = "";
            bool juegoEnFocus = false;

            // Obtener la ventana que el usuario está viendo actualmente
            IntPtr ventanaActual = GetForegroundWindow();
            GetWindowThreadProcessId(ventanaActual, out uint processIdFoco);

            foreach (var proceso in procesosMonitoreados.Keys)
            {
                Process[] procesos = Process.GetProcessesByName(proceso);
                if (procesos.Length > 0)
                {
                    procesoDetectado = proceso;

                    // Verificar si ese juego es el que tiene el Focus
                    foreach (var p in procesos)
                    {
                        if (p.Id == processIdFoco)
                        {
                            juegoEnFocus = true;
                            break;
                        }
                    }
                    break;
                }
            }

            bool modoFocusActivado = chkFocusMode.Checked;
            bool debeAplicarResolucion = !string.IsNullOrEmpty(procesoDetectado) && (!modoFocusActivado || juegoEnFocus);

            if (debeAplicarResolucion)
            {
                // ESTADO 2: Activo (o Focus)
                if (!resolucionActualModificada || procesoActualActivo != procesoDetectado)
                {
                    var config = procesosMonitoreados[procesoDetectado];
                    CambiarResolucion(config.Width, config.Height, config.RefreshRate);
                    procesoActualActivo = procesoDetectado;
                    resolucionActualModificada = true;
                }
                ActualizarIconoBandeja(2);
            }
            else if (!string.IsNullOrEmpty(procesoDetectado))
            {
                // ESTADO 3: Juego Abierto, pero perdiste el Focus
                if (resolucionActualModificada)
                {
                    RestaurarResolucionOriginal();
                    resolucionActualModificada = false;
                    // Mantenemos procesoActualActivo para no olvidar qué juego es
                }
                ActualizarIconoBandeja(3);
            }
            else
            {
                // ESTADO 1: Nada abierto
                if (resolucionActualModificada)
                {
                    RestaurarResolucionOriginal();
                    resolucionActualModificada = false;
                    procesoActualActivo = "";
                }
                ActualizarIconoBandeja(1);
            }
        }

        private void CambiarResolucion(int ancho, int alto, int hz)
        {
            try
            {
                DEVMODE dm = new DEVMODE();
                dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
                bool modoEncontrado = false;

                for (int i = 0; EnumDisplaySettings(null, i, ref dm); i++)
                {
                    if (dm.dmPelsWidth == ancho && dm.dmPelsHeight == alto && dm.dmDisplayFrequency == hz)
                    {
                        modoEncontrado = true;
                        break;
                    }
                }

                if (modoEncontrado)
                {
                    dm.dmFields |= DM_PELSWIDTH | DM_PELSHEIGHT | DM_DISPLAYFREQUENCY | DM_DISPLAYFIXEDOUTPUT;
                    dm.dmDisplayFixedOutput = DMDFO_DEFAULT;

                    int result = ChangeDisplaySettings(ref dm, CDS_UPDATEREGISTRY);
                    if (result != DISP_CHANGE_SUCCESSFUL)
                    {
                        dm.dmFields = DM_PELSWIDTH | DM_PELSHEIGHT | DM_DISPLAYFREQUENCY;
                        ChangeDisplaySettings(ref dm, CDS_UPDATEREGISTRY);
                    }
                }
                else
                {
                    DEVMODE dmManual = new DEVMODE();
                    dmManual.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
                    EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref dmManual);
                    dmManual.dmPelsWidth = ancho;
                    dmManual.dmPelsHeight = alto;
                    dmManual.dmDisplayFrequency = hz;
                    dmManual.dmFields = DM_PELSWIDTH | DM_PELSHEIGHT | DM_DISPLAYFREQUENCY | DM_DISPLAYFIXEDOUTPUT;
                    dmManual.dmDisplayFixedOutput = DMDFO_DEFAULT;
                    ChangeDisplaySettings(ref dmManual, CDS_UPDATEREGISTRY);
                }
            }
            catch (Exception ex) { Debug.WriteLine($"Error: {ex.Message}"); }
        }

        private void RestaurarResolucionOriginal()
        {
            try { ChangeDisplaySettings(ref originalMode, CDS_UPDATEREGISTRY); }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombreProceso.Text.Trim();
            if (string.IsNullOrEmpty(nombre) || nombre == "proceso") return;

            var config = new GameConfig
            {
                Width = (int)numAncho.Value,
                Height = (int)numAlto.Value,
                RefreshRate = (int)numHz.Value
            };

            if (procesosMonitoreados.ContainsKey(nombre))
            {
                procesosMonitoreados[nombre] = config;
            }
            else
            {
                procesosMonitoreados.Add(nombre, config);
            }

            ActualizarListaVisual();
            SaveConfig();

            if (!monitorTimer.Enabled) monitorTimer.Start();

            if (procesoActualActivo == nombre && (!chkFocusMode.Checked))
            {
                CambiarResolucion(config.Width, config.Height, config.RefreshRate);
            }

            LimpiarCampos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (lstProcesos.SelectedIndex == -1) return;
            string selectedText = lstProcesos.SelectedItem.ToString();
            int lastParen = selectedText.LastIndexOf(" (");
            string nombreProceso = lastParen != -1 ? selectedText.Substring(0, lastParen) : selectedText;

            if (procesosMonitoreados.ContainsKey(nombreProceso))
            {
                procesosMonitoreados.Remove(nombreProceso);
                ActualizarListaVisual();
                SaveConfig();

                LimpiarCampos();

                if (procesosMonitoreados.Count == 0)
                {
                    monitorTimer.Stop();
                    RestaurarResolucionOriginal();
                    procesoActualActivo = "";
                }
            }
        }

        private void btnActivarDesactivar_Click(object sender, EventArgs e)
        {
            programaActivo = !programaActivo;
            btnActivarDesactivar.Text = programaActivo ? "Sistema Activo" : "Sistema Pausado";
            btnActivarDesactivar.BackColor = programaActivo ? Color.FromArgb(46, 139, 87) : Color.FromArgb(178, 34, 34);
            procesoActualActivo = "";

            if (!programaActivo)
            {
                RestaurarResolucionOriginal();
                ActualizarIconoBandeja(0);
            }
        }

        private void chkFocusMode_CheckedChanged(object sender, EventArgs e)
        {
            SaveConfig(); // Guardamos el estado al vuelo
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                return;
            }
            RestaurarResolucionOriginal();
        }

        private void txtNombreProceso_Enter(object sender, EventArgs e)
        {
            if (txtNombreProceso.Text == "proceso") { txtNombreProceso.Text = ""; txtNombreProceso.ForeColor = Color.White; }
        }

        private void txtNombreProceso_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreProceso.Text)) { txtNombreProceso.Text = "proceso"; txtNombreProceso.ForeColor = Color.Gray; }
        }
    }

    public class GameConfig
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int RefreshRate { get; set; }
    }
}