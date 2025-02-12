using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PedidoRepository : IPedidoRepository
{
    private readonly List<Pedido> _pedidos = new List<Pedido>
    {
        new Pedido { Id = 1, UsuarioId = 1, Total = 99.99m, FechaPedido = DateTime.Now },
        new Pedido { Id = 2, UsuarioId = 2, Total = 59.50m, FechaPedido = DateTime.Now }
    };

    public async Task<IEnumerable<Pedido>> GetAll()
    {
        return await Task.FromResult(_pedidos);
    }

    public async Task<Pedido> GetById(int id)
    {
        var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
        return await Task.FromResult(pedido);
    }

    public async Task Add(Pedido pedido)
    {
        pedido.Id = _pedidos.Count + 1;
        pedido.FechaPedido = DateTime.Now;
        _pedidos.Add(pedido);
        await Task.CompletedTask;
    }

    public async Task Update(Pedido pedido)
    {
        var index = _pedidos.FindIndex(p => p.Id == pedido.Id);
        if (index != -1)
        {
            _pedidos[index] = pedido;
        }
        await Task.CompletedTask;
    }

    public async Task Delete(int id)
    {
        var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
        if (pedido != null)
        {
            _pedidos.Remove(pedido);
        }
        await Task.CompletedTask;
    }
}
