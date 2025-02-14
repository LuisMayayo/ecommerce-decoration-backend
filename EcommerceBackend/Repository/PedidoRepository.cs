using Microsoft.Data.SqlClient;

public class PedidoRepository : IPedidoRepository
{
    private readonly string _connectionString;

    public PedidoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Pedido> AddAsync(Pedido pedido)
    {
        if (pedido.UsuarioId <= 0 || pedido.Total <= 0)
        {
            throw new ArgumentException("Los datos del pedido son inválidos.");
        }

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO Pedido (UsuarioId, FechaPedido, Total) VALUES (@UsuarioId, @FechaPedido, @Total); SELECT SCOPE_IDENTITY();";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", pedido.UsuarioId);
                    command.Parameters.AddWithValue("@FechaPedido", pedido.FechaPedido);
                    command.Parameters.AddWithValue("@Total", pedido.Total);

                    pedido.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error al agregar el pedido", ex);
        }

        return pedido;
    }

    public async Task<Pedido> GetByIdAsync(int id)
    {
        Pedido? pedido = null;

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT Id, UsuarioId, FechaPedido, Total FROM Pedido WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            pedido = new Pedido
                            {
                                Id = reader.GetInt32(0),
                                UsuarioId = reader.GetInt32(1),
                                FechaPedido = reader.GetDateTime(2),
                                Total = reader.GetDecimal(3)
                            };
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error al obtener el pedido", ex);
        }

        return pedido;
    }

    public async Task<List<Pedido>> GetByUserIdAsync(int userId)
    {
        var pedidos = new List<Pedido>();

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT Id, UsuarioId, FechaPedido, Total FROM Pedido WHERE UsuarioId = @UsuarioId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", userId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            pedidos.Add(new Pedido
                            {
                                Id = reader.GetInt32(0),
                                UsuarioId = reader.GetInt32(1),
                                FechaPedido = reader.GetDateTime(2),
                                Total = reader.GetDecimal(3)
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error al obtener los pedidos del usuario", ex);
        }

        return pedidos;
    }
}
