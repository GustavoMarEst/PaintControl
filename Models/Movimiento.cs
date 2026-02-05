using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintControl.Models
{
    public class Movimiento
    {
        public int Id { get; set; }
        public int NumeroMovimiento { get; set; }
        public int ClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public string ClaveColor { get; set; }
        public string Descripcion { get; set; }
        public string Base { get; set; }
        public string Unidad { get; set; } // "Litro" o "Galón"
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public string Formula { get; set; } // Máximo 6 componentes

        public Movimiento()
        {
            Fecha = DateTime.Now;
        }

        // Calcular el total del movimiento
        public decimal Total
        {
            get { return Cantidad * Precio; }
        }
    }
}
