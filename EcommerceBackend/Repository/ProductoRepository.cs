using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductoRepository : IProductoRepository
{
    private readonly string _connectionString;

    public ProductoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Producto>> GetAllAsync()
    {
        var productos = new List<Producto>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = "SELECT Id, Nombre, Precio, CategoriaId, UrlImagen, Descripcion FROM Producto";
            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        productos.Add(new Producto
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Precio = reader.GetDecimal(2),
                            CategoriaId = reader.GetInt32(3),
                            UrlImagen = reader.GetString(4),
                            Descripcion = reader.GetString(5)
                        });
                    }
                }
            }
        }

        return productos;
    }

    public async Task<Producto?> GetByIdAsync(int id)
    {
        Producto? producto = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = "SELECT Id, Nombre, Precio, CategoriaId, UrlImagen, Descripcion FROM Producto WHERE Id = @Id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        producto = new Producto
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Precio = reader.GetDecimal(2),
                            CategoriaId = reader.GetInt32(3),
                            UrlImagen = reader.GetString(4),
                            Descripcion = reader.GetString(5)
                        };
                    }
                }
            }
        }

        return producto;
    }

    public async Task AddAsync(Producto producto)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = "INSERT INTO Producto (Nombre, Precio, CategoriaId, UrlImagen, Descripcion) VALUES (@Nombre, @Precio, @CategoriaId, @UrlImagen, @Descripcion)";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                command.Parameters.AddWithValue("@Precio", producto.Precio);
                command.Parameters.AddWithValue("@CategoriaId", producto.CategoriaId);
                command.Parameters.AddWithValue("@UrlImagen", producto.UrlImagen);
                command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task UpdateAsync(Producto producto)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = "UPDATE Producto SET Nombre = @Nombre, Precio = @Precio, CategoriaId = @CategoriaId, UrlImagen = @UrlImagen, Descripcion = @Descripcion WHERE Id = @Id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", producto.Id);
                command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                command.Parameters.AddWithValue("@Precio", producto.Precio);
                command.Parameters.AddWithValue("@CategoriaId", producto.CategoriaId);
                command.Parameters.AddWithValue("@UrlImagen", producto.UrlImagen);
                command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = "DELETE FROM Producto WHERE Id = @Id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}
