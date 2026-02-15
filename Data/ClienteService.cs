using PaintControl.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;

namespace PaintControl.Data
{
    public class ClienteService
    {
        // Buscar por nombre
        public List<Cliente> BuscarPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return new List<Cliente>();

            return ConnectionHelper.EjecutarConReintento(() =>
            {
                using (var context = new DOALDbContext())
                {
                    return context.Clientes
                        .AsNoTracking()
                        .Where(c => c.Nombre.Contains(nombre))
                        .OrderBy(c => c.Nombre)
                        .ToList();
                }
            });
        }

        // Buscar por código
        public Cliente BuscarPorCodigo(string codigo)
        {
            return ConnectionHelper.EjecutarConReintento(() =>
            {
                using (var context = new DOALDbContext())
                {
                    return context.Clientes
                        .AsNoTracking()
                        .FirstOrDefault(c => c.Codigo == codigo);
                }
            });
        }

        // Buscar por código o nombre (búsqueda flexible - encuentra "Martinez Perez Juan" buscando "Juan Martinez Perez")
        // Solo devuelve resultados si TODAS las palabras coinciden. Si no hay coincidencia total, devuelve lista vacía.
        public List<Cliente> BuscarPorCodigoONombre(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
                return new List<Cliente>();

            return ConnectionHelper.EjecutarConReintento(() =>
            {
                using (var context = new DOALDbContext())
                {
                    // Paso 1: Buscar coincidencia directa (código o nombre contiene el criterio completo)
                    var resultadosExactos = context.Clientes
                        .AsNoTracking()
                        .Where(c => c.Codigo.Contains(criterio) || c.Nombre.Contains(criterio))
                        .OrderBy(c => c.Nombre)
                        .ToList();

                    if (resultadosExactos.Count > 0)
                        return resultadosExactos;

                    // Paso 2: Si el criterio tiene múltiples palabras, buscar en cualquier orden
                    // pero TODAS las palabras deben estar presentes en el nombre
                    string[] palabras = criterio.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (palabras.Length <= 1)
                        return new List<Cliente>(); // Una sola palabra sin resultado = no encontrado

                    // Capturar primera palabra en variable local (EF6 no soporta ArrayIndex en LINQ)
                    string primeraPalabra = palabras[0];

                    // Traer candidatos que contengan al menos la primera palabra
                    var candidatos = context.Clientes
                        .AsNoTracking()
                        .Where(c => c.Nombre.Contains(primeraPalabra))
                        .ToList();

                    // Filtrar en memoria: el nombre DEBE contener TODAS las palabras buscadas
                    var resultadosFlexibles = candidatos
                        .Where(c =>
                        {
                            string nombreUpper = c.Nombre.ToUpperInvariant();
                            return palabras.All(p => nombreUpper.Contains(p.ToUpperInvariant()));
                        })
                        .OrderBy(c => c.Nombre)
                        .ToList();

                    // Si no hay coincidencia total, devolver lista vacía (se ofrecerá crear cliente)
                    return resultadosFlexibles;
                }
            });
        }

        // Obtener todos los clientes
        public List<Cliente> ObtenerTodos()
        {
            return ConnectionHelper.EjecutarConReintento(() =>
            {
                using (var context = new DOALDbContext())
                {
                    return context.Clientes
                        .AsNoTracking()
                        .OrderBy(c => c.Nombre)
                        .ToList();
                }
            });
        }

        // Obtener por ID
        public Cliente ObtenerPorId(int id)
        {
            return ConnectionHelper.EjecutarConReintento(() =>
            {
                using (var context = new DOALDbContext())
                {
                    return context.Clientes.Find(id);
                }
            });
        }

        // Agregar cliente
        public bool AgregarCliente(Cliente cliente)
        {
            try
            {
                return ConnectionHelper.EjecutarConReintento(() =>
                {
                    using (var context = new DOALDbContext())
                    {
                        // Generar código automáticamente
                        var ultimoId = context.Clientes.Any()
                            ? context.Clientes.Max(c => c.Id)
                            : 0;
                        cliente.Codigo = $"CLI{(ultimoId + 1):000}";
                        cliente.FechaRegistro = DateTime.Now;

                        context.Clientes.Add(cliente);
                        context.SaveChanges();
                        return true;
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al agregar cliente: {ex.Message}");
                if (ex.InnerException != null)
                    Debug.WriteLine($"  Inner: {ex.InnerException.Message}");
                return false;
            }
        }

        // Actualizar cliente
        public bool ActualizarCliente(Cliente cliente)
        {
            try
            {
                return ConnectionHelper.EjecutarConReintento(() =>
                {
                    using (var context = new DOALDbContext())
                    {
                        var clienteExistente = context.Clientes.Find(cliente.Id);
                        if (clienteExistente == null)
                            return false;

                        clienteExistente.Codigo = cliente.Codigo;
                        clienteExistente.Nombre = cliente.Nombre;

                        context.SaveChanges();
                        return true;
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al actualizar cliente {cliente.Id}: {ex.Message}");
                return false;
            }
        }

        // Contar movimientos de un cliente
        public int ContarMovimientos(int clienteId)
        {
            try
            {
                return ConnectionHelper.EjecutarConReintento(() =>
                {
                    using (var context = new DOALDbContext())
                    {
                        return context.Movimientos.Count(m => m.ClienteId == clienteId);
                    }
                });
            }
            catch
            {
                return 0;
            }
        }

        // Eliminar cliente
        public bool EliminarCliente(int id)
        {
            try
            {
                return ConnectionHelper.EjecutarConReintento(() =>
                {
                    using (var context = new DOALDbContext())
                    {
                        var cliente = context.Clientes.Find(id);
                        if (cliente == null)
                            return false;

                        context.Clientes.Remove(cliente);
                        context.SaveChanges();
                        return true;
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al eliminar cliente {id}: {ex.Message}");
                return false;
            }
        }
    }
}