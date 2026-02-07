using PaintControl.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PaintControl.Forms
{
    public partial class FormAgregarMovimiento : Form
    {
        public Movimiento MovimientoCreado { get; private set; }
        private Cliente cliente;
        private int siguienteNumero;

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

        public FormAgregarMovimiento(Cliente cliente, int siguienteNumeroMovimiento)
        {
            this.cliente = cliente;
            this.siguienteNumero = siguienteNumeroMovimiento;
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = $"Agregar Movimiento - {cliente.Nombre}";
            this.Size = new Size(780, 780);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.BackColor = Color.White;

            // ===== HEADER AZUL SÓLIDO (Dock Top = aparece ARRIBA) =====
            Panel headerPanel = new Panel
            {
                Height = 60,
                BackColor = AzulPrimario,
                Dock = DockStyle.Top
            };

            Label lblTitulo = new Label
            {
                Text = "\U0001f3a8  Nueva Compra de Pintura",
                Font = FuenteTitulo,
                ForeColor = Color.White,
                Location = new Point(24, 15),
                AutoSize = true
            };

            Label lblBadge = new Label
            {
                Text = cliente.Nombre,
                Font = FuenteBadge,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 255, 255, 255),
                AutoSize = true,
                Padding = new Padding(10, 4, 10, 4),
                TextAlign = ContentAlignment.MiddleCenter,
                MaximumSize = new Size(250, 0)
            };

            headerPanel.Controls.Add(lblTitulo);
            headerPanel.Controls.Add(lblBadge);

            // Posicionar badge a la derecha
            headerPanel.Layout += (s, e) =>
            {
                lblBadge.Location = new Point(headerPanel.Width - lblBadge.Width - 24, (headerPanel.Height - lblBadge.Height) / 2);
            };

            // ===== PANEL SCROLLABLE PARA EL CUERPO =====
            Panel scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White
            };

            // Contenedor interno con ancho fijo
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

            Panel card1 = CrearInfoCard("N\u00ba MOVIMIENTO", siguienteNumero.ToString(), padX, y, cardWidth, cardHeight);

            dtpFecha = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Font = FuenteInput,
                Location = new Point(-300, -300),
                Size = new Size(1, 1)
            };
            Panel card2 = CrearInfoCard("FECHA", DateTime.Now.ToShortDateString(), padX + cardWidth + gap, y, cardWidth, cardHeight);

            Panel card3 = CrearInfoCard("CLIENTE", cliente.Nombre, padX + (cardWidth + gap) * 2, y, cardWidth, cardHeight);

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
            txtClave = CrearTextBoxModerno(padX, y + 20, halfWidth);
            txtClave.MaxLength = 20;
            body.Controls.Add(lblClaveLabel);
            body.Controls.Add(txtClave);

            Label lblBaseLabel = CrearFieldLabel("BASE", padX + halfWidth + gap, y);
            txtBase = CrearTextBoxModerno(padX + halfWidth + gap, y + 20, halfWidth);
            txtBase.MaxLength = 50;
            body.Controls.Add(lblBaseLabel);
            body.Controls.Add(txtBase);
            y += 60;

            Label lblDescLabel = CrearFieldLabel("DESCRIPCI\u00d3N", padX, y);
            txtDescripcion = CrearTextBoxModerno(padX, y + 20, fullWidth);
            txtDescripcion.MaxLength = 100;
            body.Controls.Add(lblDescLabel);
            body.Controls.Add(txtDescripcion);
            y += 60;

            // ===== SECCION: INFORMACION DE VENTA =====
            Panel secVenta = CrearSeccionTitulo("INFORMACI\u00d3N DE VENTA", padX, y, fullWidth);
            body.Controls.Add(secVenta);
            y += 34;

            Label lblUnidadLabel = CrearFieldLabel("UNIDAD", padX, y);
            cmbUnidad = new ComboBox
            {
                Location = new Point(padX, y + 20),
                Width = halfWidth,
                Font = FuenteInput,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            cmbUnidad.Items.AddRange(new object[] { "Litro", "Gal\u00f3n", "Cubeta" });
            cmbUnidad.SelectedIndex = 0;
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
                Value = 1,
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
                Value = 100,
                BorderStyle = BorderStyle.FixedSingle
            };
            body.Controls.Add(lblPrecioLabel);
            body.Controls.Add(numPrecio);

            Panel totalPanel = CrearTotalPanel(padX + halfWidth + gap, y + 2, halfWidth);
            body.Controls.Add(totalPanel);
            y += 70;

            // ===== SECCION: FORMULA DE COLOR =====
            Panel secFormula = CrearSeccionTitulo("F\u00d3RMULA DE COLOR", padX, y, fullWidth);
            body.Controls.Add(secFormula);
            y += 34;

            panelFormulas = CrearPanelFormulas(padX, y, fullWidth, false);
            body.Controls.Add(panelFormulas);
            y += panelFormulas.Height + 20;

            // ===== BOTONES =====
            Button btnGuardar = CrearBoton("\U0001f4be  Guardar", VerdePrimario, padX, y);
            Button btnCancelar = CrearBoton("\u2715  Cancelar", GrisBotonSecundario, padX + 170, y);
            btnCancelar.ForeColor = TextoLabel;

            body.Controls.Add(btnGuardar);
            body.Controls.Add(btnCancelar);
            y += 60;

            body.Height = y;

            scrollPanel.Controls.Add(body);

            // IMPORTANTE: Agregar header PRIMERO, luego scroll panel
            // Dock.Top se apila en orden inverso de Add
            this.Controls.Add(scrollPanel);
            this.Controls.Add(headerPanel);

            // ===== EVENTOS =====
            EventHandler calcularTotal = (s, e) =>
            {
                decimal total = numCantidad.Value * numPrecio.Value;
                lblTotalValor.Text = $"${total:N2}";
            };
            numCantidad.ValueChanged += calcularTotal;
            numPrecio.ValueChanged += calcularTotal;

            btnGuardar.Click += (s, e) => GuardarMovimiento();
            btnCancelar.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Shown += (s, e) => txtClave.Focus();
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

        private TextBox CrearTextBoxModerno(int x, int y, int width)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Width = width,
                Font = FuenteInput,
                CharacterCasing = CharacterCasing.Upper,
                BorderStyle = BorderStyle.FixedSingle
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

            lblTotalValor = new Label
            {
                Text = "$100.00",
                Font = FuenteTotalValor,
                ForeColor = VerdeOscuro,
                AutoSize = true
            };
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

        // ===== LOGICA DE NEGOCIO =====

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

        private void GuardarMovimiento()
        {
            if (string.IsNullOrWhiteSpace(txtClave.Text))
            {
                MessageBox.Show("Por favor ingrese la clave del color.",
                    "Validaci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtClave.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("Por favor ingrese la descripci\u00f3n.",
                    "Validaci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescripcion.Focus();
                return;
            }

            MovimientoCreado = new Movimiento
            {
                NumeroMovimiento = siguienteNumero,
                ClienteId = cliente.Id,
                Fecha = dtpFecha.Value,
                ClaveColor = txtClave.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim(),
                Base = txtBase.Text.Trim(),
                Unidad = cmbUnidad.SelectedItem?.ToString() ?? "Litro",
                Cantidad = numCantidad.Value,
                Precio = numPrecio.Value,
                Formula = ObtenerFormula()
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}