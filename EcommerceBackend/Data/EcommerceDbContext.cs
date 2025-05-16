using Microsoft.EntityFrameworkCore;
using EcommerceBackend.Models;

namespace EcommerceBackend.Data
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options) { }
        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }
        public DbSet<Reseña> Reseñas { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; } // Nueva entidad añadida
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetallePedido>().ToTable("DetallePedido");
            modelBuilder.Entity<Pedido>().ToTable("Pedido");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Producto>().ToTable("Producto");
            modelBuilder.Entity<Categoria>().ToTable("Categoria");
            modelBuilder.Entity<Reseña>().ToTable("Reseña");
            modelBuilder.Entity<Proveedor>().ToTable("Proveedor"); // Mapeo de la nueva tabla
            
            // Definir precision decimal para evitar errores de redondeo
            modelBuilder.Entity<DetallePedido>()
                .Property(d => d.PrecioUnitario)
                .HasColumnType("decimal(18,2)");
            
            modelBuilder.Entity<Pedido>()
                .Property(p => p.Total)
                .HasColumnType("decimal(18,2)");
            
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");
            
            // Configuración de la relación entre Producto y Proveedor
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Proveedor)
                .WithMany(pr => pr.Productos)
                .HasForeignKey(p => p.ProveedorId)
                .OnDelete(DeleteBehavior.SetNull); // Cuando se elimina un proveedor, los productos quedan sin proveedor
            
            // Configurar campos únicos para Proveedor
            modelBuilder.Entity<Proveedor>()
                .HasIndex(p => p.NIF)
                .IsUnique();
            
            modelBuilder.Entity<Proveedor>()
                .HasIndex(p => p.Email)
                .IsUnique();
            
            base.OnModelCreating(modelBuilder);
        }
    }
}