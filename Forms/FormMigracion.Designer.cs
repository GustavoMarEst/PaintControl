namespace PaintControl.Forms
{
    partial class FormMigracion
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSeleccionar = new System.Windows.Forms.Button();
            this.txtRutaDBF = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnMigrarMovimientos = new System.Windows.Forms.Button();
            this.btnMigrarClientes = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSeleccionar);
            this.groupBox1.Controls.Add(this.txtRutaDBF);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(20, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(740, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "📂 Ubicación de Archivos DBF";
            // 
            // btnSeleccionar
            // 
            this.btnSeleccionar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(143)))), ((int)(((byte)(208)))));
            this.btnSeleccionar.FlatAppearance.BorderSize = 0;
            this.btnSeleccionar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSeleccionar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSeleccionar.ForeColor = System.Drawing.Color.White;
            this.btnSeleccionar.Location = new System.Drawing.Point(585, 53);
            this.btnSeleccionar.Name = "btnSeleccionar";
            this.btnSeleccionar.Size = new System.Drawing.Size(140, 30);
            this.btnSeleccionar.TabIndex = 2;
            this.btnSeleccionar.Text = "📁 Seleccionar Carpeta";
            this.btnSeleccionar.UseVisualStyleBackColor = false;
            this.btnSeleccionar.Click += new System.EventHandler(this.btnSeleccionar_Click);
            // 
            // txtRutaDBF
            // 
            this.txtRutaDBF.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtRutaDBF.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtRutaDBF.Location = new System.Drawing.Point(15, 55);
            this.txtRutaDBF.Name = "txtRutaDBF";
            this.txtRutaDBF.ReadOnly = true;
            this.txtRutaDBF.Size = new System.Drawing.Size(560, 23);
            this.txtRutaDBF.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.Location = new System.Drawing.Point(15, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ruta de la carpeta con archivos DBF:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnMigrarMovimientos);
            this.groupBox2.Controls.Add(this.btnMigrarClientes);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(20, 130);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(740, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "⚙️ Proceso de Migración";
            // 
            // btnMigrarMovimientos
            // 
            this.btnMigrarMovimientos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnMigrarMovimientos.FlatAppearance.BorderSize = 0;
            this.btnMigrarMovimientos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMigrarMovimientos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMigrarMovimientos.ForeColor = System.Drawing.Color.White;
            this.btnMigrarMovimientos.Location = new System.Drawing.Point(380, 30);
            this.btnMigrarMovimientos.Name = "btnMigrarMovimientos";
            this.btnMigrarMovimientos.Size = new System.Drawing.Size(340, 50);
            this.btnMigrarMovimientos.TabIndex = 1;
            this.btnMigrarMovimientos.Text = "2️⃣ Migrar Movimientos (CTL_MOV.DBF)";
            this.btnMigrarMovimientos.UseVisualStyleBackColor = false;
            this.btnMigrarMovimientos.Click += new System.EventHandler(this.btnMigrarMovimientos_Click);
            // 
            // btnMigrarClientes
            // 
            this.btnMigrarClientes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnMigrarClientes.FlatAppearance.BorderSize = 0;
            this.btnMigrarClientes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMigrarClientes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMigrarClientes.ForeColor = System.Drawing.Color.White;
            this.btnMigrarClientes.Location = new System.Drawing.Point(20, 30);
            this.btnMigrarClientes.Name = "btnMigrarClientes";
            this.btnMigrarClientes.Size = new System.Drawing.Size(340, 50);
            this.btnMigrarClientes.TabIndex = 0;
            this.btnMigrarClientes.Text = "1️⃣ Migrar Clientes (CTL_CLIE.DBF)";
            this.btnMigrarClientes.UseVisualStyleBackColor = false;
            this.btnMigrarClientes.Click += new System.EventHandler(this.btnMigrarClientes_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtLog);
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(20, 240);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(740, 310);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "📋 Log de Proceso";
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.Location = new System.Drawing.Point(15, 30);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(710, 265);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            // 
            // FormMigracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormMigracion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Migración de Datos DBF → SQL Server";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSeleccionar;
        private System.Windows.Forms.TextBox txtRutaDBF;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnMigrarMovimientos;
        private System.Windows.Forms.Button btnMigrarClientes;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox txtLog;
    }
}