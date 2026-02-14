using PaintControl.Data;
using PaintControl.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace PaintControl.Forms
{
    public partial class FormCatalogo : Form
    {
        private CatalogoService catalogoService;

        private Panel panelTipos;
        private Panel panelLineas;
        private Panel panelAcabados;

        private ListBox lstTipos;
        private ListBox lstLineas;
        private ListBox lstAcabados;

        private Label lblTiposInfo;
        private Label lblLineasInfo;
        private Label lblAcabadosInfo;

        private TipoPintura tipoSeleccionado;
        private LineaPintura lineaSeleccionada;

        private static readonly Color AzulPrimario = Color.FromArgb(47, 164, 231);
        private static readonly Color AzulOscuro = Color.FromArgb(46, 92, 138);
        private static readonly Color FondoPrincipal = Color.FromArgb(244, 247, 251);
        private static readonly Color FondoPanel = Color.White;
        private static readonly Color BordePanel = Color.FromArgb(226, 232, 240);
        private static readonly Color TextoSecundario = Color.FromArgb(139, 149, 166);
        private static readonly Color VerdePrimario = Color.FromArgb(76, 175, 80);
        private static readonly Color RojoEliminar = Color.FromArgb(220, 53, 69);
        private static readonly Color AmarilloEditar = Color.FromArgb(255, 152, 0);

        private static readonly Font FuenteTituloPrincipal = new Font("Segoe UI", 14F, FontStyle.Bold);
        private static readonly Font FuenteTituloSeccion = new Font("Segoe UI", 11F, FontStyle.Bold);
        private static readonly Font FuenteInfo = new Font("Segoe UI", 9F);
        private static readonly Font FuenteBoton = new Font("Segoe UI", 9F, FontStyle.Bold);
        private static readonly Font FuenteLista = new Font("Segoe UI", 10F);

        public FormCatalogo()
        {
            InitializeComponent();
            catalogoService = new CatalogoService();
            ConfigurarFormulario();
            CargarTipos();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Cat\u00e1logo de Pinturas";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = FondoPrincipal;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            // ===== HEADER =====
            Panel headerPanel = new Panel { Height = 55, Dock = DockStyle.Top, BackColor = AzulPrimario };
            Label lblTitulo = new Label
            {
                Text = "\U0001f3a8  Cat\u00e1logo de Pinturas",
                Font = FuenteTituloPrincipal,
                ForeColor = Color.White,
                Location = new Point(20, 12),
                AutoSize = true
            };
            Label lblSub = new Label
            {
                Text = "Tipo \u2192 L\u00ednea \u2192 Acabado",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(200, 255, 255, 255),
                AutoSize = true
            };
            headerPanel.Controls.Add(lblTitulo);
            headerPanel.Controls.Add(lblSub);
            headerPanel.Layout += (s, e) =>
            {
                lblSub.Location = new Point(headerPanel.Width - lblSub.Width - 20,
                    (headerPanel.Height - lblSub.Height) / 2);
            };

            // ===== BODY =====
            Panel body = new Panel { Dock = DockStyle.Fill, BackColor = FondoPrincipal };

            int colWidth = 290;
            int colHeight = 460;
            int gap = 20;
            int totalWidth = colWidth * 3 + gap * 2;

            panelTipos = CrearColumna("TIPO DE PINTURA", 0, 0, colWidth, colHeight, out lstTipos, out lblTiposInfo);
            CrearBotonesColumna(panelTipos, colWidth,
                (s, e) => AgregarTipo(),
                (s, e) => EditarTipo(),
                (s, e) => EliminarTipo());
            lstTipos.SelectedIndexChanged += LstTipos_SelectedIndexChanged;

            panelLineas = CrearColumna("L\u00cdNEA", 0, 0, colWidth, colHeight, out lstLineas, out lblLineasInfo);
            CrearBotonesColumna(panelLineas, colWidth,
                (s, e) => AgregarLinea(),
                (s, e) => EditarLinea(),
                (s, e) => EliminarLinea());
            lstLineas.SelectedIndexChanged += LstLineas_SelectedIndexChanged;

            panelAcabados = CrearColumna("ACABADO", 0, 0, colWidth, colHeight, out lstAcabados, out lblAcabadosInfo);
            CrearBotonesColumna(panelAcabados, colWidth,
                (s, e) => AgregarAcabado(),
                (s, e) => EditarAcabado(),
                (s, e) => EliminarAcabado());

            body.Controls.Add(panelTipos);
            body.Controls.Add(panelLineas);
            body.Controls.Add(panelAcabados);

            // Centrar columnas dinámicamente
            body.Resize += (s, e) =>
            {
                int startX = Math.Max(15, (body.ClientSize.Width - totalWidth) / 2);
                int topY = 15;
                panelTipos.Location = new Point(startX, topY);
                panelLineas.Location = new Point(startX + colWidth + gap, topY);
                panelAcabados.Location = new Point(startX + (colWidth + gap) * 2, topY);
            };

            // Agregar body primero (Fill), luego header (Top) para que body no se superponga
            this.Controls.Add(body);
            this.Controls.Add(headerPanel);

            ActualizarEstadoLineas(false);
            ActualizarEstadoAcabados(false);
        }

        // ========== CREACIÓN DE COLUMNA ==========

        private Panel CrearColumna(string titulo, int x, int y, int width, int height,
            out ListBox listBox, out Label lblInfo)
        {
            Panel panel = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = FondoPanel
            };
            panel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(BordePanel, 1))
                using (GraphicsPath path = CrearRR(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 10))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawPath(pen, path);
                }
            };

            // Encabezado de columna con esquinas superiores redondeadas
            Panel tituloPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(width, 38),
                BackColor = Color.Transparent
            };
            tituloPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = new GraphicsPath())
                {
                    int r = 10;
                    path.AddArc(0, 0, r, r, 180, 90);
                    path.AddArc(width - r - 1, 0, r, r, 270, 90);
                    path.AddLine(width - 1, r, width - 1, 38);
                    path.AddLine(width - 1, 38, 0, 38);
                    path.CloseFigure();
                    using (Brush b = new SolidBrush(AzulPrimario))
                        e.Graphics.FillPath(b, path);
                }
            };
            tituloPanel.Controls.Add(new Label
            {
                Text = titulo,
                Font = FuenteTituloSeccion,
                ForeColor = Color.White,
                Location = new Point(12, 8),
                AutoSize = true,
                BackColor = Color.Transparent
            });

            lblInfo = new Label
            {
                Font = FuenteInfo,
                ForeColor = TextoSecundario,
                Location = new Point(12, 44),
                Size = new Size(width - 24, 16),
                Text = ""
            };

            listBox = new ListBox
            {
                Location = new Point(12, 64),
                Size = new Size(width - 24, height - 120),
                Font = FuenteLista,
                BorderStyle = BorderStyle.FixedSingle,
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = 30
            };

            listBox.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;
                e.DrawBackground();
                bool sel = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                using (Brush bg = new SolidBrush(sel ? Color.FromArgb(214, 235, 255) : Color.White))
                    e.Graphics.FillRectangle(bg, e.Bounds);
                using (Brush txt = new SolidBrush(sel ? AzulOscuro : Color.Black))
                    e.Graphics.DrawString(((ListBox)s).Items[e.Index].ToString(), FuenteLista, txt,
                        new RectangleF(e.Bounds.X + 8, e.Bounds.Y + 5, e.Bounds.Width - 16, e.Bounds.Height));
                using (Pen p = new Pen(Color.FromArgb(240, 240, 240)))
                    e.Graphics.DrawLine(p, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
            };

            panel.Controls.Add(tituloPanel);
            panel.Controls.Add(lblInfo);
            panel.Controls.Add(listBox);

            return panel;
        }

        private void CrearBotonesColumna(Panel panel, int colWidth,
            EventHandler onAgregar, EventHandler onEditar, EventHandler onEliminar)
        {
            int btnY = panel.Height - 48;
            int btnW = 82;
            int gapBtn = 6;
            // Centrar los 3 botones dentro de la columna
            int totalBtnW = btnW * 3 + gapBtn * 2;
            int startX = Math.Max(6, (colWidth - totalBtnW) / 2);

            Button bA = new Button
            {
                Text = "+ Agregar",
                Font = FuenteBoton,
                Size = new Size(btnW, 33),
                Location = new Point(startX, btnY),
                BackColor = VerdePrimario,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            bA.FlatAppearance.BorderSize = 0;
            bA.Click += onAgregar;

            Button bE = new Button
            {
                Text = "\u270f Editar",
                Font = FuenteBoton,
                Size = new Size(btnW, 33),
                Location = new Point(startX + btnW + gapBtn, btnY),
                BackColor = AmarilloEditar,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            bE.FlatAppearance.BorderSize = 0;
            bE.Click += onEditar;

            Button bD = new Button
            {
                Text = "\U0001f5d1 Eliminar",
                Font = FuenteBoton,
                Size = new Size(btnW, 33),
                Location = new Point(startX + (btnW + gapBtn) * 2, btnY),
                BackColor = RojoEliminar,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            bD.FlatAppearance.BorderSize = 0;
            bD.Click += onEliminar;

            panel.Controls.AddRange(new Control[] { bA, bE, bD });
        }

        // ========== CARGA DE DATOS ==========

        private void CargarTipos()
        {
            lstTipos.Items.Clear();
            var tipos = catalogoService.ObtenerTodosTipos();
            foreach (var t in tipos)
                lstTipos.Items.Add(t);
            lblTiposInfo.Text = $"{tipos.Count(t => t.Activo)} tipo(s) activo(s)";

            tipoSeleccionado = null;
            lineaSeleccionada = null;
            lstLineas.Items.Clear();
            lstAcabados.Items.Clear();
            ActualizarEstadoLineas(false);
            ActualizarEstadoAcabados(false);
        }

        private void CargarLineas(int tipoId)
        {
            lstLineas.Items.Clear();
            var lineas = catalogoService.ObtenerTodasLineasPorTipo(tipoId);
            foreach (var l in lineas)
                lstLineas.Items.Add(l);
            lblLineasInfo.Text = $"{lineas.Count(l => l.Activo)} l\u00ednea(s) en {tipoSeleccionado?.Nombre ?? ""}";

            lineaSeleccionada = null;
            lstAcabados.Items.Clear();
            ActualizarEstadoAcabados(false);
        }

        private void CargarAcabados(int lineaId)
        {
            lstAcabados.Items.Clear();
            var acabados = catalogoService.ObtenerTodosAcabadosPorLinea(lineaId);
            foreach (var a in acabados)
                lstAcabados.Items.Add(a);
            lblAcabadosInfo.Text = $"{acabados.Count(a => a.Activo)} acabado(s) en {lineaSeleccionada?.Nombre ?? ""}";
        }

        // ========== EVENTOS DE SELECCIÓN ==========

        private void LstTipos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTipos.SelectedItem is TipoPintura t)
            {
                tipoSeleccionado = t;
                ActualizarEstadoLineas(true);
                CargarLineas(t.Id);
            }
        }

        private void LstLineas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstLineas.SelectedItem is LineaPintura l)
            {
                lineaSeleccionada = l;
                ActualizarEstadoAcabados(true);
                CargarAcabados(l.Id);
            }
        }

        private void ActualizarEstadoLineas(bool habilitado)
        {
            lstLineas.Enabled = habilitado;
            if (!habilitado)
            {
                lblLineasInfo.Text = "Seleccione un tipo primero";
                lstLineas.Items.Clear();
            }
        }

        private void ActualizarEstadoAcabados(bool habilitado)
        {
            lstAcabados.Enabled = habilitado;
            if (!habilitado)
            {
                lblAcabadosInfo.Text = "Seleccione una l\u00ednea primero";
                lstAcabados.Items.Clear();
            }
        }

        // ========== CRUD TIPOS ==========

        private void AgregarTipo()
        {
            using (var f = CrearFormEdicion("Agregar Tipo de Pintura", "", "", true))
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    string[] d = (f.Tag as string).Split('|');
                    if (catalogoService.AgregarTipo(new TipoPintura { Nombre = d[0], Abreviatura = d[1], Activo = true }))
                        CargarTipos();
                }
            }
        }

        private void EditarTipo()
        {
            if (!(lstTipos.SelectedItem is TipoPintura t))
            {
                MessageBox.Show("Seleccione un tipo.", "Atenci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var f = CrearFormEdicion("Editar Tipo", t.Nombre, t.Abreviatura, true))
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    string[] d = (f.Tag as string).Split('|');
                    t.Nombre = d[0];
                    t.Abreviatura = d[1];
                    if (catalogoService.ActualizarTipo(t))
                        CargarTipos();
                }
            }
        }

        private void EliminarTipo()
        {
            if (!(lstTipos.SelectedItem is TipoPintura t))
            {
                MessageBox.Show("Seleccione un tipo.", "Atenci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"\u00bfEliminar \"{t.Nombre}\" y todo su contenido?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (catalogoService.EliminarTipo(t.Id))
                    CargarTipos();
            }
        }

        // ========== CRUD LÍNEAS ==========

        private void AgregarLinea()
        {
            if (tipoSeleccionado == null)
            {
                MessageBox.Show("Seleccione un tipo primero.", "Atenci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var f = CrearFormEdicion($"Agregar L\u00ednea a \"{tipoSeleccionado.Nombre}\"", "", "", true))
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    string[] d = (f.Tag as string).Split('|');
                    if (catalogoService.AgregarLinea(new LineaPintura
                    {
                        TipoPinturaId = tipoSeleccionado.Id,
                        Nombre = d[0],
                        Abreviatura = d[1],
                        Activo = true
                    }))
                        CargarLineas(tipoSeleccionado.Id);
                }
            }
        }

        private void EditarLinea()
        {
            if (!(lstLineas.SelectedItem is LineaPintura l))
            {
                MessageBox.Show("Seleccione una l\u00ednea.", "Atenci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var f = CrearFormEdicion("Editar L\u00ednea", l.Nombre, l.Abreviatura, true))
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    string[] d = (f.Tag as string).Split('|');
                    l.Nombre = d[0];
                    l.Abreviatura = d[1];
                    if (catalogoService.ActualizarLinea(l))
                        CargarLineas(tipoSeleccionado.Id);
                }
            }
        }

        private void EliminarLinea()
        {
            if (!(lstLineas.SelectedItem is LineaPintura l))
            {
                MessageBox.Show("Seleccione una l\u00ednea.", "Atenci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"\u00bfEliminar \"{l.Nombre}\" y sus acabados?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (catalogoService.EliminarLinea(l.Id))
                    CargarLineas(tipoSeleccionado.Id);
            }
        }

        // ========== CRUD ACABADOS ==========

        private void AgregarAcabado()
        {
            if (lineaSeleccionada == null)
            {
                MessageBox.Show("Seleccione una l\u00ednea primero.", "Atenci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Crear formulario personalizado con checkbox "Universal"
            Form form = new Form
            {
                Text = $"Agregar Acabado a \"{lineaSeleccionada.Nombre}\"",
                Size = new Size(420, 240),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            int px = 18, yy = 15;
            form.Controls.Add(new Label
            {
                Text = "Nombre:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(px, yy),
                AutoSize = true
            });
            TextBox txtN = new TextBox
            {
                Text = "",
                Font = new Font("Segoe UI", 10F),
                Location = new Point(px, yy + 22),
                Width = 365,
                CharacterCasing = CharacterCasing.Upper
            };
            form.Controls.Add(txtN);
            yy += 55;

            // Checkbox Universal
            CheckBox chkUniversal = new CheckBox
            {
                Text = "Universal (agregar a todas las l\u00edneas del tipo)",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(px, yy),
                AutoSize = true,
                ForeColor = AzulOscuro
            };
            form.Controls.Add(chkUniversal);
            yy += 35;

            Button bG = new Button
            {
                Text = "\u2713 Guardar",
                Font = FuenteBoton,
                Size = new Size(100, 35),
                Location = new Point(px + 160, yy),
                BackColor = VerdePrimario,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            bG.FlatAppearance.BorderSize = 0;

            Button bC = new Button
            {
                Text = "Cancelar",
                Font = FuenteBoton,
                Size = new Size(100, 35),
                Location = new Point(px + 268, yy),
                BackColor = GrisBotonSecundario,
                ForeColor = Color.FromArgb(90, 101, 119),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            bC.FlatAppearance.BorderSize = 0;

            bG.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtN.Text))
                {
                    MessageBox.Show("El nombre es obligatorio.", "Validaci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                form.Tag = new object[] { txtN.Text.Trim().ToUpper(), chkUniversal.Checked };
                form.DialogResult = DialogResult.OK;
                form.Close();
            };

            bC.Click += (s, e) =>
            {
                form.DialogResult = DialogResult.Cancel;
                form.Close();
            };

            form.Controls.Add(bG);
            form.Controls.Add(bC);
            form.AcceptButton = bG;
            form.CancelButton = bC;

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                object[] datos = form.Tag as object[];
                string nombreAcabado = (string)datos[0];
                bool esUniversal = (bool)datos[1];

                if (esUniversal && tipoSeleccionado != null)
                {
                    // Obtener todas las líneas del tipo actual
                    var todasLineas = catalogoService.ObtenerTodasLineasPorTipo(tipoSeleccionado.Id);
                    int agregados = 0;
                    int omitidos = 0;

                    foreach (var linea in todasLineas)
                    {
                        // Verificar si ya existe ese acabado en esta línea
                        var acabadosExistentes = catalogoService.ObtenerTodosAcabadosPorLinea(linea.Id);
                        bool yaExiste = acabadosExistentes.Any(a =>
                            a.Nombre.Equals(nombreAcabado, StringComparison.OrdinalIgnoreCase));

                        if (!yaExiste)
                        {
                            catalogoService.AgregarAcabado(new AcabadoPintura
                            {
                                LineaPinturaId = linea.Id,
                                Nombre = nombreAcabado,
                                Activo = true
                            });
                            agregados++;
                        }
                        else
                        {
                            omitidos++;
                        }
                    }

                    string msg = $"Acabado \"{nombreAcabado}\" procesado:\n\n" +
                                 $"\u2713 Agregado a {agregados} l\u00ednea(s)\n" +
                                 $"\u2014 Omitido en {omitidos} l\u00ednea(s) (ya exist\u00eda)";
                    MessageBox.Show(msg, "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarAcabados(lineaSeleccionada.Id);
                }
                else
                {
                    // Agregar solo a la línea seleccionada
                    if (catalogoService.AgregarAcabado(new AcabadoPintura
                    {
                        LineaPinturaId = lineaSeleccionada.Id,
                        Nombre = nombreAcabado,
                        Activo = true
                    }))
                        CargarAcabados(lineaSeleccionada.Id);
                }
            }
            form.Dispose();
        }

        private void EditarAcabado()
        {
            if (!(lstAcabados.SelectedItem is AcabadoPintura a))
            {
                MessageBox.Show("Seleccione un acabado.", "Atenci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var f = CrearFormEdicion("Editar Acabado", a.Nombre, "", false))
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    string[] d = (f.Tag as string).Split('|');
                    a.Nombre = d[0];
                    if (catalogoService.ActualizarAcabado(a))
                        CargarAcabados(lineaSeleccionada.Id);
                }
            }
        }

        private void EliminarAcabado()
        {
            if (!(lstAcabados.SelectedItem is AcabadoPintura a))
            {
                MessageBox.Show("Seleccione un acabado.", "Atenci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"\u00bfEliminar \"{a.Nombre}\"?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (catalogoService.EliminarAcabado(a.Id))
                    CargarAcabados(lineaSeleccionada.Id);
            }
        }

        // ========== FORMULARIO DE EDICIÓN ==========

        private Form CrearFormEdicion(string titulo, string nombreActual, string abrActual, bool mostrarAbr)
        {
            Form form = new Form
            {
                Text = titulo,
                Size = new Size(420, mostrarAbr ? 240 : 190),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            int px = 18, yy = 15;
            form.Controls.Add(new Label
            {
                Text = "Nombre:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(px, yy),
                AutoSize = true
            });
            TextBox txtN = new TextBox
            {
                Text = nombreActual,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(px, yy + 22),
                Width = 365,
                CharacterCasing = CharacterCasing.Upper
            };
            form.Controls.Add(txtN);
            yy += 55;

            TextBox txtA = null;
            if (mostrarAbr)
            {
                form.Controls.Add(new Label
                {
                    Text = "Abreviatura:",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Location = new Point(px, yy),
                    AutoSize = true
                });
                form.Controls.Add(new Label
                {
                    Text = "(autom\u00e1tica, editable)",
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = TextoSecundario,
                    Location = new Point(110, yy + 3),
                    AutoSize = true
                });
                txtA = new TextBox
                {
                    Text = abrActual,
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(px, yy + 22),
                    Width = 140,
                    CharacterCasing = CharacterCasing.Upper,
                    MaxLength = 10
                };
                form.Controls.Add(txtA);

                bool manual = !string.IsNullOrEmpty(abrActual);
                txtN.TextChanged += (s, e) =>
                {
                    if (!manual)
                        txtA.Text = catalogoService.GenerarAbreviatura(txtN.Text);
                };
                txtA.KeyPress += (s, e) =>
                {
                    if (e.KeyChar != (char)Keys.Back)
                        manual = true;
                };
                txtA.TextChanged += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtA.Text))
                        manual = false;
                };
                yy += 55;
            }

            Button bG = new Button
            {
                Text = "\u2713 Guardar",
                Font = FuenteBoton,
                Size = new Size(100, 35),
                Location = new Point(px + 160, yy),
                BackColor = VerdePrimario,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            bG.FlatAppearance.BorderSize = 0;

            Button bC = new Button
            {
                Text = "Cancelar",
                Font = FuenteBoton,
                Size = new Size(100, 35),
                Location = new Point(px + 268, yy),
                BackColor = GrisBotonSecundario,
                ForeColor = Color.FromArgb(90, 101, 119),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            bC.FlatAppearance.BorderSize = 0;

            // Referencia local para captura en lambdas
            TextBox localTxtA = txtA;

            bG.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtN.Text))
                {
                    MessageBox.Show("El nombre es obligatorio.", "Validaci\u00f3n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string nombre = txtN.Text.Trim().ToUpper();
                string abr = mostrarAbr && localTxtA != null ? localTxtA.Text.Trim().ToUpper() : "";
                if (mostrarAbr && string.IsNullOrWhiteSpace(abr))
                    abr = catalogoService.GenerarAbreviatura(nombre);
                form.Tag = $"{nombre}|{abr}";
                form.DialogResult = DialogResult.OK;
                form.Close();
            };

            bC.Click += (s, e) =>
            {
                form.DialogResult = DialogResult.Cancel;
                form.Close();
            };

            form.Controls.Add(bG);
            form.Controls.Add(bC);
            form.AcceptButton = bG;
            form.CancelButton = bC;

            return form;
        }

        // ========== UTILIDADES ==========

        private static readonly Color GrisBotonSecundario = Color.FromArgb(226, 232, 240);

        private GraphicsPath CrearRR(Rectangle rect, int r)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, r, r, 180, 90);
            path.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
            path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
            path.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}