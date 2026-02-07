using PaintControl.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PaintControl.Forms
{
    public partial class FormDetalleMovimiento : Form
    {
        public Movimiento MovimientoActualizado { get; private set; }
        public bool Eliminado { get; private set; }
        public bool Modificado { get; private set; }

        private Movimiento movimiento;
        private bool modoEdicion;

        // Referencias directas a controles principales
        private DateTimePicker dtpFecha;
        private TextBox txtClave;
        private TextBox txtDescripcion;
        private TextBox txtBase;
        private ComboBox cmbUnidad;
        private NumericUpDown numCantidad;
        private NumericUpDown numPrecio;
        private Label lblTotalValor;
        private TableLayoutPanel panelFormulas;

        // Botones
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnGuardar;
        private Button btnCancelarEdicion;

        // Referencias directas a controles de formula
        private TextBox[] txtTipos = new TextBox[6];
        private TextBox[] txtValores1 = new TextBox[6];
        private TextBox[] txtValores2 = new TextBox[6];

        // Colores del tema
        private static readonly Color AzulPrimario = Color.FromArgb(47, 164, 231);
        private static readonly Color AzulOscuro = Color.FromArgb(46, 92, 138);
        private static readonly Color FondoTarjeta = Color.FromArgb(244, 247, 251);
        private static readonly Color BordeTarjeta = Color.FromArgb(226, 232, 240);
        private static readonly Color TextoSecundario = Color.FromArgb(139, 149, 166);
        private static readonly Color TextoLabel = Color.FromArgb(90, 101, 119);
        private static readonly Color VerdePrimario = Color.FromArgb(56, 161, 105);
        private static readonly Color VerdeOscuro = Color.FromArgb(34, 118, 61);
        private static readonly Color FondoVerde = Color.FromArgb(240, 255, 244);
        private static readonly Color BordeVerde = Color.FromArgb(198, 246, 213);
        private static readonly Color FondoFormula = Color.FromArgb(248, 250, 252);
        private static readonly Color GrisBotonSecundario = Color.FromArgb(226, 232, 240);
        private static readonly Color RojoEliminar = Color.FromArgb(220, 53, 69);
        private static readonly Color AzulEditar = Color.FromArgb(74, 143, 208);

        // Fuentes cacheadas
        private static readonly Font FuenteTitulo = new Font("Segoe UI", 16F, FontStyle.Bold);
        private static readonly Font FuenteBadge = new Font("Segoe UI", 9F, FontStyle.Bold);
        private static readonly Font FuenteInfoLabel = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        private static readonly Font FuenteInfoValor = new Font("Segoe UI", 11F, FontStyle.Bold);
        private static readonly Font FuenteSeccion = new Font("Segoe UI", 10F, FontStyle.Bold);
        private static readonly Font FuenteFieldLabel = new Font("Segoe UI", 9F, FontStyle.Bold);
        private static readonly Font FuenteInput = new Font("Segoe UI", 11F);
        private static readonly Font FuenteBoton = new Font("Segoe UI", 11F, FontStyle.Bold);
        private static readonly Font FuenteTotalLabel = new Font("Segoe UI", 11F, FontStyle.Bold);
        private static readonly Font FuenteTotalValor = new Font("Segoe UI", 18F, FontStyle.Bold);
        private static readonly Font FuenteFormula = new Font("Segoe UI", 9.5F);
        private static readonly Font FuenteIgual = new Font("Segoe UI", 10F, FontStyle.Bold);

        public FormDetalleMovimiento(Movimiento movimiento, bool permitirEdicion = true)
        {
            this.movimiento = movimiento;
            this.modoEdicion = false;
            InitializeComponent();
            ConfigurarFormulario(permitirEdicion);
            CargarDatos();
        }

        private void ConfigurarFormulario(bool permitirEdicion)
        {
            this.Text = $"Detalle de Movimiento #{movimiento.NumeroMovimiento}";
            this.Size = new Size(780, 780);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.BackColor = Color.White;

            // ===== HEADER AZUL SOLIDO =====
            Panel headerPanel = new Panel
            {
                Height = 60,
                BackColor = AzulPrimario,
                Dock = DockStyle.Top
            };

            Label lblTitulo = new Label
            {
                Text = $"📋  Movimiento #{movimiento.NumeroMovimiento}",
                Font = FuenteTitulo,
                ForeColor = Color.White,
                Location = new Point(24, 15),
                AutoSize = true
            };

            Label lblBadge = new Label
            {
                Text = "DETALLE",
                Font = FuenteBadge,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 255, 255, 255),
                AutoSize = true,
                Padding = new Padding(10, 4, 10, 4),
                TextAlign = ContentAlignment.MiddleCenter
            };

            headerPanel.Controls.Add(lblTitulo);
            headerPanel.Controls.Add(lblBadge);

            headerPanel.Layout += (s, e) =>
            {
                lblBadge.Location = new Point(headerPanel.Width - lblBadge.Width - 24, (headerPanel.Height - lblBadge.Height) / 2);
            };

            // ===== PANEL SCROLLABLE =====
            Panel scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White
            };

            Panel body = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(760, 680),
                BackColor = Color.White
            };

            int padX = 28;
            int y = 20;
            int fullWidth = 700;
            int halfWidth = 340;
            int gap = 20;

            // ===== INFO CARDS =====
            int cardWidth = (fullWidth - gap * 2) / 3;
            int cardHeight = 64;

            Panel card1 = CrearInfoCard("Nº MOVIMIENTO", movimiento.NumeroMovimiento.ToString(), padX, y, cardWidth, cardHeight);

            dtpFecha = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Font = FuenteInput,
                Location = new Point(-300, -300),
                Size = new Size(1, 1),
                Enabled = false
            };
            Panel card2 = CrearInfoCard("FECHA", movimiento.Fecha.ToShortDateString(), padX + cardWidth + gap, y, cardWidth, cardHeight);

            string nombreCliente = movimiento.Cliente != null ? movimiento.Cliente.Nombre : "—";
            Panel card3 = CrearInfoCard("CLIENTE", nombreCliente, padX + (cardWidth + gap) * 2, y, cardWidth, cardHeight);

            body.Controls.Add(card1);
            body.Controls.Add(card2);
            body.Controls.Add(card3);
            body.Controls.Add(dtpFecha);
            y += cardHeight + 20;

            // ===== SECCION: DATOS DEL PRODUCTO =====
            Panel secProducto = CrearSeccionTitulo("DATOS DEL PRODUCTO", padX, y, fullWidth);
            body.Controls.Add(secProducto);
            y += 34;

            Label lblClaveLabel = CrearFieldLabel("CLAVE DEL COLOR", padX, y);
            txtClave = CrearTextBoxModerno(padX, y + 20, halfWidth, true);
            body.Controls.Add(lblClaveLabel);
            body.Controls.Add(txtClave);

            Label lblBaseLabel = CrearFieldLabel("BASE", padX + halfWidth + gap, y);
            txtBase = CrearTextBoxModerno(padX + halfWidth + gap, y + 20, halfWidth, true);
            txtBase.MaxLength = 50;
            body.Controls.Add(lblBaseLabel);
            body.Controls.Add(txtBase);
            y += 60;

            Label lblDescLabel = CrearFieldLabel("DESCRIPCIÓN", padX, y);
            txtDescripcion = CrearTextBoxModerno(padX, y + 20, fullWidth, true);
            body.Controls.Add(lblDescLabel);
            body.Controls.Add(txtDescripcion);
            y += 60;

            // ===== SECCION: INFORMACION DE VENTA =====
            Panel secVenta = CrearSeccionTitulo("INFORMACIÓN DE VENTA", padX, y, fullWidth);
            body.Controls.Add(secVenta);
            y += 34;

            Label lblUnidadLabel = CrearFieldLabel("UNIDAD", padX, y);
            cmbUnidad = new ComboBox
            {
                Location = new Point(padX, y + 20),
                Width = halfWidth,
                Font = FuenteInput,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            // ✅ CORREGIDO: Usar los valores reales que vienen de la base de datos
            cmbUnidad.Items.AddRange(new object[] { "LT", "GL", "KG", "PZ", "M2", "M3", "Litro", "Galón", "Cubeta" });
            body.Controls.Add(lblUnidadLabel);
            body.Controls.Add(cmbUnidad);

            Label lblCantidadLabel = CrearFieldLabel("CANTIDAD", padX + halfWidth + gap, y);
            numCantidad = new NumericUpDown
            {
                Location = new Point(padX + halfWidth + gap, y + 20),
                Width = halfWidth,
                Font = FuenteInput,
                DecimalPlaces = 2,
                Minimum = 0.01m,
                Maximum = 9999.99m,
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            body.Controls.Add(lblCantidadLabel);
            body.Controls.Add(numCantidad);
            y += 60;

            Label lblPrecioLabel = CrearFieldLabel("PRECIO", padX, y);
            numPrecio = new NumericUpDown
            {
                Location = new Point(padX, y + 20),
                Width = halfWidth,
                Font = FuenteInput,
                DecimalPlaces = 2,
                Minimum = 0.01m,
                Maximum = 99999.99m,
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            body.Controls.Add(lblPrecioLabel);
            body.Controls.Add(numPrecio);

            Panel totalPanel = CrearTotalPanel(padX + halfWidth + gap, y + 2, halfWidth);
            body.Controls.Add(totalPanel);
            y += 70;

            // ===== SECCION: FORMULA DE COLOR =====
            Panel secFormula = CrearSeccionTitulo("FÓRMULA DE COLOR", padX, y, fullWidth);
            body.Controls.Add(secFormula);
            y += 34;

            panelFormulas = CrearPanelFormulas(padX, y, fullWidth, true);
            body.Controls.Add(panelFormulas);
            y += panelFormulas.Height + 20;

            // ===== BOTONES =====
            int btnX = padX;

            btnEditar = CrearBoton("✏️  Editar", AzulEditar, btnX, y);
            btnEditar.Visible = permitirEdicion;
            btnX += 170;

            btnEliminar = CrearBoton("🗑️  Eliminar", RojoEliminar, btnX, y);
            btnEliminar.Visible = permitirEdicion;
            btnX += 170;

            btnGuardar = CrearBoton("💾  Guardar", VerdePrimario, padX, y);
            btnGuardar.Visible = false;

            btnCancelarEdicion = CrearBoton("✕  Cancelar", GrisBotonSecundario, padX + 170, y);
            btnCancelarEdicion.ForeColor = TextoLabel;
            btnCancelarEdicion.Visible = false;

            Button btnCerrar = CrearBoton("Cerrar", GrisBotonSecundario, btnX, y);
            btnCerrar.ForeColor = TextoLabel;

            body.Controls.Add(btnEditar);
            body.Controls.Add(btnEliminar);
            body.Controls.Add(btnGuardar);
            body.Controls.Add(btnCancelarEdicion);
            body.Controls.Add(btnCerrar);
            y += 60;

            body.Height = y;

            scrollPanel.Controls.Add(body);

            // Header PRIMERO (Dock.Top), luego scroll
            this.Controls.Add(scrollPanel);
            this.Controls.Add(headerPanel);

            // ===== EVENTOS =====
            EventHandler calcularTotal = (s, e) =>
            {
                decimal total = numCantidad.Value * numPrecio.Value;
                lblTotalValor.Text = $"${total:N2}";
                // ✅ CORREGIDO: Reposicionar el label después de cambiar el texto
                lblTotalValor.Location = new Point(
                    totalPanel.Width - lblTotalValor.PreferredWidth - 16,
                    10
                );
            };
            numCantidad.ValueChanged += calcularTotal;
            numPrecio.ValueChanged += calcularTotal;

            btnEditar.Click += (s, e) => ActivarModoEdicion();
            btnCancelarEdicion.Click += (s, e) => DesactivarModoEdicion();
            btnGuardar.Click += (s, e) => GuardarCambios();
            btnEliminar.Click += (s, e) => EliminarMovimiento();
            btnCerrar.Click += (s, e) => this.Close();
        }

        // ===== HELPERS =====

        private Panel CrearInfoCard(string label, string value, int x, int y, int width, int height)
        {
            Panel card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = FondoTarjeta
            };

            card.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(BordeTarjeta, 1))
                {
                    Rectangle rect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);
                    using (GraphicsPath path = CrearRectanguloRedondeado(rect, 10))
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            };

            Label lblLabel = new Label
            {
                Text = label,
                Font = FuenteInfoLabel,
                ForeColor = TextoSecundario,
                Location = new Point(14, 10),
                AutoSize = true
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = FuenteInfoValor,
                ForeColor = AzulOscuro,
                Location = new Point(14, 32),
                Size = new Size(width - 28, height - 36),
                AutoEllipsis = true
            };

            card.Controls.Add(lblLabel);
            card.Controls.Add(lblValue);

            return card;
        }

        private Panel CrearSeccionTitulo(string texto, int x, int y, int width)
        {
            Panel seccion = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, 28),
                BackColor = Color.Transparent
            };

            seccion.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (Brush brush = new SolidBrush(AzulPrimario))
                    e.Graphics.FillRectangle(brush, 0, 4, 4, 16);

                using (Brush brush = new SolidBrush(AzulPrimario))
                    e.Graphics.DrawString(texto, FuenteSeccion, brush, 12, 2);

                using (Pen pen = new Pen(Color.FromArgb(232, 244, 253), 2))
                    e.Graphics.DrawLine(pen, 0, seccion.Height - 1, seccion.Width, seccion.Height - 1);
            };

            return seccion;
        }

        private Label CrearFieldLabel(string texto, int x, int y)
        {
            return new Label
            {
                Text = texto,
                Font = FuenteFieldLabel,
                ForeColor = TextoLabel,
                Location = new Point(x, y),
                AutoSize = true
            };
        }

        private TextBox CrearTextBoxModerno(int x, int y, int width, bool readOnly = false)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Width = width,
                Font = FuenteInput,
                CharacterCasing = CharacterCasing.Upper,
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = readOnly
            };
        }

        private Panel CrearTotalPanel(int x, int y, int width)
        {
            Panel panel = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, 55),
                BackColor = FondoVerde
            };

            panel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(BordeVerde, 2))
                {
                    Rectangle rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
                    using (GraphicsPath path = CrearRectanguloRedondeado(rect, 10))
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            };

            Label lblTotal = new Label
            {
                Text = "TOTAL",
                Font = FuenteTotalLabel,
                ForeColor = VerdePrimario,
                Location = new Point(16, 17),
                AutoSize = true
            };

            // ✅ CORREGIDO: Configurar el label del total correctamente
            lblTotalValor = new Label
            {
                Text = "$0.00",
                Font = FuenteTotalValor,
                ForeColor = VerdeOscuro,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleRight
            };

            // Posicionar después de establecer el texto inicial
            lblTotalValor.Location = new Point(width - lblTotalValor.PreferredWidth - 16, 10);

            panel.Controls.Add(lblTotal);
            panel.Controls.Add(lblTotalValor);

            return panel;
        }

        private TableLayoutPanel CrearPanelFormulas(int x, int y, int width, bool readOnly)
        {
            TableLayoutPanel panel = new TableLayoutPanel
            {
                Name = "panelFormulas",
                Location = new Point(x, y),
                Width = width,
                Height = 130,
                BackColor = FondoFormula,
                ColumnCount = 2,
                RowCount = 3,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                Padding = new Padding(12, 8, 12, 8)
            };

            panel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(BordeTarjeta, 1))
                {
                    Rectangle rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
                    using (GraphicsPath path = CrearRectanguloRedondeado(rect, 10))
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            };

            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            for (int i = 0; i < 3; i++)
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));

            for (int i = 0; i < 6; i++)
            {
                int col = i / 3;
                int row = i % 3;

                Panel filaPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(2),
                    BackColor = FondoFormula
                };

                FlowLayoutPanel flowPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.LeftToRight,
                    AutoSize = true,
                    WrapContents = false,
                    Anchor = AnchorStyles.None
                };

                TextBox txtTipo = new TextBox
                {
                    Name = $"txtTipo{i}",
                    Width = 56,
                    Font = FuenteFormula,
                    TextAlign = HorizontalAlignment.Center,
                    CharacterCasing = CharacterCasing.Upper,
                    BorderStyle = BorderStyle.FixedSingle,
                    ReadOnly = readOnly,
                    Margin = new Padding(0)
                };
                txtTipos[i] = txtTipo;

                Label lblIgual = new Label
                {
                    Text = "=",
                    Font = FuenteIgual,
                    ForeColor = Color.FromArgb(160, 174, 192),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(5, 0, 5, 0)
                };

                TextBox txtValor1 = new TextBox
                {
                    Name = $"txtValor1_{i}",
                    Width = 56,
                    Font = FuenteFormula,
                    TextAlign = HorizontalAlignment.Center,
                    BorderStyle = BorderStyle.FixedSingle,
                    ReadOnly = readOnly,
                    Margin = new Padding(0, 0, 5, 0)
                };
                txtValores1[i] = txtValor1;

                TextBox txtValor2 = new TextBox
                {
                    Name = $"txtValor2_{i}",
                    Width = 56,
                    Font = FuenteFormula,
                    TextAlign = HorizontalAlignment.Center,
                    BorderStyle = BorderStyle.FixedSingle,
                    ReadOnly = readOnly,
                    Margin = new Padding(0)
                };
                txtValores2[i] = txtValor2;

                flowPanel.Controls.AddRange(new Control[] { txtTipo, lblIgual, txtValor1, txtValor2 });

                flowPanel.Location = new Point(
                    (filaPanel.Width - flowPanel.Width) / 2,
                    (filaPanel.Height - flowPanel.Height) / 2
                );

                filaPanel.Controls.Add(flowPanel);

                filaPanel.Resize += (s, e) =>
                {
                    if (flowPanel.Parent != null)
                    {
                        flowPanel.Location = new Point(
                            Math.Max(0, (filaPanel.Width - flowPanel.Width) / 2),
                            Math.Max(0, (filaPanel.Height - flowPanel.Height) / 2)
                        );
                    }
                };

                panel.Controls.Add(filaPanel, col, row);
            }

            return panel;
        }

        private Button CrearBoton(string texto, Color backColor, int x, int y)
        {
            Button btn = new Button
            {
                Text = texto,
                Size = new Size(155, 44),
                Location = new Point(x, y),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = FuenteBoton,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private GraphicsPath CrearRectanguloRedondeado(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        // ===== CARGA DE DATOS =====

        private void CargarDatos()
        {
            dtpFecha.Value = movimiento.Fecha;
            txtClave.Text = movimiento.ClaveColor ?? "";
            txtDescripcion.Text = movimiento.Descripcion ?? "";
            txtBase.Text = movimiento.Base ?? "";

            // ✅ CORREGIDO: Cargar la unidad correctamente
            if (!string.IsNullOrWhiteSpace(movimiento.Unidad))
            {
                // Buscar el índice del item que coincide
                int index = cmbUnidad.FindStringExact(movimiento.Unidad);
                if (index >= 0)
                {
                    cmbUnidad.SelectedIndex = index;
                }
                else
                {
                    // Si no lo encuentra, agregarlo y seleccionarlo
                    cmbUnidad.Items.Add(movimiento.Unidad);
                    cmbUnidad.SelectedItem = movimiento.Unidad;
                }
            }
            else
            {
                // Si está vacío, seleccionar el primero por defecto
                if (cmbUnidad.Items.Count > 0)
                {
                    cmbUnidad.SelectedIndex = 0;
                }
            }

            numCantidad.Value = movimiento.Cantidad;
            numPrecio.Value = movimiento.Precio;

            // ✅ CORREGIDO: Actualizar el total y reposicionar el label
            lblTotalValor.Text = $"${movimiento.Total:N2}";
            lblTotalValor.Location = new Point(
                lblTotalValor.Parent.Width - lblTotalValor.PreferredWidth - 16,
                10
            );

            CargarFormula(movimiento.Formula);
        }

        private void CargarFormula(string formula)
        {
            if (string.IsNullOrWhiteSpace(formula)) return;

            string[] lineas = formula.Split(new[] { '|', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lineas.Length && i < 6; i++)
            {
                string linea = lineas[i].Trim();
                if (string.IsNullOrEmpty(linea)) continue;

                string[] partes = linea.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (partes.Length != 2) continue;

                string tipo = partes[0].Trim();
                string[] valores = partes[1].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (txtTipos[i] != null) txtTipos[i].Text = tipo;
                if (valores.Length > 0 && txtValores1[i] != null) txtValores1[i].Text = valores[0];
                if (valores.Length > 1 && txtValores2[i] != null) txtValores2[i].Text = valores[1];
            }
        }

        private string ObtenerFormula()
        {
            var formulas = new List<string>();

            for (int i = 0; i < 6; i++)
            {
                if (txtTipos[i] == null || txtValores1[i] == null) continue;
                if (string.IsNullOrWhiteSpace(txtTipos[i].Text)) continue;

                string formula = $"{txtTipos[i].Text.Trim()} = {txtValores1[i].Text.Trim()}";
                if (txtValores2[i] != null && !string.IsNullOrWhiteSpace(txtValores2[i].Text))
                    formula += $" {txtValores2[i].Text.Trim()}";

                formulas.Add(formula);
            }

            return string.Join("|", formulas);
        }

        // ===== MODO EDICION =====

        private void ActivarModoEdicion()
        {
            modoEdicion = true;

            dtpFecha.Enabled = true;
            txtClave.ReadOnly = false;
            txtDescripcion.ReadOnly = false;
            txtBase.ReadOnly = false;
            cmbUnidad.Enabled = true;
            numCantidad.ReadOnly = false;
            numPrecio.ReadOnly = false;

            for (int i = 0; i < 6; i++)
            {
                if (txtTipos[i] != null) txtTipos[i].ReadOnly = false;
                if (txtValores1[i] != null) txtValores1[i].ReadOnly = false;
                if (txtValores2[i] != null) txtValores2[i].ReadOnly = false;
            }

            btnEditar.Visible = false;
            btnEliminar.Visible = false;
            btnGuardar.Visible = true;
            btnCancelarEdicion.Visible = true;
        }

        private void DesactivarModoEdicion()
        {
            modoEdicion = false;
            CargarDatos();

            dtpFecha.Enabled = false;
            txtClave.ReadOnly = true;
            txtDescripcion.ReadOnly = true;
            txtBase.ReadOnly = true;
            cmbUnidad.Enabled = false;
            numCantidad.ReadOnly = true;
            numPrecio.ReadOnly = true;

            for (int i = 0; i < 6; i++)
            {
                if (txtTipos[i] != null) txtTipos[i].ReadOnly = true;
                if (txtValores1[i] != null) txtValores1[i].ReadOnly = true;
                if (txtValores2[i] != null) txtValores2[i].ReadOnly = true;
            }

            btnEditar.Visible = true;
            btnEliminar.Visible = true;
            btnGuardar.Visible = false;
            btnCancelarEdicion.Visible = false;
        }

        private void GuardarCambios()
        {
            if (string.IsNullOrWhiteSpace(txtClave.Text) || string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("La clave y descripción son obligatorias.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MovimientoActualizado = new Movimiento
            {
                Id = movimiento.Id,
                NumeroMovimiento = movimiento.NumeroMovimiento,
                ClienteId = movimiento.ClienteId,
                Fecha = dtpFecha.Value,
                ClaveColor = txtClave.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim(),
                Base = txtBase.Text.Trim(),
                Unidad = cmbUnidad.SelectedItem?.ToString() ?? movimiento.Unidad,
                Cantidad = numCantidad.Value,
                Precio = numPrecio.Value,
                Formula = ObtenerFormula()
            };

            Modificado = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void EliminarMovimiento()
        {
            var resultado = MessageBox.Show(
                $"¿Está seguro de que desea eliminar el movimiento #{movimiento.NumeroMovimiento}?{Environment.NewLine}{Environment.NewLine}Esta acción no se puede deshacer.",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (resultado == DialogResult.Yes)
            {
                Eliminado = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}