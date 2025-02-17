using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICategoriaService
{
    Task<List<Categoria>> GetAllAsync();
    Task<Categoria?> GetByIdAsync(int id);
}
