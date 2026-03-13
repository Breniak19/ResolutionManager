using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;

namespace ResolutionManager
{
    // LA CLASE FORM1 DEBE SER LA PRIMERA PARA QUE EL DISEÑADOR FUNCIONE
    public partial class Form1 : Form
    {
        private bool programaActivo = true;

        [DllImport("user32.dll")]
        private static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
        [DllImport("user32.dll")]
        private static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

        private const int ENUM_CURRENT_SETTINGS = -1;
        private const int CDS_UPDATEREGISTRY = 0x01;
        private const int DISP_CHANGE_SUCCESSFUL = 0;

        // Flags de campos
        private const int DM_PELSWIDTH = 0x00080000;
        private const int DM_PELSHEIGHT = 0x00100000;
        private const int DM_DISPLAYFREQUENCY = 0x00400000;
        private const int DM_DISPLAYFIXEDOUTPUT = 0x20000000;

        // Constantes para salida fija (control de escalado)
        private const int DMDFO_DEFAULT = 0; // Usar el escalado del monitor (Nativo)

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
        private string procesoActualActivo = "";

        public Form1()
        {
            InitializeComponent();
            SetupControls();
            ConfigureTrayIcon();

            procesosMonitoreados = new Dictionary<string, GameConfig>();
            monitorTimer = new Timer { Interval = 2000 };
            monitorTimer.Tick += MonitorTimer_Tick;

            LoadConfig();

            originalMode = new DEVMODE();
            originalMode.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref originalMode);
        }

        private void SetupControls()
        {
            numAncho.Minimum = 320; numAncho.Maximum = 7680; numAncho.Value = 1920;
            numAlto.Minimum = 200; numAlto.Maximum = 4320; numAlto.Value = 1080;
            numHz.Minimum = 30; numHz.Maximum = 500; numHz.Value = 60;
        }

        private void ConfigureTrayIcon()
        {
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Mostrar", null, (s, e) => ShowMainWindow());
            trayMenu.Items.Add("Salir", null, (s, e) => ExitApplication());

            trayIcon.Icon = SystemIcons.Application;
            trayIcon.Text = "Resolution Manager";
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
                if (File.Exists(ConfigFile))
                {
                    string json = File.ReadAllText(ConfigFile);
                    procesosMonitoreados = JsonSerializer.Deserialize<Dictionary<string, GameConfig>>(json) ?? new Dictionary<string, GameConfig>();
                    
                    lstProcesos.Items.Clear();
                    foreach (var item in procesosMonitoreados)
                    {
                        lstProcesos.Items.Add($"{item.Key} ({item.Value.Width}x{item.Value.Height} @ {item.Value.RefreshRate}Hz)");
                    }
                    if (procesosMonitoreados.Count > 0) monitorTimer.Start();
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
            }
            catch (Exception ex) { Console.WriteLine("Error al guardar: " + ex.Message); }
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

        private void MonitorTimer_Tick(object sender, EventArgs e)
        {
            if (!programaActivo) return;

            string procesoDetectado = "";
            foreach (var proceso in procesosMonitoreados.Keys)
            {
                if (Process.GetProcessesByName(proceso).Length > 0)
                {
                    procesoDetectado = proceso;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(procesoDetectado))
            {
                if (procesoActualActivo != procesoDetectado)
                {
                    var config = procesosMonitoreados[procesoDetectado];
                    CambiarResolucion(config.Width, config.Height, config.RefreshRate);
                    procesoActualActivo = procesoDetectado;
                }
            }
            else if (procesoActualActivo != "")
            {
                RestaurarResolucionOriginal();
                procesoActualActivo = "";
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
                    if (dm.dmPelsWidth == ancho && 
                        dm.dmPelsHeight == alto && 
                        dm.dmDisplayFrequency == hz)
                    {
                        modoEncontrado = true;
                        break; 
                    }
                }

                if (modoEncontrado)
                {
                    // Forzamos el modo nativo limpiando cualquier preferencia de escalado anterior
                    dm.dmFields |= DM_DISPLAYFIXEDOUTPUT;
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
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }

        private void RestaurarResolucionOriginal()
        {
            try
            {
                ChangeDisplaySettings(ref originalMode, CDS_UPDATEREGISTRY);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombreProceso.Text.Trim();
            if (string.IsNullOrEmpty(nombre) || nombre == "proceso") return;

            var config = new GameConfig {
                Width = (int)numAncho.Value,
                Height = (int)numAlto.Value,
                RefreshRate = (int)numHz.Value
            };

            if (procesosMonitoreados.ContainsKey(nombre))
            {
                procesosMonitoreados[nombre] = config; // Actualizar si ya existe
                // Actualizar visualmente la lista
                for (int i = 0; i < lstProcesos.Items.Count; i++)
                {
                    if (lstProcesos.Items[i].ToString().StartsWith(nombre + " ("))
                    {
                        lstProcesos.Items[i] = $"{nombre} ({config.Width}x{config.Height} @ {config.RefreshRate}Hz)";
                        break;
                    }
                }
            }
            else
            {
                procesosMonitoreados.Add(nombre, config);
                lstProcesos.Items.Add($"{nombre} ({config.Width}x{config.Height} @ {config.RefreshRate}Hz)");
            }
            
            SaveConfig();
            if (!monitorTimer.Enabled) monitorTimer.Start();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (lstProcesos.SelectedIndex == -1) return;
            
            string selectedText = lstProcesos.SelectedItem.ToString();
            string nombreProceso = "";
            
            // BUSQUEDA MEJORADA: Buscamos el último " (" para obtener el nombre real con espacios
            int lastParen = selectedText.LastIndexOf(" (");
            if (lastParen != -1)
                nombreProceso = selectedText.Substring(0, lastParen);
            else
                nombreProceso = selectedText;

            if (procesosMonitoreados.ContainsKey(nombreProceso))
            {
                procesosMonitoreados.Remove(nombreProceso);
                lstProcesos.Items.RemoveAt(lstProcesos.SelectedIndex);
                SaveConfig();
                
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
            btnActivarDesactivar.Text = programaActivo ? "Desactivar" : "Activar";
            btnActivarDesactivar.BackColor = programaActivo ? Color.LightGreen : Color.LightSalmon;
            
            // Forzamos el reseteo del estado para que detecte al instante
            procesoActualActivo = ""; 

            if (!programaActivo)
            {
                RestaurarResolucionOriginal();
            }
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
