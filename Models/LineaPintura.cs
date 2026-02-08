using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintControl.Models
{
    [Table("LineasPintura")]
    public class LineaPintura
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TipoPinturaId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(10)]
        public string Abreviatura { get; set; }

        public bool Activo { get; set; }

        [ForeignKey("TipoPinturaId")]
        public virtual TipoPintura TipoPintura { get; set; }

        public virtual ICollection<AcabadoPintura> Acabados { get; set; }

        public LineaPintura()
        {
            Activo = true;
            Acabados = new List<AcabadoPintura>();
        }

        public override string ToString()
        {
            return $"{Nombre} ({Abreviatura})";
        }
    }
}