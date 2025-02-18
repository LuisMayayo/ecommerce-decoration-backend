using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly string _connectionString;

    public CategoriaRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Categoria>> GetAllAsync()
    {
        var categorias = new List<Categoria>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = "SELECT Id, Nombre, Descripcion, UrlImagen FROM Categoria"; 
            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categorias.Add(new Categoria
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Descripcion = reader.GetString(2),
                            UrlImagen = reader.GetString(3) 
                        });
                    }
                }
            }
        }

        return categorias;
    }

    public async Task<Categoria?> GetByIdAsync(int id)
    {
        Categoria? categoria = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = "SELECT Id, Nombre, Descripcion, UrlImagen FROM Categoria WHERE Id = @Id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        categoria = new Categoria
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Descripcion = reader.GetString(2),
                            UrlImagen = reader.GetString(3) 
                        };
                    }
                }
            }
        }

        return categoria;
    }
}

