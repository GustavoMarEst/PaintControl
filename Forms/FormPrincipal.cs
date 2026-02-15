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

            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    using (var context = new DOALDbContext())
                    {
                        // Forzar la compilación de consultas en segundo plano
                        var count = context.Clientes.Count();
                    }
                }
                catch { }
            });

            // Alinear logo
            picLogo.Top = (headerPanel.Height - picLogo.Height) / 2;

            // Colocar texto a la derecha del logo
            lblTituloTexto.Left = picLogo.Right + 12;
            lblTituloTexto.Top = (headerPanel.Height - lblTituloTexto.Height) / 2;


            // Configurar el reloj
            ConfigurarReloj();

            // Agregar eventos a los controles
            ConfigurarEventos();

            // Evento para ajustar controles al redimensionar
            this.Resize += FormPrincipal_Resize;

            // Establecer el foco en el campo de búsqueda
            txtCriterio.Focus();
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
                dgvMovimientos.Size = new Size(anchoDisponible - 30, altoDisponible - 130);
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

            // Eventos de los botones
            btnBuscarInline.Click += BtnBuscarCliente_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            btnVerClientes.Click += BtnVerClientes_Click;
            btnTodosMovimientos.Click += BtnTodosMovimientos_Click;
            btnArticulos.Click += BtnArticulos_Click;


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
                case Keys.F8:
                    MostrarTodosMovimientos();
                    break;
                case Keys.F9:
                    MostrarListaClientes();
                    break;
                case Keys.F10:
                    BtnArticulos_Click(null, null);
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

            List<Cliente> resultados;
            try
            {
                resultados = clienteService.BuscarPorCodigoONombre(criterio);
            }
            catch (ConexionException)
            {
                MessageBox.Show(
                    "No se pudo conectar con el servidor para realizar la búsqueda.\n" +
                    "Verifique la conexión e intente nuevamente.",
                    "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                Size = new Size(900, 600),
                StartPosition = FormStartPosition.CenterParent
            };

            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(10)
            };

            Label lblInfo = new Label
            {
                Text = $"Se encontraron {clientes.Count} clientes. Doble clic para seleccionar:",
                Location = new Point(10, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 92, 138)
            };

            topPanel.Controls.Add(lblInfo);

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
                    BackColor = Color.FromArgb(47, 164, 231),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    SelectionBackColor = Color.FromArgb(214, 235, 255),
                    SelectionForeColor = Color.Black,
                    Font = new Font("Segoe UI", 12F),
                    Padding = new Padding(5),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
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
                    using (Brush brush = new SolidBrush(Color.FromArgb(47, 164, 231)))
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

            foreach (var cliente in clientes)
            {
                dgv.Rows.Add(
                    cliente.Codigo,
                    cliente.Nombre,
                    cliente.FechaRegistro.ToShortDateString()
                );
                dgv.Rows[dgv.Rows.Count - 1].Tag = cliente;
            }

            dgv.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    Cliente clienteSeleccionado = (Cliente)dgv.Rows[e.RowIndex].Tag;
                    SeleccionarCliente(clienteSeleccionado);
                    seleccionForm.Close();
                }
            };

            seleccionForm.BackColor = Color.FromArgb(244, 247, 251);
            seleccionForm.Controls.Add(dgv);
            seleccionForm.Controls.Add(topPanel);
            seleccionForm.ShowDialog(this);
        }

        private void SeleccionarCliente(Cliente cliente)
        {
            clienteActual = cliente;
            emptyStatePanel.Visible = false;
            searchPanel.BackColor = Color.FromArgb(164, 231, 47);

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
                AutoSize = false,
                AutoEllipsis = true,
                Size = new Size(anchoDisponible - 460, 25),
                ForeColor = Color.FromArgb(46, 92, 138),
                Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right)))
            };

            // Botón pequeño para editar nombre del cliente (posición fija junto a Agregar Compra)
            Button btnEditarCliente = new Button
            {
                Name = "btnEditarCliente",
                Text = "✏️ Editar",
                Size = new Size(80, 32),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(74, 143, 208),
                ForeColor = Color.White,
                Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)))
            };
            btnEditarCliente.FlatAppearance.BorderSize = 0;
            btnEditarCliente.Location = new Point(anchoDisponible - 430, 13);
            btnEditarCliente.Click += (s, e) => EditarNombreClienteActual();

            // Botón para eliminar cliente (posición fija junto a Editar)
            Button btnEliminarCliente = new Button
            {
                Name = "btnEliminarCliente",
                Text = "🗑️ Eliminar",
                Size = new Size(95, 32),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)))
            };
            btnEliminarCliente.FlatAppearance.BorderSize = 0;
            btnEliminarCliente.Location = new Point(anchoDisponible - 345, 13);
            btnEliminarCliente.Click += (s, e) => EliminarClienteActual();

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
                BackColor = Color.FromArgb(231, 47, 164),

                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                CornerRadius = 10,
                Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)))
            };
            btnAgregarMovimiento.Click += BtnAgregarMovimiento_Click;


            // G R I D   D E  M O V I M I E N T O S


            dgvMovimientos = new DataGridView
            {
                Location = new Point(15, 85),
                Size = new Size(anchoDisponible - 30, altoDisponible - 130),
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
                    BackColor = Color.FromArgb(47, 164, 231),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    SelectionBackColor = Color.FromArgb(214, 235, 255),
                    SelectionForeColor = Color.Black,
                    Font = new Font("Segoe UI", 9.5F),
                    Padding = new Padding(5),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                EnableHeadersVisualStyles = false,
                GridColor = Color.LightGray,
            };

            dgvMovimientos.Columns.Add("NumMov", "No. Mov.");
            dgvMovimientos.Columns.Add("Fecha", "Fecha");
            dgvMovimientos.Columns.Add("FechaUltCompra", "Últ. Compra");
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
                    using (Brush brush = new SolidBrush(Color.FromArgb(47, 164, 231)))
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

            dgvMovimientos.Columns["NumMov"].FillWeight = 40;
            dgvMovimientos.Columns["Fecha"].FillWeight = 60;
            dgvMovimientos.Columns["FechaUltCompra"].FillWeight = 60;
            dgvMovimientos.Columns["Clave"].FillWeight = 70;
            dgvMovimientos.Columns["Descripcion"].FillWeight = 120;
            dgvMovimientos.Columns["Base"].FillWeight = 40;
            dgvMovimientos.Columns["Unidad"].FillWeight = 50;
            dgvMovimientos.Columns["Cantidad"].FillWeight = 40;
            dgvMovimientos.Columns["Precio"].FillWeight = 60;
            dgvMovimientos.Columns["Formula"].FillWeight = 180;

            dgvMovimientos.Columns["Cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvMovimientos.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvMovimientos.Columns["Precio"].DefaultCellStyle.Format = "C2";

            // Evento de doble clic para ver detalle
            dgvMovimientos.CellDoubleClick += DgvMovimientos_CellDoubleClick;

            panelMovimientos.Controls.Add(lblClienteInfo);
            panelMovimientos.Controls.Add(btnEditarCliente);
            panelMovimientos.Controls.Add(btnEliminarCliente);
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

            dgvMovimientos.SuspendLayout();
            dgvMovimientos.Rows.Clear();

            List<Movimiento> movimientos;

            try
            {
                if (chkFiltrarFecha != null && chkFiltrarFecha.Checked)
                {
                    DateTime fechaInicio = dtpFechaInicio.Value.Date;
                    DateTime fechaFin = dtpFechaFin.Value.Date.AddDays(1).AddSeconds(-1);

                    movimientos = movimientoService.ObtenerPorClienteYFechas(
                        clienteActual.Id, fechaInicio, fechaFin);

                    lblClienteInfo.Text += $" (Filtrado: {fechaInicio.ToShortDateString()} - {dtpFechaFin.Value.ToShortDateString()})";
                }
                else
                {
                    movimientos = movimientoService.ObtenerPorCliente(clienteActual.Id);
                }
            }
            catch (ConexionException)
            {
                dgvMovimientos.ResumeLayout();
                MessageBox.Show(
                    "No se pudieron cargar los movimientos.\n" +
                    "Verifique la conexión con el servidor e intente nuevamente.",
                    "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var mov in movimientos)
            {
                dgvMovimientos.Rows.Add(
                    mov.NumeroMovimiento,
                    mov.Fecha.ToShortDateString(),
                    mov.FechaUltimaCompra.HasValue ? mov.FechaUltimaCompra.Value.ToShortDateString() : "—",
                    mov.ClaveColor,
                    mov.Descripcion,
                    mov.Base,
                    mov.Unidad,
                    mov.Cantidad,
                    mov.Precio,
                    mov.Formula
                );
            }

            dgvMovimientos.ResumeLayout();

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

            searchPanel.BackColor = Color.FromArgb(191, 227, 248);
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
            FormCatalogo formCatalogo = new FormCatalogo();
            formCatalogo.ShowDialog(this);
        }



        // ===== ADMINISTRACIÓN DE CLIENTES =====

        private void EditarNombreClienteActual()
        {
            if (clienteActual == null) return;

            string nuevoNombre = MostrarInputDialog(
                "Editar nombre del cliente",
                "Nuevo nombre:",
                clienteActual.Nombre);

            if (nuevoNombre == null) return; // Canceló

            nuevoNombre = nuevoNombre.Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(nuevoNombre))
            {
                MessageBox.Show("El nombre no puede estar vacío.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (nuevoNombre == clienteActual.Nombre)
                return; // No cambió nada

            var clienteModificado = new Cliente
            {
                Id = clienteActual.Id,
                Codigo = clienteActual.Codigo,
                Nombre = nuevoNombre
            };

            bool exito = clienteService.ActualizarCliente(clienteModificado);
            if (exito)
            {
                clienteActual.Nombre = nuevoNombre;
                MessageBox.Show("Nombre actualizado correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarMovimientos();
            }
            else
            {
                MessageBox.Show("Error al actualizar el nombre del cliente.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarClienteActual()
        {
            if (clienteActual == null) return;
            EliminarClienteConConfirmacion(clienteActual, true);
        }

        private void EliminarClienteConConfirmacion(Cliente cliente, bool volverALimpiar)
        {
            int numMovimientos = clienteService.ContarMovimientos(cliente.Id);

            // Primera confirmación
            string mensaje = $"¿Está seguro que desea eliminar al cliente?\n\n" +
                           $"Código: {cliente.Codigo}\n" +
                           $"Nombre: {cliente.Nombre}\n\n";

            if (numMovimientos > 0)
            {
                mensaje += $"⚠️ ATENCIÓN: Se eliminarán también {numMovimientos} movimiento(s) asociado(s).\n\n";
            }

            mensaje += "Esta acción NO se puede deshacer.";

            var r1 = MessageBox.Show(mensaje,
                "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (r1 != DialogResult.Yes) return;

            // Segunda confirmación (doble confirmación)
            string mensaje2 = $"⚠️ SEGUNDA CONFIRMACIÓN ⚠️\n\n" +
                             $"¿Realmente desea eliminar a \"{cliente.Nombre}\"";

            if (numMovimientos > 0)
                mensaje2 += $" y sus {numMovimientos} movimiento(s)";

            mensaje2 += "?\n\nEsta es la última oportunidad para cancelar.";

            var r2 = MessageBox.Show(mensaje2,
                "⚠️ Última Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);

            if (r2 != DialogResult.Yes) return;

            bool exito = clienteService.EliminarCliente(cliente.Id);

            if (exito)
            {
                MessageBox.Show($"El cliente \"{cliente.Nombre}\" y todos sus datos fueron eliminados.",
                    "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (volverALimpiar && clienteActual != null && clienteActual.Id == cliente.Id)
                {
                    LimpiarFormulario();
                }
            }
            else
            {
                MessageBox.Show("Error al eliminar el cliente.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string MostrarInputDialog(string titulo, string etiqueta, string valorInicial)
        {
            Form inputForm = new Form
            {
                Text = titulo,
                Size = new Size(500, 200),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            Label lbl = new Label
            {
                Text = etiqueta,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 92, 138),
                Location = new Point(20, 20),
                AutoSize = true
            };

            TextBox txt = new TextBox
            {
                Text = valorInicial,
                Font = new Font("Segoe UI", 12F),
                Location = new Point(20, 52),
                Size = new Size(440, 32),
                CharacterCasing = CharacterCasing.Upper
            };

            Button btnOk = new Button
            {
                Text = "✔ Guardar",
                DialogResult = DialogResult.OK,
                Size = new Size(130, 38),
                Location = new Point(200, 100),
                BackColor = Color.FromArgb(56, 161, 105),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;

            Button btnCancel = new Button
            {
                Text = "✕ Cancelar",
                DialogResult = DialogResult.Cancel,
                Size = new Size(130, 38),
                Location = new Point(340, 100),
                BackColor = Color.FromArgb(226, 232, 240),
                ForeColor = Color.FromArgb(90, 101, 119),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            inputForm.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
            inputForm.AcceptButton = btnOk;
            inputForm.CancelButton = btnCancel;

            txt.SelectAll();

            if (inputForm.ShowDialog() == DialogResult.OK)
                return txt.Text;
            return null;
        }

        private void EditarClienteEnLista(DataGridView dgv, Form listaForm)
        {
            if (dgv.CurrentRow == null) return;
            Cliente cliente = dgv.CurrentRow.Tag as Cliente;
            if (cliente == null) return;

            string nuevoNombre = MostrarInputDialog(
                "Editar nombre del cliente",
                "Nuevo nombre:",
                cliente.Nombre);

            if (nuevoNombre == null) return;

            nuevoNombre = nuevoNombre.Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(nuevoNombre))
            {
                MessageBox.Show("El nombre no puede estar vacío.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (nuevoNombre == cliente.Nombre) return;

            var clienteModificado = new Cliente
            {
                Id = cliente.Id,
                Codigo = cliente.Codigo,
                Nombre = nuevoNombre
            };

            bool exito = clienteService.ActualizarCliente(clienteModificado);
            if (exito)
            {
                cliente.Nombre = nuevoNombre;
                dgv.CurrentRow.Cells["Nombre"].Value = nuevoNombre;
                dgv.CurrentRow.Tag = cliente;

                // Actualizar también el panel de movimientos si es el mismo cliente
                if (clienteActual != null && clienteActual.Id == cliente.Id)
                {
                    clienteActual.Nombre = nuevoNombre;
                    CargarMovimientos();
                }

                MessageBox.Show("Nombre actualizado correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error al actualizar el nombre.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarClienteEnLista(DataGridView dgv, Form listaForm)
        {
            if (dgv.CurrentRow == null) return;
            Cliente cliente = dgv.CurrentRow.Tag as Cliente;
            if (cliente == null) return;

            EliminarClienteConConfirmacion(cliente, false);

            // Refrescar la lista si se eliminó
            var clienteVerificar = clienteService.ObtenerPorId(cliente.Id);
            if (clienteVerificar == null)
            {
                dgv.Rows.Remove(dgv.CurrentRow);

                // Si era el cliente seleccionado, limpiar
                if (clienteActual != null && clienteActual.Id == cliente.Id)
                {
                    LimpiarFormulario();
                }
            }
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

            Label lblTitulo = new Label
            {
                Text = "Lista de Clientes",
                Location = new Point(10, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 92, 138)
            };

            topPanel.Controls.Add(lblTitulo);

            // Cargar todos los clientes UNA SOLA VEZ
            List<Cliente> todosLosClientes;
            try
            {
                todosLosClientes = clienteService.ObtenerTodos();
            }
            catch (ConexionException)
            {
                MessageBox.Show(
                    "No se pudo cargar la lista de clientes.\n" +
                    "Verifique la conexión con el servidor e intente nuevamente.",
                    "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                    BackColor = Color.FromArgb(47, 164, 231),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    SelectionBackColor = Color.FromArgb(214, 235, 255),
                    SelectionForeColor = Color.Black,
                    Font = new Font("Segoe UI", 12F),
                    Padding = new Padding(5),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                EnableHeadersVisualStyles = false,
                BorderStyle = BorderStyle.Fixed3D,
                GridColor = Color.LightGray,
            };

            dgv.Columns.Add("Codigo", "Código");
            dgv.Columns.Add("Nombre", "Nombre");
            dgv.Columns.Add("FechaRegistro", "Fecha de Registro");

            // ✅ EVENTO CELLPAINTING CON FLECHAS DE ORDENAMIENTO
            dgv.CellPainting += (s, e) =>
            {
                if (e.RowIndex == -1 && e.ColumnIndex >= 0)
                {
                    e.PaintBackground(e.CellBounds, true);
                    using (Brush brush = new SolidBrush(Color.FromArgb(47, 164, 231)))
                    {
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                    }

                    // Calcular espacio para la flecha
                    int arrowWidth = 20;
                    Rectangle textBounds = e.CellBounds;

                    // Si hay ordenamiento activo en esta columna, dejar espacio para la flecha
                    DataGridView grid = s as DataGridView;
                    if (grid.SortedColumn != null && grid.SortedColumn.Index == e.ColumnIndex)
                    {
                        textBounds.Width -= arrowWidth;
                    }

                    // Dibujar el texto del encabezado
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
                            textBounds,
                            sf);
                    }

                    // Dibujar la flecha de ordenamiento si esta columna está ordenada
                    if (grid.SortedColumn != null && grid.SortedColumn.Index == e.ColumnIndex)
                    {
                        int arrowX = e.CellBounds.Right - arrowWidth;
                        int arrowY = e.CellBounds.Y + (e.CellBounds.Height - 8) / 2;

                        System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

                        if (grid.SortOrder == SortOrder.Ascending)
                        {
                            // Flecha hacia arriba (triángulo)
                            Point[] arrowPoints = new Point[]
                            {
                                new Point(arrowX + 5, arrowY + 8),      // Izquierda abajo
                                new Point(arrowX + 10, arrowY),         // Punta arriba
                                new Point(arrowX + 15, arrowY + 8)      // Derecha abajo
                            };
                            path.AddPolygon(arrowPoints);
                        }
                        else if (grid.SortOrder == SortOrder.Descending)
                        {
                            // Flecha hacia abajo (triángulo invertido)
                            Point[] arrowPoints = new Point[]
                            {
                                new Point(arrowX + 5, arrowY),          // Izquierda arriba
                                new Point(arrowX + 10, arrowY + 8),     // Punta abajo
                                new Point(arrowX + 15, arrowY)          // Derecha arriba
                            };
                            path.AddPolygon(arrowPoints);
                        }

                        using (Brush arrowBrush = new SolidBrush(Color.White))
                        {
                            e.Graphics.FillPath(arrowBrush, path);
                        }
                        path.Dispose();
                    }

                    e.Handled = true;
                }
            };

            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToOrderColumns = false;

            dgv.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // ✅ HABILITAR ordenamiento por clic en columnas
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
            }

            dgv.Columns["Codigo"].FillWeight = 20;
            dgv.Columns["Nombre"].FillWeight = 40;
            dgv.Columns["FechaRegistro"].FillWeight = 40;

            // Cargar clientes inicialmente ordenados por nombre (A-Z)
            dgv.SuspendLayout();
            foreach (var cliente in todosLosClientes.OrderBy(c => c.Nombre))
            {
                dgv.Rows.Add(
                    cliente.Codigo,
                    cliente.Nombre,
                    cliente.FechaRegistro.ToShortDateString()
                );
                dgv.Rows[dgv.Rows.Count - 1].Tag = cliente;
            }
            dgv.ResumeLayout();

            // Evento de ordenamiento personalizado para la columna de fecha
            dgv.SortCompare += (s, e) =>
            {
                if (e.Column.Name == "FechaRegistro")
                {
                    // Obtener los objetos Cliente de las filas
                    Cliente cliente1 = dgv.Rows[e.RowIndex1].Tag as Cliente;
                    Cliente cliente2 = dgv.Rows[e.RowIndex2].Tag as Cliente;

                    // Comparar por fecha real, no por el texto
                    e.SortResult = DateTime.Compare(cliente1.FechaRegistro, cliente2.FechaRegistro);
                    e.Handled = true;
                }
            };

            dgv.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    Cliente cliente = (Cliente)dgv.Rows[e.RowIndex].Tag;
                    SeleccionarCliente(cliente);
                    listaForm.Close();
                }
            };

            // ===== PANEL INFERIOR CON BOTONES DE ADMINISTRACIÓN =====
            Panel bottomPanel2 = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.FromArgb(232, 241, 248),
                Padding = new Padding(15, 10, 15, 10)
            };

            Button btnEditarLista = new Button
            {
                Text = "✏️ Editar Nombre",
                Size = new Size(170, 40),
                Location = new Point(15, 10),
                BackColor = Color.FromArgb(74, 143, 208),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEditarLista.FlatAppearance.BorderSize = 0;
            btnEditarLista.Click += (s, e) => EditarClienteEnLista(dgv, listaForm);

            Button btnEliminarLista = new Button
            {
                Text = "🗑️ Eliminar Cliente",
                Size = new Size(180, 40),
                Location = new Point(195, 10),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEliminarLista.FlatAppearance.BorderSize = 0;
            btnEliminarLista.Click += (s, e) => EliminarClienteEnLista(dgv, listaForm);

            Label lblTip = new Label
            {
                Text = "Seleccione un cliente y use los botones, o doble clic para ver sus movimientos",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.FromArgb(120, 130, 145),
                AutoSize = true,
                Location = new Point(390, 20)
            };

            bottomPanel2.Controls.AddRange(new Control[] { btnEditarLista, btnEliminarLista, lblTip });

            listaForm.BackColor = Color.FromArgb(244, 247, 251);
            listaForm.Controls.Add(dgv);
            listaForm.Controls.Add(bottomPanel2);
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

            Movimiento movimiento;
            try
            {
                movimiento = movimientoService.ObtenerPorNumero(numeroMovimiento);
            }
            catch (ConexionException)
            {
                MessageBox.Show(
                    "No se pudo cargar el detalle del movimiento.\n" +
                    "Verifique la conexión con el servidor e intente nuevamente.",
                    "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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