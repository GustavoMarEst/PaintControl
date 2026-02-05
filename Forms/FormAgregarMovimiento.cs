using PaintControl.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PaintControl.Forms
{
    public partial class FormAgregarMovimiento : Form
    {
        public Movimiento MovimientoCreado { get; private set; }
        private Cliente cliente;
        private int siguienteNumero;

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
                Text = "Nueva Compra de Pintura",
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
            int controlWidth = 0;

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

            // Número de Movimiento
            Label lblNumMovLabel = CrearLabel("Nº Movimiento:", 20, yPos);
            TextBox txtNumMov = new TextBox
            {
                Name = "txtNumMov",
                Location = new Point(controlX, yPos),
                Width = controlWidth,
                Font = new Font("Segoe UI", 11F),
                Text = siguienteNumero.ToString(),
                ReadOnly = true,
                BackColor = Color.FromArgb(230, 230, 230),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            yPos += 45;

            // Fecha
            Label lblFechaLabel = CrearLabel("Fecha:", 20, yPos);
            DateTimePicker dtpFecha = new DateTimePicker
            {
                Name = "dtpFecha",
                Location = new Point(controlX, yPos - 2),
                Width = controlWidth,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 11F),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            yPos += 45;

            // Clave del Color
            Label lblClaveLabel = CrearLabel("Clave del Color:", 20, yPos);
            TextBox txtClave = new TextBox
            {
                Name = "txtClave",
                Location = new Point(controlX, yPos),
                Width = controlWidth,
                Font = new Font("Segoe UI", 11F),
                MaxLength = 20,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            yPos += 45;

            // Descripción
            Label lblDescLabel = CrearLabel("Descripción:", 20, yPos);
            TextBox txtDescripcion = new TextBox
            {
                Name = "txtDescripcion",
                Location = new Point(controlX, yPos),
                Width = controlWidth,
                Font = new Font("Segoe UI", 11F),
                MaxLength = 100,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            yPos += 45;

            // Base
            Label lblBaseLabel = CrearLabel("Base:", 20, yPos);
            ComboBox cmbBase = new ComboBox
            {
                Name = "cmbBase",
                Location = new Point(controlX, yPos),
                Width = controlWidth,
                Font = new Font("Segoe UI", 11F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            cmbBase.Items.AddRange(new object[] { "Base A", "Base B", "Base C", "Base D", "Base E" });
            cmbBase.SelectedIndex = 0;
            yPos += 45;

            // Unidad
            Label lblUnidadLabel = CrearLabel("Unidad:", 20, yPos);
            ComboBox cmbUnidad = new ComboBox
            {
                Name = "cmbUnidad",
                Location = new Point(controlX, yPos),
                Width = controlWidth,
                Font = new Font("Segoe UI", 11F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            cmbUnidad.Items.AddRange(new object[] { "Litro", "Galón" });
            cmbUnidad.SelectedIndex = 0;
            yPos += 45;

            // Cantidad
            Label lblCantidadLabel = CrearLabel("Cantidad:", 20, yPos);
            NumericUpDown numCantidad = new NumericUpDown
            {
                Name = "numCantidad",
                Location = new Point(controlX, yPos),
                Width = 200,
                Font = new Font("Segoe UI", 11F),
                DecimalPlaces = 2,
                Minimum = 0.01m,
                Maximum = 9999.99m,
                Value = 1
            };
            yPos += 45;

            // Precio
            Label lblPrecioLabel = CrearLabel("Precio:", 20, yPos);
            NumericUpDown numPrecio = new NumericUpDown
            {
                Name = "numPrecio",
                Location = new Point(controlX, yPos),
                Width = 200,
                Font = new Font("Segoe UI", 11F),
                DecimalPlaces = 2,
                Minimum = 0.01m,
                Maximum = 99999.99m,
                Value = 100
            };
            yPos += 45;

            // Total (calculado)
            Label lblTotalLabel = CrearLabel("Total:", 20, yPos);
            Label lblTotalValor = new Label
            {
                Name = "lblTotalValor",
                Location = new Point(controlX, yPos),
                AutoSize = true,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 128, 0),
                Text = "$100.00"
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

                // Panel con FlowLayoutPanel para centrar horizontalmente
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

                // TextBox para el tipo (B, O, T, KX, etc.)
                TextBox txtTipo = new TextBox
                {
                    Name = $"txtTipo{i}",
                    Width = 50,
                    Font = new Font("Segoe UI", 9.5F),
                    TextAlign = HorizontalAlignment.Center,
                    Margin = new Padding(0)
                };

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
                    TextAlign = HorizontalAlignment.Center,
                    Margin = new Padding(0, 0, 5, 0)
                };

                // Segundo número (opcional)
                TextBox txtValor2 = new TextBox
                {
                    Name = $"txtValor2_{i}",
                    Width = 50,
                    Font = new Font("Segoe UI", 9.5F),
                    TextAlign = HorizontalAlignment.Center,
                    Margin = new Padding(0)
                };

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
                lblNumMovLabel, txtNumMov,
                lblFechaLabel, dtpFecha,
                lblClaveLabel, txtClave,
                lblDescLabel, txtDescripcion,
                lblBaseLabel, cmbBase,
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

            Button btnGuardar = new Button
            {
                Name = "btnGuardar",
                Text = "💾 Guardar",
                Size = new Size(150, 45),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 10, 0)
            };
            btnGuardar.FlatAppearance.BorderSize = 0;

            Button btnCancelar = new Button
            {
                Name = "btnCancelar",
                Text = "✗ Cancelar",
                Size = new Size(150, 45),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 0, 0)
            };
            btnCancelar.FlatAppearance.BorderSize = 0;

            // Eventos
            EventHandler calcularTotal = (s, e) =>
            {
                decimal total = numCantidad.Value * numPrecio.Value;
                lblTotalValor.Text = $"${total:N2}";
            };
            numCantidad.ValueChanged += calcularTotal;
            numPrecio.ValueChanged += calcularTotal;

            btnGuardar.Click += (s, e) => GuardarMovimiento(
                txtNumMov, dtpFecha, txtClave, txtDescripcion,
                cmbBase, cmbUnidad, numCantidad, numPrecio, panelFormulas);

            btnCancelar.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            // Agregar botones al panel
            buttonPanel.Controls.AddRange(new Control[] { btnGuardar, btnCancelar });

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

        private string ObtenerFormula(TableLayoutPanel panelFormulas)
        {
            var formulas = new System.Collections.Generic.List<string>();

            for (int i = 0; i < 6; i++)
            {
                var txtTipo = this.Controls.Find($"txtTipo{i}", true);
                var txtValor1 = this.Controls.Find($"txtValor1_{i}", true);
                var txtValor2 = this.Controls.Find($"txtValor2_{i}", true);

                if (txtTipo.Length == 0 || txtValor1.Length == 0) continue;

                TextBox txtT = txtTipo[0] as TextBox;
                TextBox txt1 = txtValor1[0] as TextBox;
                TextBox txt2 = txtValor2.Length > 0 ? txtValor2[0] as TextBox : null;

                if (string.IsNullOrWhiteSpace(txtT.Text)) continue;

                string formula = $"{txtT.Text.Trim()} = {txt1.Text.Trim()}";
                if (txt2 != null && !string.IsNullOrWhiteSpace(txt2.Text))
                    formula += $" {txt2.Text.Trim()}";

                formulas.Add(formula);
            }

            return string.Join("|", formulas);
        }

        private void GuardarMovimiento(TextBox txtNumMov, DateTimePicker dtpFecha,
            TextBox txtClave, TextBox txtDescripcion, ComboBox cmbBase, ComboBox cmbUnidad,
            NumericUpDown numCantidad, NumericUpDown numPrecio, TableLayoutPanel panelFormulas)
        {
            if (string.IsNullOrWhiteSpace(txtClave.Text))
            {
                MessageBox.Show("Por favor ingrese la clave del color.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtClave.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("Por favor ingrese la descripción.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescripcion.Focus();
                return;
            }

            MovimientoCreado = new Movimiento
            {
                NumeroMovimiento = int.Parse(txtNumMov.Text),
                ClienteId = cliente.Id,
                Fecha = dtpFecha.Value,
                ClaveColor = txtClave.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim(),
                Base = cmbBase.SelectedItem?.ToString() ?? "Base A",
                Unidad = cmbUnidad.SelectedItem?.ToString() ?? "Litro",
                Cantidad = numCantidad.Value,
                Precio = numPrecio.Value,
                Formula = ObtenerFormula(panelFormulas)
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}