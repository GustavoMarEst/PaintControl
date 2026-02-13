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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
            this.headerPanel = new System.Windows.Forms.Panel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblTituloTexto = new System.Windows.Forms.Label();
            this.lblHora = new PaintControl.RoundedLabel();
            this.lblTitulo = new System.Windows.Forms.Label();
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
            this.headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.mainPanel.SuspendLayout();
            this.searchPanel.SuspendLayout();
            this.panelTituloSearch.SuspendLayout();
            this.emptyStatePanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(227)))), ((int)(((byte)(248)))));
            this.headerPanel.Controls.Add(this.picLogo);
            this.headerPanel.Controls.Add(this.lblTituloTexto);
            this.headerPanel.Controls.Add(this.lblHora);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(1200, 80);
            this.headerPanel.TabIndex = 0;
            this.headerPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.HeaderPanel_Paint);
            // 
            // picLogo
            // 
            this.picLogo.Image = global::PaintControl.Properties.Resources.logo_doal;
            this.picLogo.Location = new System.Drawing.Point(20, 14);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(180, 55);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 10;
            this.picLogo.TabStop = false;
            // 
            // lblTituloTexto
            // 
            this.lblTituloTexto.AutoSize = true;
            this.lblTituloTexto.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTituloTexto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(92)))), ((int)(((byte)(138)))));
            this.lblTituloTexto.Location = new System.Drawing.Point(300, 22);
            this.lblTituloTexto.Name = "lblTituloTexto";
            this.lblTituloTexto.Size = new System.Drawing.Size(324, 46);
            this.lblTituloTexto.TabIndex = 11;
            this.lblTituloTexto.Text = "Control de Clientes";
            // 
            // lblHora
            // 
            this.lblHora.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHora.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(164)))), ((int)(((byte)(231)))));
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
            // lblTitulo
            // 
            this.lblTitulo.Location = new System.Drawing.Point(0, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(100, 23);
            this.lblTitulo.TabIndex = 0;
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
            this.searchPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(227)))), ((int)(((byte)(248)))));
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
            this.panelTituloSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(164)))), ((int)(((byte)(231)))));
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
            this.lblCriterio.Size = new System.Drawing.Size(178, 25);
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
            this.btnBuscarInline.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(164)))), ((int)(((byte)(231)))));
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
            this.emptyStatePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))));
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
            this.lblSinDatos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(148)))), ((int)(((byte)(166)))));
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
            this.lblInstruccion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(148)))), ((int)(((byte)(166)))));
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
            this.btnArticulos.Location = new System.Drawing.Point(640, 12);
            this.btnArticulos.Name = "btnArticulos";
            this.btnArticulos.Size = new System.Drawing.Size(170, 46);
            this.btnArticulos.TabIndex = 1;
            this.btnArticulos.Text = "📦 Catálogo (F10)";
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
            this.btnVerClientes.Location = new System.Drawing.Point(825, 12);
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
            this.btnTodosMovimientos.Location = new System.Drawing.Point(1010, 12);
            this.btnTodosMovimientos.Name = "btnTodosMovimientos";
            this.btnTodosMovimientos.Size = new System.Drawing.Size(180, 46);
            this.btnTodosMovimientos.TabIndex = 3;
            this.btnTodosMovimientos.Text = "📋 Movimientos (F8)";
            this.btnTodosMovimientos.UseVisualStyleBackColor = false;
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(950, 600);
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DOAL - Sistema de Clientes";
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
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

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblTituloTexto;


    }
}