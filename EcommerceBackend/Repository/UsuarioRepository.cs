using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

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
            string query = @"
                INSERT INTO Usuario (Nombre, Email, PasswordHash, PasswordSalt, FechaRegistro, EsAdmin) 
                OUTPUT INSERTED.Id 
                VALUES (@Nombre, @Email, @PasswordHash, @PasswordSalt, @FechaRegistro, @EsAdmin)";
            
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@PasswordHash", usuario.PasswordHash);
                command.Parameters.AddWithValue("@PasswordSalt", usuario.PasswordSalt);
                command.Parameters.AddWithValue("@FechaRegistro", usuario.FechaRegistro);
                command.Parameters.AddWithValue("@EsAdmin", usuario.EsAdmin); 

                // Recuperar el ID generado automáticamente
                usuario.Id = (int)await command.ExecuteScalarAsync();
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
            string query = "SELECT Id, Nombre, Email, PasswordHash, PasswordSalt, FechaRegistro, EsAdmin FROM Usuario WHERE Id = @Id";
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
                            FechaRegistro = reader.GetDateTime(5),
                            EsAdmin = reader.GetBoolean(6)
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
            string query = "SELECT Id, Nombre, Email, PasswordHash, PasswordSalt, FechaRegistro, EsAdmin FROM Usuario WHERE Email = @Email";
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
                            FechaRegistro = reader.GetDateTime(5),
                            EsAdmin = reader.GetBoolean(6)
                        };
                    }
                }
            }
        }

        return usuario;
    }
}
