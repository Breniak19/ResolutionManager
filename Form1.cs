using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.IO;
using Microsoft.Win32; // Necesario para Iniciar con Windows

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

        // --- API PARA MOVER LA VENTANA SIN BORDES ---
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

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
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string dmDeviceName;
            public short dmSpecVersion; public short dmDriverVersion; public short dmSize;
            public short dmDriverExtra; public int dmFields; public int dmPositionX;
            public int dmPositionY; public int dmDisplayOrientation; public int dmDisplayFixedOutput;
            public short dmColor; public short dmDuplex; public short dmYResolution;
            public short dmTTOption; public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string dmFormName;
            public short dmLogPixels; public int dmBitsPerPel; public int dmPelsWidth;
            public int dmPelsHeight; public int dmDisplayFlags; public int dmDisplayFrequency;
            public int dmICMMethod; public int dmICMIntent; public int dmMediaType;
            public int dmDitherType; public int dmReserved1; public int dmReserved2;
            public int dmPanningWidth; public int dmPanningHeight;
        }

        private DEVMODE originalMode;
        private Timer monitorTimer;
        private Dictionary<string, GameConfig> procesosMonitoreados;
        private AppSettings configuracionApp; // Nueva clase para ajustes globales

        private const string ConfigFile = "config.json";
        private const string SettingsFile = "appsettings.json";

        private string procesoActualActivo = "";
        private bool resolucionActualModificada = false;
        private bool isFirstShow = true; // Para el inicio minimizado sigiloso

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
            EstablecerPrioridadTiempoReal();
            GenerarIconosOptimizados();

            InitializeComponent();
            SetupCustomUI();
            SetupControls();
            ConfigureTrayIcon();

            procesosMonitoreados = new Dictionary<string, GameConfig>();
            configuracionApp = new AppSettings();

            monitorTimer = new Timer { Interval = 1500 };
            monitorTimer.Tick += MonitorTimer_Tick;

            LoadConfig();

            originalMode = new DEVMODE();
            originalMode.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref originalMode);
        }

        // --- TRUCO PARA INICIAR MINIMIZADO SIN PARPADEOS ---
        protected override void SetVisibleCore(bool value)
        {
            if (isFirstShow && configuracionApp != null && configuracionApp.StartMinimized)
            {
                value = false;
                if (!this.IsHandleCreated) CreateHandle();
            }
            base.SetVisibleCore(value);
            isFirstShow = false;
        }

        private void EstablecerPrioridadTiempoReal()
        {
            try
            {
                using (Process p = Process.GetCurrentProcess()) { p.PriorityClass = ProcessPriorityClass.RealTime; }
            }
            catch
            {
                try { using (Process p = Process.GetCurrentProcess()) { p.PriorityClass = ProcessPriorityClass.High; } } catch { }
            }
        }

        private void GenerarIconosOptimizados()
        {
            iconEstado1_Idle = CrearIconoCirculo(Color.DodgerBlue);
            iconEstado2_Activo = CrearIconoCirculo(Color.LimeGreen);
            iconEstado3_FocoPerdido = CrearIconoCirculo(Color.Orange);
            iconEstado0_Desactivado = CrearIconoCirculo(Color.FromArgb(229, 57, 53)); // Rojo Acento
        }

        private Icon CrearIconoCirculo(Color color)
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                using (Brush b = new SolidBrush(color)) { g.FillEllipse(b, 2, 2, 12, 12); }
                using (Pen p = new Pen(Color.White, 1)) { g.DrawEllipse(p, 2, 2, 12, 12); }
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
                        trayIcon.Text = "RM Pro: Esperando juego...";
                    }
                    break;
                case 2:
                    if (trayIcon.Icon != iconEstado2_Activo)
                    {
                        trayIcon.Icon = iconEstado2_Activo;
                        trayIcon.Text = "RM Pro: ¡Juego en Focus! Resolución aplicada.";
                    }
                    break;
                case 3:
                    if (trayIcon.Icon != iconEstado3_FocoPerdido)
                    {
                        trayIcon.Icon = iconEstado3_FocoPerdido;
                        trayIcon.Text = "RM Pro: Juego minimizado (Resolución normal).";
                    }
                    break;
            }
        }

        private void SetupCustomUI()
        {
            toolTip = new ToolTip { AutoPopDelay = 5000, InitialDelay = 500, ReshowDelay = 500, ShowAlways = true };
            toolTip.SetToolTip(btnAgregar, "Guarda o actualiza la configuración.");
            toolTip.SetToolTip(btnActivarDesactivar, "Pausa o reanuda el sistema global.");

            pnlTabGames.BringToFront(); // Pestaña inicial
            ActualizarBotonNavegacion(btnNavGames);
        }

        private void SetupControls()
        {
            numAncho.Minimum = 320; numAncho.Maximum = 7680; numAncho.Value = 1920;
            numAlto.Minimum = 200; numAlto.Maximum = 4320; numAlto.Value = 1080;
            numHz.Minimum = 30; numHz.Maximum = 500; numHz.Value = 60;

            cmbAspectRatio.Items.Clear();
            cmbAspectRatio.Items.Add("Manual");
            foreach (var key in resolucionesPorAspecto.Keys) cmbAspectRatio.Items.Add(key);
            cmbAspectRatio.SelectedIndex = 0;

            lstProcesos.SelectedIndexChanged += lstProcesos_SelectedIndexChanged;
        }

        // --- NAVEGACIÓN Y ARRASTRE DE VENTANA ---
        private void pnlTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); } // Cierra a la bandeja
        private void btnMinimize_Click(object sender, EventArgs e) { this.WindowState = FormWindowState.Minimized; }

        private void btnNavGames_Click(object sender, EventArgs e)
        {
            pnlTabGames.BringToFront();
            ActualizarBotonNavegacion(btnNavGames);
        }

        private void btnNavSettings_Click(object sender, EventArgs e)
        {
            pnlTabSettings.BringToFront();
            ActualizarBotonNavegacion(btnNavSettings);
        }

        private void ActualizarBotonNavegacion(Button btnActivo)
        {
            btnNavGames.BackColor = Color.FromArgb(15, 15, 19);
            btnNavSettings.BackColor = Color.FromArgb(15, 15, 19);
            btnNavGames.ForeColor = Color.Gray;
            btnNavSettings.ForeColor = Color.Gray;

            btnActivo.BackColor = Color.FromArgb(22, 22, 26);
            btnActivo.ForeColor = Color.White;
        }

        private void LimpiarCampos()
        {
            lstProcesos.SelectedIndexChanged -= lstProcesos_SelectedIndexChanged;
            lstProcesos.SelectedIndex = -1;
            lstProcesos.SelectedIndexChanged += lstProcesos_SelectedIndexChanged;

            txtNombreProceso.Text = "proceso";
            txtNombreProceso.ForeColor = Color.Gray;

            cmbAspectRatio.SelectedIndex = 0;
            numAncho.Value = 1920; numAlto.Value = 1080; numHz.Value = 60;

            btnAgregar.Text = "Agregar";
            btnAgregar.BackColor = Color.FromArgb(229, 57, 53); // Rojo Acento
            this.ActiveControl = null;
        }

        private void pnlContent_Click(object sender, EventArgs e) { LimpiarCampos(); }
        private void pnlTabGames_Click(object sender, EventArgs e) { LimpiarCampos(); }

        private void lstProcesos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstProcesos.SelectedIndex == -1) { LimpiarCampos(); return; }

            string selectedText = lstProcesos.SelectedItem.ToString();
            int lastParen = selectedText.LastIndexOf(" (");
            string nombreProceso = lastParen != -1 ? selectedText.Substring(0, lastParen) : selectedText;

            if (procesosMonitoreados.ContainsKey(nombreProceso))
            {
                var config = procesosMonitoreados[nombreProceso];
                txtNombreProceso.Text = nombreProceso;
                txtNombreProceso.ForeColor = Color.White;

                cmbAspectRatio.SelectedIndex = 0;
                numAncho.Value = config.Width; numAlto.Value = config.Height; numHz.Value = config.RefreshRate;

                btnAgregar.Text = "Actualizar";
                btnAgregar.BackColor = Color.FromArgb(43, 144, 217); // Azul para editar
            }
        }

        private void cmbAspectRatio_SelectedIndexChanged(object sender, EventArgs e)
        {
            string seleccionado = cmbAspectRatio.SelectedItem.ToString();
            cmbResoluciones.Items.Clear();

            if (seleccionado.StartsWith("Manual"))
            {
                cmbResoluciones.Enabled = false; numAncho.Enabled = true; numAlto.Enabled = true;
            }
            else
            {
                cmbResoluciones.Enabled = true; numAncho.Enabled = false; numAlto.Enabled = false;
                foreach (var res in resolucionesPorAspecto[seleccionado]) cmbResoluciones.Items.Add(res);
                if (cmbResoluciones.Items.Count > 0) cmbResoluciones.SelectedIndex = 0;
            }
        }

        private void cmbResoluciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbResoluciones.SelectedIndex == -1) return;
            string[] partes = cmbResoluciones.SelectedItem.ToString().Split('x');
            if (partes.Length == 2) { numAncho.Value = int.Parse(partes[0]); numAlto.Value = int.Parse(partes[1]); }
        }

        private void ConfigureTrayIcon()
        {
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Mostrar Panel", null, (s, e) => ShowMainWindow());
            trayMenu.Items.Add(new ToolStripSeparator());
            trayMenu.Items.Add("Salir Completamente", null, (s, e) => ExitApplication());

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
            this.BringToFront();
            this.Activate();
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
                // Cargar Juegos
                if (File.Exists(ConfigFile))
                {
                    string json = File.ReadAllText(ConfigFile);
                    procesosMonitoreados = JsonSerializer.Deserialize<Dictionary<string, GameConfig>>(json) ?? new Dictionary<string, GameConfig>();
                    ActualizarListaVisual();
                    if (procesosMonitoreados.Count > 0) monitorTimer.Start();
                }

                // Cargar Ajustes Globales
                if (File.Exists(SettingsFile))
                {
                    string jsonSettings = File.ReadAllText(SettingsFile);
                    configuracionApp = JsonSerializer.Deserialize<AppSettings>(jsonSettings) ?? new AppSettings();
                }

                // Aplicar configuraciones a la UI y al sistema
                chkFocusMode.Checked = configuracionApp.FocusMode;
                chkTopMost.Checked = configuracionApp.TopMost;
                chkStartMinimized.Checked = configuracionApp.StartMinimized;
                chkStartWithWindows.Checked = configuracionApp.StartWithWindows;

                this.TopMost = configuracionApp.TopMost;
            }
            catch { procesosMonitoreados = new Dictionary<string, GameConfig>(); configuracionApp = new AppSettings(); }
        }

        private void SaveConfig()
        {
            try
            {
                File.WriteAllText(ConfigFile, JsonSerializer.Serialize(procesosMonitoreados));

                configuracionApp.FocusMode = chkFocusMode.Checked;
                configuracionApp.TopMost = chkTopMost.Checked;
                configuracionApp.StartMinimized = chkStartMinimized.Checked;
                configuracionApp.StartWithWindows = chkStartWithWindows.Checked;

                File.WriteAllText(SettingsFile, JsonSerializer.Serialize(configuracionApp));
            }
            catch (Exception ex) { Debug.WriteLine("Error al guardar: " + ex.Message); }
        }

        // --- SISTEMA DE INICIO CON WINDOWS (Vía Regedit - Seguro) ---
        private void ConfigurarInicioConWindows(bool activar)
        {
            try
            {
                string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(runKey, true))
                {
                    if (activar) key.SetValue("ResolutionManagerPro", Application.ExecutablePath);
                    else key.DeleteValue("ResolutionManagerPro", false);
                }
            }
            catch (Exception ex) { Debug.WriteLine("Error de registro: " + ex.Message); }
        }

        private void chkSetting_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkTopMost.Checked;
            ConfigurarInicioConWindows(chkStartWithWindows.Checked);
            SaveConfig();
        }

        private void ActualizarListaVisual()
        {
            lstProcesos.Items.Clear();
            foreach (var item in procesosMonitoreados)
                lstProcesos.Items.Add($"{item.Key} ({item.Value.Width}x{item.Value.Height} @ {item.Value.RefreshRate}Hz)");
        }

        // --- NÚCLEO OPTIMIZADO ---
        private void MonitorTimer_Tick(object sender, EventArgs e)
        {
            if (!programaActivo) { ActualizarIconoBandeja(0); return; }

            string procesoDetectado = "";
            bool juegoEnFocus = false;

            IntPtr ventanaActual = GetForegroundWindow();
            GetWindowThreadProcessId(ventanaActual, out uint processIdFoco);

            foreach (var proceso in procesosMonitoreados.Keys)
            {
                Process[] procesos = Process.GetProcessesByName(proceso);
                if (procesos.Length > 0)
                {
                    procesoDetectado = proceso;
                    foreach (var p in procesos)
                    {
                        if (p.Id == processIdFoco) { juegoEnFocus = true; break; }
                    }
                    break;
                }
            }

            bool modoFocusActivado = chkFocusMode.Checked;
            bool debeAplicarResolucion = !string.IsNullOrEmpty(procesoDetectado) && (!modoFocusActivado || juegoEnFocus);

            if (debeAplicarResolucion)
            {
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
                if (resolucionActualModificada)
                {
                    RestaurarResolucionOriginal();
                    resolucionActualModificada = false;
                }
                ActualizarIconoBandeja(3);
            }
            else
            {
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
                DEVMODE dm = new DEVMODE(); dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
                bool modoEncontrado = false;
                for (int i = 0; EnumDisplaySettings(null, i, ref dm); i++)
                {
                    if (dm.dmPelsWidth == ancho && dm.dmPelsHeight == alto && dm.dmDisplayFrequency == hz)
                    { modoEncontrado = true; break; }
                }

                if (modoEncontrado)
                {
                    dm.dmFields |= DM_PELSWIDTH | DM_PELSHEIGHT | DM_DISPLAYFREQUENCY | DM_DISPLAYFIXEDOUTPUT;
                    dm.dmDisplayFixedOutput = DMDFO_DEFAULT;
                    if (ChangeDisplaySettings(ref dm, CDS_UPDATEREGISTRY) != DISP_CHANGE_SUCCESSFUL)
                    {
                        dm.dmFields = DM_PELSWIDTH | DM_PELSHEIGHT | DM_DISPLAYFREQUENCY;
                        ChangeDisplaySettings(ref dm, CDS_UPDATEREGISTRY);
                    }
                }
                else
                {
                    DEVMODE dmManual = new DEVMODE(); dmManual.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
                    EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref dmManual);
                    dmManual.dmPelsWidth = ancho; dmManual.dmPelsHeight = alto; dmManual.dmDisplayFrequency = hz;
                    dmManual.dmFields = DM_PELSWIDTH | DM_PELSHEIGHT | DM_DISPLAYFREQUENCY | DM_DISPLAYFIXEDOUTPUT;
                    dmManual.dmDisplayFixedOutput = DMDFO_DEFAULT;
                    ChangeDisplaySettings(ref dmManual, CDS_UPDATEREGISTRY);
                }
            }
            catch (Exception ex) { Debug.WriteLine($"Error: {ex.Message}"); }
        }

        private void RestaurarResolucionOriginal()
        {
            try { ChangeDisplaySettings(ref originalMode, CDS_UPDATEREGISTRY); } catch (Exception) { }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombreProceso.Text.Trim();
            if (string.IsNullOrEmpty(nombre) || nombre == "proceso") return;

            var config = new GameConfig { Width = (int)numAncho.Value, Height = (int)numAlto.Value, RefreshRate = (int)numHz.Value };

            if (procesosMonitoreados.ContainsKey(nombre)) procesosMonitoreados[nombre] = config;
            else procesosMonitoreados.Add(nombre, config);

            ActualizarListaVisual();
            SaveConfig();

            if (!monitorTimer.Enabled) monitorTimer.Start();
            if (procesoActualActivo == nombre && (!chkFocusMode.Checked))
                CambiarResolucion(config.Width, config.Height, config.RefreshRate);

            LimpiarCampos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (lstProcesos.SelectedIndex == -1) return;
            string selectedText = lstProcesos.SelectedItem.ToString();
            string nombreProceso = selectedText.Contains(" (") ? selectedText.Substring(0, selectedText.LastIndexOf(" (")) : selectedText;

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
            if (programaActivo)
            {
                btnActivarDesactivar.Text = "PAUSAR SISTEMA";
                btnActivarDesactivar.BackColor = Color.FromArgb(229, 57, 53); // Rojo
                btnActivarDesactivar.ForeColor = Color.White;
            }
            else
            {
                btnActivarDesactivar.Text = "SISTEMA PAUSADO";
                btnActivarDesactivar.BackColor = Color.FromArgb(30, 30, 35); // Gris oscuro
                btnActivarDesactivar.ForeColor = Color.Gray;
                RestaurarResolucionOriginal();
                ActualizarIconoBandeja(0);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                this.ShowInTaskbar = false;
                return;
            }
            RestaurarResolucionOriginal();
        }

        private void txtNombreProceso_Enter(object sender, EventArgs e) { if (txtNombreProceso.Text == "proceso") { txtNombreProceso.Text = ""; txtNombreProceso.ForeColor = Color.White; } }
        private void txtNombreProceso_Leave(object sender, EventArgs e) { if (string.IsNullOrWhiteSpace(txtNombreProceso.Text)) { txtNombreProceso.Text = "proceso"; txtNombreProceso.ForeColor = Color.Gray; } }
    }

    public class GameConfig { public int Width { get; set; } public int Height { get; set; } public int RefreshRate { get; set; } }

    public class AppSettings
    {
        public bool FocusMode { get; set; } = true;
        public bool TopMost { get; set; } = false;
        public bool StartMinimized { get; set; } = false;
        public bool StartWithWindows { get; set; } = false;
    }
}