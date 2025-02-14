using Microsoft.Data.SqlClient;
public class DetallePedidoRepository : IDetallePedidoRepository
{
    private readonly string _connectionString;

    public DetallePedidoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<DetallePedido>> GetByPedidoIdAsync(int pedidoId)
    {
        var detalles = new List<DetallePedido>();

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT Id, PedidoId, ProductoId, Cantidad, PrecioUnitario FROM DetallePedido WHERE PedidoId = @PedidoId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PedidoId", pedidoId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            detalles.Add(new DetallePedido
                            {
                                Id = reader.GetInt32(0),
                                PedidoId = reader.GetInt32(1),
                                ProductoId = reader.GetInt32(2),
                                Cantidad = reader.GetInt32(3),
                                PrecioUnitario = reader.GetDecimal(4)
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Manejo de excepciones
            throw new ApplicationException("Error al obtener los detalles del pedido", ex);
        }

        return detalles;
    }

    public async Task AddAsync(DetallePedido detallePedido)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO DetallePedido (PedidoId, ProductoId, Cantidad, PrecioUnitario) VALUES (@PedidoId, @ProductoId, @Cantidad, @PrecioUnitario)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PedidoId", detallePedido.PedidoId);
                    command.Parameters.AddWithValue("@ProductoId", detallePedido.ProductoId);
                    command.Parameters.AddWithValue("@Cantidad", detallePedido.Cantidad);
                    command.Parameters.AddWithValue("@PrecioUnitario", detallePedido.PrecioUnitario);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        catch (Exception ex)
        {
            // Manejo de excepciones
            throw new ApplicationException("Error al agregar el detalle del pedido", ex);
        }
    }
}
