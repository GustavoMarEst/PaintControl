using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintControl.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(20)]
        public string Codigo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [MaxLength(20)]
        public string Telefono { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(200)]
        public string Direccion { get; set; }

        public DateTime FechaRegistro { get; set; }

        // Relación con Movimientos
        public virtual ICollection<Movimiento> Movimientos { get; set; }

        public Cliente()
        {
            FechaRegistro = DateTime.Now;
            Movimientos = new List<Movimiento>();
        }

        public override string ToString()
        {
            return $"{Codigo} - {Nombre}";
        }
    }
}