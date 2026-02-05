using System.Data.Entity;
using System.Configuration;
using PaintControl.Models;

namespace PaintControl.Data
{
    public class DOALDbContext : DbContext
    {
        public DOALDbContext() : base("name=DOALConnection")
        {
            // Crear la base de datos si no existe
            Database.SetInitializer(new CreateDatabaseIfNotExists<DOALDbContext>());
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }

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

            base.OnModelCreating(modelBuilder);
        }
    }
}