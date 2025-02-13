using Microsoft.Data.SqlClient;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly string _connectionString;

    public UsuarioRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Usuario> AddAsync(Usuario usuario)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = "INSERT INTO Usuario (Nombre, Email, PasswordHash, PasswordSalt, FechaRegistro) VALUES (@Nombre, @Email, @PasswordHash, @PasswordSalt, @FechaRegistro)";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@PasswordHash", usuario.PasswordHash);
                command.Parameters.AddWithValue("@PasswordSalt", usuario.PasswordSalt);
                command.Parameters.AddWithValue("@FechaRegistro", usuario.FechaRegistro);
                await command.ExecuteNonQueryAsync();
            }
        }
        return usuario;
    }

    public async Task<Usuario> GetByIdAsync(int id)
    {
        Usuario? usuario = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = "SELECT Id, Nombre, Email, PasswordHash, PasswordSalt, FechaRegistro FROM Usuario WHERE Id = @Id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        usuario = new Usuario
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Email = reader.GetString(2),
                            PasswordHash = reader.GetString(3),
                            PasswordSalt = reader.GetString(4),
                            FechaRegistro = reader.GetDateTime(5)
                        };
                    }
                }
            }
        }

        return usuario;
    }

    public async Task<Usuario> GetByEmailAsync(string email)
    {
        Usuario? usuario = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = "SELECT Id, Nombre, Email, PasswordHash, PasswordSalt, FechaRegistro FROM Usuario WHERE Email = @Email";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        usuario = new Usuario
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Email = reader.GetString(2),
                            PasswordHash = reader.GetString(3),
                            PasswordSalt = reader.GetString(4),
                            FechaRegistro = reader.GetDateTime(5)
                        };
                    }
                }
            }
        }

        return usuario;
    }
}
