using PaintControl.Data;
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
        private CatalogoService catalogoService;

        // Referencias directas a controles principales
        private DateTimePicker dtpFecha;
        private TextBox txtClave;
        private TextBox txtBase;
        private ComboBox cmbUnidad;
        private NumericUpDown numCantidad;
        private NumericUpDown numPrecio;
        private Label lblTotalValor;
        private TableLayoutPanel panelFormulas;

        // Controles del catálogo - descripción unificada
        private ComboBox cmbTipoPintura;
        private ComboBox cmbLineaPintura;
        private ComboBox cmbAcabado;
        private Label lblDescripcionValor; // Label que muestra la descripción generada

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
        private static readonly Font FuenteTitulo = new Font("Segoe UI", 14F, FontStyle.Bold);
        private static readonly Font FuenteBadge = new Font("Segoe UI", 9F, FontStyle.Bold);
        private static readonly Font FuenteInfoLabel = new Font("Segoe UI", 8F, FontStyle.Bold);
        private static readonly Font FuenteInfoValor = new Font("Segoe UI", 10F, FontStyle.Bold);
        private static readonly Font FuenteSeccion = new Font("Segoe UI", 10F, FontStyle.Bold);
        private static readonly Font FuenteFieldLabel = new Font("Segoe UI", 9F, FontStyle.Bold);
        private static readonly Font FuenteInput = new Font("Segoe UI", 10F);
        private static readonly Font FuenteBoton = new Font("Segoe UI", 10F, FontStyle.Bold);
        private static readonly Font FuenteTotalLabel = new Font("Segoe UI", 10F, FontStyle.Bold);
        private static readonly Font FuenteTotalValor = new Font("Segoe UI", 16F, FontStyle.Bold);
        private static readonly Font FuenteFormula = new Font("Segoe UI", 9F);
        private static readonly Font FuenteIgual = new Font("Segoe UI", 10F, FontStyle.Bold);
        private static readonly Font FuenteDescripcion = new Font("Segoe UI", 11F, FontStyle.Bold);

        public FormAgregarMovimiento(Cliente cliente, int siguienteNumeroMovimiento)
        {
            this.cliente = cliente;
            this.siguienteNumero = siguienteNumeroMovimiento;
            this.catalogoService = new CatalogoService();
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = $"Agregar Movimiento - {cliente.Nombre}";
            this.Size = new Size(720, 720);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.BackColor = Color.White;

            // ===== HEADER =====
            Panel headerPanel = new Panel
            {
                Height = 50,
                BackColor = AzulPrimario,
                Dock = DockStyle.Top
            };

            Label lblTitulo = new Label
            {
                Text = "\U0001f3a8  Nueva Compra de Pintura",
                Font = FuenteTitulo,
                ForeColor = Color.White,
                Location = new Point(20, 12),
                AutoSize = true
            };

            Label lblBadge = new Label
            {
                Text = cliente.Nombre,
                Font = FuenteBadge,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 255, 255, 255),
                AutoSize = true,
                Padding = new Padding(8, 3, 8, 3),
                TextAlign = ContentAlignment.MiddleCenter,
                MaximumSize = new Size(220, 0)
            };

            headerPanel.Controls.Add(lblTitulo);
            headerPanel.Controls.Add(lblBadge);
            headerPanel.Layout += (s, e) =>
            {
                lblBadge.Location = new Point(headerPanel.Width - lblBadge.Width - 20, (headerPanel.Height - lblBadge.Height) / 2);
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
                Size = new Size(700, 640),
                BackColor = Color.White
            };

            int padX = 20;
            int y = 15;
            int fullWidth = 655;
            int halfWidth = 317;
            int gap = 20;

            // ===== INFO CARDS =====
            int cardWidth = (fullWidth - gap * 2) / 3;
            int cardHeight = 55;

            dtpFecha = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Font = FuenteInput,
                Location = new Point(-300, -300),
                Size = new Size(1, 1)
            };

            Panel card1 = CrearInfoCard("N\u00ba MOVIMIENTO", siguienteNumero.ToString(), padX, y, cardWidth, cardHeight);
            Panel card2 = CrearInfoCard("FECHA", DateTime.Now.ToShortDateString(), padX + cardWidth + gap, y, cardWidth, cardHeight);
            Panel card3 = CrearInfoCard("CLIENTE", cliente.Nombre, padX + (cardWidth + gap) * 2, y, cardWidth, cardHeight);

            body.Controls.Add(card1);
            body.Controls.Add(card2);
            body.Controls.Add(card3);
            body.Controls.Add(dtpFecha);
            y += cardHeight + 15;

            // ===== DATOS DEL PRODUCTO =====
            body.Controls.Add(CrearSeccionTitulo("DATOS DEL PRODUCTO", padX, y, fullWidth));
            y += 30;

            body.Controls.Add(CrearFieldLabel("CLAVE DEL COLOR", padX, y));
            txtClave = CrearTextBoxModerno(padX, y + 18, halfWidth);
            txtClave.MaxLength = 20;
            body.Controls.Add(txtClave);

            body.Controls.Add(CrearFieldLabel("BASE", padX + halfWidth + gap, y));
            txtBase = CrearTextBoxModerno(padX + halfWidth + gap, y + 18, halfWidth);
            txtBase.MaxLength = 50;
            body.Controls.Add(txtBase);
            y += 52;

            // ===== DESCRIPCIÓN (CATÁLOGO) - ComboBoxes + Label unificado =====
            body.Controls.Add(CrearSeccionTitulo("DESCRIPCI\u00d3N (CAT\u00c1LOGO)", padX, y, fullWidth));
            y += 30;

            int cmbWidth = (fullWidth - gap * 2) / 3;

            body.Controls.Add(CrearFieldLabel("TIPO", padX, y));
            cmbTipoPintura = new ComboBox
            {
                Location = new Point(padX, y + 18),
                Width = cmbWidth,
                Font = FuenteInput,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            body.Controls.Add(cmbTipoPintura);

            body.Controls.Add(CrearFieldLabel("L\u00cdNEA", padX + cmbWidth + gap, y));
            cmbLineaPintura = new ComboBox
            {
                Location = new Point(padX + cmbWidth + gap, y + 18),
                Width = cmbWidth,
                Font = FuenteInput,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            body.Controls.Add(cmbLineaPintura);

            body.Controls.Add(CrearFieldLabel("ACABADO", padX + (cmbWidth + gap) * 2, y));
            cmbAcabado = new ComboBox
            {
                Location = new Point(padX + (cmbWidth + gap) * 2, y + 18),
                Width = cmbWidth,
                Font = FuenteInput,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            body.Controls.Add(cmbAcabado);
            y += 50;

            // Label unificado de descripción (muestra resultado y se envía a BD)
            body.Controls.Add(CrearFieldLabel("DESCRIPCI\u00d3N", padX, y));
            lblDescripcionValor = new Label
            {
                Location = new Point(padX, y + 18),
                Size = new Size(fullWidth, 28),
                Font = FuenteDescripcion,
                ForeColor = AzulOscuro,
                BackColor = FondoTarjeta,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                Text = ""
            };
            // Borde sutil
            lblDescripcionValor.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(BordeTarjeta, 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, lblDescripcionValor.Width - 1, lblDescripcionValor.Height - 1);
            };
            body.Controls.Add(lblDescripcionValor);
            y += 52;

            // ===== INFORMACIÓN DE VENTA =====
            body.Controls.Add(CrearSeccionTitulo("INFORMACI\u00d3N DE VENTA", padX, y, fullWidth));
            y += 30;

            body.Controls.Add(CrearFieldLabel("UNIDAD", padX, y));
            cmbUnidad = new ComboBox
            {
                Location = new Point(padX, y + 18),
                Width = halfWidth,
                Font = FuenteInput,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            cmbUnidad.Items.AddRange(new object[] { "Litro", "Gal\u00f3n", "Cubeta" });
            cmbUnidad.SelectedIndex = 0;
            body.Controls.Add(cmbUnidad);

            body.Controls.Add(CrearFieldLabel("CANTIDAD", padX + halfWidth + gap, y));
            numCantidad = new NumericUpDown
            {
                Location = new Point(padX + halfWidth + gap, y + 18),
                Width = halfWidth,
                Font = FuenteInput,
                DecimalPlaces = 2,
                Minimum = 0.01m,
                Maximum = 9999.99m,
                Value = 1,
                BorderStyle = BorderStyle.FixedSingle
            };
            body.Controls.Add(numCantidad);
            y += 52;

            body.Controls.Add(CrearFieldLabel("PRECIO", padX, y));
            numPrecio = new NumericUpDown
            {
                Location = new Point(padX, y + 18),
                Width = halfWidth,
                Font = FuenteInput,
                DecimalPlaces = 2,
                Minimum = 0.01m,
                Maximum = 99999.99m,
                Value = 100,
                BorderStyle = BorderStyle.FixedSingle
            };
            body.Controls.Add(numPrecio);

            Panel totalPanel = CrearTotalPanel(padX + halfWidth + gap, y + 2, halfWidth);
            body.Controls.Add(totalPanel);
            y += 60;

            // ===== FÓRMULA DE COLOR =====
            body.Controls.Add(CrearSeccionTitulo("F\u00d3RMULA DE COLOR", padX, y, fullWidth));
            y += 30;

            panelFormulas = CrearPanelFormulas(padX, y, fullWidth, false);
            body.Controls.Add(panelFormulas);
            y += panelFormulas.Height + 15;

            // ===== BOTONES =====
            Button btnGuardar = CrearBoton("\U0001f4be  Guardar", VerdePrimario, padX, y);
            Button btnCancelar = CrearBoton("\u2715  Cancelar", GrisBotonSecundario, padX + 160, y);
            btnCancelar.ForeColor = TextoLabel;
            body.Controls.Add(btnGuardar);
            body.Controls.Add(btnCancelar);
            y += 55;

            body.Height = y;
            scrollPanel.Controls.Add(body);
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

            ConfigurarCatalogoCascada();
            this.Shown += (s, e) => txtClave.Focus();
        }

        // ===== CATÁLOGO EN CASCADA =====

        private void ConfigurarCatalogoCascada()
        {
            CargarTiposPintura();

            cmbTipoPintura.SelectedIndexChanged += (s, e) =>
            {
                cmbLineaPintura.Items.Clear();
                cmbAcabado.Items.Clear();
                cmbLineaPintura.Enabled = false;
                cmbAcabado.Enabled = false;

                if (cmbTipoPintura.SelectedItem is TipoPintura tipo)
                {
                    var lineas = catalogoService.ObtenerLineasPorTipo(tipo.Id);
                    if (lineas.Count > 0)
                    {
                        cmbLineaPintura.Items.Add(new LineaPlaceholder());
                        foreach (var linea in lineas)
                            cmbLineaPintura.Items.Add(linea);
                        cmbLineaPintura.SelectedIndex = 0;
                        cmbLineaPintura.Enabled = true;
                    }
                }
                ActualizarDescripcion();
            };

            cmbLineaPintura.SelectedIndexChanged += (s, e) =>
            {
                cmbAcabado.Items.Clear();
                cmbAcabado.Enabled = false;

                if (cmbLineaPintura.SelectedItem is LineaPintura linea)
                {
                    var acabados = catalogoService.ObtenerAcabadosPorLinea(linea.Id);
                    if (acabados.Count > 0)
                    {
                        cmbAcabado.Items.Add(new AcabadoPlaceholder());
                        foreach (var acabado in acabados)
                            cmbAcabado.Items.Add(acabado);
                        cmbAcabado.SelectedIndex = 0;
                        cmbAcabado.Enabled = true;
                    }
                }
                ActualizarDescripcion();
            };

            cmbAcabado.SelectedIndexChanged += (s, e) => ActualizarDescripcion();
        }

        private void CargarTiposPintura()
        {
            cmbTipoPintura.Items.Clear();
            cmbTipoPintura.Items.Add(new TipoPlaceholder());
            var tipos = catalogoService.ObtenerTiposActivos();
            foreach (var tipo in tipos)
                cmbTipoPintura.Items.Add(tipo);
            cmbTipoPintura.SelectedIndex = 0;
        }

        private void ActualizarDescripcion()
        {
            string abrTipo = (cmbTipoPintura.SelectedItem is TipoPintura t) ? t.Abreviatura : "";
            string abrLinea = (cmbLineaPintura.SelectedItem is LineaPintura l) ? l.Abreviatura : "";
            string nombreAcabado = (cmbAcabado.SelectedItem is AcabadoPintura a) ? a.Nombre : "";

            string desc = catalogoService.ConstruirDescripcion(abrTipo, abrLinea, nombreAcabado);
            lblDescripcionValor.Text = desc;
        }

        private string ObtenerDescripcion()
        {
            return lblDescripcionValor.Text.Trim();
        }

        // Placeholders
        private class TipoPlaceholder { public override string ToString() => "-- Seleccione --"; }
        private class LineaPlaceholder { public override string ToString() => "-- Seleccione --"; }
        private class AcabadoPlaceholder { public override string ToString() => "-- Seleccione --"; }

        // ===== HELPERS =====

        private Panel CrearInfoCard(string label, string value, int x, int y, int width, int height)
        {
            Panel card = new Panel { Location = new Point(x, y), Size = new Size(width, height), BackColor = FondoTarjeta };
            card.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(BordeTarjeta, 1))
                using (GraphicsPath path = CrearRectanguloRedondeado(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 8))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawPath(pen, path);
                }
            };
            card.Controls.Add(new Label { Text = label, Font = FuenteInfoLabel, ForeColor = TextoSecundario, Location = new Point(10, 6), AutoSize = true });
            card.Controls.Add(new Label { Text = value, Font = FuenteInfoValor, ForeColor = AzulOscuro, Location = new Point(10, 26), Size = new Size(width - 20, height - 30), AutoEllipsis = true });
            return card;
        }

        private Panel CrearSeccionTitulo(string texto, int x, int y, int width)
        {
            Panel s = new Panel { Location = new Point(x, y), Size = new Size(width, 24), BackColor = Color.Transparent };
            s.Paint += (sender, e) =>
            {
                using (Brush b = new SolidBrush(AzulPrimario))
                {
                    e.Graphics.FillRectangle(b, 0, 3, 4, 14);
                    e.Graphics.DrawString(texto, FuenteSeccion, b, 12, 1);
                }
                using (Pen p = new Pen(Color.FromArgb(232, 244, 253), 1))
                    e.Graphics.DrawLine(p, 0, s.Height - 1, s.Width, s.Height - 1);
            };
            return s;
        }

        private Label CrearFieldLabel(string texto, int x, int y)
        {
            return new Label { Text = texto, Font = FuenteFieldLabel, ForeColor = TextoLabel, Location = new Point(x, y), AutoSize = true };
        }

        private TextBox CrearTextBoxModerno(int x, int y, int width)
        {
            return new TextBox { Location = new Point(x, y), Width = width, Font = FuenteInput, CharacterCasing = CharacterCasing.Upper, BorderStyle = BorderStyle.FixedSingle };
        }

        private Panel CrearTotalPanel(int x, int y, int width)
        {
            Panel panel = new Panel { Location = new Point(x, y), Size = new Size(width, 50), BackColor = FondoVerde };
            panel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(BordeVerde, 2))
                using (GraphicsPath path = CrearRectanguloRedondeado(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 8))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawPath(pen, path);
                }
            };
            panel.Controls.Add(new Label { Text = "TOTAL", Font = FuenteTotalLabel, ForeColor = VerdePrimario, Location = new Point(12, 14), AutoSize = true });
            lblTotalValor = new Label { Text = "$100.00", Font = FuenteTotalValor, ForeColor = VerdeOscuro, AutoSize = true };
            lblTotalValor.Location = new Point(width - lblTotalValor.PreferredWidth - 12, 8);
            panel.Controls.Add(lblTotalValor);
            return panel;
        }

        private TableLayoutPanel CrearPanelFormulas(int x, int y, int width, bool readOnly)
        {
            TableLayoutPanel panel = new TableLayoutPanel
            {
                Location = new Point(x, y),
                Width = width,
                Height = 110,
                BackColor = FondoFormula,
                ColumnCount = 2,
                RowCount = 3,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                Padding = new Padding(8, 5, 8, 5)
            };
            panel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(BordeTarjeta, 1))
                using (GraphicsPath path = CrearRectanguloRedondeado(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 8))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawPath(pen, path);
                }
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            for (int i = 0; i < 3; i++) panel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));

            for (int i = 0; i < 6; i++)
            {
                int col = i / 3, row = i % 3;
                Panel fp = new Panel { Dock = DockStyle.Fill, Margin = new Padding(2), BackColor = FondoFormula };
                FlowLayoutPanel fl = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true, WrapContents = false };

                txtTipos[i] = new TextBox { Width = 50, Font = FuenteFormula, TextAlign = HorizontalAlignment.Center, CharacterCasing = CharacterCasing.Upper, BorderStyle = BorderStyle.FixedSingle, ReadOnly = readOnly, Margin = new Padding(0) };
                Label eq = new Label { Text = "=", Font = FuenteIgual, ForeColor = Color.FromArgb(160, 174, 192), AutoSize = true, Margin = new Padding(4, 0, 4, 0) };
                txtValores1[i] = new TextBox { Width = 50, Font = FuenteFormula, TextAlign = HorizontalAlignment.Center, BorderStyle = BorderStyle.FixedSingle, ReadOnly = readOnly, Margin = new Padding(0, 0, 4, 0) };
                txtValores2[i] = new TextBox { Width = 50, Font = FuenteFormula, TextAlign = HorizontalAlignment.Center, BorderStyle = BorderStyle.FixedSingle, ReadOnly = readOnly, Margin = new Padding(0) };

                fl.Controls.AddRange(new Control[] { txtTipos[i], eq, txtValores1[i], txtValores2[i] });
                fp.Controls.Add(fl);
                fp.Resize += (s, e) => { fl.Location = new Point(Math.Max(0, (fp.Width - fl.Width) / 2), Math.Max(0, (fp.Height - fl.Height) / 2)); };
                panel.Controls.Add(fp, col, row);
            }
            return panel;
        }

        private Button CrearBoton(string texto, Color backColor, int x, int y)
        {
            Button btn = new Button { Text = texto, Size = new Size(145, 40), Location = new Point(x, y), BackColor = backColor, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = FuenteBoton, Cursor = Cursors.Hand };
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
                if (txtTipos[i] == null || string.IsNullOrWhiteSpace(txtTipos[i].Text)) continue;
                string f = $"{txtTipos[i].Text.Trim()} = {txtValores1[i].Text.Trim()}";
                if (txtValores2[i] != null && !string.IsNullOrWhiteSpace(txtValores2[i].Text))
                    f += $" {txtValores2[i].Text.Trim()}";
                formulas.Add(f);
            }
            return string.Join("|", formulas);
        }

        private void GuardarMovimiento()
        {
            if (string.IsNullOrWhiteSpace(txtClave.Text))
            {
                MessageBox.Show("Por favor ingrese la clave del color.", "Validaci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtClave.Focus();
                return;
            }

            string descripcion = ObtenerDescripcion();
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                MessageBox.Show("Seleccione tipo, l\u00ednea y acabado del cat\u00e1logo para generar la descripci\u00f3n.", "Validaci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTipoPintura.Focus();
                return;
            }

            MovimientoCreado = new Movimiento
            {
                NumeroMovimiento = siguienteNumero,
                ClienteId = cliente.Id,
                Fecha = dtpFecha.Value,
                ClaveColor = txtClave.Text.Trim(),
                Descripcion = descripcion,
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