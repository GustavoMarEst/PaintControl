using System;
using System.Windows.Forms;
using PaintControl.Data;

namespace PaintControl
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Manejador global: atrapa errores de conexión que no fueron
            // capturados por try/catch específicos en los formularios.
            // Esto evita que la app muestre el error técnico feo y truene.
            Application.ThreadException += (sender, e) =>
            {
                if (e.Exception is ConexionException ||
                    e.Exception.InnerException is ConexionException)
                {
                    MessageBox.Show(
                        "No se pudo conectar con el servidor.\n\n" +
                        "Verifique que:\n" +
                        "• La computadora principal esté encendida\n" +
                        "• Los cables de red estén bien conectados\n" +
                        "• El servicio de SQL Server esté activo\n\n" +
                        "Intente la operación nuevamente en unos segundos.",
                        "Error de Conexión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Ocurrió un error inesperado:\n\n" + e.Exception.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            };

            // Atrapar excepciones no manejadas en otros hilos
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Exception ex = e.ExceptionObject as Exception;
                MessageBox.Show(
                    "Error crítico:\n\n" + (ex?.Message ?? "Error desconocido"),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            };

            Application.Run(new FormPrincipal());
        }
    }
}