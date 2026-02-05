using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintControl.Models
{
    [Table("Movimientos")]
    public class Movimiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int NumeroMovimiento { get; set; }

        [Required]
        public int ClienteId { get; set; }

        public DateTime Fecha { get; set; }

        [MaxLength(50)]
        public string ClaveColor { get; set; }

        [MaxLength(200)]
        public string Descripcion { get; set; }

        [MaxLength(50)]
        public string Base { get; set; }

        [MaxLength(20)]
        public string Unidad { get; set; }

        // QUITAR [Column(TypeName = "decimal(10,2)")] - se configura en DbContext
        public decimal Cantidad { get; set; }

        // QUITAR [Column(TypeName = "decimal(10,2)")] - se configura en DbContext
        public decimal Precio { get; set; }

        [MaxLength(500)]
        public string Formula { get; set; }

        // Relación con Cliente
        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        public Movimiento()
        {
            Fecha = DateTime.Now;
        }

        [NotMapped]
        public decimal Total
        {
            get { return Cantidad * Precio; }
        }
    }
}