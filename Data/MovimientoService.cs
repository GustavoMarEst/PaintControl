using PaintControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintControl.Data
{
    public class MovimientoService
    {
        private List<Movimiento> movimientos;
        private int ultimoNumeroMovimiento = 0;

        public MovimientoService()
        {
            movimientos = new List<Movimiento>();
            InicializarDatosEjemplo();
        }

        private void InicializarDatosEjemplo()
        {
            // Movimientos de ejemplo para el cliente 1 (Juan Pérez)
            AgregarMovimiento(new Movimiento
            {
                Id = 1,
                NumeroMovimiento = 1001,
                ClienteId = 1,
                Fecha = DateTime.Now.AddDays(-10),
                ClaveColor = "BL-001",
                Descripcion = "Blanco Mate",
                Base = "Base A",
                Unidad = "Litro",
                Cantidad = 5,
                Precio = 150.50m,
                Formula = "R:255,G:255,B:255"
            });

            AgregarMovimiento(new Movimiento
            {
                Id = 2,
                NumeroMovimiento = 1002,
                ClienteId = 1,
                Fecha = DateTime.Now.AddDays(-5),
                ClaveColor = "AZ-025",
                Descripcion = "Azul Cielo",
                Base = "Base C",
                Unidad = "Galón",
                Cantidad = 2,
                Precio = 450.00m,
                Formula = "R:135,G:206,B:250"
            });

            // Movimientos de ejemplo para el cliente 2 (María González)
            AgregarMovimiento(new Movimiento
            {
                Id = 3,
                NumeroMovimiento = 1003,
                ClienteId = 2,
                Fecha = DateTime.Now.AddDays(-3),
                ClaveColor = "VR-100",
                Descripcion = "Verde Menta",
                Base = "Base B",
                Unidad = "Litro",
                Cantidad = 3,
                Precio = 180.00m,
                Formula = "R:152,G:251,B:152"
            });

            ultimoNumeroMovimiento = 1003;
        }

        public bool AgregarMovimiento(Movimiento movimiento)
        {
            if (movimiento.NumeroMovimiento == 0)
            {
                movimiento.NumeroMovimiento = ++ultimoNumeroMovimiento;
            }

            movimiento.Id = movimientos.Count > 0 ? movimientos.Max(m => m.Id) + 1 : 1;
            movimientos.Add(movimiento);
            return true;
        }

        public List<Movimiento> ObtenerPorCliente(int clienteId)
        {
            return movimientos.Where(m => m.ClienteId == clienteId)
                             .OrderByDescending(m => m.Fecha)
                             .ToList();
        }

        public List<Movimiento> ObtenerPorClienteYFechas(int clienteId, DateTime fechaInicio, DateTime fechaFin)
        {
            return movimientos.Where(m => m.ClienteId == clienteId &&
                                         m.Fecha >= fechaInicio &&
                                         m.Fecha <= fechaFin)
                             .OrderByDescending(m => m.Fecha)
                             .ToList();
        }

        public List<Movimiento> ObtenerTodos()
        {
            return movimientos.OrderByDescending(m => m.Fecha).ToList();
        }

        public Movimiento ObtenerPorNumero(int numeroMovimiento)
        {
            return movimientos.FirstOrDefault(m => m.NumeroMovimiento == numeroMovimiento);
        }

        public bool ActualizarMovimiento(Movimiento movimiento)
        {
            var movimientoExistente = movimientos.FirstOrDefault(m => m.Id == movimiento.Id);
            if (movimientoExistente == null)
                return false;

            movimientoExistente.Fecha = movimiento.Fecha;
            movimientoExistente.ClaveColor = movimiento.ClaveColor;
            movimientoExistente.Descripcion = movimiento.Descripcion;
            movimientoExistente.Base = movimiento.Base;
            movimientoExistente.Unidad = movimiento.Unidad;
            movimientoExistente.Cantidad = movimiento.Cantidad;
            movimientoExistente.Precio = movimiento.Precio;
            movimientoExistente.Formula = movimiento.Formula;
            return true;
        }

        public bool EliminarMovimiento(int id)
        {
            var movimiento = movimientos.FirstOrDefault(m => m.Id == id);
            if (movimiento == null)
                return false;

            movimientos.Remove(movimiento);
            return true;
        }

        public int ObtenerSiguienteNumeroMovimiento()
        {
            return ultimoNumeroMovimiento + 1;
        }


        // Agregar estos métodos a la clase MovimientoService

        public List<Movimiento> ObtenerTodosPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            return movimientos
                .Where(m => m.Fecha >= fechaInicio && m.Fecha <= fechaFin)
                .OrderByDescending(m => m.Fecha)
                .ToList();
        }
    }
}
