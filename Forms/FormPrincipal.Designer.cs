using PaintControl;

namespace PaintControl
{
    partial class FormPrincipal
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

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblHora = new PaintControl.RoundedLabel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.searchPanel = new PaintControl.RoundedPanel();
            this.panelTituloSearch = new System.Windows.Forms.Panel();
            this.lblBuscarCliente = new System.Windows.Forms.Label();
            this.lblCriterio = new System.Windows.Forms.Label();
            this.txtCriterio = new System.Windows.Forms.TextBox();
            this.btnBuscarInline = new PaintControl.RoundedButton();
            this.emptyStatePanel = new System.Windows.Forms.Panel();
            this.lblSinDatos = new System.Windows.Forms.Label();
            this.lblInstruccion = new System.Windows.Forms.Label();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.btnLimpiar = new PaintControl.RoundedButton();
            this.btnArticulos = new PaintControl.RoundedButton();
            this.btnVerClientes = new PaintControl.RoundedButton();
            this.btnTodosMovimientos = new PaintControl.RoundedButton();
            this.btnBases = new PaintControl.RoundedButton();
            this.headerPanel.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.searchPanel.SuspendLayout();
            this.panelTituloSearch.SuspendLayout();
            this.emptyStatePanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.headerPanel.Controls.Add(this.lblTitulo);
            this.headerPanel.Controls.Add(this.lblHora);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(1200, 80);
            this.headerPanel.TabIndex = 0;
            this.headerPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.HeaderPanel_Paint);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(92)))), ((int)(((byte)(138)))));
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(500, 50);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "🎨 DOAL - Control de Pintura";
            // 
            // lblHora
            // 
            this.lblHora.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHora.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(123)))), ((int)(((byte)(157)))));
            this.lblHora.CornerRadius = 12;
            this.lblHora.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblHora.ForeColor = System.Drawing.Color.White;
            this.lblHora.Location = new System.Drawing.Point(1050, 23);
            this.lblHora.Name = "lblHora";
            this.lblHora.Size = new System.Drawing.Size(140, 38);
            this.lblHora.TabIndex = 1;
            this.lblHora.Text = "11:50 AM";
            this.lblHora.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.BackColor = System.Drawing.Color.Transparent;
            this.mainPanel.Controls.Add(this.searchPanel);
            this.mainPanel.Controls.Add(this.emptyStatePanel);
            this.mainPanel.Location = new System.Drawing.Point(0, 80);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.mainPanel.Size = new System.Drawing.Size(1200, 540);
            this.mainPanel.TabIndex = 1;
            // 
            // searchPanel
            // 
            this.searchPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(200)))), ((int)(((byte)(225)))));
            this.searchPanel.Controls.Add(this.panelTituloSearch);
            this.searchPanel.Controls.Add(this.lblCriterio);
            this.searchPanel.Controls.Add(this.txtCriterio);
            this.searchPanel.Controls.Add(this.btnBuscarInline);
            this.searchPanel.CornerRadius = 20;
            this.searchPanel.Location = new System.Drawing.Point(20, 15);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(1160, 120);
            this.searchPanel.TabIndex = 0;
            this.searchPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.SearchPanel_Paint);
            // 
            // panelTituloSearch
            // 
            this.panelTituloSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            //this.panelTituloSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(170)))), ((int)(((byte)(205)))));
            this.panelTituloSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(143)))), ((int)(((byte)(208)))));

            this.panelTituloSearch.Controls.Add(this.lblBuscarCliente);
            this.panelTituloSearch.Location = new System.Drawing.Point(0, 0);
            this.panelTituloSearch.Name = "panelTituloSearch";
            this.panelTituloSearch.Size = new System.Drawing.Size(1160, 50);
            this.panelTituloSearch.TabIndex = 0;
            // 
            // lblBuscarCliente
            // 
            this.lblBuscarCliente.AutoSize = true;
            this.lblBuscarCliente.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblBuscarCliente.ForeColor = System.Drawing.Color.White;
            this.lblBuscarCliente.Location = new System.Drawing.Point(15, 10);
            this.lblBuscarCliente.Name = "lblBuscarCliente";
            this.lblBuscarCliente.Size = new System.Drawing.Size(217, 32);
            this.lblBuscarCliente.TabIndex = 0;
            this.lblBuscarCliente.Text = "🔍 Buscar Cliente";
            // 
            // lblCriterio
            // 
            this.lblCriterio.AutoSize = true;
            this.lblCriterio.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCriterio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(92)))), ((int)(((byte)(138)))));
            this.lblCriterio.Location = new System.Drawing.Point(15, 70);
            this.lblCriterio.Name = "lblCriterio";
            this.lblCriterio.Size = new System.Drawing.Size(165, 25);
            this.lblCriterio.TabIndex = 1;
            this.lblCriterio.Text = "Nombre o Código:";
            // 
            // txtCriterio
            // 
            this.txtCriterio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCriterio.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCriterio.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtCriterio.Location = new System.Drawing.Point(185, 67);
            this.txtCriterio.Name = "txtCriterio";
            this.txtCriterio.Size = new System.Drawing.Size(770, 32);
            this.txtCriterio.TabIndex = 2;
            // 
            // btnBuscarInline
            // 
            this.btnBuscarInline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuscarInline.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(143)))), ((int)(((byte)(208)))));
            this.btnBuscarInline.CornerRadius = 12;
            this.btnBuscarInline.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscarInline.FlatAppearance.BorderSize = 0;
            this.btnBuscarInline.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscarInline.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnBuscarInline.ForeColor = System.Drawing.Color.White;
            this.btnBuscarInline.Location = new System.Drawing.Point(970, 63);
            this.btnBuscarInline.Name = "btnBuscarInline";
            this.btnBuscarInline.Size = new System.Drawing.Size(170, 40);
            this.btnBuscarInline.TabIndex = 3;
            this.btnBuscarInline.Text = "🔍 Buscar";
            this.btnBuscarInline.UseVisualStyleBackColor = false;
            // 
            // emptyStatePanel
            // 
            this.emptyStatePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.emptyStatePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.emptyStatePanel.Controls.Add(this.lblSinDatos);
            this.emptyStatePanel.Controls.Add(this.lblInstruccion);
            this.emptyStatePanel.Location = new System.Drawing.Point(20, 150);
            this.emptyStatePanel.Name = "emptyStatePanel";
            this.emptyStatePanel.Size = new System.Drawing.Size(1160, 375);
            this.emptyStatePanel.TabIndex = 1;
            this.emptyStatePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.EmptyStatePanel_Paint);
            // 
            // lblSinDatos
            // 
            this.lblSinDatos.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblSinDatos.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblSinDatos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(155)))), ((int)(((byte)(168)))));
            this.lblSinDatos.Location = new System.Drawing.Point(0, 155);
            this.lblSinDatos.Name = "lblSinDatos";
            this.lblSinDatos.Size = new System.Drawing.Size(1160, 35);
            this.lblSinDatos.TabIndex = 0;
            this.lblSinDatos.Text = "📁 Sin Cliente Seleccionado";
            this.lblSinDatos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInstruccion
            // 
            this.lblInstruccion.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblInstruccion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblInstruccion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(176)))), ((int)(((byte)(192)))));
            this.lblInstruccion.Location = new System.Drawing.Point(0, 200);
            this.lblInstruccion.Name = "lblInstruccion";
            this.lblInstruccion.Size = new System.Drawing.Size(1160, 25);
            this.lblInstruccion.TabIndex = 1;
            this.lblInstruccion.Text = "Busque un cliente por nombre o código para ver sus movimientos";
            this.lblInstruccion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(241)))), ((int)(((byte)(248)))));
            this.bottomPanel.Controls.Add(this.btnLimpiar);
            this.bottomPanel.Controls.Add(this.btnArticulos);
            this.bottomPanel.Controls.Add(this.btnVerClientes);
            this.bottomPanel.Controls.Add(this.btnTodosMovimientos);
            this.bottomPanel.Controls.Add(this.btnBases);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 620);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Padding = new System.Windows.Forms.Padding(20, 12, 20, 12);
            this.bottomPanel.Size = new System.Drawing.Size(1200, 70);
            this.bottomPanel.TabIndex = 2;
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(195)))), ((int)(((byte)(74)))));
            this.btnLimpiar.CornerRadius = 15;
            this.btnLimpiar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpiar.FlatAppearance.BorderSize = 0;
            this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiar.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLimpiar.ForeColor = System.Drawing.Color.White;
            this.btnLimpiar.Location = new System.Drawing.Point(20, 12);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(180, 46);
            this.btnLimpiar.TabIndex = 0;
            this.btnLimpiar.Text = "🗑️ Limpiar (ESC)";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            // 
            // btnArticulos
            // 
            this.btnArticulos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnArticulos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnArticulos.CornerRadius = 15;
            this.btnArticulos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnArticulos.FlatAppearance.BorderSize = 0;
            this.btnArticulos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArticulos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnArticulos.ForeColor = System.Drawing.Color.White;
            this.btnArticulos.Location = new System.Drawing.Point(455, 12);
            this.btnArticulos.Name = "btnArticulos";
            this.btnArticulos.Size = new System.Drawing.Size(170, 46);
            this.btnArticulos.TabIndex = 1;
            this.btnArticulos.Text = "📦 Artículos (F10)";
            this.btnArticulos.UseVisualStyleBackColor = false;
            // 
            // btnVerClientes
            // 
            this.btnVerClientes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVerClientes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnVerClientes.CornerRadius = 15;
            this.btnVerClientes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVerClientes.FlatAppearance.BorderSize = 0;
            this.btnVerClientes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVerClientes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnVerClientes.ForeColor = System.Drawing.Color.White;
            this.btnVerClientes.Location = new System.Drawing.Point(640, 12);
            this.btnVerClientes.Name = "btnVerClientes";
            this.btnVerClientes.Size = new System.Drawing.Size(170, 46);
            this.btnVerClientes.TabIndex = 2;
            this.btnVerClientes.Text = "👥 Clientes (F9)";
            this.btnVerClientes.UseVisualStyleBackColor = false;
            // 
            // btnTodosMovimientos
            // 
            this.btnTodosMovimientos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTodosMovimientos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnTodosMovimientos.CornerRadius = 15;
            this.btnTodosMovimientos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTodosMovimientos.FlatAppearance.BorderSize = 0;
            this.btnTodosMovimientos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTodosMovimientos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTodosMovimientos.ForeColor = System.Drawing.Color.White;
            this.btnTodosMovimientos.Location = new System.Drawing.Point(820, 12);
            this.btnTodosMovimientos.Name = "btnTodosMovimientos";
            this.btnTodosMovimientos.Size = new System.Drawing.Size(180, 46);
            this.btnTodosMovimientos.TabIndex = 3;
            this.btnTodosMovimientos.Text = "📋 Movimientos (F8)";
            this.btnTodosMovimientos.UseVisualStyleBackColor = false;
            // 
            // btnBases
            // 
            this.btnBases.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBases.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnBases.CornerRadius = 15;
            this.btnBases.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBases.FlatAppearance.BorderSize = 0;
            this.btnBases.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBases.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnBases.ForeColor = System.Drawing.Color.White;
            this.btnBases.Location = new System.Drawing.Point(1015, 12);
            this.btnBases.Name = "btnBases";
            this.btnBases.Size = new System.Drawing.Size(165, 46);
            this.btnBases.TabIndex = 4;
            this.btnBases.Text = "🎨 Bases (F11)";
            this.btnBases.UseVisualStyleBackColor = false;
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(241)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(1200, 690);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(950, 600);
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DOAL - Sistema de Clientes";
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            this.panelTituloSearch.ResumeLayout(false);
            this.panelTituloSearch.PerformLayout();
            this.emptyStatePanel.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitulo;
        private RoundedLabel lblHora;
        private System.Windows.Forms.Panel mainPanel;
        private RoundedPanel searchPanel;
        private System.Windows.Forms.Panel panelTituloSearch;
        private System.Windows.Forms.Label lblBuscarCliente;
        private System.Windows.Forms.Label lblCriterio;
        private System.Windows.Forms.TextBox txtCriterio;
        private RoundedButton btnBuscarInline;
        private System.Windows.Forms.Panel emptyStatePanel;
        private System.Windows.Forms.Label lblSinDatos;
        private System.Windows.Forms.Label lblInstruccion;
        private System.Windows.Forms.Panel bottomPanel;
        private RoundedButton btnLimpiar;
        private RoundedButton btnVerClientes;
        private RoundedButton btnArticulos;
        private RoundedButton btnTodosMovimientos;
        private RoundedButton btnBases;
    }
}