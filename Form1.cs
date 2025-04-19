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
    public partial class Form1 : Form
    {
        // Importar funciones de user32.dll para cambiar resolución
        [DllImport("user32.dll")]
        private static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
        [DllImport("user32.dll")]
        private static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

        private const int ENUM_CURRENT_SETTINGS = -1;
        private const int CDS_UPDATEREGISTRY = 0x01;
        private const int DISP_CHANGE_SUCCESSFUL = 0;

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
        private Dictionary<string, Size> procesosMonitoreados;
        private const string ConfigFile = "config.json";

        public Form1()
        {
            InitializeComponent();

            // Configurar controles
            numAncho.Minimum = 300;
            numAncho.Maximum = 7680;
            numAncho.Value = 300;

            numAlto.Minimum = 200;
            numAlto.Maximum = 4320;
            numAlto.Value = 200;

            // Configurar el icono en la bandeja del sistema
            ConfigureTrayIcon();

            // Inicializar componentes
            procesosMonitoreados = new Dictionary<string, Size>();
            monitorTimer = new Timer { Interval = 1000 };
            monitorTimer.Tick += MonitorTimer_Tick;

            // Cargar configuraciones guardadas
            LoadConfig();

            // Guardar resolución actual
            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref dm);
            originalMode = dm;
        }

        private void ConfigureTrayIcon()
        {
            // Crear un menú contextual para el icono de la bandeja
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Mostrar", null, (s, e) => ShowMainWindow());
            trayMenu.Items.Add("Salir", null, (s, e) => ExitApplication());

            // Crear el icono de la bandeja
            trayIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Text = "Resolution Manager",
                ContextMenuStrip = trayMenu,
                Visible = true
            };

            // Configurar el evento de doble clic
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
                    var config = JsonSerializer.Deserialize<Dictionary<string, Size>>(json);

                    if (config != null)
                    {
                        procesosMonitoreados = config;
                        lstProcesos.Items.Clear();
                        foreach (var item in procesosMonitoreados)
                        {
                            lstProcesos.Items.Add($"{item.Key} ({item.Value.Width}x{item.Value.Height})");
                        }

                        if (procesosMonitoreados.Count > 0)
                            monitorTimer.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar configuración: {ex.Message}");
            }
        }

        private void SaveConfig()
        {
            try
            {
                string json = JsonSerializer.Serialize(procesosMonitoreados);
                File.WriteAllText(ConfigFile, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar configuración: {ex.Message}");
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.ShowInTaskbar = false;
                trayIcon.ShowBalloonTip(1000, "Resolution Manager",
                    "La aplicación se está ejecutando en segundo plano", ToolTipIcon.Info);
            }
        }

        private void MonitorTimer_Tick(object sender, EventArgs e)
        {
            bool algunJuegoEnEjecucion = false;

            // Primero verificamos si algún juego está en ejecución
            foreach (var proceso in procesosMonitoreados)
            {
                if (Process.GetProcessesByName(proceso.Key).Length > 0)
                {
                    algunJuegoEnEjecucion = true;
                    break;
                }
            }

            // Luego aplicamos los cambios de resolución según corresponda
            if (algunJuegoEnEjecucion)
            {
                foreach (var proceso in procesosMonitoreados)
                {
                    Process[] procesos = Process.GetProcessesByName(proceso.Key);
                    if (procesos.Length > 0)
                    {
                        CambiarResolucion(proceso.Value.Width, proceso.Value.Height);
                        break; // Solo cambiamos una vez
                    }
                }
            }
            else
            {
                RestaurarResolucionOriginal();
            }
        }

        private async void CambiarResolucion(int ancho, int alto)
        {
            try
            {
                DEVMODE dm = new DEVMODE();
                dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
                dm.dmPelsWidth = ancho;
                dm.dmPelsHeight = alto;
                dm.dmFields = 0x00080000 | 0x00100000; // DM_PELSWIDTH | DM_PELSHEIGHT

                int result = ChangeDisplaySettings(ref dm, CDS_UPDATEREGISTRY);
                if (result != DISP_CHANGE_SUCCESSFUL)
                {
                    Console.WriteLine("No se pudo cambiar la resolución");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar resolución: {ex.Message}");
            }
            await Task.Delay(500);
        }

        private void RestaurarResolucionOriginal()
        {
            try
            {
                int result = ChangeDisplaySettings(ref originalMode, CDS_UPDATEREGISTRY);
                if (result != DISP_CHANGE_SUCCESSFUL)
                {
                    Console.WriteLine("No se pudo restaurar la resolución original");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al restaurar resolución: {ex.Message}");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombreProceso = txtNombreProceso.Text.Trim();
            if (string.IsNullOrEmpty(nombreProceso))
            {
                MessageBox.Show("Por favor ingresa el nombre del proceso");
                return;
            }

            int ancho = (int)numAncho.Value;
            int alto = (int)numAlto.Value;

            if (!procesosMonitoreados.ContainsKey(nombreProceso))
            {
                procesosMonitoreados.Add(nombreProceso, new Size(ancho, alto));
                lstProcesos.Items.Add($"{nombreProceso} ({ancho}x{alto})");
                txtNombreProceso.Clear();
                SaveConfig(); // Guardar cambios
            }
            else
            {
                MessageBox.Show("Este proceso ya está siendo monitoreado");
            }

            if (!monitorTimer.Enabled)
                monitorTimer.Start();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (lstProcesos.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor selecciona un proceso de la lista");
                return;
            }

            string item = lstProcesos.SelectedItem.ToString();
            // Corregido: ahora toma todo excepto la parte de la resolución (últimos 9 caracteres " (000x000)")
            string nombreProceso = item.Substring(0, item.Length - 9).Trim();

            if (procesosMonitoreados.ContainsKey(nombreProceso))
            {
                procesosMonitoreados.Remove(nombreProceso);
                lstProcesos.Items.RemoveAt(lstProcesos.SelectedIndex);
                SaveConfig();

                if (procesosMonitoreados.Count == 0)
                    monitorTimer.Stop();
            }
            else
            {
                MessageBox.Show("No se pudo eliminar el proceso. No se encontró en la lista.");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Opción para minimizar a la bandeja en lugar de cerrar
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                return;
            }

            // Asegurarse de restaurar la resolución al cerrar la aplicación
            RestaurarResolucionOriginal();
            SaveConfig();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Administrador de Resolución para Juegos";
        }

        private void txtNombreProceso_Enter(object sender, EventArgs e)
        {
            if (txtNombreProceso.Text == "proceso")
            {
                txtNombreProceso.Text = "";
                txtNombreProceso.ForeColor = Color.Black;
            }
        }

        private void txtNombreProceso_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreProceso.Text))
            {
                txtNombreProceso.Text = "proceso";
                txtNombreProceso.ForeColor = Color.Gray;
            }
        }
    }
}