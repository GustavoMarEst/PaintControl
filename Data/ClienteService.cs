using PaintControl.Models;
using System;
using System.Collections.Generic;
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

            using (var context = new DOALDbContext())
            {
                return context.Clientes
                    .Where(c => c.Nombre.Contains(nombre))
                    .OrderBy(c => c.Nombre)
                    .ToList();
            }
        }

        // Buscar por código
        public Cliente BuscarPorCodigo(string codigo)
        {
            using (var context = new DOALDbContext())
            {
                return context.Clientes
                    .FirstOrDefault(c => c.Codigo == codigo);
            }
        }

        // Buscar por código o nombre
        public List<Cliente> BuscarPorCodigoONombre(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
                return new List<Cliente>();

            using (var context = new DOALDbContext())
            {
                return context.Clientes
                    .Where(c => c.Codigo.Contains(criterio) || c.Nombre.Contains(criterio))
                    .OrderBy(c => c.Nombre)
                    .ToList();
            }
        }

        // Obtener todos los clientes
        public List<Cliente> ObtenerTodos()
        {
            using (var context = new DOALDbContext())
            {
                return context.Clientes
                    .OrderBy(c => c.Nombre)
                    .ToList();
            }
        }

        // Obtener por ID
        public Cliente ObtenerPorId(int id)
        {
            using (var context = new DOALDbContext())
            {
                return context.Clientes.Find(id);
            }
        }

        // Agregar cliente
        public bool AgregarCliente(Cliente cliente)
        {
            try
            {
                using (var context = new DOALDbContext())
                {
                    // Generar código automáticamente
                    var ultimoId = context.Clientes.Any()
                        ? context.Clientes.Max(c => c.Id)
                        : 0;
                    cliente.Codigo = $"CLI{(ultimoId + 1):000}";

                    // Establecer fecha de registro
                    cliente.FechaRegistro = DateTime.Now;

                    context.Clientes.Add(cliente);
                    context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // Actualizar cliente
        public bool ActualizarCliente(Cliente cliente)
        {
            try
            {
                using (var context = new DOALDbContext())
                {
                    var clienteExistente = context.Clientes.Find(cliente.Id);
                    if (clienteExistente == null)
                        return false;

                    clienteExistente.Codigo = cliente.Codigo;
                    clienteExistente.Nombre = cliente.Nombre;
                    clienteExistente.Telefono = cliente.Telefono;
                    clienteExistente.Email = cliente.Email;
                    clienteExistente.Direccion = cliente.Direccion;

                    context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // Eliminar cliente
        public bool EliminarCliente(int id)
        {
            try
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
            }
            catch
            {
                return false;
            }
        }
    }
}