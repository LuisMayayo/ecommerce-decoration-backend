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
        public DbSet<DetallePedido> DetallePedido { get; set; } // 👈 Nombre correcto (sin "s")
        public DbSet<Reseña> Reseñas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetallePedido>().ToTable("DetallePedido"); // 👈 Asegura el nombre correcto
            modelBuilder.Entity<Pedido>().ToTable("Pedido");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Producto>().ToTable("Producto");
            modelBuilder.Entity<Categoria>().ToTable("Categoria");
            modelBuilder.Entity<Reseña>().ToTable("Reseña");

            // Precisión decimal para precios
            modelBuilder.Entity<DetallePedido>()
                .Property(d => d.PrecioUnitario)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Total)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }

}
