namespace ResolutionManager
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pnlTitleBar = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.btnNavGames = new System.Windows.Forms.Button();
            this.btnNavSettings = new System.Windows.Forms.Button();
            this.btnActivarDesactivar = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlTabGames = new System.Windows.Forms.Panel();
            this.lstProcesos = new System.Windows.Forms.ListBox();
            this.txtNombreProceso = new System.Windows.Forms.TextBox();
            this.cmbAspectRatio = new System.Windows.Forms.ComboBox();
            this.cmbResoluciones = new System.Windows.Forms.ComboBox();
            this.numAncho = new System.Windows.Forms.NumericUpDown();
            this.lblX = new System.Windows.Forms.Label();
            this.numAlto = new System.Windows.Forms.NumericUpDown();
            this.numHz = new System.Windows.Forms.NumericUpDown();
            this.lblHz = new System.Windows.Forms.Label();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.pnlTabSettings = new System.Windows.Forms.Panel();
            this.chkFocusMode = new System.Windows.Forms.CheckBox();
            this.chkTopMost = new System.Windows.Forms.CheckBox();
            this.chkStartMinimized = new System.Windows.Forms.CheckBox();
            this.chkStartWithWindows = new System.Windows.Forms.CheckBox();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.pnlTitleBar.SuspendLayout();
            this.pnlSidebar.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlTabGames.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAncho)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAlto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHz)).BeginInit();
            this.pnlTabSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTitleBar
            // 
            this.pnlTitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(19)))));
            this.pnlTitleBar.Controls.Add(this.lblTitle);
            this.pnlTitleBar.Controls.Add(this.btnMinimize);
            this.pnlTitleBar.Controls.Add(this.btnClose);
            this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTitleBar.Name = "pnlTitleBar";
            this.pnlTitleBar.Size = new System.Drawing.Size(460, 30);
            this.pnlTitleBar.TabIndex = 2;
            this.pnlTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlTitleBar_MouseDown);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblTitle.Location = new System.Drawing.Point(10, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(203, 15);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Resolution Manager Pro By Breniak";
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlTitleBar_MouseDown);
            // 
            // btnMinimize
            // 
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(45)))));
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.ForeColor = System.Drawing.Color.White;
            this.btnMinimize.Location = new System.Drawing.Point(380, 0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(40, 30);
            this.btnMinimize.TabIndex = 1;
            this.btnMinimize.Text = "—";
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(57)))), ((int)(((byte)(53)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(420, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(40, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "✕";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(19)))));
            this.pnlSidebar.Controls.Add(this.lblLogo);
            this.pnlSidebar.Controls.Add(this.btnNavGames);
            this.pnlSidebar.Controls.Add(this.btnNavSettings);
            this.pnlSidebar.Controls.Add(this.btnActivarDesactivar);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 30);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(130, 240);
            this.pnlSidebar.TabIndex = 1;
            // 
            // lblLogo
            // 
            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI Black", 16F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(57)))), ((int)(((byte)(53)))));
            this.lblLogo.Location = new System.Drawing.Point(15, 20);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(59, 60);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "RM\nPRO";
            // 
            // btnNavGames
            // 
            this.btnNavGames.FlatAppearance.BorderSize = 0;
            this.btnNavGames.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavGames.Location = new System.Drawing.Point(0, 90);
            this.btnNavGames.Name = "btnNavGames";
            this.btnNavGames.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnNavGames.Size = new System.Drawing.Size(130, 40);
            this.btnNavGames.TabIndex = 1;
            this.btnNavGames.Text = "🎮 Juegos";
            this.btnNavGames.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavGames.Click += new System.EventHandler(this.btnNavGames_Click);
            // 
            // btnNavSettings
            // 
            this.btnNavSettings.FlatAppearance.BorderSize = 0;
            this.btnNavSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavSettings.Location = new System.Drawing.Point(0, 130);
            this.btnNavSettings.Name = "btnNavSettings";
            this.btnNavSettings.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnNavSettings.Size = new System.Drawing.Size(130, 40);
            this.btnNavSettings.TabIndex = 2;
            this.btnNavSettings.Text = "⚙️ Ajustes";
            this.btnNavSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavSettings.Click += new System.EventHandler(this.btnNavSettings_Click);
            // 
            // btnActivarDesactivar
            // 
            this.btnActivarDesactivar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(57)))), ((int)(((byte)(53)))));
            this.btnActivarDesactivar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnActivarDesactivar.FlatAppearance.BorderSize = 0;
            this.btnActivarDesactivar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActivarDesactivar.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnActivarDesactivar.ForeColor = System.Drawing.Color.White;
            this.btnActivarDesactivar.Location = new System.Drawing.Point(0, 200);
            this.btnActivarDesactivar.Name = "btnActivarDesactivar";
            this.btnActivarDesactivar.Size = new System.Drawing.Size(130, 40);
            this.btnActivarDesactivar.TabIndex = 3;
            this.btnActivarDesactivar.Text = "PAUSAR SISTEMA";
            this.btnActivarDesactivar.UseVisualStyleBackColor = false;
            this.btnActivarDesactivar.Click += new System.EventHandler(this.btnActivarDesactivar_Click);
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(26)))));
            this.pnlContent.Controls.Add(this.pnlTabGames);
            this.pnlContent.Controls.Add(this.pnlTabSettings);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(130, 30);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(330, 240);
            this.pnlContent.TabIndex = 0;
            this.pnlContent.Click += new System.EventHandler(this.pnlContent_Click);
            // 
            // pnlTabGames
            // 
            this.pnlTabGames.Controls.Add(this.lstProcesos);
            this.pnlTabGames.Controls.Add(this.txtNombreProceso);
            this.pnlTabGames.Controls.Add(this.cmbAspectRatio);
            this.pnlTabGames.Controls.Add(this.cmbResoluciones);
            this.pnlTabGames.Controls.Add(this.numAncho);
            this.pnlTabGames.Controls.Add(this.lblX);
            this.pnlTabGames.Controls.Add(this.numAlto);
            this.pnlTabGames.Controls.Add(this.numHz);
            this.pnlTabGames.Controls.Add(this.lblHz);
            this.pnlTabGames.Controls.Add(this.btnAgregar);
            this.pnlTabGames.Controls.Add(this.btnEliminar);
            this.pnlTabGames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTabGames.Location = new System.Drawing.Point(0, 0);
            this.pnlTabGames.Name = "pnlTabGames";
            this.pnlTabGames.Size = new System.Drawing.Size(330, 240);
            this.pnlTabGames.TabIndex = 0;
            this.pnlTabGames.Click += new System.EventHandler(this.pnlTabGames_Click);
            // 
            // lstProcesos
            // 
            this.lstProcesos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            this.lstProcesos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstProcesos.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lstProcesos.ForeColor = System.Drawing.Color.White;
            this.lstProcesos.ItemHeight = 15;
            this.lstProcesos.Location = new System.Drawing.Point(15, 15);
            this.lstProcesos.Name = "lstProcesos";
            this.lstProcesos.Size = new System.Drawing.Size(150, 210);
            this.lstProcesos.TabIndex = 0;
            // 
            // txtNombreProceso
            // 
            this.txtNombreProceso.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            this.txtNombreProceso.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNombreProceso.ForeColor = System.Drawing.Color.Gray;
            this.txtNombreProceso.Location = new System.Drawing.Point(180, 15);
            this.txtNombreProceso.Name = "txtNombreProceso";
            this.txtNombreProceso.Size = new System.Drawing.Size(140, 23);
            this.txtNombreProceso.TabIndex = 1;
            this.txtNombreProceso.Text = "proceso";
            this.txtNombreProceso.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtNombreProceso.Enter += new System.EventHandler(this.txtNombreProceso_Enter);
            this.txtNombreProceso.Leave += new System.EventHandler(this.txtNombreProceso_Leave);
            // 
            // cmbAspectRatio
            // 
            this.cmbAspectRatio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(45)))));
            this.cmbAspectRatio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAspectRatio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbAspectRatio.ForeColor = System.Drawing.Color.White;
            this.cmbAspectRatio.Location = new System.Drawing.Point(180, 50);
            this.cmbAspectRatio.Name = "cmbAspectRatio";
            this.cmbAspectRatio.Size = new System.Drawing.Size(65, 23);
            this.cmbAspectRatio.TabIndex = 2;
            this.cmbAspectRatio.SelectedIndexChanged += new System.EventHandler(this.cmbAspectRatio_SelectedIndexChanged);
            // 
            // cmbResoluciones
            // 
            this.cmbResoluciones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(45)))));
            this.cmbResoluciones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResoluciones.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbResoluciones.ForeColor = System.Drawing.Color.White;
            this.cmbResoluciones.Location = new System.Drawing.Point(250, 50);
            this.cmbResoluciones.Name = "cmbResoluciones";
            this.cmbResoluciones.Size = new System.Drawing.Size(70, 23);
            this.cmbResoluciones.TabIndex = 3;
            this.cmbResoluciones.SelectedIndexChanged += new System.EventHandler(this.cmbResoluciones_SelectedIndexChanged);
            // 
            // numAncho
            // 
            this.numAncho.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            this.numAncho.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numAncho.ForeColor = System.Drawing.Color.White;
            this.numAncho.Location = new System.Drawing.Point(180, 85);
            this.numAncho.Name = "numAncho";
            this.numAncho.Size = new System.Drawing.Size(60, 23);
            this.numAncho.TabIndex = 4;
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.ForeColor = System.Drawing.Color.Gray;
            this.lblX.Location = new System.Drawing.Point(242, 87);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(12, 15);
            this.lblX.TabIndex = 5;
            this.lblX.Text = "x";
            // 
            // numAlto
            // 
            this.numAlto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            this.numAlto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numAlto.ForeColor = System.Drawing.Color.White;
            this.numAlto.Location = new System.Drawing.Point(260, 85);
            this.numAlto.Name = "numAlto";
            this.numAlto.Size = new System.Drawing.Size(60, 23);
            this.numAlto.TabIndex = 6;
            // 
            // numHz
            // 
            this.numHz.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            this.numHz.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numHz.ForeColor = System.Drawing.Color.White;
            this.numHz.Location = new System.Drawing.Point(180, 120);
            this.numHz.Name = "numHz";
            this.numHz.Size = new System.Drawing.Size(60, 23);
            this.numHz.TabIndex = 7;
            // 
            // lblHz
            // 
            this.lblHz.AutoSize = true;
            this.lblHz.ForeColor = System.Drawing.Color.Gray;
            this.lblHz.Location = new System.Drawing.Point(245, 122);
            this.lblHz.Name = "lblHz";
            this.lblHz.Size = new System.Drawing.Size(21, 15);
            this.lblHz.TabIndex = 8;
            this.lblHz.Text = "Hz";
            // 
            // btnAgregar
            // 
            this.btnAgregar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(57)))), ((int)(((byte)(53)))));
            this.btnAgregar.FlatAppearance.BorderSize = 0;
            this.btnAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAgregar.ForeColor = System.Drawing.Color.White;
            this.btnAgregar.Location = new System.Drawing.Point(180, 160);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(140, 30);
            this.btnAgregar.TabIndex = 9;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = false;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            this.btnEliminar.FlatAppearance.BorderSize = 0;
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.ForeColor = System.Drawing.Color.Gray;
            this.btnEliminar.Location = new System.Drawing.Point(180, 195);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(140, 30);
            this.btnEliminar.TabIndex = 10;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // pnlTabSettings
            // 
            this.pnlTabSettings.Controls.Add(this.chkFocusMode);
            this.pnlTabSettings.Controls.Add(this.chkTopMost);
            this.pnlTabSettings.Controls.Add(this.chkStartMinimized);
            this.pnlTabSettings.Controls.Add(this.chkStartWithWindows);
            this.pnlTabSettings.Controls.Add(this.lblAuthor);
            this.pnlTabSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTabSettings.Location = new System.Drawing.Point(0, 0);
            this.pnlTabSettings.Name = "pnlTabSettings";
            this.pnlTabSettings.Size = new System.Drawing.Size(330, 240);
            this.pnlTabSettings.TabIndex = 1;
            // 
            // chkFocusMode
            // 
            this.chkFocusMode.AutoSize = true;
            this.chkFocusMode.ForeColor = System.Drawing.Color.White;
            this.chkFocusMode.Location = new System.Drawing.Point(20, 20);
            this.chkFocusMode.Name = "chkFocusMode";
            this.chkFocusMode.Size = new System.Drawing.Size(226, 34);
            this.chkFocusMode.TabIndex = 0;
            this.chkFocusMode.Text = "Modo Focus (Recomendado)\nRegresa la resolución al hacer Alt+Tab";
            this.chkFocusMode.CheckedChanged += new System.EventHandler(this.chkSetting_CheckedChanged);
            // 
            // chkTopMost
            // 
            this.chkTopMost.AutoSize = true;
            this.chkTopMost.ForeColor = System.Drawing.Color.White;
            this.chkTopMost.Location = new System.Drawing.Point(20, 70);
            this.chkTopMost.Name = "chkTopMost";
            this.chkTopMost.Size = new System.Drawing.Size(209, 19);
            this.chkTopMost.TabIndex = 1;
            this.chkTopMost.Text = "Mantener ventana siempre encima";
            this.chkTopMost.CheckedChanged += new System.EventHandler(this.chkSetting_CheckedChanged);
            // 
            // chkStartMinimized
            // 
            this.chkStartMinimized.AutoSize = true;
            this.chkStartMinimized.ForeColor = System.Drawing.Color.White;
            this.chkStartMinimized.Location = new System.Drawing.Point(20, 105);
            this.chkStartMinimized.Name = "chkStartMinimized";
            this.chkStartMinimized.Size = new System.Drawing.Size(197, 19);
            this.chkStartMinimized.TabIndex = 2;
            this.chkStartMinimized.Text = "Iniciar minimizado en la bandeja";
            this.chkStartMinimized.CheckedChanged += new System.EventHandler(this.chkSetting_CheckedChanged);
            // 
            // chkStartWithWindows
            // 
            this.chkStartWithWindows.AutoSize = true;
            this.chkStartWithWindows.ForeColor = System.Drawing.Color.White;
            this.chkStartWithWindows.Location = new System.Drawing.Point(20, 140);
            this.chkStartWithWindows.Name = "chkStartWithWindows";
            this.chkStartWithWindows.Size = new System.Drawing.Size(243, 19);
            this.chkStartWithWindows.TabIndex = 3;
            this.chkStartWithWindows.Text = "Arrancar automáticamente con Windows";
            this.chkStartWithWindows.CheckedChanged += new System.EventHandler(this.chkSetting_CheckedChanged);
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic);
            this.lblAuthor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
            this.lblAuthor.Location = new System.Drawing.Point(17, 210);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(108, 13);
            this.lblAuthor.TabIndex = 4;
            this.lblAuthor.Text = "Diseñado por Breniak";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(460, 270);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlTitleBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.pnlTitleBar.ResumeLayout(false);
            this.pnlTitleBar.PerformLayout();
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebar.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlTabGames.ResumeLayout(false);
            this.pnlTabGames.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAncho)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAlto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHz)).EndInit();
            this.pnlTabSettings.ResumeLayout(false);
            this.pnlTabSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlTitleBar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMinimize;

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Button btnNavGames;
        private System.Windows.Forms.Button btnNavSettings;
        private System.Windows.Forms.Button btnActivarDesactivar;

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlTabGames;
        private System.Windows.Forms.Panel pnlTabSettings;

        private System.Windows.Forms.ListBox lstProcesos;
        private System.Windows.Forms.TextBox txtNombreProceso;
        private System.Windows.Forms.ComboBox cmbAspectRatio;
        private System.Windows.Forms.ComboBox cmbResoluciones;
        private System.Windows.Forms.NumericUpDown numAncho;
        private System.Windows.Forms.NumericUpDown numAlto;
        private System.Windows.Forms.NumericUpDown numHz;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Label lblHz;

        private System.Windows.Forms.CheckBox chkFocusMode;
        private System.Windows.Forms.CheckBox chkTopMost;
        private System.Windows.Forms.CheckBox chkStartMinimized;
        private System.Windows.Forms.CheckBox chkStartWithWindows;
        private System.Windows.Forms.Label lblAuthor;

        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
    }
}