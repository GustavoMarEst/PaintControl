using PaintControl.Models;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintControl.Data
{
    public class ClienteService
    {
        private List<Cliente> clientes;

        public ClienteService()
        {
            clientes = new List<Cliente>
            {
                new Cliente(1, "CLI001", "Juan Pérez", "834-123-4567", "juan@email.com", "Calle Principal 123"),
                new Cliente(2, "CLI002", "María González", "834-234-5678", "maria@email.com", "Av. Hidalgo 456"),
                new Cliente(3, "CLI003", "Pedro Martínez", "834-345-6789", "pedro@email.com", "Col. Centro 789"),
                new Cliente(4, "CLI004", "Ana López", "834-456-7890", "ana@email.com", "Fracc. Las Flores 321"),
                new Cliente(5, "CLI005", "Juan Rodríguez", "834-567-8901", "carlos@email.com", "Zona Industrial 654")
            };
        }

        // Buscar por nombre (búsqueda principal)
        public List<Cliente> BuscarPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return new List<Cliente>();

            return clientes.Where(c => c.Nombre.ToLower().Contains(nombre.ToLower()))
                          .OrderBy(c => c.Nombre)
                          .ToList();
        }

        // Buscar por código (búsqueda alternativa)
        public Cliente BuscarPorCodigo(string codigo)
        {
            return clientes.FirstOrDefault(c => c.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
        }

        // Buscar por código o nombre
        public List<Cliente> BuscarPorCodigoONombre(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
                return new List<Cliente>();

            return clientes.Where(c =>
                c.Codigo.ToLower().Contains(criterio.ToLower()) ||
                c.Nombre.ToLower().Contains(criterio.ToLower()))
                .OrderBy(c => c.Nombre)
                .ToList();
        }

        public List<Cliente> ObtenerTodos()
        {
            return clientes.OrderBy(c => c.Nombre).ToList();
        }

        public Cliente ObtenerPorId(int id)
        {
            return clientes.FirstOrDefault(c => c.Id == id);
        }

        public bool AgregarCliente(Cliente cliente)
        {
            // Generar ID automáticamente
            cliente.Id = clientes.Count > 0 ? clientes.Max(c => c.Id) + 1 : 1;

            // Generar código automáticamente (CLI + número de 3 dígitos)
            cliente.Codigo = $"CLI{cliente.Id:000}";

            // Establecer fecha de registro
            cliente.FechaRegistro = DateTime.Now;

            // Agregar a la lista
            clientes.Add(cliente);
            return true;
        }

        public bool ActualizarCliente(Cliente cliente)
        {
            var clienteExistente = clientes.FirstOrDefault(c => c.Id == cliente.Id);
            if (clienteExistente == null)
                return false;

            clienteExistente.Codigo = cliente.Codigo;
            clienteExistente.Nombre = cliente.Nombre;
            clienteExistente.Telefono = cliente.Telefono;
            clienteExistente.Email = cliente.Email;
            clienteExistente.Direccion = cliente.Direccion;
            return true;
        }

        public bool EliminarCliente(int id)
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
                return false;

            clientes.Remove(cliente);
            return true;
        }
    }
}
