using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintControl.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaRegistro { get; set; }

        public Cliente()
        {
            FechaRegistro = DateTime.Now;
        }

        public Cliente(int id, string codigo, string nombre, string telefono, string email, string direccion)
        {
            Id = id;
            Codigo = codigo;
            Nombre = nombre;
            Telefono = telefono;
            Email = email;
            Direccion = direccion;
            FechaRegistro = DateTime.Now;
        }

        // Para mostrar en listas
        public override string ToString()
        {
            return $"{Codigo} - {Nombre}";
        }
    }
}
