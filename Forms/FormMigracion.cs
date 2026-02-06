using System;
using System.Windows.Forms;
using PaintControl.Migracion;

namespace PaintControl.Forms
{
    public partial class FormMigracion : Form
    {
        public FormMigracion()
        {
            InitializeComponent();
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Seleccione la carpeta donde están los archivos DBF";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtRutaDBF.Text = dialog.SelectedPath;
                }
            }
        }

        private void btnMigrarClientes_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRutaDBF.Text))
            {
                MessageBox.Show("Seleccione primero la carpeta con los archivos DBF.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtLog.AppendText("=== INICIANDO MIGRACIÓN DE CLIENTES ===\n");
            txtLog.AppendText("Ruta: " + txtRutaDBF.Text + "\n");

            try
            {
                // Archivo CTL_CLIE.DBF
                string archivoClientes = System.IO.Path.Combine(txtRutaDBF.Text, "CTL_CLIE.DBF");

                if (!System.IO.File.Exists(archivoClientes))
                {
                    MessageBox.Show("No se encontró el archivo: CTL_CLIE.DBF\n\nRuta buscada:\n" + archivoClientes,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtLog.AppendText("✗ Archivo no encontrado: " + archivoClientes + "\n\n");
                    return;
                }

                txtLog.AppendText("Archivo encontrado, iniciando migración...\n");

                using (var migrador = new MigradorDBF(archivoClientes))
                {
                    var resultado = migrador.MigrarClientes("CTL_CLIE");

                    txtLog.AppendText("✓ Migrados exitosamente: " + resultado.exitos + "\n");
                    txtLog.AppendText("✗ Errores: " + resultado.errores + "\n");

                    if (!string.IsNullOrEmpty(resultado.mensajeError))
                    {
                        txtLog.AppendText("Detalles de errores:\n" + resultado.mensajeError + "\n");
                    }

                    txtLog.AppendText("=== MIGRACIÓN DE CLIENTES COMPLETADA ===\n\n");

                    MessageBox.Show("Migración de clientes completada.\n\nÉxitos: " + resultado.exitos + "\nErrores: " + resultado.errores,
                        "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                txtLog.AppendText("ERROR: " + ex.Message + "\n");
                if (ex.InnerException != null)
                {
                    txtLog.AppendText("Detalles: " + ex.InnerException.Message + "\n\n");
                }

                string mensaje = "Error durante la migración:\n\n" + ex.Message;
                if (ex.InnerException != null)
                {
                    mensaje += "\n\n" + ex.InnerException.Message;
                }
                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMigrarMovimientos_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRutaDBF.Text))
            {
                MessageBox.Show("Seleccione primero la carpeta con los archivos DBF.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtLog.AppendText("=== INICIANDO MIGRACIÓN DE MOVIMIENTOS ===\n");
            txtLog.AppendText("Ruta: " + txtRutaDBF.Text + "\n");

            try
            {
                // Archivo CTL_MOV.DBF
                string archivoMovimientos = System.IO.Path.Combine(txtRutaDBF.Text, "CTL_MOV.DBF");

                if (!System.IO.File.Exists(archivoMovimientos))
                {
                    MessageBox.Show("No se encontró el archivo: CTL_MOV.DBF\n\nRuta buscada:\n" + archivoMovimientos,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtLog.AppendText("✗ Archivo no encontrado: " + archivoMovimientos + "\n\n");
                    return;
                }

                txtLog.AppendText("Archivo encontrado, iniciando migración...\n");

                using (var migrador = new MigradorDBF(archivoMovimientos))
                {
                    var resultado = migrador.MigrarMovimientos("CTL_MOV");

                    txtLog.AppendText("✓ Migrados exitosamente: " + resultado.exitos + "\n");
                    txtLog.AppendText("✗ Errores: " + resultado.errores + "\n");

                    if (!string.IsNullOrEmpty(resultado.mensajeError))
                    {
                        txtLog.AppendText("Detalles de errores:\n" + resultado.mensajeError + "\n");
                    }

                    txtLog.AppendText("=== MIGRACIÓN DE MOVIMIENTOS COMPLETADA ===\n\n");

                    MessageBox.Show("Migración de movimientos completada.\n\nÉxitos: " + resultado.exitos + "\nErrores: " + resultado.errores,
                        "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                txtLog.AppendText("ERROR: " + ex.Message + "\n");
                if (ex.InnerException != null)
                {
                    txtLog.AppendText("Detalles: " + ex.InnerException.Message + "\n\n");
                }

                string mensaje = "Error durante la migración:\n\n" + ex.Message;
                if (ex.InnerException != null)
                {
                    mensaje += "\n\n" + ex.InnerException.Message;
                }
                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}