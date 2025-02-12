using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPedidoRepository
{
    Task<IEnumerable<Pedido>> GetAll();
    Task<Pedido> GetById(int id);
    Task Add(Pedido pedido);
    Task Update(Pedido pedido);
    Task Delete(int id);
}
