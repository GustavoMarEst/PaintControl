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

            // Timeout para comandos SQL: si una consulta no responde en 15 segundos,
            // se cancela en vez de esperar los 30 segundos por defecto.
            // Esto evita que la app se "congele" esperando al servidor.
            this.Database.CommandTimeout = 15;

            // Desactivar detección automática de cambios para lecturas AsNoTracking
            // (mejora rendimiento en consultas de solo lectura)
            this.Configuration.AutoDetectChangesEnabled = true;

            // Desactivar lazy loading - cargamos todo explícitamente con Include()
            // Evita consultas N+1 accidentales que degradan rendimiento en red
            this.Configuration.LazyLoadingEnabled = false;

            // Desactivar proxy - no necesitamos entidades proxy para este uso
            this.Configuration.ProxyCreationEnabled = false;
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