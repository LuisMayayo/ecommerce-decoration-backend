using Microsoft.Data.SqlClient;

public class ProveedoresRepository : IProveedoresRepository
{
    private readonly string _connectionString;

    public ProveedoresRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<Proveedor>> GetAllAsync()
    {
        var proveedores = new List<Proveedor>();
        
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            using (var command = new SqlCommand("SELECT Id, Nombre, Email, Telefono FROM Proveedores", connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        proveedores.Add(new Proveedor
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Email = reader.GetString(2),
                            Telefono = reader.GetString(3)
                        });
                    }
                }
            }
        }
        
        return proveedores;
    }

    public async Task<Proveedor> GetByIdAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            using (var command = new SqlCommand("SELECT Id, Nombre, Email, Telefono FROM Proveedores WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Proveedor
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Email = reader.GetString(2),
                            Telefono = reader.GetString(3)
                        };
                    }
                }
            }
        }
        
        return null;
    }

    public async Task<Proveedor> AddAsync(Proveedor proveedor)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            var query = @"INSERT INTO Proveedores (Nombre, Email, Telefono) 
                         VALUES (@Nombre, @Email, @Telefono);
                         SELECT CAST(SCOPE_IDENTITY() as int)";
            
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                command.Parameters.AddWithValue("@Email", proveedor.Email);
                command.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                
                proveedor.Id = (int)await command.ExecuteScalarAsync();
            }
        }
        
        return proveedor;
    }

    public async Task<bool> UpdateAsync(Proveedor proveedor)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            var query = @"UPDATE Proveedores 
                         SET Nombre = @Nombre, Email = @Email, Telefono = @Telefono 
                         WHERE Id = @Id";
            
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", proveedor.Id);
                command.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                command.Parameters.AddWithValue("@Email", proveedor.Email);
                command.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            using (var command = new SqlCommand("DELETE FROM Proveedores WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}
