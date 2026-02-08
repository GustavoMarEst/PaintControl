using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintControl.Models
{
    [Table("AcabadosPintura")]
    public class AcabadoPintura
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int LineaPinturaId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        public bool Activo { get; set; }

        [ForeignKey("LineaPinturaId")]
        public virtual LineaPintura LineaPintura { get; set; }

        public AcabadoPintura()
        {
            Activo = true;
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}