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

    private SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public async Task<List<Producto>> GetAllAsync()
    {
        var productos = new List<Producto>();

        using (var connection = CreateConnection())
        {
            await connection.OpenAsync();
            string query = "SELECT Id, Nombre, Precio, CategoriaId, UrlImagen, Descripcion FROM Producto";
            using (var command = new SqlCommand(query, connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    productos.Add(MapReaderToProducto(reader));
                }
            }
        }

        return productos;
    }

    public async Task<Producto?> GetByIdAsync(int id)
    {
        using (var connection = CreateConnection())
        {
            await connection.OpenAsync();
            string query = "SELECT Id, Nombre, Precio, CategoriaId, UrlImagen, Descripcion FROM Producto WHERE Id = @Id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    return await reader.ReadAsync() ? MapReaderToProducto(reader) : null;
                }
            }
        }
    }

    public async Task<bool> AddAsync(Producto producto)
    {
        using (var connection = CreateConnection())
        {
            await connection.OpenAsync();
            string query = "INSERT INTO Producto (Nombre, Precio, CategoriaId, UrlImagen, Descripcion) OUTPUT INSERTED.Id VALUES (@Nombre, @Precio, @CategoriaId, @UrlImagen, @Descripcion)";
            using (var command = new SqlCommand(query, connection))
            {
                AddParameters(command, producto);
                var id = await command.ExecuteScalarAsync();
                return id != null;
            }
        }
    }

    public async Task<bool> UpdateAsync(Producto producto)
    {
        using (var connection = CreateConnection())
        {
            await connection.OpenAsync();
            string query = "UPDATE Producto SET Nombre = @Nombre, Precio = @Precio, CategoriaId = @CategoriaId, UrlImagen = @UrlImagen, Descripcion = @Descripcion WHERE Id = @Id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", producto.Id);
                AddParameters(command, producto);
                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using (var connection = CreateConnection())
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

    public async Task<List<Producto>> GetByCategoriaIdAsync(int categoriaId)
    {
        var productos = new List<Producto>();

        using (var connection = CreateConnection())
        {
            await connection.OpenAsync();
            string query = "SELECT Id, Nombre, Precio, CategoriaId, UrlImagen, Descripcion FROM Producto WHERE CategoriaId = @CategoriaId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CategoriaId", categoriaId);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        productos.Add(MapReaderToProducto(reader));
                    }
                }
            }
        }

        return productos;
    }

    private static Producto MapReaderToProducto(SqlDataReader reader)
    {
        return new Producto
        {
            Id = reader.GetInt32(0),
            Nombre = reader.GetString(1),
            Precio = reader.GetDecimal(2),
            CategoriaId = reader.GetInt32(3),
            UrlImagen = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
            Descripcion = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
        };
    }

    private static void AddParameters(SqlCommand command, Producto producto)
    {
        command.Parameters.AddWithValue("@Nombre", producto.Nombre);
        command.Parameters.AddWithValue("@Precio", producto.Precio);
        command.Parameters.AddWithValue("@CategoriaId", producto.CategoriaId);
        command.Parameters.AddWithValue("@UrlImagen", producto.UrlImagen ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Descripcion", producto.Descripcion ?? (object)DBNull.Value);
    }
}
