using System.Data.Entity;
using System.Configuration;
using PaintControl.Models;

namespace PaintControl.Data
{
    public class DOALDbContext : DbContext
    {
        public DOALDbContext() : base("name=DOALConnection")
        {
            // No intentar crear ni migrar la base de datos
            // Las tablas ya existen (creadas manualmente o por script SQL)
            Database.SetInitializer<DOALDbContext>(null);
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<TipoPintura> TiposPintura { get; set; }
        public DbSet<LineaPintura> LineasPintura { get; set; }
        public DbSet<AcabadoPintura> AcabadosPintura { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configuración de relación Cliente-Movimientos
            modelBuilder.Entity<Movimiento>()
                .HasRequired(m => m.Cliente)
                .WithMany(c => c.Movimientos)
                .HasForeignKey(m => m.ClienteId)
                .WillCascadeOnDelete(true);

            // Configuración de precisión decimal para Movimiento
            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Cantidad)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Precio)
                .HasPrecision(10, 2);

            // Configuración de relación TipoPintura -> Lineas
            modelBuilder.Entity<LineaPintura>()
                .HasRequired(l => l.TipoPintura)
                .WithMany(t => t.Lineas)
                .HasForeignKey(l => l.TipoPinturaId)
                .WillCascadeOnDelete(true);

            // Configuración de relación LineaPintura -> Acabados
            modelBuilder.Entity<AcabadoPintura>()
                .HasRequired(a => a.LineaPintura)
                .WithMany(l => l.Acabados)
                .HasForeignKey(a => a.LineaPinturaId)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}