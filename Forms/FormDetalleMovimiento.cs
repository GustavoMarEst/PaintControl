using PaintControl.Models;
using System;
using System.Drawing;
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

        // Referencias directas a controles (evita Controls.Find)
        private DateTimePicker dtpFecha;
        private TextBox txtClave;
        private TextBox txtDescripcion;
        private TextBox txtBase;
        private ComboBox cmbUnidad;
        private NumericUpDown numCantidad;
        private NumericUpDown numPrecio;
        private Label lblTotalValor;
        private TextBox[] txtTipos = new TextBox[6];
        private TextBox[] txtValores1 = new TextBox[6];
        private TextBox[] txtValores2 = new TextBox[6];

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
            this.MinimumSize = new Size(750, 720);
            this.Size = new Size(750, 720);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.BackColor = Color.White;

            // TableLayoutPanel principal para diseño responsivo
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Name = "mainLayout",
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.White,
                ColumnCount = 1,
                RowCount = 3,
                AutoScroll = true
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Título
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Panel de información
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Botones

            // Título
            Label lblTitulo = new Label
            {
                Text = $"Movimiento #{movimiento.NumeroMovimiento}",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.FromArgb(46, 92, 138),
                Margin = new Padding(10, 0, 10, 20),
                Anchor = AnchorStyles.Left
            };

            // Panel de información con scroll
            Panel scrollablePanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White,
                Margin = new Padding(0)
            };

            Panel infoPanel = new Panel
            {
                Name = "infoPanel",
                BackColor = Color.FromArgb(245, 248, 250),
                BorderStyle = BorderStyle.FixedSingle,
                AutoSize = true,
                MinimumSize = new Size(600, 520),
                Padding = new Padding(20),
                Dock = DockStyle.Top
            };

            int yPos = 20;
            int labelWidth = 150;
            int controlX = labelWidth + 30;
            int controlWidth = 0; // Se calculará dinámicamente

            // Función auxiliar para calcular el ancho de los controles
            EventHandler ResizeControls = (s, e) =>
            {
                int newControlWidth = infoPanel.ClientSize.Width - controlX - 40;
                if (newControlWidth < 200) newControlWidth = 200;

                foreach (Control ctrl in infoPanel.Controls)
                {
                    if (ctrl is TextBox || ctrl is ComboBox || ctrl is DateTimePicker)
                    {
                        if (ctrl.Left == controlX)
                        {
                            ctrl.Width = newControlWidth;
                        }
                    }
                }

                // Ajustar panel de fórmulas
                Control[] panelFormulasArray = infoPanel.Controls.Find("panelFormulas", false);
                if (panelFormulasArray.Length > 0)
                {
                    panelFormulasArray[0].Width = infoPanel.ClientSize.Width - 40;
                }
            };
            infoPanel.Resize += ResizeControls;

            // Calcular ancho inicial
            controlWidth = infoPanel.MinimumSize.Width - controlX - 60;

            // Fecha
            Label lblFechaLabel = CrearLabel("Fecha:", 20, yPos);
            dtpFecha = new DateTimePicker
            {
                Name = "dtpFecha",
                Location = new Point(controlX, yPos - 2),
                Width = controlWidth,
                Format = DateTimePickerFormat.Short,
                Enabled = false,
                Font = new Font("Segoe UI", 11F),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            yPos += 45;

            // Clave del Color
            Label lblClaveLabel = CrearLabel("Clave del Color:", 20, yPos);
            txtClave = new TextBox
            {
                Name = "txtClave",
                Location = new Point(controlX, yPos),
                Width = controlWidth,
                Font = new Font("Segoe UI", 11F),
                ReadOnly = true,
                CharacterCasing = CharacterCasing.Upper,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            yPos += 45;

            // Descripción
            Label lblDescLabel = CrearLabel("Descripción:", 20, yPos);
            txtDescripcion = new TextBox
            {
                Name = "txtDescripcion",
                Location = new Point(controlX, yPos),
                Width = controlWidth,
                Font = new Font("Segoe UI", 11F),
                ReadOnly = true,
                CharacterCasing = CharacterCasing.Upper,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            yPos += 45;

            // Base
            Label lblBaseLabel = CrearLabel("Base:", 20, yPos);
            txtBase = new TextBox
            {
                Name = "txtBase",
                Location = new Point(controlX, yPos),
                Width = controlWidth,
                Font = new Font("Segoe UI", 11F),
                ReadOnly = true,
                MaxLength = 50,
                CharacterCasing = CharacterCasing.Upper,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            yPos += 45;

            // Unidad
            Label lblUnidadLabel = CrearLabel("Unidad:", 20, yPos);
            cmbUnidad = new ComboBox
            {
                Name = "cmbUnidad",
                Location = new Point(controlX, yPos),
                Width = controlWidth,
                Font = new Font("Segoe UI", 11F),
                Enabled = false,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            cmbUnidad.Items.AddRange(new object[] { "Litro", "Galón" });
            yPos += 45;

            // Cantidad
            Label lblCantidadLabel = CrearLabel("Cantidad:", 20, yPos);
            numCantidad = new NumericUpDown
            {
                Name = "numCantidad",
                Location = new Point(controlX, yPos),
                Width = 200,
                Font = new Font("Segoe UI", 11F),
                DecimalPlaces = 2,
                Minimum = 0.01m,
                Maximum = 9999.99m,
                ReadOnly = true
            };
            yPos += 45;

            // Precio
            Label lblPrecioLabel = CrearLabel("Precio:", 20, yPos);
            numPrecio = new NumericUpDown
            {
                Name = "numPrecio",
                Location = new Point(controlX, yPos),
                Width = 200,
                Font = new Font("Segoe UI", 11F),
                DecimalPlaces = 2,
                Minimum = 0.01m,
                Maximum = 99999.99m,
                ReadOnly = true
            };
            yPos += 45;

            // Total (calculado)
            Label lblTotalLabel = CrearLabel("Total:", 20, yPos);
            lblTotalValor = new Label
            {
                Name = "lblTotalValor",
                Location = new Point(controlX, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 128, 0)
            };
            yPos += 50;

            // === SECCIÓN DE FÓRMULA ===
            Label lblFormulaLabel = new Label
            {
                Text = "Fórmula:",
                Location = new Point(20, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };
            yPos += 35;

            // TableLayoutPanel para las fórmulas (2 columnas, 3 filas)
            TableLayoutPanel panelFormulas = new TableLayoutPanel
            {
                Name = "panelFormulas",
                Location = new Point(20, yPos),
                Width = infoPanel.MinimumSize.Width - 60,
                Height = 120,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ColumnCount = 2,
                RowCount = 3,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                Padding = new Padding(10, 5, 10, 5),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            // Configurar columnas (50% cada una)
            panelFormulas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panelFormulas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            // Configurar filas (automáticas)
            for (int i = 0; i < 3; i++)
            {
                panelFormulas.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            }

            // Crear 6 filas para la fórmula (3 por columna)
            for (int i = 0; i < 6; i++)
            {
                int col = i / 3;  // 0 para i=0,1,2 y 1 para i=3,4,5
                int row = i % 3;

                // Panel con TableLayoutPanel para centrar horizontalmente
                Panel filaPanel = new Panel
                {
                    Name = $"filaPanel{i}",
                    Dock = DockStyle.Fill,
                    Margin = new Padding(2),
                    BackColor = Color.White
                };

                // FlowLayoutPanel para centrar el contenido
                FlowLayoutPanel flowPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.LeftToRight,
                    AutoSize = true,
                    WrapContents = false,
                    Anchor = AnchorStyles.None  // Esto centra el FlowLayoutPanel
                };

                // TextBox para el tipo (B, O, T, KX, etc.) - CAMBIADO DE ComboBox
                TextBox txtTipo = new TextBox
                {
                    Name = $"txtTipo{i}",
                    Width = 50,
                    Font = new Font("Segoe UI", 9.5F),
                    ReadOnly = true,
                    TextAlign = HorizontalAlignment.Center,
                    CharacterCasing = CharacterCasing.Upper,
                    Margin = new Padding(0)
                };
                txtTipos[i] = txtTipo;

                Label lblIgual = new Label
                {
                    Text = "=",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(5, 0, 5, 0)
                };

                // Primer número
                TextBox txtValor1 = new TextBox
                {
                    Name = $"txtValor1_{i}",
                    Width = 50,
                    Font = new Font("Segoe UI", 9.5F),
                    ReadOnly = true,
                    TextAlign = HorizontalAlignment.Center,
                    Margin = new Padding(0, 0, 5, 0)
                };
                txtValores1[i] = txtValor1;

                // Segundo número (opcional)
                TextBox txtValor2 = new TextBox
                {
                    Name = $"txtValor2_{i}",
                    Width = 50,
                    Font = new Font("Segoe UI", 9.5F),
                    ReadOnly = true,
                    TextAlign = HorizontalAlignment.Center,
                    Margin = new Padding(0)
                };
                txtValores2[i] = txtValor2;

                flowPanel.Controls.AddRange(new Control[] { txtTipo, lblIgual, txtValor1, txtValor2 });

                // Centrar el FlowLayoutPanel en el filaPanel
                flowPanel.Location = new Point(
                    (filaPanel.Width - flowPanel.Width) / 2,
                    (filaPanel.Height - flowPanel.Height) / 2
                );

                filaPanel.Controls.Add(flowPanel);

                // Ajustar posición cuando el panel cambie de tamaño
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

                panelFormulas.Controls.Add(filaPanel, col, row);
            }

            // Agregar controles al panel de información
            infoPanel.Controls.AddRange(new Control[]
            {
                lblFechaLabel, dtpFecha,
                lblClaveLabel, txtClave,
                lblDescLabel, txtDescripcion,
                lblBaseLabel, txtBase,
                lblUnidadLabel, cmbUnidad,
                lblCantidadLabel, numCantidad,
                lblPrecioLabel, numPrecio,
                lblTotalLabel, lblTotalValor,
                lblFormulaLabel, panelFormulas
            });

            scrollablePanel.Controls.Add(infoPanel);

            // Panel de botones con FlowLayoutPanel para diseño responsivo
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Name = "buttonPanel",
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoSize = true,
                Padding = new Padding(0, 10, 0, 10),
                Margin = new Padding(0)
            };

            Button btnEditar = new Button
            {
                Name = "btnEditar",
                Text = "✏️ Editar",
                Size = new Size(130, 45),
                BackColor = Color.FromArgb(74, 143, 208),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = permitirEdicion,
                Margin = new Padding(0, 0, 10, 0)
            };
            btnEditar.FlatAppearance.BorderSize = 0;

            Button btnEliminar = new Button
            {
                Name = "btnEliminar",
                Text = "🗑️ Eliminar",
                Size = new Size(130, 45),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = permitirEdicion,
                Margin = new Padding(0, 0, 10, 0)
            };
            btnEliminar.FlatAppearance.BorderSize = 0;

            Button btnGuardar = new Button
            {
                Name = "btnGuardar",
                Text = "💾 Guardar",
                Size = new Size(130, 45),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = false,
                Margin = new Padding(0, 0, 10, 0)
            };
            btnGuardar.FlatAppearance.BorderSize = 0;

            Button btnCancelarEdicion = new Button
            {
                Name = "btnCancelarEdicion",
                Text = "✗ Cancelar",
                Size = new Size(130, 45),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = false,
                Margin = new Padding(0, 0, 10, 0)
            };
            btnCancelarEdicion.FlatAppearance.BorderSize = 0;

            Button btnCerrar = new Button
            {
                Name = "btnCerrar",
                Text = "Cerrar",
                Size = new Size(130, 45),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 0, 0)
            };
            btnCerrar.FlatAppearance.BorderSize = 0;

            // Eventos
            EventHandler calcularTotal = (s, e) =>
            {
                decimal total = numCantidad.Value * numPrecio.Value;
                lblTotalValor.Text = $"${total:N2}";
            };
            numCantidad.ValueChanged += calcularTotal;
            numPrecio.ValueChanged += calcularTotal;

            btnEditar.Click += (s, e) => ActivarModoEdicion(
                dtpFecha, txtClave, txtDescripcion, txtBase, cmbUnidad,
                numCantidad, numPrecio, panelFormulas,
                btnEditar, btnEliminar, btnGuardar, btnCancelarEdicion);

            btnCancelarEdicion.Click += (s, e) => DesactivarModoEdicion(
                dtpFecha, txtClave, txtDescripcion, txtBase, cmbUnidad,
                numCantidad, numPrecio, panelFormulas,
                btnEditar, btnEliminar, btnGuardar, btnCancelarEdicion);

            btnGuardar.Click += (s, e) => GuardarCambios(
                dtpFecha, txtClave, txtDescripcion, txtBase, cmbUnidad,
                numCantidad, numPrecio, panelFormulas);

            btnEliminar.Click += (s, e) => EliminarMovimiento();

            btnCerrar.Click += (s, e) => this.Close();

            // Agregar botones al panel
            buttonPanel.Controls.AddRange(new Control[]
            {
                btnEditar, btnEliminar, btnGuardar, btnCancelarEdicion, btnCerrar
            });

            // Agregar todo al layout principal
            mainLayout.Controls.Add(lblTitulo, 0, 0);
            mainLayout.Controls.Add(scrollablePanel, 0, 1);
            mainLayout.Controls.Add(buttonPanel, 0, 2);

            this.Controls.Add(mainLayout);
        }

        private Label CrearLabel(string texto, int xPos, int yPos)
        {
            return new Label
            {
                Text = texto,
                Location = new Point(xPos, yPos + 3),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
        }

        private void CargarDatos()
        {
            dtpFecha.Value = movimiento.Fecha;
            txtClave.Text = movimiento.ClaveColor;
            txtDescripcion.Text = movimiento.Descripcion;
            txtBase.Text = movimiento.Base;
            cmbUnidad.SelectedItem = movimiento.Unidad;
            numCantidad.Value = movimiento.Cantidad;
            numPrecio.Value = movimiento.Precio;
            lblTotalValor.Text = $"${movimiento.Total:N2}";

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

        private string ObtenerFormula(TableLayoutPanel panelFormulas)
        {
            var formulas = new System.Collections.Generic.List<string>();

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

        private void ActivarModoEdicion(DateTimePicker dtpFecha, TextBox txtClave,
            TextBox txtDescripcion, TextBox txtBase, ComboBox cmbUnidad,
            NumericUpDown numCantidad, NumericUpDown numPrecio, TableLayoutPanel panelFormulas,
            Button btnEditar, Button btnEliminar, Button btnGuardar, Button btnCancelarEdicion)
        {
            modoEdicion = true;

            dtpFecha.Enabled = true;
            txtClave.ReadOnly = false;
            txtDescripcion.ReadOnly = false;
            txtBase.ReadOnly = false;
            cmbUnidad.Enabled = true;
            numCantidad.ReadOnly = false;
            numPrecio.ReadOnly = false;

            // Habilitar campos de fórmula
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

        private void DesactivarModoEdicion(DateTimePicker dtpFecha, TextBox txtClave,
            TextBox txtDescripcion, TextBox txtBase, ComboBox cmbUnidad,
            NumericUpDown numCantidad, NumericUpDown numPrecio, TableLayoutPanel panelFormulas,
            Button btnEditar, Button btnEliminar, Button btnGuardar, Button btnCancelarEdicion)
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

            // Deshabilitar campos de fórmula
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

        private void GuardarCambios(DateTimePicker dtpFecha, TextBox txtClave,
            TextBox txtDescripcion, TextBox txtBase, ComboBox cmbUnidad,
            NumericUpDown numCantidad, NumericUpDown numPrecio, TableLayoutPanel panelFormulas)
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
                Formula = ObtenerFormula(panelFormulas)
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