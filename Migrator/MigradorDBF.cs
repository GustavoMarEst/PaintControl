using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using PaintControl.Data;
using PaintControl.Models;

namespace PaintControl.Migracion
{
    public class MigradorDBF : IDisposable
    {
        private string rutaDBF;
        private DOALDbContext context;
        private bool disposed = false;

        public MigradorDBF(string rutaArchivoDBF)
        {
            this.rutaDBF = Path.GetDirectoryName(rutaArchivoDBF);
            this.context = new DOALDbContext();
        }

        // Migrar clientes desde CTL_CLIE.DBF
        public (int exitos, int errores, string mensajeError) MigrarClientes(string nombreArchivoDBF)
        {
            int exitos = 0;
            int errores = 0;
            string mensajeError = "";

            try
            {
                string connectionString = "Provider=VFPOLEDB.1;Data Source=" + rutaDBF + ";";

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT * FROM " + nombreArchivoDBF;

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                // Leer datos del DBF
                                string clieNo = reader["CLIE_NO"]?.ToString()?.Trim() ?? "";
                                string clieDesc = reader["CLIE_DESC"]?.ToString()?.Trim() ?? "";

                                if (string.IsNullOrEmpty(clieNo) || string.IsNullOrEmpty(clieDesc))
                                {
                                    errores++;
                                    continue;
                                }

                                // Crear el código en formato CLI + número
                                string codigo = "CLI" + clieNo.PadLeft(3, '0');

                                // Verificar que no exista ya
                                var existe = context.Clientes.Any(c => c.Codigo == codigo);

                                if (!existe)
                                {
                                    var cliente = new Cliente
                                    {
                                        Codigo = codigo,
                                        Nombre = clieDesc,
                                        FechaRegistro = DateTime.Now
                                    };

                                    context.Clientes.Add(cliente);
                                    context.SaveChanges();
                                    exitos++;
                                }
                            }
                            catch (Exception ex)
                            {
                                errores++;
                                mensajeError += "Error en registro: " + ex.Message + "\n";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string innerMsg = ex.InnerException != null ? ex.InnerException.Message : "";
                mensajeError = "Error general: " + ex.Message + "\n" + innerMsg;
            }

            return (exitos, errores, mensajeError);
        }

        // Migrar movimientos desde CTL_MOV.DBF
        public (int exitos, int errores, string mensajeError) MigrarMovimientos(string nombreArchivoDBF)
        {
            int exitos = 0;
            int errores = 0;
            string mensajeError = "";

            try
            {
                string connectionString = "Provider=VFPOLEDB.1;Data Source=" + rutaDBF + ";";

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT * FROM " + nombreArchivoDBF;

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                // Leer número de cliente del DBF
                                string movClien = reader["MOV_CLIEN"]?.ToString()?.Trim();

                                if (string.IsNullOrEmpty(movClien))
                                {
                                    errores++;
                                    mensajeError += "Movimiento sin cliente\n";
                                    continue;
                                }

                                // Buscar el cliente por código
                                string codigoBuscar = "CLI" + movClien.PadLeft(3, '0');
                                var cliente = context.Clientes.FirstOrDefault(c => c.Codigo == codigoBuscar);

                                if (cliente != null)
                                {
                                    var movimiento = new Movimiento
                                    {
                                        ClienteId = cliente.Id,
                                        NumeroMovimiento = reader["MOV_NO"] != DBNull.Value
                                            ? Convert.ToInt32(reader["MOV_NO"])
                                            : 0,
                                        Fecha = reader["FECHA"] != DBNull.Value
                                            ? Convert.ToDateTime(reader["FECHA"])
                                            : DateTime.Now,
                                        ClaveColor = reader["CLAVE"]?.ToString()?.Trim() ?? "",
                                        Descripcion = reader["DESC"]?.ToString()?.Trim() ?? "",
                                        Base = reader["BASE"]?.ToString()?.Trim() ?? "",
                                        Unidad = reader["UNIDAD"]?.ToString()?.Trim() ?? "",
                                        Cantidad = reader["CANTIDAD"] != DBNull.Value
                                            ? Convert.ToDecimal(reader["CANTIDAD"])
                                            : 0,
                                        Precio = reader["PRECIO"] != DBNull.Value
                                            ? Convert.ToDecimal(reader["PRECIO"])
                                            : 0,
                                        Formula = reader["COMENTARIO"]?.ToString()?.Trim() ?? ""
                                    };

                                    context.Movimientos.Add(movimiento);
                                    context.SaveChanges();
                                    exitos++;
                                }
                                else
                                {
                                    errores++;
                                    mensajeError += "Cliente no encontrado: " + movClien + " (buscado como " + codigoBuscar + ")\n";
                                }
                            }
                            catch (Exception ex)
                            {
                                errores++;
                                mensajeError += "Error: " + ex.Message + "\n";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string innerMsg = ex.InnerException != null ? ex.InnerException.Message : "";
                mensajeError = "Error general: " + ex.Message + "\n" + innerMsg;
            }

            return (exitos, errores, mensajeError);
        }

        // Implementación correcta de IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (context != null)
                    {
                        context.Dispose();
                        context = null;
                    }
                }
                disposed = true;
            }
        }

        ~MigradorDBF()
        {
            Dispose(false);
        }
    }
}