using Microsoft.EntityFrameworkCore;
using EcommerceBackend.Models;

namespace EcommerceBackend.Data
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }  // Aunque en la BD se llame "Categoria", aquí lo llamamos "Categorias" para la colección.
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }
        public DbSet<Reseña> Reseñas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapear la entidad Categoria a la tabla "Categoria"
            modelBuilder.Entity<Categoria>().ToTable("Categoria");
            // Otras configuraciones si es necesario

            base.OnModelCreating(modelBuilder);
        }
    }
}
