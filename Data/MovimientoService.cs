using PaintControl.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;

namespace PaintControl.Data
{
    public class MovimientoService
    {
        // Agregar movimiento
        public bool AgregarMovimiento(Movimiento movimiento)
        {
            try
            {
                using (var context = new DOALDbContext())
                {
                    if (movimiento.NumeroMovimiento == 0)
                    {
                        var ultimoNumero = context.Movimientos.Any()
                            ? context.Movimientos.Max(m => m.NumeroMovimiento)
                            : 1000;
                        movimiento.NumeroMovimiento = ultimoNumero + 1;
                    }

                    context.Movimientos.Add(movimiento);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al agregar movimiento: {ex.Message}");
                if (ex.InnerException != null)
                    Debug.WriteLine($"  Inner: {ex.InnerException.Message}");
                return false;
            }
        }

        // Obtener movimientos por cliente
        public List<Movimiento> ObtenerPorCliente(int clienteId)
        {
            using (var context = new DOALDbContext())
            {
                return context.Movimientos
                    .AsNoTracking()
                    .Where(m => m.ClienteId == clienteId)
                    .OrderByDescending(m => m.Fecha)
                    .ToList();
            }
        }

        // Obtener por cliente y fechas
        public List<Movimiento> ObtenerPorClienteYFechas(int clienteId, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var context = new DOALDbContext())
            {
                return context.Movimientos
                    .AsNoTracking()
                    .Where(m => m.ClienteId == clienteId &&
                               m.Fecha >= fechaInicio &&
                               m.Fecha <= fechaFin)
                    .OrderByDescending(m => m.Fecha)
                    .ToList();
            }
        }

        // Obtener todos los movimientos (con cliente incluido)
        public List<Movimiento> ObtenerTodos()
        {
            using (var context = new DOALDbContext())
            {
                return context.Movimientos
                    .AsNoTracking()
                    .Include(m => m.Cliente)
                    .OrderByDescending(m => m.Fecha)
                    .ToList();
            }
        }

        // Obtener por número de movimiento
        public Movimiento ObtenerPorNumero(int numeroMovimiento)
        {
            using (var context = new DOALDbContext())
            {
                return context.Movimientos
                    .Include(m => m.Cliente)
                    .FirstOrDefault(m => m.NumeroMovimiento == numeroMovimiento);
            }
        }

        // Actualizar movimiento
        public bool ActualizarMovimiento(Movimiento movimiento)
        {
            try
            {
                using (var context = new DOALDbContext())
                {
                    var movimientoExistente = context.Movimientos.Find(movimiento.Id);
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

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al actualizar movimiento {movimiento.Id}: {ex.Message}");
                return false;
            }
        }

        // Eliminar movimiento
        public bool EliminarMovimiento(int id)
        {
            try
            {
                using (var context = new DOALDbContext())
                {
                    var movimiento = context.Movimientos.Find(id);
                    if (movimiento == null)
                        return false;

                    context.Movimientos.Remove(movimiento);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al eliminar movimiento {id}: {ex.Message}");
                return false;
            }
        }

        // Obtener siguiente número de movimiento
        public int ObtenerSiguienteNumeroMovimiento()
        {
            using (var context = new DOALDbContext())
            {
                if (!context.Movimientos.Any())
                    return 1001;

                return context.Movimientos.Max(m => m.NumeroMovimiento) + 1;
            }
        }

        // Obtener todos por fechas
        public List<Movimiento> ObtenerTodosPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            using (var context = new DOALDbContext())
            {
                return context.Movimientos
                    .AsNoTracking()
                    .Where(m => m.Fecha >= fechaInicio && m.Fecha <= fechaFin)
                    .OrderByDescending(m => m.Fecha)
                    .ToList();
            }
        }
    }
}