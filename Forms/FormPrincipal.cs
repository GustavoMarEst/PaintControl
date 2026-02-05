using PaintControl.Data;
using PaintControl.Forms;
using PaintControl.Models;
using PaintControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PaintControl
{
    public partial class FormPrincipal : Form
    {
        private ClienteService clienteService;
        private MovimientoService movimientoService;
        private Cliente clienteActual;
        private Timer relojTimer;
        private DataGridView dgvMovimientos;
        private Panel panelMovimientos;
        private RoundedButton btnAgregarMovimiento;

        // Controles de filtrado de fecha (solo para el panel de movimientos)
        private Label lblFechaInicio;
        private DateTimePicker dtpFechaInicio;
        private Label lblFechaFin;
        private DateTimePicker dtpFechaFin;
        private CheckBox chkFiltrarFecha;

        public FormPrincipal()
        {
            InitializeComponent();
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            // Inicializar servicios
            clienteService = new ClienteService();
            movimientoService = new MovimientoService();

            // Configurar el reloj
            ConfigurarReloj();

            // Agregar eventos a los controles
            ConfigurarEventos();

            // Evento para ajustar controles al redimensionar
            this.Resize += FormPrincipal_Resize;

            // Establecer el foco en el campo de búsqueda
            txtCriterio.Focus();
        }

        // ELIMINAR DESPUES 
        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            // PRUEBA DE CONEXIÓN A LA BASE DE DATOS
            try
            {
                using (var context = new DOALDbContext())
                {
                    bool conectado = context.Database.Exists();

                    if (conectado)
                    {
                        MessageBox.Show("✓ Conexión exitosa a la base de datos\n\nLa base de datos está lista para usar.",
                            "Conexión Exitosa",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("✗ La base de datos no existe.\n\nSe creará automáticamente.",
                            "Aviso",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        // Crear la base de datos
                        context.Database.Create();

                        MessageBox.Show("✓ Base de datos creada exitosamente.",
                            "Éxito",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error de conexión a la base de datos:\n\n{ex.Message}\n\nDetalles:\n{ex.InnerException?.Message}",
                    "Error de Conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }






        private void FormPrincipal_Resize(object sender, EventArgs e)
        {
            // Ajustar el panel de movimientos cuando se redimensione
            if (panelMovimientos != null && panelMovimientos.Visible)
            {
                AjustarPanelMovimientos();
            }
        }

        private void AjustarPanelMovimientos()
        {
            if (panelMovimientos == null) return;

            int anchoDisponible = mainPanel.ClientSize.Width - 40;
            int altoDisponible = mainPanel.ClientSize.Height - 145;

            panelMovimientos.Location = new Point(20, 130);
            panelMovimientos.Size = new Size(anchoDisponible, altoDisponible);

            if (dgvMovimientos != null)
            {
                dgvMovimientos.Size = new Size(anchoDisponible - 40, altoDisponible - 130);
            }

            if (btnAgregarMovimiento != null)
            {
                btnAgregarMovimiento.Location = new Point(anchoDisponible - 220, 10);
            }
        }

        private void ConfigurarReloj()
        {
            relojTimer = new Timer();
            relojTimer.Interval = 1000;
            relojTimer.Tick += RelojTimer_Tick;
            relojTimer.Start();
            ActualizarHora();
        }

        private void RelojTimer_Tick(object sender, EventArgs e)
        {
            ActualizarHora();
        }

        private void ActualizarHora()
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm tt");
        }

        private void ConfigurarEventos()
        {
            // Eventos del campo de búsqueda
            txtCriterio.KeyPress += TxtCriterio_KeyPress;
            txtCriterio.TextChanged += TxtCriterio_TextChanged;

            // Eventos de los botones
            btnBuscarInline.Click += BtnBuscarCliente_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            btnVerClientes.Click += BtnVerClientes_Click;
            btnTodosMovimientos.Click += BtnTodosMovimientos_Click;
            btnArticulos.Click += BtnArticulos_Click;
            btnBases.Click += BtnBases_Click;

            // Atajos de teclado
            this.KeyPreview = true;
            this.KeyDown += FormPrincipal_KeyDown;

            // Evento de cierre del formulario
            this.FormClosing += (s, e) =>
            {
                if (relojTimer != null)
                {
                    relojTimer.Stop();
                    relojTimer.Dispose();
                }
            };
        }

        private void ChkFiltrarFecha_CheckedChanged(object sender, EventArgs e)
        {
            dtpFechaInicio.Enabled = chkFiltrarFecha.Checked;
            dtpFechaFin.Enabled = chkFiltrarFecha.Checked;

            if (chkFiltrarFecha.Checked)
            {
                // Establecer fechas por defecto: último mes
                dtpFechaInicio.Value = DateTime.Now.AddMonths(-1).Date;
                dtpFechaFin.Value = DateTime.Now.Date;
            }

            // Recargar movimientos si hay un cliente seleccionado
            if (clienteActual != null)
            {
                CargarMovimientos();
            }
        }

        private void FormPrincipal_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                //case Keys.F2:
                //    BuscarCliente();
                //    break;
                case Keys.F8:
                    MostrarTodosMovimientos();
                    break;
                case Keys.F9:
                    MostrarListaClientes();
                    break;
                case Keys.F10:
                    MessageBox.Show("Módulo de Artículos en desarrollo...",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case Keys.F11:
                    MessageBox.Show("Módulo de Bases en desarrollo...",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case Keys.Escape:
                    LimpiarFormulario();
                    break;
            }
        }

        private void TxtCriterio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BuscarCliente();
                e.Handled = true;
            }
        }

        private void TxtCriterio_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCriterio.Text))
            {
                // No limpiar automáticamente al vaciar el campo
            }
        }

        private void BtnBuscarCliente_Click(object sender, EventArgs e)
        {
            BuscarCliente();
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void BuscarCliente()
        {
            string criterio = txtCriterio.Text.Trim();

            if (string.IsNullOrWhiteSpace(criterio))
            {
                MessageBox.Show("Por favor ingrese un nombre o código de cliente.",
                    "Búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCriterio.Focus();
                return;
            }

            List<Cliente> resultados = clienteService.BuscarPorCodigoONombre(criterio);

            if (resultados.Count == 0)
            {
                // NO SE ENCONTRÓ - PREGUNTAR SI QUIERE CREAR NUEVO CLIENTE
                var respuesta = MessageBox.Show(
                    $"No se encontró ningún cliente con '{criterio}'.{Environment.NewLine}{Environment.NewLine}¿Desea crear un nuevo cliente?",
                    "Cliente no encontrado",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (respuesta == DialogResult.Yes)
                {
                    CrearNuevoCliente(criterio);
                }
                return;
            }
            else if (resultados.Count == 1)
            {
                SeleccionarCliente(resultados[0]);
            }
            else
            {
                MostrarSeleccionClientes(resultados);
            }
        }

        private void MostrarSeleccionClientes(List<Cliente> clientes)
        {
            Form seleccionForm = new Form
            {
                Text = "Seleccione un Cliente",
                Size = new Size(700, 400),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblInfo = new Label
            {
                Text = "Se encontraron múltiples clientes. Seleccione uno:",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F)
            };

            DataGridView dgv = new DataGridView
            {
                Location = new Point(20, 50),
                Size = new Size(640, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            dgv.Columns.Add("Codigo", "Código");
            dgv.Columns.Add("Nombre", "Nombre");
            //dgv.Columns.Add("Telefono", "Teléfono");

            foreach (var cliente in clientes)
            {
                dgv.Rows.Add(cliente.Codigo, cliente.Nombre);
                dgv.Rows[dgv.Rows.Count - 1].Tag = cliente;
            }

            Button btnSeleccionar = new Button
            {
                Text = "Seleccionar",
                Location = new Point(460, 310),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(74, 143, 208),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSeleccionar.FlatAppearance.BorderSize = 0;

            Button btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(570, 310),
                Size = new Size(100, 35),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar.FlatAppearance.BorderSize = 0;

            btnSeleccionar.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    Cliente clienteSeleccionado = (Cliente)dgv.SelectedRows[0].Tag;
                    SeleccionarCliente(clienteSeleccionado);
                    seleccionForm.Close();
                }
            };

            btnCancelar.Click += (s, e) => seleccionForm.Close();

            dgv.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    Cliente clienteSeleccionado = (Cliente)dgv.Rows[e.RowIndex].Tag;
                    SeleccionarCliente(clienteSeleccionado);
                    seleccionForm.Close();
                }
            };

            seleccionForm.Controls.Add(lblInfo);
            seleccionForm.Controls.Add(dgv);
            seleccionForm.Controls.Add(btnSeleccionar);
            seleccionForm.Controls.Add(btnCancelar);

            seleccionForm.ShowDialog(this);
        }

        private void SeleccionarCliente(Cliente cliente)
        {
            clienteActual = cliente;
            emptyStatePanel.Visible = false;
            searchPanel.BackColor = Color.FromArgb(124, 184, 111);
            MostrarMovimientos();
        }

        private void MostrarMovimientos()
        {
            if (panelMovimientos == null)
            {
                CrearPanelMovimientos();
            }

            panelMovimientos.Visible = true;
            panelMovimientos.BringToFront();
            AjustarPanelMovimientos();
            CargarMovimientos();
        }

        private void CrearPanelMovimientos()
        {
            int anchoDisponible = mainPanel.ClientSize.Width - 40;
            int altoDisponible = mainPanel.ClientSize.Height - 145;

            panelMovimientos = new Panel
            {
                Location = new Point(20, 130),
                Size = new Size(anchoDisponible, altoDisponible),
                BackColor = Color.White,
                Name = "panelMovimientos",
                Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                    | AnchorStyles.Left)
                    | AnchorStyles.Right)))
            };

            Label lblClienteInfo = new Label
            {
                Name = "lblClienteInfo",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(15, 12),
                AutoSize = true,
                ForeColor = Color.FromArgb(46, 92, 138)
            };

            // Crear controles de filtrado por fecha (dentro del panel de movimientos)
            lblFechaInicio = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(15, 47),
                Text = "Fecha Inicio:"
            };

            dtpFechaInicio = new DateTimePicker
            {
                Enabled = false,
                Font = new Font("Segoe UI", 9.5F),
                Format = DateTimePickerFormat.Short,
                Location = new Point(110, 45),
                Size = new Size(150, 29)
            };

            lblFechaFin = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(280, 47),
                Text = "Fecha Fin:"
            };

            dtpFechaFin = new DateTimePicker
            {
                Enabled = false,
                Font = new Font("Segoe UI", 9.5F),
                Format = DateTimePickerFormat.Short,
                Location = new Point(360, 45),
                Size = new Size(150, 29)
            };

            chkFiltrarFecha = new CheckBox
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(530, 47),
                Text = "Filtrar por fecha"
            };

            chkFiltrarFecha.CheckedChanged += ChkFiltrarFecha_CheckedChanged;

            // Agregar eventos a los DateTimePickers para recargar al cambiar fecha
            dtpFechaInicio.ValueChanged += (s, e) =>
            {
                if (chkFiltrarFecha.Checked && clienteActual != null)
                {
                    CargarMovimientos();
                }
            };

            dtpFechaFin.ValueChanged += (s, e) =>
            {
                if (chkFiltrarFecha.Checked && clienteActual != null)
                {
                    CargarMovimientos();
                }
            };

            btnAgregarMovimiento = new RoundedButton
            {
                Text = "➕ Agregar Compra",
                Location = new Point(anchoDisponible - 220, 10),
                Size = new Size(200, 38),
                BackColor = Color.FromArgb(74, 143, 208),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                CornerRadius = 10,
                Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)))
            };
            btnAgregarMovimiento.Click += BtnAgregarMovimiento_Click;

            dgvMovimientos = new DataGridView
            {
                Location = new Point(15, 85),
                Size = new Size(anchoDisponible - 40, altoDisponible - 130),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                    | AnchorStyles.Left)
                    | AnchorStyles.Right))),
                Font = new Font("Segoe UI", 14F),
                RowTemplate = { Height = 35 },
                ColumnHeadersHeight = 40,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(74, 143, 208),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    SelectionBackColor = Color.FromArgb(155, 185, 210),
                    SelectionForeColor = Color.White,
                    Font = new Font("Segoe UI", 9.5F),
                    Padding = new Padding(5),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(245, 248, 250)
                },
                EnableHeadersVisualStyles = false,
                GridColor = Color.LightGray,
            };

            dgvMovimientos.Columns.Add("NumMov", "No. Mov.");
            dgvMovimientos.Columns.Add("Fecha", "Fecha");
            dgvMovimientos.Columns.Add("Clave", "Clave Color");
            dgvMovimientos.Columns.Add("Descripcion", "Descripción");
            dgvMovimientos.Columns.Add("Base", "Base");
            dgvMovimientos.Columns.Add("Unidad", "Unidad");
            dgvMovimientos.Columns.Add("Cantidad", "Cantidad");
            dgvMovimientos.Columns.Add("Precio", "Precio");
            dgvMovimientos.Columns.Add("Formula", "Fórmula");

            // Evento para mantener el color del encabezado fijo
            dgvMovimientos.CellPainting += (s, e) =>
            {
                if (e.RowIndex == -1 && e.ColumnIndex >= 0)
                {
                    e.PaintBackground(e.CellBounds, true);
                    using (Brush brush = new SolidBrush(Color.FromArgb(74, 143, 208)))
                    {
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                    }
                    using (Brush textBrush = new SolidBrush(Color.White))
                    {
                        StringFormat sf = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };
                        e.Graphics.DrawString(e.Value?.ToString() ?? "",
                            new Font("Segoe UI", 11F, FontStyle.Bold),
                            textBrush,
                            e.CellBounds,
                            sf);
                    }
                    e.Handled = true;
                }
            };

            dgvMovimientos.AllowUserToResizeColumns = false;
            dgvMovimientos.AllowUserToResizeRows = false;
            dgvMovimientos.AllowUserToOrderColumns = false;

            dgvMovimientos.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            foreach (DataGridViewColumn col in dgvMovimientos.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dgvMovimientos.Columns["NumMov"].FillWeight = 60;
            dgvMovimientos.Columns["Fecha"].FillWeight = 70;
            dgvMovimientos.Columns["Clave"].FillWeight = 80;
            dgvMovimientos.Columns["Descripcion"].FillWeight = 150;
            dgvMovimientos.Columns["Base"].FillWeight = 60;
            dgvMovimientos.Columns["Unidad"].FillWeight = 60;
            dgvMovimientos.Columns["Cantidad"].FillWeight = 60;
            dgvMovimientos.Columns["Precio"].FillWeight = 70;
            dgvMovimientos.Columns["Formula"].FillWeight = 120;

            dgvMovimientos.Columns["Cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvMovimientos.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvMovimientos.Columns["Precio"].DefaultCellStyle.Format = "C2";

            // Evento de doble clic para ver detalle
            dgvMovimientos.CellDoubleClick += DgvMovimientos_CellDoubleClick;

            panelMovimientos.Controls.Add(lblClienteInfo);
            panelMovimientos.Controls.Add(lblFechaInicio);
            panelMovimientos.Controls.Add(dtpFechaInicio);
            panelMovimientos.Controls.Add(lblFechaFin);
            panelMovimientos.Controls.Add(dtpFechaFin);
            panelMovimientos.Controls.Add(chkFiltrarFecha);
            panelMovimientos.Controls.Add(btnAgregarMovimiento);
            panelMovimientos.Controls.Add(dgvMovimientos);

            mainPanel.Controls.Add(panelMovimientos);
        }

        private void CargarMovimientos()
        {
            if (dgvMovimientos == null || clienteActual == null)
                return;

            Label lblClienteInfo = panelMovimientos.Controls.Find("lblClienteInfo", false)[0] as Label;
            lblClienteInfo.Text = $"Movimientos de: {clienteActual.Nombre} ({clienteActual.Codigo})";

            dgvMovimientos.Rows.Clear();

            List<Movimiento> movimientos;

            if (chkFiltrarFecha != null && chkFiltrarFecha.Checked)
            {
                DateTime fechaInicio = dtpFechaInicio.Value.Date; // Inicio del día (00:00:00)
                DateTime fechaFin = dtpFechaFin.Value.Date.AddDays(1).AddSeconds(-1); // Fin del día (23:59:59)

                movimientos = movimientoService.ObtenerPorClienteYFechas(
                    clienteActual.Id,
                    fechaInicio,
                    fechaFin
                );

                lblClienteInfo.Text += $" (Filtrado: {fechaInicio.ToShortDateString()} - {dtpFechaFin.Value.ToShortDateString()})";
            }
            else
            {
                movimientos = movimientoService.ObtenerPorCliente(clienteActual.Id);
            }

            foreach (var mov in movimientos)
            {
                dgvMovimientos.Rows.Add(
                    mov.NumeroMovimiento,
                    mov.Fecha.ToShortDateString(),
                    mov.ClaveColor,
                    mov.Descripcion,
                    mov.Base,
                    mov.Unidad,
                    mov.Cantidad,
                    mov.Precio,
                    mov.Formula
                );
            }

            if (movimientos.Count == 0)
            {
                if (chkFiltrarFecha != null && chkFiltrarFecha.Checked)
                {
                    lblClienteInfo.Text += " - Sin movimientos en el rango de fechas seleccionado";
                }
                else
                {
                    lblClienteInfo.Text += " - Sin movimientos registrados";
                }
            }
            else
            {
                decimal totalGeneral = movimientos.Sum(m => m.Total);
                lblClienteInfo.Text += $" - Total: {totalGeneral:C2} ({movimientos.Count} movimiento{(movimientos.Count > 1 ? "s" : "")})";
            }
        }

        private void BtnAgregarMovimiento_Click(object sender, EventArgs e)
        {
            if (clienteActual == null)
                return;

            int siguienteNumero = movimientoService.ObtenerSiguienteNumeroMovimiento();
            FormAgregarMovimiento formMovimiento = new FormAgregarMovimiento(clienteActual, siguienteNumero);

            if (formMovimiento.ShowDialog(this) == DialogResult.OK)
            {
                bool exito = movimientoService.AgregarMovimiento(formMovimiento.MovimientoCreado);

                if (exito)
                {
                    MessageBox.Show("La compra se registró correctamente.",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarMovimientos();
                }
                else
                {
                    MessageBox.Show("Hubo un error al registrar la compra.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LimpiarFormulario()
        {
            txtCriterio.Text = "";
            clienteActual = null;
            emptyStatePanel.Visible = true;

            if (chkFiltrarFecha != null)
            {
                chkFiltrarFecha.Checked = false;
            }

            if (panelMovimientos != null)
            {
                panelMovimientos.Visible = false;
            }

            searchPanel.BackColor = Color.FromArgb(168, 200, 225);
            txtCriterio.Focus();
        }

        private void BtnVerClientes_Click(object sender, EventArgs e)
        {
            MostrarListaClientes();
        }

        private void BtnTodosMovimientos_Click(object sender, EventArgs e)
        {
            MostrarTodosMovimientos();
        }

        private void BtnArticulos_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Módulo de Artículos en desarrollo...",
                "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnBases_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Módulo de Bases en desarrollo...",
                "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MostrarListaClientes()
        {
            Form listaForm = new Form
            {
                Text = "Lista de Clientes",
                Size = new Size(900, 600),
                StartPosition = FormStartPosition.CenterParent
            };

            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(10)
            };

            Label lblOrdenar = new Label
            {
                Text = "Ordenar por:",
                Location = new Point(10, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold)
            };

            ComboBox cmbOrdenar = new ComboBox
            {
                Location = new Point(160, 12),
                Width = 260,
                Height = 35,
                Font = new Font("Segoe UI", 13F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbOrdenar.Items.AddRange(new object[] {
                "Nombre (A-Z)",
                "Nombre (Z-A)",
                "Más reciente",
                "Más antiguo"
            });
            cmbOrdenar.SelectedIndex = 0;

            topPanel.Controls.Add(lblOrdenar);
            topPanel.Controls.Add(cmbOrdenar);

            DataGridView dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 14F),
                RowTemplate = { Height = 35 },
                ColumnHeadersHeight = 40,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(74, 143, 208),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    SelectionBackColor = Color.FromArgb(155, 185, 210),
                    SelectionForeColor = Color.White,
                    Font = new Font("Segoe UI", 12F),
                    Padding = new Padding(5),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(245, 248, 250)
                },
                EnableHeadersVisualStyles = false,
                BorderStyle = BorderStyle.Fixed3D,
                GridColor = Color.LightGray,
            };

            dgv.Columns.Add("Codigo", "Código");
            dgv.Columns.Add("Nombre", "Nombre");
            dgv.Columns.Add("FechaRegistro", "Fecha de Registro");

            dgv.CellPainting += (s, e) =>
            {
                if (e.RowIndex == -1 && e.ColumnIndex >= 0)
                {
                    e.PaintBackground(e.CellBounds, true);
                    using (Brush brush = new SolidBrush(Color.FromArgb(74, 143, 208)))
                    {
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                    }
                    using (Brush textBrush = new SolidBrush(Color.White))
                    {
                        StringFormat sf = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };
                        e.Graphics.DrawString(e.Value?.ToString() ?? "",
                            new Font("Segoe UI", 13F, FontStyle.Bold),
                            textBrush,
                            e.CellBounds,
                            sf);
                    }
                    e.Handled = true;
                }
            };

            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToOrderColumns = false;

            dgv.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dgv.Columns["Codigo"].FillWeight = 20;
            dgv.Columns["Nombre"].FillWeight = 40;
            dgv.Columns["FechaRegistro"].FillWeight = 40;

            Action cargarClientes = () =>
            {
                dgv.Rows.Clear();
                var clientes = clienteService.ObtenerTodos();

                switch (cmbOrdenar.SelectedIndex)
                {
                    case 0: // Nombre A-Z
                        clientes = clientes.OrderBy(c => c.Nombre).ToList();
                        break;
                    case 1: // Nombre Z-A
                        clientes = clientes.OrderByDescending(c => c.Nombre).ToList();
                        break;
                    case 2: // Más reciente
                        clientes = clientes.OrderByDescending(c => c.FechaRegistro).ToList();
                        break;
                    case 3: // Más antiguo
                        clientes = clientes.OrderBy(c => c.FechaRegistro).ToList();
                        break;
                }

                foreach (var cliente in clientes)
                {
                    dgv.Rows.Add(
                        cliente.Codigo,
                        cliente.Nombre,
                        cliente.FechaRegistro.ToShortDateString()
                    );
                    dgv.Rows[dgv.Rows.Count - 1].Tag = cliente;
                }
            };

            cargarClientes();
            cmbOrdenar.SelectedIndexChanged += (s, e) => cargarClientes();

            dgv.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    Cliente cliente = (Cliente)dgv.Rows[e.RowIndex].Tag;
                    SeleccionarCliente(cliente);
                    listaForm.Close();
                }
            };

            listaForm.Controls.Add(dgv);
            listaForm.Controls.Add(topPanel);
            listaForm.ShowDialog(this);
        }

        private void MostrarTodosMovimientos()
        {
            FormTodosMovimientos formTodosMovimientos = new FormTodosMovimientos();
            formTodosMovimientos.ShowDialog(this);
        }

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            // Línea inferior con gradiente para dar profundidad
            using (System.Drawing.Drawing2D.LinearGradientBrush brush =
                new System.Drawing.Drawing2D.LinearGradientBrush(
                    new Rectangle(0, headerPanel.Height - 3, headerPanel.Width, 3),
                    Color.FromArgb(100, 46, 92, 138),
                    Color.FromArgb(30, 46, 92, 138),
                    System.Drawing.Drawing2D.LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, 0, headerPanel.Height - 3, headerPanel.Width, 3);
            }
        }

        private void SearchPanel_Paint(object sender, PaintEventArgs e)
        {
            // Sombra suave alrededor del panel
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                int radius = searchPanel.CornerRadius;
                Rectangle rect = new Rectangle(2, 2, searchPanel.Width - 5, searchPanel.Height - 5);

                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();

                // Sombra exterior
                using (System.Drawing.Drawing2D.PathGradientBrush brush = new System.Drawing.Drawing2D.PathGradientBrush(path))
                {
                    brush.CenterColor = Color.FromArgb(0, Color.Black);
                    brush.SurroundColors = new[] { Color.FromArgb(40, Color.Black) };
                    brush.FocusScales = new PointF(0.9f, 0.9f);
                }
            }
        }

        private void EmptyStatePanel_Paint(object sender, PaintEventArgs e)
        {
            // Borde sutil
            using (Pen pen = new Pen(Color.FromArgb(30, 139, 155, 168), 1))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, emptyStatePanel.Width - 1, emptyStatePanel.Height - 1);
            }
        }

        private void CrearNuevoCliente(string nombreSugerido)
        {
            FormAltaCliente formAlta = new FormAltaCliente(nombreSugerido);

            if (formAlta.ShowDialog(this) == DialogResult.OK)
            {
                bool exito = clienteService.AgregarCliente(formAlta.ClienteCreado);

                if (exito)
                {
                    string mensaje = $"Cliente registrado correctamente.\n\n" +
                                   $"ID: {formAlta.ClienteCreado.Id}\n" +
                                   $"Código: {formAlta.ClienteCreado.Codigo}\n" +
                                   $"Nombre: {formAlta.ClienteCreado.Nombre}\n" +
                                   $"Fecha de Registro: {formAlta.ClienteCreado.FechaRegistro.ToShortDateString()}";

                    MessageBox.Show(mensaje, "Cliente Registrado",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SeleccionarCliente(formAlta.ClienteCreado);
                }
                else
                {
                    MessageBox.Show("Hubo un error al registrar el cliente.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DgvMovimientos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int numeroMovimiento = Convert.ToInt32(dgvMovimientos.Rows[e.RowIndex].Cells["NumMov"].Value);

            var movimiento = movimientoService.ObtenerPorNumero(numeroMovimiento);

            if (movimiento != null)
            {
                MostrarDetalleMovimiento(movimiento);
            }
        }

        private void MostrarDetalleMovimiento(Movimiento movimiento)
        {
            FormDetalleMovimiento formDetalle = new FormDetalleMovimiento(movimiento);

            if (formDetalle.ShowDialog(this) == DialogResult.OK)
            {
                if (formDetalle.Eliminado)
                {
                    bool exito = movimientoService.EliminarMovimiento(movimiento.Id);

                    if (exito)
                    {
                        MessageBox.Show("El movimiento se eliminó correctamente.",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarMovimientos();
                    }
                }
                else if (formDetalle.Modificado)
                {
                    bool exito = movimientoService.ActualizarMovimiento(formDetalle.MovimientoActualizado);

                    if (exito)
                    {
                        MessageBox.Show("El movimiento se actualizó correctamente.",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarMovimientos();
                    }
                }
            }
        }
    }
}