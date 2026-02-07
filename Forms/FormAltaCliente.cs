using PaintControl.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PaintControl.Forms
{
    public partial class FormAltaCliente : Form
    {
        public Cliente ClienteCreado { get; private set; }

        public FormAltaCliente(string nombreSugerido = "")
        {
            InitializeComponent();
            ConfigurarFormulario(nombreSugerido);
        }

        private void ConfigurarFormulario(string nombreSugerido)
        {
            this.Text = "Alta de Nuevo Cliente";
            this.Size = new Size(500, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30),
                BackColor = Color.White
            };

            // Título
            Label lblTitulo = new Label
            {
                Text = "Registrar Nuevo Cliente",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Location = new Point(30, 20),
                AutoSize = true,
                ForeColor = Color.FromArgb(46, 92, 138)
            };

            // Mensaje informativo
            Label lblInfo = new Label
            {
                Text = "Los datos del cliente (ID y fecha) se generarán automáticamente.",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(30, 60),
                AutoSize = true,
                ForeColor = Color.Gray
            };

            // Nombre
            Label lblNombre = new Label
            {
                Text = "Nombre del Cliente:",
                Location = new Point(30, 95),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            TextBox txtNombre = new TextBox
            {
                Name = "txtNombre",
                Location = new Point(30, 120),
                Width = 420,
                Font = new Font("Segoe UI", 11F),
                CharacterCasing = CharacterCasing.Upper,
                Text = nombreSugerido
            };

            // Línea separadora
            Panel separador = new Panel
            {
                Location = new Point(30, 160),
                Size = new Size(420, 1),
                BackColor = Color.LightGray
            };

            // Botones
            Button btnGuardar = new Button
            {
                Text = "✓ Guardar",
                Location = new Point(260, 175),
                Size = new Size(110, 40),
                BackColor = Color.FromArgb(74, 143, 208),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;

            Button btnCancelar = new Button
            {
                Text = "✗ Cancelar",
                Location = new Point(380, 175),
                Size = new Size(110, 40),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;

            // Efecto hover en botones
            btnGuardar.MouseEnter += (s, e) => btnGuardar.BackColor = Color.FromArgb(94, 163, 228);
            btnGuardar.MouseLeave += (s, e) => btnGuardar.BackColor = Color.FromArgb(74, 143, 208);
            btnCancelar.MouseEnter += (s, e) => btnCancelar.BackColor = Color.DimGray;
            btnCancelar.MouseLeave += (s, e) => btnCancelar.BackColor = Color.Gray;

            // Eventos
            btnGuardar.Click += (s, e) =>
            {
                string nombre = txtNombre.Text.Trim();

                if (string.IsNullOrWhiteSpace(nombre))
                {
                    MessageBox.Show("Por favor ingrese el nombre del cliente.",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNombre.Focus();
                    return;
                }

                // Crear el cliente solo con el nombre
                // El ID, código y fecha se generarán automáticamente en el servicio
                ClienteCreado = new Cliente
                {
                    Nombre = nombre,
                    //Telefono = "",
                    //Email = "",
                    //Direccion = ""
                };

                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            btnCancelar.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            // Permitir Enter para guardar
            txtNombre.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    btnGuardar.PerformClick();
                    e.Handled = true;
                }
            };

            // Agregar controles al panel
            mainPanel.Controls.AddRange(new Control[]
            {
                lblTitulo, lblInfo, lblNombre, txtNombre,
                separador, btnGuardar, btnCancelar
            });

            this.Controls.Add(mainPanel);

            // Establecer foco en el campo de nombre
            this.Shown += (s, e) => txtNombre.Focus();
        }
    }
}