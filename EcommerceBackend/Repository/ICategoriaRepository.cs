using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICategoriaRepository
{
    Task<List<Categoria>> GetAllAsync();
    Task<Categoria?> GetByIdAsync(int id);
}
