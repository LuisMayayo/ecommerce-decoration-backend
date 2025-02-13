using Microsoft.Data.SqlClient;

public class ReseñaRepository : IReseñaRepository
{
    private readonly string _connectionString;

    public ReseñaRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Reseña> AddAsync(Reseña reseña)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = "INSERT INTO Reseña (ProductoId, UsuarioId, Comentario, Calificacion) VALUES (@ProductoId, @UsuarioId, @Comentario, @Calificacion); SELECT SCOPE_IDENTITY();";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProductoId", reseña.ProductoId);
                command.Parameters.AddWithValue("@UsuarioId", reseña.UsuarioId);
                command.Parameters.AddWithValue("@Comentario", reseña.Comentario);
                command.Parameters.AddWithValue("@Calificacion", reseña.Calificacion);

                reseña.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
            }
        }
        return reseña;
    }

    public async Task<List<Reseña>> GetByProductoIdAsync(int productoId)
    {
        var reseñas = new List<Reseña>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = "SELECT Id, ProductoId, UsuarioId, Comentario, Calificacion FROM Reseña WHERE ProductoId = @ProductoId";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProductoId", productoId);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        reseñas.Add(new Reseña
                        {
                            Id = reader.GetInt32(0),
                            ProductoId = reader.GetInt32(1),
                            UsuarioId = reader.GetInt32(2),
                            Comentario = reader.GetString(3),
                            Calificacion = reader.GetInt32(4)
                        });
                    }
                }
            }
        }

        return reseñas;
    }
}
