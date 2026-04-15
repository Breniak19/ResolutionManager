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
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.lstProcesos = new System.Windows.Forms.ListBox();
            this.numAncho = new System.Windows.Forms.NumericUpDown();
            this.numAlto = new System.Windows.Forms.NumericUpDown();
            this.txtNombreProceso = new System.Windows.Forms.TextBox();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnActivarDesactivar = new System.Windows.Forms.Button();
            this.numHz = new System.Windows.Forms.NumericUpDown();
            this.lblHz = new System.Windows.Forms.Label();
            this.cmbAspectRatio = new System.Windows.Forms.ComboBox();
            this.lblAspect = new System.Windows.Forms.Label();
            this.cmbResoluciones = new System.Windows.Forms.ComboBox();
            this.lblResPreset = new System.Windows.Forms.Label();
            this.chkFocusMode = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numAncho)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAlto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHz)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAgregar
            // 
            this.btnAgregar.BackColor = System.Drawing.Color.DimGray;
            this.btnAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregar.ForeColor = System.Drawing.Color.Lime;
            this.btnAgregar.Location = new System.Drawing.Point(214, 50);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(110, 30);
            this.btnAgregar.TabIndex = 0;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = false;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.BackColor = System.Drawing.Color.DimGray;
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.ForeColor = System.Drawing.Color.Red;
            this.btnEliminar.Location = new System.Drawing.Point(214, 86);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(110, 30);
            this.btnEliminar.TabIndex = 1;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // lstProcesos
            // 
            this.lstProcesos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.lstProcesos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstProcesos.ForeColor = System.Drawing.Color.White;
            this.lstProcesos.FormattingEnabled = true;
            this.lstProcesos.ItemHeight = 16;
            this.lstProcesos.Location = new System.Drawing.Point(12, 12);
            this.lstProcesos.Name = "lstProcesos";
            this.lstProcesos.Size = new System.Drawing.Size(190, 144);
            this.lstProcesos.TabIndex = 2;
            // 
            // numAncho
            // 
            this.numAncho.Location = new System.Drawing.Point(12, 235);
            this.numAncho.Name = "numAncho";
            this.numAncho.Size = new System.Drawing.Size(70, 23);
            this.numAncho.TabIndex = 3;
            // 
            // numAlto
            // 
            this.numAlto.Location = new System.Drawing.Point(105, 235);
            this.numAlto.Name = "numAlto";
            this.numAlto.Size = new System.Drawing.Size(70, 23);
            this.numAlto.TabIndex = 4;
            // 
            // txtNombreProceso
            // 
            this.txtNombreProceso.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtNombreProceso.ForeColor = System.Drawing.Color.Gray;
            this.txtNombreProceso.Location = new System.Drawing.Point(214, 12);
            this.txtNombreProceso.Name = "txtNombreProceso";
            this.txtNombreProceso.Size = new System.Drawing.Size(110, 23);
            this.txtNombreProceso.TabIndex = 5;
            this.txtNombreProceso.Text = "proceso";
            this.txtNombreProceso.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtNombreProceso.Enter += new System.EventHandler(this.txtNombreProceso_Enter);
            this.txtNombreProceso.Leave += new System.EventHandler(this.txtNombreProceso_Leave);
            // 
            // trayMenu
            // 
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(88, 237);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "x";
            this.label1.Click += new System.EventHandler(this.Form1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(260, 320);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "By Breniak";
            this.label2.Click += new System.EventHandler(this.Form1_Click);
            // 
            // btnActivarDesactivar
            // 
            this.btnActivarDesactivar.BackColor = System.Drawing.Color.LightGreen;
            this.btnActivarDesactivar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActivarDesactivar.Location = new System.Drawing.Point(214, 122);
            this.btnActivarDesactivar.Name = "btnActivarDesactivar";
            this.btnActivarDesactivar.Size = new System.Drawing.Size(110, 34);
            this.btnActivarDesactivar.TabIndex = 8;
            this.btnActivarDesactivar.Text = "Desactivar";
            this.btnActivarDesactivar.UseVisualStyleBackColor = false;
            this.btnActivarDesactivar.Click += new System.EventHandler(this.btnActivarDesactivar_Click);
            // 
            // numHz
            // 
            this.numHz.Location = new System.Drawing.Point(12, 275);
            this.numHz.Name = "numHz";
            this.numHz.Size = new System.Drawing.Size(70, 23);
            this.numHz.TabIndex = 9;
            // 
            // lblHz
            // 
            this.lblHz.AutoSize = true;
            this.lblHz.ForeColor = System.Drawing.Color.White;
            this.lblHz.Location = new System.Drawing.Point(88, 277);
            this.lblHz.Name = "lblHz";
            this.lblHz.Size = new System.Drawing.Size(21, 16);
            this.lblHz.TabIndex = 10;
            this.lblHz.Text = "Hz";
            this.lblHz.Click += new System.EventHandler(this.Form1_Click);
            // 
            // cmbAspectRatio
            // 
            this.cmbAspectRatio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAspectRatio.FormattingEnabled = true;
            this.cmbAspectRatio.Location = new System.Drawing.Point(12, 195);
            this.cmbAspectRatio.Name = "cmbAspectRatio";
            this.cmbAspectRatio.Size = new System.Drawing.Size(70, 24);
            this.cmbAspectRatio.TabIndex = 11;
            this.cmbAspectRatio.SelectedIndexChanged += new System.EventHandler(this.cmbAspectRatio_SelectedIndexChanged);
            // 
            // lblAspect
            // 
            this.lblAspect.AutoSize = true;
            this.lblAspect.ForeColor = System.Drawing.Color.White;
            this.lblAspect.Location = new System.Drawing.Point(12, 175);
            this.lblAspect.Name = "lblAspect";
            this.lblAspect.Size = new System.Drawing.Size(39, 16);
            this.lblAspect.TabIndex = 12;
            this.lblAspect.Text = "Ratio:";
            this.lblAspect.Click += new System.EventHandler(this.Form1_Click);
            // 
            // cmbResoluciones
            // 
            this.cmbResoluciones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResoluciones.FormattingEnabled = true;
            this.cmbResoluciones.Location = new System.Drawing.Point(105, 195);
            this.cmbResoluciones.Name = "cmbResoluciones";
            this.cmbResoluciones.Size = new System.Drawing.Size(100, 24);
            this.cmbResoluciones.TabIndex = 13;
            this.cmbResoluciones.SelectedIndexChanged += new System.EventHandler(this.cmbResoluciones_SelectedIndexChanged);
            // 
            // lblResPreset
            // 
            this.lblResPreset.AutoSize = true;
            this.lblResPreset.ForeColor = System.Drawing.Color.White;
            this.lblResPreset.Location = new System.Drawing.Point(105, 175);
            this.lblResPreset.Name = "lblResPreset";
            this.lblResPreset.Size = new System.Drawing.Size(66, 16);
            this.lblResPreset.TabIndex = 14;
            this.lblResPreset.Text = "Resolución:";
            this.lblResPreset.Click += new System.EventHandler(this.Form1_Click);
            // 
            // chkFocusMode
            // 
            this.chkFocusMode.AutoSize = true;
            this.chkFocusMode.ForeColor = System.Drawing.Color.White;
            this.chkFocusMode.Location = new System.Drawing.Point(12, 315);
            this.chkFocusMode.Name = "chkFocusMode";
            this.chkFocusMode.Size = new System.Drawing.Size(225, 20);
            this.chkFocusMode.TabIndex = 15;
            this.chkFocusMode.Text = "Modo Focus (Solo activo en pantalla)";
            this.chkFocusMode.UseVisualStyleBackColor = true;
            this.chkFocusMode.CheckedChanged += new System.EventHandler(this.chkFocusMode_CheckedChanged);
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(340, 345);
            this.Controls.Add(this.chkFocusMode);
            this.Controls.Add(this.lblResPreset);
            this.Controls.Add(this.cmbResoluciones);
            this.Controls.Add(this.lblAspect);
            this.Controls.Add(this.cmbAspectRatio);
            this.Controls.Add(this.lblHz);
            this.Controls.Add(this.numHz);
            this.Controls.Add(this.btnActivarDesactivar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNombreProceso);
            this.Controls.Add(this.numAlto);
            this.Controls.Add(this.numAncho);
            this.Controls.Add(this.lstProcesos);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnAgregar);
            this.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Resolution Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Click += new System.EventHandler(this.Form1_Click);
            ((System.ComponentModel.ISupportInitialize)(this.numAncho)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAlto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHz)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.ListBox lstProcesos;
        private System.Windows.Forms.NumericUpDown numAncho;
        private System.Windows.Forms.NumericUpDown numAlto;
        private System.Windows.Forms.TextBox txtNombreProceso;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnActivarDesactivar;
        private System.Windows.Forms.NumericUpDown numHz;
        private System.Windows.Forms.Label lblHz;
        private System.Windows.Forms.ComboBox cmbAspectRatio;
        private System.Windows.Forms.Label lblAspect;
        private System.Windows.Forms.ComboBox cmbResoluciones;
        private System.Windows.Forms.Label lblResPreset;
        private System.Windows.Forms.CheckBox chkFocusMode;
    }
}