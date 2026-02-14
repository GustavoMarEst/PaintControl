using PaintControl.Data;
using PaintControl.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PaintControl.Forms
{
    public partial class FormTodosMovimientos : Form
    {
        private MovimientoService movimientoService;
        private DataGridView dgvTodosMovimientos;
        private DateTimePicker dtpFechaInicio;
        private DateTimePicker dtpFechaFin;
        private CheckBox chkFiltrarFecha;
        private Label lblTotalMovimientos;

        // Fuentes cacheadas para evitar recrearlas
        private static readonly Font FuenteTitulo = new Font("Segoe UI", 14F, FontStyle.Bold);
        private static readonly Font FuenteNormal = new Font("Segoe UI", 9.5F);
        private static readonly Font FuenteGrid = new Font("Segoe UI", 10F);
        private static readonly Font FuenteGridHeader = new Font("Segoe UI", 11F, FontStyle.Bold);
        private static readonly Font FuenteInfo = new Font("Segoe UI", 10F, FontStyle.Bold);

        // Lista de movimientos cargada (para evitar consultas repetidas al ordenar)
        private List<Movimiento> todosLosMovimientos;

        public FormTodosMovimientos()
        {
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            movimientoService = new MovimientoService();

            this.Text = "Todos los Movimientos";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(232, 241, 248);

            CrearControles();
            CargarMovimientos();
        }

        private void CrearControles()
        {
            // Panel superior para filtros
            Panel panelFiltros = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(191, 227, 248),
                Padding = new Padding(15)
            };

            Label lblTitulo = new Label
            {
                Text = "📋 Todos los Movimientos",
                Font = FuenteTitulo,
                ForeColor = Color.FromArgb(46, 92, 138),
                Location = new Point(15, 10),
                AutoSize = true
            };

            Label lblFechaInicio = new Label
            {
                Text = "Fecha Inicio:",
                Font = FuenteNormal,
                Location = new Point(15, 45),
                AutoSize = true
            };

            dtpFechaInicio = new DateTimePicker
            {
                Font = FuenteNormal,
                Format = DateTimePickerFormat.Short,
                Location = new Point(110, 43),
                Size = new Size(150, 29),
                Enabled = false
            };

            Label lblFechaFin = new Label
            {
                Text = "Fecha Fin:",
                Font = FuenteNormal,
                Location = new Point(280, 45),
                AutoSize = true
            };

            dtpFechaFin = new DateTimePicker
            {
                Font = FuenteNormal,
                Format = DateTimePickerFormat.Short,
                Location = new Point(360, 43),
                Size = new Size(150, 29),
                Enabled = false
            };

            chkFiltrarFecha = new CheckBox
            {
                Text = "Filtrar por fecha",
                Font = FuenteNormal,
                Location = new Point(530, 45),
                AutoSize = true
            };

            chkFiltrarFecha.CheckedChanged += ChkFiltrarFecha_CheckedChanged;

            dtpFechaInicio.ValueChanged += (s, e) =>
            {
                if (chkFiltrarFecha.Checked) CargarMovimientos();
            };

            dtpFechaFin.ValueChanged += (s, e) =>
            {
                if (chkFiltrarFecha.Checked) CargarMovimientos();
            };

            panelFiltros.Controls.Add(lblTitulo);
            panelFiltros.Controls.Add(lblFechaInicio);
            panelFiltros.Controls.Add(dtpFechaInicio);
            panelFiltros.Controls.Add(lblFechaFin);
            panelFiltros.Controls.Add(dtpFechaFin);
            panelFiltros.Controls.Add(chkFiltrarFecha);

            // Panel para el DataGridView
            Panel panelGrid = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(15)
            };

            dgvTodosMovimientos = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                Font = FuenteGrid,
                RowTemplate = { Height = 35 },
                ColumnHeadersHeight = 40,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(47, 164, 231),
                    ForeColor = Color.White,
                    Font = FuenteGridHeader,
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    SelectionBackColor = Color.FromArgb(214, 235, 255),
                    SelectionForeColor = Color.Black,
                    Padding = new Padding(5),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                EnableHeadersVisualStyles = false,
                BorderStyle = BorderStyle.None,
                GridColor = Color.LightGray,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false,
                AllowUserToOrderColumns = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            };

            dgvTodosMovimientos.Columns.Add("NumMov", "No. Movimiento");
            dgvTodosMovimientos.Columns.Add("Fecha", "Fecha");
            dgvTodosMovimientos.Columns.Add("FechaUltCompra", "Últ. Compra");
            dgvTodosMovimientos.Columns.Add("Cliente", "Cliente");
            dgvTodosMovimientos.Columns.Add("Descripcion", "Descripción");
            dgvTodosMovimientos.Columns.Add("Cantidad", "Cantidad");
            dgvTodosMovimientos.Columns.Add("Total", "Total");

            // ✅ EVENTO CELLPAINTING CON FLECHAS DE ORDENAMIENTO
            dgvTodosMovimientos.CellPainting += (s, e) =>
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
                            FuenteGridHeader,
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

            dgvTodosMovimientos.Columns["NumMov"].FillWeight = 60;
            dgvTodosMovimientos.Columns["Fecha"].FillWeight = 70;
            dgvTodosMovimientos.Columns["FechaUltCompra"].FillWeight = 70;
            dgvTodosMovimientos.Columns["Cliente"].FillWeight = 150;
            dgvTodosMovimientos.Columns["Descripcion"].FillWeight = 150;
            dgvTodosMovimientos.Columns["Cantidad"].FillWeight = 50;
            dgvTodosMovimientos.Columns["Total"].FillWeight = 70;

            dgvTodosMovimientos.Columns["Cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvTodosMovimientos.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvTodosMovimientos.Columns["Total"].DefaultCellStyle.Format = "C2";

            // ✅ HABILITAR ordenamiento por clic en columnas
            foreach (DataGridViewColumn col in dgvTodosMovimientos.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
            }

            // ✅ EVENTO DE ORDENAMIENTO PERSONALIZADO
            dgvTodosMovimientos.SortCompare += (s, e) =>
            {
                if (e.Column.Name == "Fecha")
                {
                    // Obtener los objetos Movimiento de las filas
                    Movimiento mov1 = dgvTodosMovimientos.Rows[e.RowIndex1].Tag as Movimiento;
                    Movimiento mov2 = dgvTodosMovimientos.Rows[e.RowIndex2].Tag as Movimiento;

                    // Comparar por fecha real, no por el texto
                    e.SortResult = DateTime.Compare(mov1.Fecha, mov2.Fecha);
                    e.Handled = true;
                }
                else if (e.Column.Name == "FechaUltCompra")
                {
                    Movimiento mov1 = dgvTodosMovimientos.Rows[e.RowIndex1].Tag as Movimiento;
                    Movimiento mov2 = dgvTodosMovimientos.Rows[e.RowIndex2].Tag as Movimiento;

                    DateTime fecha1 = mov1.FechaUltimaCompra ?? DateTime.MinValue;
                    DateTime fecha2 = mov2.FechaUltimaCompra ?? DateTime.MinValue;
                    e.SortResult = DateTime.Compare(fecha1, fecha2);
                    e.Handled = true;
                }
                else if (e.Column.Name == "NumMov")
                {
                    // Ordenar por número de movimiento (entero)
                    Movimiento mov1 = dgvTodosMovimientos.Rows[e.RowIndex1].Tag as Movimiento;
                    Movimiento mov2 = dgvTodosMovimientos.Rows[e.RowIndex2].Tag as Movimiento;

                    e.SortResult = mov1.NumeroMovimiento.CompareTo(mov2.NumeroMovimiento);
                    e.Handled = true;
                }
                else if (e.Column.Name == "Cantidad")
                {
                    // Ordenar por cantidad (decimal)
                    Movimiento mov1 = dgvTodosMovimientos.Rows[e.RowIndex1].Tag as Movimiento;
                    Movimiento mov2 = dgvTodosMovimientos.Rows[e.RowIndex2].Tag as Movimiento;

                    e.SortResult = mov1.Cantidad.CompareTo(mov2.Cantidad);
                    e.Handled = true;
                }
                else if (e.Column.Name == "Total")
                {
                    // Ordenar por total (decimal)
                    Movimiento mov1 = dgvTodosMovimientos.Rows[e.RowIndex1].Tag as Movimiento;
                    Movimiento mov2 = dgvTodosMovimientos.Rows[e.RowIndex2].Tag as Movimiento;

                    e.SortResult = mov1.Total.CompareTo(mov2.Total);
                    e.Handled = true;
                }
            };

            // Evento de doble clic para ver detalles
            dgvTodosMovimientos.CellDoubleClick += DgvTodosMovimientos_CellDoubleClick;

            panelGrid.Controls.Add(dgvTodosMovimientos);

            // Panel inferior para información
            Panel panelInferior = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(232, 241, 248),
                Padding = new Padding(15, 10, 15, 10)
            };

            lblTotalMovimientos = new Label
            {
                Font = FuenteInfo,
                ForeColor = Color.FromArgb(46, 92, 138),
                Location = new Point(15, 15),
                AutoSize = true
            };

            panelInferior.Controls.Add(lblTotalMovimientos);

            this.Controls.Add(panelGrid);
            this.Controls.Add(panelFiltros);
            this.Controls.Add(panelInferior);
        }

        private void ChkFiltrarFecha_CheckedChanged(object sender, EventArgs e)
        {
            dtpFechaInicio.Enabled = chkFiltrarFecha.Checked;
            dtpFechaFin.Enabled = chkFiltrarFecha.Checked;

            if (chkFiltrarFecha.Checked)
            {
                dtpFechaInicio.Value = DateTime.Now.AddMonths(-1).Date;
                dtpFechaFin.Value = DateTime.Now.Date;
            }

            CargarMovimientos();
        }

        private void CargarMovimientos()
        {
            dgvTodosMovimientos.SuspendLayout();
            dgvTodosMovimientos.Rows.Clear();

            List<Movimiento> movimientos;

            if (chkFiltrarFecha.Checked)
            {
                DateTime fechaInicio = dtpFechaInicio.Value.Date;
                DateTime fechaFin = dtpFechaFin.Value.Date.AddDays(1).AddSeconds(-1);
                movimientos = movimientoService.ObtenerTodosPorFechas(fechaInicio, fechaFin);
            }
            else
            {
                // ObtenerTodos() ya incluye .Include(m => m.Cliente)
                movimientos = movimientoService.ObtenerTodos();
            }

            // Guardar en variable de clase para evitar consultas repetidas
            todosLosMovimientos = movimientos;

            // Ordenamiento inicial: más recientes primero
            movimientos = movimientos
                .OrderByDescending(m => m.Fecha)
                .ThenByDescending(m => m.NumeroMovimiento)
                .ToList();

            foreach (var mov in movimientos)
            {
                // Usar la relación de navegación directamente (viene con Include)
                string nombreCliente = mov.Cliente?.Nombre ?? "Cliente desconocido";

                dgvTodosMovimientos.Rows.Add(
                    mov.NumeroMovimiento,
                    mov.Fecha.ToShortDateString(),
                    mov.FechaUltimaCompra.HasValue ? mov.FechaUltimaCompra.Value.ToShortDateString() : "—",
                    nombreCliente,
                    mov.Descripcion,
                    mov.Cantidad,
                    mov.Total
                );

                // ✅ IMPORTANTE: Guardar el objeto completo en Tag para ordenamiento
                dgvTodosMovimientos.Rows[dgvTodosMovimientos.Rows.Count - 1].Tag = mov;
            }

            dgvTodosMovimientos.ResumeLayout();

            // Actualizar información de totales
            if (movimientos.Count == 0)
            {
                lblTotalMovimientos.Text = chkFiltrarFecha.Checked
                    ? "Sin movimientos en el rango de fechas seleccionado"
                    : "Sin movimientos registrados";
            }
            else
            {
                decimal totalGeneral = movimientos.Sum(m => m.Total);
                lblTotalMovimientos.Text = $"Total de movimientos: {movimientos.Count} | Monto total: {totalGeneral:C2}";
            }
        }

        private void DgvTodosMovimientos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Movimiento movimiento = dgvTodosMovimientos.Rows[e.RowIndex].Tag as Movimiento;
            if (movimiento == null) return;

            FormDetalleMovimiento formDetalle = new FormDetalleMovimiento(movimiento);

            if (formDetalle.ShowDialog(this) == DialogResult.OK)
            {
                if (formDetalle.Eliminado)
                {
                    if (movimientoService.EliminarMovimiento(movimiento.Id))
                    {
                        MessageBox.Show("El movimiento se eliminó correctamente.",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarMovimientos();
                    }
                }
                else if (formDetalle.Modificado)
                {
                    if (movimientoService.ActualizarMovimiento(formDetalle.MovimientoActualizado))
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