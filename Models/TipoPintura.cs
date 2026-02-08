using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintControl.Models
{
    [Table("TiposPintura")]
    public class TipoPintura
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(10)]
        public string Abreviatura { get; set; }

        public bool Activo { get; set; }

        public virtual ICollection<LineaPintura> Lineas { get; set; }

        public TipoPintura()
        {
            Activo = true;
            Lineas = new List<LineaPintura>();
        }

        public override string ToString()
        {
            return $"{Nombre} ({Abreviatura})";
        }
    }
}