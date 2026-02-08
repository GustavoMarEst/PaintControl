using PaintControl.Data;
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
        private CatalogoService catalogoService;

        // Controles principales
        private DateTimePicker dtpFecha;
        private TextBox txtClave;
        private TextBox txtBase;
        private ComboBox cmbUnidad;
        private NumericUpDown numCantidad;
        private NumericUpDown numPrecio;
        private Label lblTotalValor;
        private TableLayoutPanel panelFormulas;

        // Catálogo
        private ComboBox cmbTipoPintura;
        private ComboBox cmbLineaPintura;
        private ComboBox cmbAcabado;
        private Label lblDescripcionValor;

        // Botones
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnGuardar;
        private Button btnCancelarEdicion;

        // Fórmulas
        private TextBox[] txtTipos = new TextBox[6];
        private TextBox[] txtValores1 = new TextBox[6];
        private TextBox[] txtValores2 = new TextBox[6];

        // Colores
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

        // Fuentes
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

        public FormDetalleMovimiento(Movimiento movimiento, bool permitirEdicion = true)
        {
            this.movimiento = movimiento;
            this.modoEdicion = false;
            this.catalogoService = new CatalogoService();
            InitializeComponent();
            ConfigurarFormulario(permitirEdicion);
            CargarDatos();
        }

        private void DeshabilitarScrollCombo(ComboBox cmb)
        {
            cmb.MouseWheel += (s, e) => ((HandledMouseEventArgs)e).Handled = true;
        }

        private void DeshabilitarScrollNumeric(NumericUpDown num)
        {
            num.MouseWheel += (s, e) => ((HandledMouseEventArgs)e).Handled = true;
        }

        private void ConfigurarFormulario(bool permitirEdicion)
        {
            this.Text = $"Detalle de Movimiento #{movimiento.NumeroMovimiento}";
            this.Size = new Size(720, 720);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.BackColor = Color.White;

            // ===== HEADER =====
            Panel headerPanel = new Panel { Height = 50, BackColor = AzulPrimario, Dock = DockStyle.Top };
            Label lblTitulo = new Label { Text = $"\U0001f4cb  Movimiento #{movimiento.NumeroMovimiento}", Font = FuenteTitulo, ForeColor = Color.White, Location = new Point(20, 12), AutoSize = true };
            Label lblBadge = new Label { Text = "DETALLE", Font = FuenteBadge, ForeColor = Color.White, BackColor = Color.FromArgb(60, 255, 255, 255), AutoSize = true, Padding = new Padding(8, 3, 8, 3) };
            headerPanel.Controls.Add(lblTitulo);
            headerPanel.Controls.Add(lblBadge);
            headerPanel.Layout += (s, e) => { lblBadge.Location = new Point(headerPanel.Width - lblBadge.Width - 20, (headerPanel.Height - lblBadge.Height) / 2); };

            // ===== SCROLL =====
            Panel scrollPanel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White };
            Panel body = new Panel { Location = new Point(0, 0), Size = new Size(700, 640), BackColor = Color.White };

            int padX = 20, y = 15, fullWidth = 655, halfWidth = 317, gap = 20;
            int cardWidth = (fullWidth - gap * 2) / 3, cardHeight = 55;

            dtpFecha = new DateTimePicker { Format = DateTimePickerFormat.Short, Font = FuenteInput, Location = new Point(-300, -300), Size = new Size(1, 1), Enabled = false };

            string nombreCliente = movimiento.Cliente != null ? movimiento.Cliente.Nombre : "\u2014";
            body.Controls.Add(CrearInfoCard("N\u00ba MOVIMIENTO", movimiento.NumeroMovimiento.ToString(), padX, y, cardWidth, cardHeight));
            body.Controls.Add(CrearInfoCard("FECHA", movimiento.Fecha.ToShortDateString(), padX + cardWidth + gap, y, cardWidth, cardHeight));
            body.Controls.Add(CrearInfoCard("CLIENTE", nombreCliente, padX + (cardWidth + gap) * 2, y, cardWidth, cardHeight));
            body.Controls.Add(dtpFecha);
            y += cardHeight + 15;

            // ===== DATOS DEL PRODUCTO =====
            body.Controls.Add(CrearSeccionTitulo("DATOS DEL PRODUCTO", padX, y, fullWidth));
            y += 30;

            body.Controls.Add(CrearFieldLabel("CLAVE DEL COLOR", padX, y));
            txtClave = CrearTextBoxModerno(padX, y + 18, halfWidth, true);
            body.Controls.Add(txtClave);

            body.Controls.Add(CrearFieldLabel("BASE", padX + halfWidth + gap, y));
            txtBase = CrearTextBoxModerno(padX + halfWidth + gap, y + 18, halfWidth, true);
            body.Controls.Add(txtBase);
            y += 52;

            // ===== DESCRIPCIÓN (CATÁLOGO) =====
            body.Controls.Add(CrearSeccionTitulo("DESCRIPCI\u00d3N (CAT\u00c1LOGO)", padX, y, fullWidth));
            y += 30;

            int cmbWidth = (fullWidth - gap * 2) / 3;

            body.Controls.Add(CrearFieldLabel("TIPO", padX, y));
            cmbTipoPintura = new ComboBox { Location = new Point(padX, y + 18), Width = cmbWidth, Font = FuenteInput, DropDownStyle = ComboBoxStyle.DropDownList, Enabled = false };
            DeshabilitarScrollCombo(cmbTipoPintura);
            body.Controls.Add(cmbTipoPintura);

            body.Controls.Add(CrearFieldLabel("L\u00cdNEA", padX + cmbWidth + gap, y));
            cmbLineaPintura = new ComboBox { Location = new Point(padX + cmbWidth + gap, y + 18), Width = cmbWidth, Font = FuenteInput, DropDownStyle = ComboBoxStyle.DropDownList, Enabled = false };
            DeshabilitarScrollCombo(cmbLineaPintura);
            body.Controls.Add(cmbLineaPintura);

            body.Controls.Add(CrearFieldLabel("ACABADO", padX + (cmbWidth + gap) * 2, y));
            cmbAcabado = new ComboBox { Location = new Point(padX + (cmbWidth + gap) * 2, y + 18), Width = cmbWidth, Font = FuenteInput, DropDownStyle = ComboBoxStyle.DropDownList, Enabled = false };
            DeshabilitarScrollCombo(cmbAcabado);
            body.Controls.Add(cmbAcabado);
            y += 50;

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
            cmbUnidad = new ComboBox { Location = new Point(padX, y + 18), Width = halfWidth, Font = FuenteInput, DropDownStyle = ComboBoxStyle.DropDownList, Enabled = false };
            cmbUnidad.Items.AddRange(new object[] { "LT", "GL", "KG", "PZ", "M2", "M3", "Litro", "Gal\u00f3n", "Cubeta" });
            DeshabilitarScrollCombo(cmbUnidad);
            body.Controls.Add(cmbUnidad);

            body.Controls.Add(CrearFieldLabel("CANTIDAD", padX + halfWidth + gap, y));
            numCantidad = new NumericUpDown { Location = new Point(padX + halfWidth + gap, y + 18), Width = halfWidth, Font = FuenteInput, DecimalPlaces = 0, Minimum = 1, Maximum = 9999, ReadOnly = true, BorderStyle = BorderStyle.FixedSingle };
            DeshabilitarScrollNumeric(numCantidad);
            body.Controls.Add(numCantidad);
            y += 52;

            body.Controls.Add(CrearFieldLabel("PRECIO", padX, y));
            numPrecio = new NumericUpDown { Location = new Point(padX, y + 18), Width = halfWidth, Font = FuenteInput, DecimalPlaces = 2, Minimum = 0.01m, Maximum = 99999.99m, ReadOnly = true, BorderStyle = BorderStyle.FixedSingle };
            DeshabilitarScrollNumeric(numPrecio);
            body.Controls.Add(numPrecio);

            Panel totalPanel = CrearTotalPanel(padX + halfWidth + gap, y + 2, halfWidth);
            body.Controls.Add(totalPanel);
            y += 60;

            // ===== FÓRMULA =====
            body.Controls.Add(CrearSeccionTitulo("F\u00d3RMULA DE COLOR", padX, y, fullWidth));
            y += 30;

            panelFormulas = CrearPanelFormulas(padX, y, fullWidth, true);
            body.Controls.Add(panelFormulas);
            y += panelFormulas.Height + 15;

            // ===== BOTONES =====
            int btnX = padX;
            btnEditar = CrearBoton("\u270f\ufe0f  Editar", AzulEditar, btnX, y); btnEditar.Visible = permitirEdicion; btnX += 155;
            btnEliminar = CrearBoton("\U0001f5d1\ufe0f  Eliminar", RojoEliminar, btnX, y); btnEliminar.Visible = permitirEdicion; btnX += 155;
            btnGuardar = CrearBoton("\U0001f4be  Guardar", VerdePrimario, padX, y); btnGuardar.Visible = false;
            btnCancelarEdicion = CrearBoton("\u2715  Cancelar", GrisBotonSecundario, padX + 155, y); btnCancelarEdicion.ForeColor = TextoLabel; btnCancelarEdicion.Visible = false;
            Button btnCerrar = CrearBoton("Cerrar", GrisBotonSecundario, btnX, y); btnCerrar.ForeColor = TextoLabel;

            body.Controls.AddRange(new Control[] { btnEditar, btnEliminar, btnGuardar, btnCancelarEdicion, btnCerrar });
            y += 55;

            body.Height = y;
            scrollPanel.Controls.Add(body);
            this.Controls.Add(scrollPanel);
            this.Controls.Add(headerPanel);

            // ===== EVENTOS =====
            EventHandler calcTotal = (s, e) =>
            {
                decimal total = numCantidad.Value * numPrecio.Value;
                lblTotalValor.Text = $"${total:N2}";
                lblTotalValor.Location = new Point(totalPanel.Width - lblTotalValor.PreferredWidth - 12, 8);
            };
            numCantidad.ValueChanged += calcTotal;
            numPrecio.ValueChanged += calcTotal;

            // Cascada del catálogo (habilitado solo en modo edición)
            ConfigurarCatalogoCascada();

            btnEditar.Click += (s, e) => ActivarModoEdicion();
            btnCancelarEdicion.Click += (s, e) => DesactivarModoEdicion();
            btnGuardar.Click += (s, e) => GuardarCambios();
            btnEliminar.Click += (s, e) => EliminarMovimiento();
            btnCerrar.Click += (s, e) => this.Close();
        }

        // ===== CATÁLOGO CASCADA =====

        private void ConfigurarCatalogoCascada()
        {
            cmbTipoPintura.SelectedIndexChanged += (s, e) =>
            {
                if (!modoEdicion) return;
                cmbLineaPintura.Items.Clear();
                cmbAcabado.Items.Clear();
                cmbLineaPintura.Enabled = false;
                cmbAcabado.Enabled = false;

                if (cmbTipoPintura.SelectedItem is TipoPintura tipo)
                {
                    var lineas = catalogoService.ObtenerLineasPorTipo(tipo.Id);
                    if (lineas.Count > 0)
                    {
                        foreach (var linea in lineas) cmbLineaPintura.Items.Add(linea);
                        cmbLineaPintura.SelectedIndex = 0;
                        cmbLineaPintura.Enabled = true;
                    }
                }
                ActualizarDescripcion();
            };

            cmbLineaPintura.SelectedIndexChanged += (s, e) =>
            {
                if (!modoEdicion) return;
                cmbAcabado.Items.Clear();
                cmbAcabado.Enabled = false;

                if (cmbLineaPintura.SelectedItem is LineaPintura linea)
                {
                    var acabados = catalogoService.ObtenerAcabadosPorLinea(linea.Id);
                    if (acabados.Count > 0)
                    {
                        foreach (var acabado in acabados) cmbAcabado.Items.Add(acabado);
                        cmbAcabado.SelectedIndex = 0;
                        cmbAcabado.Enabled = true;
                    }
                }
                ActualizarDescripcion();
            };

            cmbAcabado.SelectedIndexChanged += (s, e) =>
            {
                if (modoEdicion) ActualizarDescripcion();
            };
        }

        private void ActualizarDescripcion()
        {
            string abrTipo = (cmbTipoPintura.SelectedItem is TipoPintura t) ? t.Abreviatura : "";
            string abrLinea = (cmbLineaPintura.SelectedItem is LineaPintura l) ? l.Abreviatura : "";
            string nombreAcabado = (cmbAcabado.SelectedItem is AcabadoPintura a) ? a.Nombre : "";
            lblDescripcionValor.Text = catalogoService.ConstruirDescripcion(abrTipo, abrLinea, nombreAcabado);
        }

        // ===== CARGA DE DATOS =====

        private void CargarDatos()
        {
            dtpFecha.Value = movimiento.Fecha;
            txtClave.Text = movimiento.ClaveColor ?? "";
            txtBase.Text = movimiento.Base ?? "";
            lblDescripcionValor.Text = movimiento.Descripcion ?? "";

            if (!string.IsNullOrWhiteSpace(movimiento.Unidad))
            {
                int idx = cmbUnidad.FindStringExact(movimiento.Unidad);
                if (idx >= 0) cmbUnidad.SelectedIndex = idx;
                else { cmbUnidad.Items.Add(movimiento.Unidad); cmbUnidad.SelectedItem = movimiento.Unidad; }
            }
            else if (cmbUnidad.Items.Count > 0) cmbUnidad.SelectedIndex = 0;

            numCantidad.Value = Math.Max(numCantidad.Minimum, Math.Min(numCantidad.Maximum, Math.Round(movimiento.Cantidad)));
            numPrecio.Value = Math.Max(numPrecio.Minimum, Math.Min(numPrecio.Maximum, movimiento.Precio));
            lblTotalValor.Text = $"${movimiento.Total:N2}";
            lblTotalValor.Location = new Point(lblTotalValor.Parent.Width - lblTotalValor.PreferredWidth - 12, 8);

            CargarFormula(movimiento.Formula);
        }

        private void CargarFormula(string formula)
        {
            if (string.IsNullOrWhiteSpace(formula)) return;
            string[] lineas = formula.Split(new[] { '|', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lineas.Length && i < 6; i++)
            {
                string[] partes = lineas[i].Trim().Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (partes.Length != 2) continue;
                string[] valores = partes[1].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (txtTipos[i] != null) txtTipos[i].Text = partes[0].Trim();
                if (valores.Length > 0 && txtValores1[i] != null) txtValores1[i].Text = valores[0];
                if (valores.Length > 1 && txtValores2[i] != null) txtValores2[i].Text = valores[1];
            }
        }

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

        // ===== MODO EDICIÓN =====

        private void ActivarModoEdicion()
        {
            modoEdicion = true;
            dtpFecha.Enabled = true;
            txtClave.ReadOnly = false;
            txtBase.ReadOnly = false;
            cmbUnidad.Enabled = true;
            numCantidad.ReadOnly = false;
            numPrecio.ReadOnly = false;

            // Habilitar catálogo
            cmbTipoPintura.Enabled = true;
            cmbTipoPintura.Items.Clear();
            var tipos = catalogoService.ObtenerTiposActivos();
            foreach (var tipo in tipos) cmbTipoPintura.Items.Add(tipo);
            // No seleccionar ninguno — el usuario elige si quiere cambiar la descripción

            for (int i = 0; i < 6; i++)
            {
                if (txtTipos[i] != null) txtTipos[i].ReadOnly = false;
                if (txtValores1[i] != null) txtValores1[i].ReadOnly = false;
                if (txtValores2[i] != null) txtValores2[i].ReadOnly = false;
            }

            btnEditar.Visible = false; btnEliminar.Visible = false;
            btnGuardar.Visible = true; btnCancelarEdicion.Visible = true;
        }

        private void DesactivarModoEdicion()
        {
            modoEdicion = false;
            CargarDatos();
            dtpFecha.Enabled = false;
            txtClave.ReadOnly = true;
            txtBase.ReadOnly = true;
            cmbUnidad.Enabled = false;
            numCantidad.ReadOnly = true;
            numPrecio.ReadOnly = true;
            cmbTipoPintura.Enabled = false;
            cmbLineaPintura.Enabled = false;
            cmbAcabado.Enabled = false;
            cmbTipoPintura.Items.Clear();
            cmbLineaPintura.Items.Clear();
            cmbAcabado.Items.Clear();

            for (int i = 0; i < 6; i++)
            {
                if (txtTipos[i] != null) txtTipos[i].ReadOnly = true;
                if (txtValores1[i] != null) txtValores1[i].ReadOnly = true;
                if (txtValores2[i] != null) txtValores2[i].ReadOnly = true;
            }

            btnEditar.Visible = true; btnEliminar.Visible = true;
            btnGuardar.Visible = false; btnCancelarEdicion.Visible = false;
        }

        private void GuardarCambios()
        {
            if (string.IsNullOrWhiteSpace(txtClave.Text))
            {
                MessageBox.Show("La clave es obligatoria.", "Validaci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string descripcion = lblDescripcionValor.Text.Trim();
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                MessageBox.Show("La descripci\u00f3n es obligatoria.", "Validaci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MovimientoActualizado = new Movimiento
            {
                Id = movimiento.Id,
                NumeroMovimiento = movimiento.NumeroMovimiento,
                ClienteId = movimiento.ClienteId,
                Fecha = dtpFecha.Value,
                ClaveColor = txtClave.Text.Trim(),
                Descripcion = descripcion,
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
            var r = MessageBox.Show($"\u00bfEliminar movimiento #{movimiento.NumeroMovimiento}?\n\nEsta acci\u00f3n no se puede deshacer.",
                "Confirmar Eliminaci\u00f3n", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (r == DialogResult.Yes) { Eliminado = true; this.DialogResult = DialogResult.OK; this.Close(); }
        }

        // ===== HELPERS =====

        private Panel CrearInfoCard(string label, string value, int x, int y, int width, int height)
        {
            Panel card = new Panel { Location = new Point(x, y), Size = new Size(width, height), BackColor = FondoTarjeta };
            card.Paint += (s, e) => { using (Pen p = new Pen(BordeTarjeta, 1)) using (GraphicsPath gp = CrearRR(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 8)) { e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; e.Graphics.DrawPath(p, gp); } };
            card.Controls.Add(new Label { Text = label, Font = FuenteInfoLabel, ForeColor = TextoSecundario, Location = new Point(10, 6), AutoSize = true });
            card.Controls.Add(new Label { Text = value, Font = FuenteInfoValor, ForeColor = AzulOscuro, Location = new Point(10, 26), Size = new Size(width - 20, height - 30), AutoEllipsis = true });
            return card;
        }

        private Panel CrearSeccionTitulo(string texto, int x, int y, int width)
        {
            Panel s = new Panel { Location = new Point(x, y), Size = new Size(width, 24), BackColor = Color.Transparent };
            s.Paint += (sender, e) => { using (Brush b = new SolidBrush(AzulPrimario)) { e.Graphics.FillRectangle(b, 0, 3, 4, 14); e.Graphics.DrawString(texto, FuenteSeccion, b, 12, 1); } using (Pen p = new Pen(Color.FromArgb(232, 244, 253), 1)) e.Graphics.DrawLine(p, 0, s.Height - 1, s.Width, s.Height - 1); };
            return s;
        }

        private Label CrearFieldLabel(string t, int x, int y) => new Label { Text = t, Font = FuenteFieldLabel, ForeColor = TextoLabel, Location = new Point(x, y), AutoSize = true };

        private TextBox CrearTextBoxModerno(int x, int y, int w, bool ro = false) => new TextBox { Location = new Point(x, y), Width = w, Font = FuenteInput, CharacterCasing = CharacterCasing.Upper, BorderStyle = BorderStyle.FixedSingle, ReadOnly = ro };

        private Panel CrearTotalPanel(int x, int y, int width)
        {
            Panel panel = new Panel { Location = new Point(x, y), Size = new Size(width, 50), BackColor = FondoVerde };
            panel.Paint += (s, e) => { using (Pen p = new Pen(BordeVerde, 2)) using (GraphicsPath gp = CrearRR(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 8)) { e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; e.Graphics.DrawPath(p, gp); } };
            panel.Controls.Add(new Label { Text = "TOTAL", Font = FuenteTotalLabel, ForeColor = VerdePrimario, Location = new Point(12, 14), AutoSize = true });
            lblTotalValor = new Label { Text = "$0.00", Font = FuenteTotalValor, ForeColor = VerdeOscuro, AutoSize = true };
            lblTotalValor.Location = new Point(width - lblTotalValor.PreferredWidth - 12, 8);
            panel.Controls.Add(lblTotalValor);
            return panel;
        }

        private TableLayoutPanel CrearPanelFormulas(int x, int y, int width, bool readOnly)
        {
            TableLayoutPanel panel = new TableLayoutPanel { Location = new Point(x, y), Width = width, Height = 110, BackColor = FondoFormula, ColumnCount = 2, RowCount = 3, CellBorderStyle = TableLayoutPanelCellBorderStyle.None, Padding = new Padding(8, 5, 8, 5) };
            panel.Paint += (s, e) => { using (Pen p = new Pen(BordeTarjeta, 1)) using (GraphicsPath gp = CrearRR(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 8)) { e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; e.Graphics.DrawPath(p, gp); } };
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

        private Button CrearBoton(string texto, Color bc, int x, int y)
        {
            Button btn = new Button { Text = texto, Size = new Size(145, 40), Location = new Point(x, y), BackColor = bc, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = FuenteBoton, Cursor = Cursors.Hand };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private GraphicsPath CrearRR(Rectangle rect, int r)
        {
            GraphicsPath p = new GraphicsPath();
            p.AddArc(rect.X, rect.Y, r, r, 180, 90);
            p.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
            p.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
            p.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
            p.CloseFigure();
            return p;
        }
    }
}