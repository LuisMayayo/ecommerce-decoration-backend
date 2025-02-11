using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ProductoRepository : IProductoRepository
{
    private readonly List<Producto> _productos = new List<Producto>
    {
        new Producto { Id = 1, Nombre = "Lámpara", Descripcion = "Lámpara LED", Precio = 45.99m, Stock = 10 },
        new Producto { Id = 2, Nombre = "Cojín", Descripcion = "Cojín decorativo", Precio = 19.99m, Stock = 20 }
    };

    public async Task<IEnumerable<Producto>> GetAll()
    {
        return await Task.FromResult(_productos);
    }

    public async Task<Producto> GetById(int id)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == id);
        return await Task.FromResult(producto);
    }

    public async Task Add(Producto producto)
    {
        producto.Id = _productos.Count + 1;
        _productos.Add(producto);
        await Task.CompletedTask;
    }

    public async Task Update(Producto producto)
    {
        var index = _productos.FindIndex(p => p.Id == producto.Id);
        if (index != -1)
        {
            _productos[index] = producto;
        }
        await Task.CompletedTask;
    }

    public async Task Delete(int id)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == id);
        if (producto != null)
        {
            _productos.Remove(producto);
        }
        await Task.CompletedTask;
    }
}
